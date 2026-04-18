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
using AM.DBService.Services.System;
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
                reporter.Alarm("AppBootstrap", AlarmCode.DatabaseError, AlarmLevel.Alarm, "默认运动配置种子初始化失败，应用启动终止");
                return;
            }

            // 5.1 初始化 PLC 配置种子数据
            var plcConfigSeedService = new PlcConfigSeedService(reporter);
            var plcSeedResult = plcConfigSeedService.EnsureSeedData();
            if (!plcSeedResult.Success)
            {
                reporter.Error("AppBootstrap", "默认 PLC 配置种子初始化失败，应用启动终止", plcSeedResult.Code);
                reporter.Alarm("AppBootstrap", AlarmCode.DatabaseError, AlarmLevel.Alarm, "默认 PLC 配置种子初始化失败，应用启动终止");
                return;
            }

            // 5.2 加载协议实现程序集
            ProtocolAssemblyRegistry.Reload();

            // 6. 从数据库加载完整设备配置并重建 MachineContext
            // 完成后 MachineContext 中已有所有控制卡服务实例、轴/DI/DO 映射、执行器配置，但控制卡尚未物理连接
            var reloadService = new MachineConfigReloadService();
            var reloadResult = reloadService.ReloadAndRebuild();
            if (!reloadResult.Success)
            {
                reporter.Error("AppBootstrap", "数据库运动控制配置加载或设备上下文重建失败，应用启动终止", reloadResult.Code);
                reporter.Alarm("AppBootstrap", AlarmCode.DatabaseError, AlarmLevel.Alarm, "数据库运动控制配置加载或设备上下文重建失败，应用启动终止");
                return;
            }
            reporter.Info("AppBootstrap", "数据库运动控制配置加载并完成设备上下文重建");

            // 6.1 从数据库加载 PLC 配置并重建 PLC 客户端上下文
            var plcConfigAppService = new PlcConfigAppService();
            var plcReloadResult = plcConfigAppService.ReloadFromDatabase();
            if (!plcReloadResult.Success)
            {
                reporter.Error("AppBootstrap", "数据库 PLC 配置加载或 PLC 上下文重建失败，应用启动终止", plcReloadResult.Code);
                reporter.Alarm("AppBootstrap", AlarmCode.DatabaseError, AlarmLevel.Alarm, "数据库 PLC 配置加载或 PLC 上下文重建失败，应用启动终止");
                return;
            }
            reporter.Info("AppBootstrap", "数据库 PLC 配置加载并完成 PLC 上下文重建");

            // 6.2 启动期读取并校验本地授权文件，将最终状态写入 LicenseRuntimeContext
            var licenseRuntimeLoader = new LicenseRuntimeLoader(reporter);
            var licenseLoadResult = licenseRuntimeLoader.Load();
            if (!licenseLoadResult.Success)
            {
                reporter.Warn("AppBootstrap", "启动期授权装载执行失败，系统将按最小功能模式继续启动", licenseLoadResult.Code);
            }

            // 7. 建立硬件连接（容错：单卡失败不阻断后续后台服务注册）
            var machineResult = InitializeMachine();
            if (!machineResult.Success)
            {
                reporter.Warn("AppBootstrap", "部分运动控制卡初始化失败，后台服务将继续注册，请在报警面板中处理", machineResult.Code);
            }

            // 8. 注册后台工作单元（容错：单个 worker 失败不阻断其他 worker）
            var ioScanConfig = config.IoScanConfig;
            RegisterRuntimeWorkerWithTolerance(
                runtimeTaskManager,
                new IoScanWorker(reporter, ioScanConfig.ScanIntervalMs),
                ioScanConfig.AutoStart,
                "Motion IO 扫描工作单元",
                AlarmCode.MotionIoScanWorkerStartFailed);

            // 8.1 注册后台工作单元（此时控制卡已连接，autoStart 扫描安全）
            // 第一版：轴运行态采样独立于 IO 扫描注册。仅供 UI / 监视使用，不参与控制安全逻辑。
            RegisterRuntimeWorkerWithTolerance(
                runtimeTaskManager,
                new MotionAxisScanWorker(reporter),
                true,
                "轴运行态采样服务",
                AlarmCode.MotionAxisScanWorkerStartFailed);

            // 8.2 注册后台工作单元（此时 PLC 上下文已重建，autoStart 扫描安全）
            RegisterRuntimeWorkerWithTolerance(
                runtimeTaskManager,
                new PlcScanWorker(reporter),
                true,
                "PLC 扫描工作单元",
                AlarmCode.PlcScanWorkerStartFailed);

            // 8.3 注册统一后台上报工作单元。
            // 非关键后台任务：只做静默启动，不影响 PLC / Motion 等主后台链路。
            RegisterOptionalRuntimeWorker(
                runtimeTaskManager,
                new UsageUploadWorker(reporter),
                true,
                "后台上报工作单元");

            reporter.Info("AppBootstrap", "应用启动完成");
        }

        /// <summary>
        /// 按 InitOrder 顺序依次 Initialize + Connect 所有已注册控制卡。
        ///
        /// 前置条件：MachineConfigReloadService.ReloadAndRebuild() 已调用，
        /// MachineContext.MotionCards 已按配置创建好控制卡服务实例。
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

            int successCount = 0;
            int failedCount = 0;
            int firstErrorCode = 0;

            foreach (var cfg in ordered)
            {
                var card = machine.MotionCards[cfg.CardId];
                string label = string.Format("CardId={0} ({1})", cfg.CardId, cfg.DisplayName ?? cfg.Name);

                try
                {
                    // 驱动层初始化（厂商卡加载底层驱动；虚拟卡为空操作）
                    var initResult = card.Initialize(null);
                    if (!initResult.Success)
                    {
                        failedCount++;
                        if (firstErrorCode == 0)
                        {
                            firstErrorCode = initResult.Code;
                        }

                        string message = string.Format("控制卡 {0} 驱动初始化失败: {1}", label, initResult.Message);
                        reporter.Error("AppBootstrap", message, initResult.Code, cfg.CardId);
                        reporter.Alarm("AppBootstrap", AlarmCode.MotionCardInitializeFailed, AlarmLevel.Alarm, message, cfg.CardId);
                        continue;
                    }

                    // 建立物理连接（虚拟卡：内存中创建轴状态并标记已连接）
                    var connectResult = card.Connect();
                    if (!connectResult.Success)
                    {
                        failedCount++;
                        if (firstErrorCode == 0)
                        {
                            firstErrorCode = connectResult.Code;
                        }

                        string message = string.Format("控制卡 {0} 连接失败: {1}", label, connectResult.Message);
                        reporter.Error("AppBootstrap", message, connectResult.Code, cfg.CardId);
                        reporter.Alarm("AppBootstrap", AlarmCode.MotionCardInitializeFailed, AlarmLevel.Alarm, message, cfg.CardId);
                        continue;
                    }

                    successCount++;
                    reporter.Info("AppBootstrap", string.Format("控制卡 {0} 连接成功", label));
                }
                catch (Exception ex)
                {
                    failedCount++;
                    if (firstErrorCode == 0)
                    {
                        firstErrorCode = (int)MotionErrorCode.Unknown;
                    }

                    string message = string.Format("控制卡 {0} 初始化过程中发生异常", label);
                    reporter.Error("AppBootstrap", ex, message, (int)MotionErrorCode.Unknown, cfg.CardId);
                    reporter.Alarm("AppBootstrap", AlarmCode.MotionCardInitializeFailed, AlarmLevel.Alarm, message, cfg.CardId);
                }
            }

            if (failedCount > 0)
            {
                string summary = string.Format(
                    "运动控制卡初始化完成，但存在失败项。成功 {0} 张，失败 {1} 张。",
                    successCount,
                    failedCount);

                reporter.Warn("AppBootstrap", summary, firstErrorCode == 0 ? (int)MotionErrorCode.Unknown : firstErrorCode);
                return Result.Fail(
                    firstErrorCode == 0 ? (int)MotionErrorCode.Unknown : firstErrorCode,
                    summary,
                    ResultSource.Motion);
            }

            reporter.Info("AppBootstrap", string.Format("全部 {0} 张控制卡初始化完成", successCount));
            return Result.Ok("所有控制卡初始化成功");
        }

        private static void RegisterRuntimeWorkerWithTolerance(
            IRuntimeTaskManager runtimeTaskManager,
            IRuntimeWorker worker,
            bool autoStart,
            string displayName,
            AlarmCode alarmCode)
        {
            var reporter = SystemContext.Instance.Reporter;
            var registerResult = runtimeTaskManager.Register(worker, autoStart);

            if (!registerResult.Success)
            {
                string message = string.Format("{0}注册失败: {1}", displayName, registerResult.Message);
                reporter.Error("AppBootstrap", message, registerResult.Code);
                reporter.Alarm("AppBootstrap", alarmCode, AlarmLevel.Alarm, message);
                return;
            }

            if (autoStart && !worker.IsRunning)
            {
                string detail = string.IsNullOrWhiteSpace(worker.LastError)
                    ? "自动启动后未进入运行状态"
                    : worker.LastError;

                string message = string.Format("{0}自动启动失败: {1}", displayName, detail);
                reporter.Error("AppBootstrap", message, AlarmCodeToInt(alarmCode));
                reporter.Alarm("AppBootstrap", alarmCode, AlarmLevel.Alarm, message);
                return;
            }

            reporter.Info("AppBootstrap", string.Format("{0}已注册{1}", displayName, autoStart ? "并尝试自动启动" : ""));
        }

        private static int AlarmCodeToInt(AlarmCode code)
        {
            return (int)code;
        }

        private static void RegisterOptionalRuntimeWorker(
            IRuntimeTaskManager runtimeTaskManager,
            IRuntimeWorker worker,
            bool autoStart,
            string displayName)
        {
            var reporter = SystemContext.Instance.Reporter;
            var registerResult = runtimeTaskManager.Register(worker, autoStart);

            if (!registerResult.Success)
            {
                reporter.Warn("AppBootstrap", string.Format("{0}注册失败，将忽略该后台任务: {1}", displayName, registerResult.Message), registerResult.Code);
                return;
            }

            if (autoStart && !worker.IsRunning)
            {
                string detail = string.IsNullOrWhiteSpace(worker.LastError)
                    ? "自动启动后未进入运行状态"
                    : worker.LastError;

                reporter.Warn("AppBootstrap", string.Format("{0}未启动，将以静默模式跳过: {1}", displayName, detail), -1);
                return;
            }

            reporter.Info("AppBootstrap", string.Format("{0}已注册{1}", displayName, autoStart ? "并尝试自动启动" : ""));
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