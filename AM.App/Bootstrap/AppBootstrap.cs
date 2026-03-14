using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.MotionService.Factory;
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
            AlarmManager alarmManager = new AlarmManager(messageBus, logger);
            IErrorCatalog errorCatalog = new JsonErrorCatalog();
            IAppReporter reporter = new AppReporter(messageBus, logger, alarmManager, errorCatalog);

            // 3. 初始化系统上下文
            SystemContext.Instance.Initialize(logger, messageBus, alarmManager, errorCatalog, reporter);

            // 4. 初始化硬件
            InitializeMachine();
        }

        /// <summary>
        /// 初始化运动控制硬件。
        /// </summary>
        private static void InitializeMachine()
        {
            var motionCfg = ConfigContext.Instance.Config.MotionCardConfig;
            var motion = MotionCardFactory.Create(motionCfg);

            // 启动阶段统一加载轴映射配置
            motion.LoadAxisConfig(motionCfg.AxisConfigs);

            MachineContext.Instance.MotionCard = motion;
        }
    }
}