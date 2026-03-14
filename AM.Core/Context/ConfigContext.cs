using AM.Model.Common;

namespace AM.Core.Context
{
    /// <summary>
    /// 配置上下文。
    /// 负责保存系统当前加载的全局配置对象。
    /// </summary>
    public sealed class ConfigContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static ConfigContext Instance { get; } = new ConfigContext();

        /// <summary>
        /// 私有构造，禁止外部实例化。
        /// </summary>
        private ConfigContext()
        {
        }

        /// <summary>
        /// 当前全局配置对象。
        /// </summary>
        public Config Config { get; private set; }

        /// <summary>
        /// 初始化配置上下文。
        /// </summary>
        /// <param name="config">配置对象。</param>
        public void Initialize(Config config)
        {
            Config = config ?? new Config();
        }
    }
}
