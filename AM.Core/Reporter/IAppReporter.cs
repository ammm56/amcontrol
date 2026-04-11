using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Common;
using System;

namespace AM.Core.Reporter
{
    /// <summary>
    /// 应用统一报告接口。
    /// 负责统一封装日志记录、消息通知和报警输出。
    /// </summary>
    public interface IAppReporter
    {
        /// <summary>
        /// 输出信息级消息。
        /// </summary>
        void Info(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的信息级消息。
        /// </summary>
        void Info(string source, string message, int code, short? cardId = null);

        /// <summary>
        /// 输出信息级消息，并指定输出渠道。
        /// </summary>
        void Info(string source, string message, string code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出带整型错误码的信息级消息，并指定输出渠道。
        /// </summary>
        void Info(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出警告级消息。
        /// </summary>
        void Warn(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的警告级消息。
        /// </summary>
        void Warn(string source, string message, int code, short? cardId = null);

        /// <summary>
        /// 输出警告级消息，并指定输出渠道。
        /// </summary>
        void Warn(string source, string message, string code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出带整型错误码的警告级消息，并指定输出渠道。
        /// </summary>
        void Warn(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出错误级消息。
        /// </summary>
        void Error(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的错误级消息。
        /// </summary>
        void Error(string source, string message, int code, short? cardId = null);

        /// <summary>
        /// 输出错误级消息，并指定输出渠道。
        /// </summary>
        void Error(string source, string message, string code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出带整型错误码的错误级消息，并指定输出渠道。
        /// </summary>
        void Error(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出异常级错误消息。
        /// </summary>
        void Error(string source, Exception ex, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的异常级错误消息。
        /// </summary>
        void Error(string source, Exception ex, string message, int code, short? cardId = null);

        /// <summary>
        /// 输出异常级错误消息，并指定输出渠道。
        /// </summary>
        void Error(string source, Exception ex, string message, string code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出带整型错误码的异常级错误消息，并指定输出渠道。
        /// </summary>
        void Error(string source, Exception ex, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出报警。
        /// </summary>
        void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null);

        /// <summary>
        /// 按错误描述对象统一输出消息。
        /// </summary>
        void Report(string source, ErrorDescriptor error, SystemMessageType type, Exception ex = null, short? cardId = null);
    }
}