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
    }
}
