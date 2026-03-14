using AM.Core.Alarm;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;

namespace AM.Core.Context
{
    /// <summary>
    /// 系统运行时上下文。
    /// 负责集中保存全局基础设施对象，供业务服务、工具类、视图模型统一访问。
    /// </summary>
    public sealed class SystemContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static SystemContext Instance { get; } = new SystemContext();

        /// <summary>
        /// 私有构造，禁止外部实例化。
        /// </summary>
        private SystemContext()
        {
        }

        /// <summary>
        /// 日志记录器。
        /// </summary>
        public IAMLogger Logger { get; private set; }

        /// <summary>
        /// 系统消息总线。
        /// </summary>
        public IMessageBus MessageBus { get; private set; }

        /// <summary>
        /// 报警管理器。
        /// </summary>
        public AlarmManager AlarmManager { get; private set; }

        /// <summary>
        /// 错误码目录。
        /// </summary>
        public IErrorCatalog ErrorCatalog { get; private set; }

        /// <summary>
        /// 统一报告器。
        /// </summary>
        public IAppReporter Reporter { get; private set; }

        /// <summary>
        /// 初始化系统上下文。
        /// 注意：所有实例由组合根统一创建并注入，避免重复构造导致状态不一致。
        /// </summary>
        /// <param name="logger">日志记录器。</param>
        /// <param name="bus">消息总线。</param>
        /// <param name="alarmManager">报警管理器。</param>
        /// <param name="errorCatalog">错误目录。</param>
        /// <param name="reporter">统一报告器。</param>
        public void Initialize(
            IAMLogger logger,
            IMessageBus bus,
            AlarmManager alarmManager,
            IErrorCatalog errorCatalog,
            IAppReporter reporter)
        {
            Logger = logger;
            MessageBus = bus;
            AlarmManager = alarmManager;
            ErrorCatalog = errorCatalog;
            Reporter = reporter;
        }
    }
}