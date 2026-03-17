using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.DBService.Services;
using AM.DBService.Services.Auth;
using AM.Model.Common;
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
            // 1. 初始化配置上下文
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

            // 5. 用数据库参数覆盖配置文件中的轴参数
            ApplyAxisConfigOverlay(config);

            // 6. 初始化硬件
            InitializeMachine();
        }

        /// <summary>
        /// 使用数据库参数覆盖配置文件中的轴配置。
        /// </summary>
        private static void ApplyAxisConfigOverlay(Config config)
        {
            if (config == null || config.MotionCardsConfig == null || config.MotionCardsConfig.Count == 0)
            {
                SystemContext.Instance.Reporter?.Warn("AppBootstrap", "未找到可覆盖的运动控制卡轴配置");
                return;
            }

            var axisConfigOverlayService = new AxisConfigOverlayService();
            var overlayResult = axisConfigOverlayService.ApplyToMotionCards(config.MotionCardsConfig);

            if (!overlayResult.Success)
            {
                SystemContext.Instance.Reporter?.Error(
                    "AppBootstrap",
                    "数据库轴参数覆盖失败，将继续使用 config.json 中的轴参数配置",
                    overlayResult.Code);
                return;
            }

            SystemContext.Instance.Reporter?.Info("AppBootstrap", "数据库轴参数覆盖成功");
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
                SystemContext.Instance.Reporter?.Warn("AppBootstrap", "未配置任何运动控制卡");
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
                SystemContext.Instance.Reporter?.Warn("AppBootstrap", "存在空的运动控制卡配置，已跳过");
                return;
            }

            if (machine.MotionCards.ContainsKey(motionCfg.CardId))
            {
                SystemContext.Instance.Reporter?.Error("AppBootstrap", "控制卡 CardId 重复: " + motionCfg.CardId);
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
            if (motionCfg.AxisConfigs == null) return;

            foreach (var axisCfg in motionCfg.AxisConfigs)
            {
                if (machine.AxisMotionCards.ContainsKey(axisCfg.LogicalAxis))
                {
                    SystemContext.Instance.Reporter?.Error("AppBootstrap", "逻辑轴重复映射: " + axisCfg.LogicalAxis);
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
            if (motionCfg.DIBitMaps == null) return;

            foreach (var di in motionCfg.DIBitMaps)
            {
                if (machine.DICards.ContainsKey(di.LogicalBit))
                {
                    SystemContext.Instance.Reporter?.Error("AppBootstrap", "逻辑DI重复映射: " + di.LogicalBit);
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
            if (motionCfg.DOBitMaps == null) return;

            foreach (var dob in motionCfg.DOBitMaps)
            {
                if (machine.DOCards.ContainsKey(dob.LogicalBit))
                {
                    SystemContext.Instance.Reporter?.Error("AppBootstrap", "逻辑DO重复映射: " + dob.LogicalBit);
                    continue;
                }

                machine.DOCards[dob.LogicalBit] = motion;
            }
        }
    }
}