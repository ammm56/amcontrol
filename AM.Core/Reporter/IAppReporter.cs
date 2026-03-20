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
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">错误码或业务码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Info(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的信息级消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">整型错误码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Info(string source, string message, int code, short? cardId = null);

        void Info(string source, string message, string code, short? cardId, ReportChannels channels);
        void Info(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出警告级消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">错误码或业务码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Warn(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的警告级消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">整型错误码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Warn(string source, string message, int code, short? cardId = null);

        //void Warn(string source, string message, string code, short? cardId, ReportChannels channels);
        //void Warn(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出错误级消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">错误码或业务码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Error(string source, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的错误级消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">整型错误码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Error(string source, string message, int code, short? cardId = null);

        //void Error(string source, string message, string code, short? cardId, ReportChannels channels);
        //void Error(string source, string message, int code, short? cardId, ReportChannels channels);

        /// <summary>
        /// 输出异常级错误消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="ex">异常对象。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">错误码或业务码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Error(string source, Exception ex, string message, string code = null, short? cardId = null);

        /// <summary>
        /// 输出带整型错误码的异常级错误消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="ex">异常对象。</param>
        /// <param name="message">消息内容。</param>
        /// <param name="code">整型错误码。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Error(string source, Exception ex, string message, int code, short? cardId = null);

        /// <summary>
        /// 输出报警。
        /// </summary>
        /// <param name="source">报警来源。</param>
        /// <param name="code">报警代码。</param>
        /// <param name="level">报警等级。</param>
        /// <param name="message">报警消息。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null);

        /// <summary>
        /// 按错误描述对象统一输出消息。
        /// </summary>
        /// <param name="source">消息来源。</param>
        /// <param name="error">错误描述对象。</param>
        /// <param name="type">消息类型。</param>
        /// <param name="ex">异常对象。</param>
        /// <param name="cardId">控制卡编号。</param>
        void Report(string source, ErrorDescriptor error, SystemMessageType type, Exception ex = null, short? cardId = null);
    }
}
