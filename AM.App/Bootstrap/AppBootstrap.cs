using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.DBService.Services;
using AM.DBService.Services.Auth;
using AM.DBService.Services.Dev;
using AM.DBService.Services.Motion.App;
using AM.DBService.Services.Plc.App;
using AM.DBService.Services.Plc.Driver;
using AM.DBService.Services.Plc.Runtime;
using AM.DBService.Services.Runtime;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.MotionCard;
using AM.Model.Interfaces.Runtime;
using AM.Model.MotionCard;
using AM.MotionService.Factory;
using AM.MotionService.Hub;
using AM.Tools.Logging;
using AM.Tools.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.App.Bootstrap
{
    /// <summary>
    /// 应用启动组合根。
    /// 负责配置加载、系统基础设施实例化与硬件初始化。
    /// </summary>
    public static class AppBootstrap
    {
        /// <summary>
        /// 初始化应用运行环境。
        /// 启动顺序：基础设施 → DB种子 → 配置重载 → 硬件连接 → 后台扫描
        /// </summary>
        public static void Initialize()
        {
            // 1. 初始化基础配置上下文（仅基础参数，不含设备拓扑）
            var configResult = AM.Tools.Tools.ReadConfig<Config>("config.json");
            var config = configResult.Success ? configResult.Item : new Config();
            ConfigContext.Instance.Initialize(config);

            // 2. 构造系统基础设施
            IAMLogger logger = new NLogLogger("System");
            IMessageBus messageBus = new MessageBus();
            IErrorCatalog errorCatalog = new JsonErrorCatalog();

            // 报警持久化：保留强类型引用用于启动时恢复未清除报警，DevAlarmRecordService，不走 reporter 避免循环调用
            var devAlarmRecord = new DevAlarmRecordService();
            AlarmManager alarmManager = new AlarmManager(messageBus, logger, devAlarmRecord);
            // 恢复上次未清除的历史报警（跨重启持续有效，直到被明确清除）
            RestoreUnclearedAlarms(devAlarmRecord, alarmManager, logger);
            IAppReporter reporter = new AppReporter(messageBus, logger, alarmManager, errorCatalog);

            // 3. 初始化系统上下文
            IRuntimeTaskManager runtimeTaskManager = new RuntimeTaskManager(reporter);
            SystemContext.Instance.Initialize(logger, messageBus, alarmManager, errorCatalog, reporter, runtimeTaskManager);

            // 4. 初始化认证相关表与默认管理员账号
            var authSeedService = new AuthSeedService(reporter);
            authSeedService.EnsureSeedData();

            // 5. 初始化运动配置种子数据
            var machineConfigSeedService = new MachineConfigSeedService(reporter);
            var seedResult = machineConfigSeedService.EnsureSeedData();
            if (!seedResult.Success)
            {
                reporter.Error("AppBootstrap", "默认运动配置种子初始化失败，应用启动终止", seedResult.Code);
                return;
            }

            // 5.1 初始化 PLC 配置种子数据
            var plcConfigSeedService = new PlcConfigSeedService(reporter);
            var plcSeedResult = plcConfigSeedService.EnsureSeedData();
            if (!plcSeedResult.Success)
            {
                reporter.Error("AppBootstrap", "默认 PLC 配置种子初始化失败，应用启动终止", plcSeedResult.Code);
                return;
            }

            // 5.2 加载协议实现程序集
            ProtocolAssemblyRegistry.Reload();

            // 6. 从数据库加载完整设备配置并重建 MachineContext
            //    完成后 MachineContext 中已有所有控制卡服务实例、轴/DI/DO 映射、执行器配置
            //    但控制卡尚未物理连接
            var reloadService = new MachineConfigReloadService();
            var reloadResult = reloadService.ReloadAndRebuild();
            if (!reloadResult.Success)
            {
                reporter.Error("AppBootstrap", "数据库运动控制配置加载或设备上下文重建失败，应用启动终止", reloadResult.Code);
                return;
            }
            reporter.Info("AppBootstrap", "数据库运动控制配置加载并完成设备上下文重建");

            // 6.1 从数据库加载 PLC 配置并重建 PLC 客户端上下文
            var plcConfigAppService = new PlcConfigAppService();
            var plcReloadResult = plcConfigAppService.ReloadFromDatabase();
            if (!plcReloadResult.Success)
            {
                reporter.Error("AppBootstrap", "数据库 PLC 配置加载或 PLC 上下文重建失败，应用启动终止", plcReloadResult.Code);
                return;
            }
            reporter.Info("AppBootstrap", "数据库 PLC 配置加载并完成 PLC 上下文重建");

            // 7. 建立硬件连接（必须在 IoScanWorker 注册前完成，确保 autoStart 时卡已就绪）
            var machineResult = InitializeMachine();
            if (!machineResult.Success)
            {
                reporter.Error("AppBootstrap", "硬件初始化失败，应用启动终止", machineResult.Code);
                return;
            }

            // 8. 注册后台工作单元（此时控制卡已连接，autoStart 扫描安全）
            var ioScanConfig = config.IoScanConfig;
            var ioScanWorker = new IoScanWorker(reporter, ioScanConfig.ScanIntervalMs);
            var registerResult = runtimeTaskManager.Register(ioScanWorker, ioScanConfig.AutoStart);
            if (!registerResult.Success)
            {
                reporter.Error("AppBootstrap", "IO 扫描工作单元注册失败，应用启动终止", registerResult.Code);
                return;
            }

            // 8.1 注册后台工作单元（此时控制卡已连接，autoStart 扫描安全）
            // 第一版：轴运行态采样独立于 IO 扫描注册。仅供 UI / 监视使用，不参与控制安全逻辑。
            var axisScanWorker = new MotionAxisScanWorker(reporter, 100);
            var axisRegisterResult = runtimeTaskManager.Register(axisScanWorker, true);
            if (!axisRegisterResult.Success)
            {
                reporter.Error("AppBootstrap", "轴运行态采样服务注册失败，应用启动终止", axisRegisterResult.Code);
                return;
            }

            // 8.2 注册后台工作单元（此时 PLC 上下文已重建，autoStart 扫描安全）
            var plcScanWorker = new PlcScanWorker(reporter);
            var plcRegisterResult = runtimeTaskManager.Register(plcScanWorker, true);
            if (!plcRegisterResult.Success)
            {
                reporter.Error("AppBootstrap", "PLC 扫描工作单元注册失败，应用启动终止", plcRegisterResult.Code);
                return;
            }

            reporter.Info("AppBootstrap", "应用启动完成");
        }


        /// <summary>
        /// 按 InitOrder 顺序依次 Initialize + Connect 所有已注册控制卡。
        ///
        /// 前置条件：MachineConfigReloadService.ReloadAndRebuild() 已调用，
        ///   MachineContext.MotionCards 已按配置创建好控制卡服务实例。
        /// 任意一张卡失败则立即终止，不尝试其余卡。
        /// </summary>
        private static Result InitializeMachine()
        {
            var machine = MachineContext.Instance;
            var reporter = SystemContext.Instance.Reporter;
            var cardConfigs = ConfigContext.Instance.Config.MotionCardsConfig;

            if (machine.MotionCards.Count == 0)
            {
                reporter.Info("AppBootstrap", "当前无运动控制卡注册，跳过硬件初始化");
                return Result.Ok("无运动控制卡，跳过硬件初始化");
            }

            // 按 InitOrder 升序处理，多卡场景下确保初始化顺序可控
            var ordered = cardConfigs
                .Where(c => c != null && machine.MotionCards.ContainsKey(c.CardId))
                .OrderBy(c => c.InitOrder)
                .ToList();

            foreach (var cfg in ordered)
            {
                var card = machine.MotionCards[cfg.CardId];
                string label = string.Format("CardId={0} ({1})", cfg.CardId, cfg.DisplayName ?? cfg.Name);

                // 驱动层初始化（厂商卡加载底层驱动；虚拟卡为空操作）
                var initResult = card.Initialize(null);
                if (!initResult.Success)
                {
                    reporter.Error("AppBootstrap", string.Format("控制卡 {0} 驱动初始化失败: {1}", label, initResult.Message), initResult.Code);
                    return initResult;
                }

                // 建立物理连接（虚拟卡：内存中创建轴状态并标记已连接）
                var connectResult = card.Connect();
                if (!connectResult.Success)
                {
                    reporter.Error("AppBootstrap", string.Format("控制卡 {0} 连接失败: {1}", label, connectResult.Message), connectResult.Code);
                    return connectResult;
                }

                reporter.Info("AppBootstrap", string.Format("控制卡 {0} 连接成功", label));
            }

            reporter.Info("AppBootstrap", string.Format("全部 {0} 张控制卡初始化完成", ordered.Count));
            return Result.Ok("所有控制卡初始化成功");
        }

        /// <summary>
        /// 从数据库查询未清除报警并还原到 AlarmManager 内存中。
        /// 失败只写日志，不阻断启动流程。
        /// </summary>
        private static void RestoreUnclearedAlarms(DevAlarmRecordService record, AlarmManager alarmManager, IAMLogger logger)
        {
            try
            {
                var result = record.QueryPage(1, 500, isCleared: false);
                if (!result.Success || result.Items == null || result.Items.Count == 0)
                {
                    return;
                }

                var infos = new List<AlarmInfo>();
                foreach (var e in result.Items)
                {
                    AlarmLevel level;
                    if (!Enum.TryParse(e.AlarmLevel, out level))
                    {
                        level = AlarmLevel.Info;
                    }

                    infos.Add(new AlarmInfo(
                        (AlarmCode)e.AlarmCode,
                        level,
                        e.Message ?? string.Empty,
                        e.Source ?? string.Empty,
                        e.CardId,
                        e.Description,
                        e.Suggestion,
                        e.RaisedTime));
                }

                alarmManager.RestoreUnclearedAlarms(infos);
                logger.Info("AppBootstrap 已从数据库恢复 " + infos.Count + " 条未清除报警到活动报警面板");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "从数据库恢复未清除报警失败，此次忽略，不影响启动流程");
            }
        }
    }
}