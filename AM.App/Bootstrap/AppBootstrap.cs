using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.DBService.Services;
using AM.DBService.Services.Auth;
using AM.Model.Common;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.MotionService.Factory;
using AM.MotionService.Hub;
using AM.Tools.Logging;
using AM.Tools.Messaging;
using AM.Tools.Reporter;

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
        /// </summary>
        public static void Initialize()
        {
            // 1. 初始化基础配置上下文（仅基础参数，不含设备拓扑）
            var configResult = AM.Tools.Tools.ReadConfig<Config>("config.json");
            var config = configResult.Item1 ? configResult.Item2 : new Config();
            ConfigContext.Instance.Initialize(config);

            // 2. 构造系统基础设施
            IAMLogger logger = new NLogLogger("System");
            IMessageBus messageBus = new MessageBusToolkit();
            IErrorCatalog errorCatalog = new JsonErrorCatalog();

            // 数据库未启用时先不记录报警
            IAlarmRecord alarmRecord = new NullAlarmRecordService();
            // IAlarmRecord alarmRecord = new AlarmRecordService();

            AlarmManager alarmManager = new AlarmManager(messageBus, logger, alarmRecord);
            IAppReporter reporter = new AppReporter(messageBus, logger, alarmManager, errorCatalog);

            // 3. 初始化系统上下文
            SystemContext.Instance.Initialize(logger, messageBus, alarmManager, errorCatalog, reporter);

            // 4. 初始化认证相关表与默认管理员
            var authSeedService = new AuthSeedService(reporter);
            authSeedService.EnsureSeedData();

            // 5. 初始化运动配置种子
            var machineConfigSeedService = new MachineConfigSeedService(reporter);
            var seedResult = machineConfigSeedService.EnsureSeedData();
            if (!seedResult.Success)
            {
                SystemContext.Instance.Reporter.Error(
                    "AppBootstrap",
                    "默认运动配置种子初始化失败，应用启动终止",
                    seedResult.Code);
                return;
            }

            // 6. 从数据库加载完整设备配置
            var machineConfigResult = LoadMachineConfigFromDatabase();
            if (!machineConfigResult.Success)
            {
                return;
            }

            // 7. 初始化硬件
            InitializeMachine();
        }
        /// <summary>
        /// 从数据库加载完整设备配置到运行时上下文。
        /// </summary>
        private static Result LoadMachineConfigFromDatabase()
        {
            IMachineConfigAppService machineConfigAppService = new MachineConfigAppService();

            var ensureResult = machineConfigAppService.EnsureTables();
            if (!ensureResult.Success)
            {
                SystemContext.Instance.Reporter.Error(
                    "AppBootstrap",
                    "运动控制数据库表初始化失败，应用启动终止",
                    ensureResult.Code);
                return ensureResult;
            }

            var reloadResult = machineConfigAppService.ReloadFromDatabase();
            if (!reloadResult.Success)
            {
                SystemContext.Instance.Reporter.Error(
                    "AppBootstrap",
                    "数据库运动控制配置加载失败，应用启动终止",
                    reloadResult.Code);
                return reloadResult;
            }

            SystemContext.Instance.Reporter.Info("AppBootstrap", "数据库运动控制配置加载成功");
            return reloadResult;
        }

        /// <summary>
        /// 初始化运动控制硬件。
        /// </summary>
        private static void InitializeMachine()
        {
            var machine = MachineContext.Instance;
            machine.MotionCards.Clear();
            machine.AxisMotionCards.Clear();
            machine.DICards.Clear();
            machine.DOCards.Clear();

            var cardConfigs = ConfigContext.Instance.Config.MotionCardsConfig;
            if (cardConfigs == null || cardConfigs.Count == 0)
            {
                SystemContext.Instance.Reporter.Warn("AppBootstrap", "未配置任何运动控制卡");
                machine.MotionHub = new MotionServiceHub();
                return;
            }

            foreach (var motionCfg in cardConfigs)
            {
                RegisterMotionCard(machine, motionCfg);
            }

            machine.MotionHub = new MotionServiceHub();
        }

        /// <summary>
        /// 注册单张运动控制卡及其轴、DI、DO 映射。
        /// </summary>
        private static void RegisterMotionCard(MachineContext machine, MotionCardConfig motionCfg)
        {
            if (motionCfg == null)
            {
                SystemContext.Instance.Reporter.Warn("AppBootstrap", "存在空的运动控制卡配置，已跳过");
                return;
            }

            if (machine.MotionCards.ContainsKey(motionCfg.CardId))
            {
                SystemContext.Instance.Reporter.Error("AppBootstrap", "控制卡 CardId 重复: " + motionCfg.CardId);
                return;
            }

            IMotionCardService motion = MotionCardFactory.Create(motionCfg);
            motion.LoadAxisConfig(motionCfg.AxisConfigs);

            machine.MotionCards[motionCfg.CardId] = motion;

            RegisterAxisMappings(machine, motion, motionCfg);
            RegisterDIMappings(machine, motion, motionCfg);
            RegisterDOMappings(machine, motion, motionCfg);
        }

        /// <summary>
        /// 注册逻辑轴映射。
        /// </summary>
        private static void RegisterAxisMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.AxisConfigs == null)
            {
                return;
            }

            foreach (var axisCfg in motionCfg.AxisConfigs)
            {
                if (machine.AxisMotionCards.ContainsKey(axisCfg.LogicalAxis))
                {
                    SystemContext.Instance.Reporter.Error("AppBootstrap", "逻辑轴重复映射: " + axisCfg.LogicalAxis);
                    continue;
                }

                machine.AxisMotionCards[axisCfg.LogicalAxis] = motion;
            }
        }

        /// <summary>
        /// 注册逻辑 DI 映射。
        /// </summary>
        private static void RegisterDIMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.DIBitMaps == null)
            {
                return;
            }

            foreach (var di in motionCfg.DIBitMaps)
            {
                if (machine.DICards.ContainsKey(di.LogicalBit))
                {
                    SystemContext.Instance.Reporter.Error("AppBootstrap", "逻辑DI重复映射: " + di.LogicalBit);
                    continue;
                }

                machine.DICards[di.LogicalBit] = motion;
            }
        }

        /// <summary>
        /// 注册逻辑 DO 映射。
        /// </summary>
        private static void RegisterDOMappings(MachineContext machine, IMotionCardService motion, MotionCardConfig motionCfg)
        {
            if (motionCfg.DOBitMaps == null)
            {
                return;
            }

            foreach (var dob in motionCfg.DOBitMaps)
            {
                if (machine.DOCards.ContainsKey(dob.LogicalBit))
                {
                    SystemContext.Instance.Reporter.Error("AppBootstrap", "逻辑DO重复映射: " + dob.LogicalBit);
                    continue;
                }

                machine.DOCards[dob.LogicalBit] = motion;
            }
        }
    }
}