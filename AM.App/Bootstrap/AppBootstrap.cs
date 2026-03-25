using AM.Core.Alarm;
using AM.Core.Context;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using AM.DBService.Services;
using AM.DBService.Services.Auth;
using AM.DBService.Services.Motion.App;
using AM.DBService.Services.Runtime;
using AM.Model.Common;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.MotionCard;
using AM.Model.Interfaces.Runtime;
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
            var config = configResult.Success ? configResult.Item : new Config();
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
            IRuntimeTaskManager runtimeTaskManager = new RuntimeTaskManager(reporter);
            SystemContext.Instance.Initialize(logger, messageBus, alarmManager, errorCatalog, reporter, runtimeTaskManager);

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
            var reloadService = new MachineConfigReloadService();
            var reloadResult = reloadService.ReloadAndRebuild();
            if (!reloadResult.Success)
            {
                SystemContext.Instance.Reporter.Error(
                    "AppBootstrap",
                    "数据库运动控制配置加载或设备上下文重建失败，应用启动终止",
                    reloadResult.Code);
                return;
            }
            SystemContext.Instance.Reporter.Info("AppBootstrap", "数据库运动控制配置加载并完成设备上下文重建");

            // 7. 注册后台工作单元
            //    Register(worker, autoStart) 统一管理注册与条件启动
            //    注册失败 → 致命错误，终止启动
            //    自动启动失败 → 内部 Warn，非致命，可后续手动启动
            var ioScanConfig = config.IoScanConfig;
            var ioScanWorker = new IoScanWorker(reporter, ioScanConfig.ScanIntervalMs);
            var registerResult = runtimeTaskManager.Register(ioScanWorker, ioScanConfig.AutoStart);
            if (!registerResult.Success)
            {
                SystemContext.Instance.Reporter.Error(
                    "AppBootstrap",
                    "IO 扫描工作单元注册失败，应用启动终止",
                    registerResult.Code);
                return;
            }

            // 8. 初始化硬件
            InitializeMachine();
        }


        /// <summary>
        /// 初始化硬件
        /// </summary>
        private static void InitializeMachine()
        {

        }

        
    }
}