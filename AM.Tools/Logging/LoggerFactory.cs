using AM.Core.Logging;
using System;

namespace AM.Tools.Logging
{
    public static class LoggerFactory
    {
        /// <summary>
        /// 创建系统级全局日志对象。
        /// 供 AppBootstrap 使用。
        /// </summary>
        public static IAMLogger CreateSystemLogger()
        {
            return new NLogLogger("System");
        }

        /// <summary>
        /// 创建指定名称的日志对象。
        /// 仅在基础设施层使用。
        /// </summary>
        public static IAMLogger GetLogger(string name)
        {
            return new NLogLogger(name);
        }

        /// <summary>
        /// 按类型创建指定名称的日志对象。
        /// 仅在基础设施层使用。
        /// </summary>
        public static IAMLogger GetLogger<T>()
        {
            return new NLogLogger(typeof(T).FullName);
        }
    }
}
