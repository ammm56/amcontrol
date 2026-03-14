using System;

namespace AM.Core.Logging
{
    /// <summary>
    /// 日志记录器接口。
    /// 负责统一日志输出，供系统基础设施和业务层调用。
    /// </summary>
    public interface IAMLogger
    {
        /// <summary>
        /// 日志系统是否已完成初始化。
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 当前日志配置文件路径。
        /// </summary>
        string ConfigFilePath { get; }

        /// <summary>
        /// 当前日志文件实际写入路径。
        /// </summary>
        string CurrentLogFilePath { get; }

        /// <summary>
        /// 输出信息级日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        void Info(string message);

        /// <summary>
        /// 输出调试级日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        void Debug(string message);

        /// <summary>
        /// 输出警告级日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        void Warn(string message);

        /// <summary>
        /// 输出错误级日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        void Error(string message);

        /// <summary>
        /// 输出异常级错误日志。
        /// </summary>
        /// <param name="ex">异常对象。</param>
        /// <param name="message">日志内容。</param>
        void Error(Exception ex, string message);

        /// <summary>
        /// 立即刷新日志缓冲区。
        /// </summary>
        void Flush();

        /// <summary>
        /// 关闭日志系统并释放资源。
        /// </summary>
        void Shutdown();
    }
}
