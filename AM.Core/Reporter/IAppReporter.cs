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
        void Info(string source, string message, string code = null, short? cardId = null);
        void Info(string source, string message, int code, short? cardId = null);
        void Info(string source, string message, string code, short? cardId, ReportChannels channels);
        void Info(string source, string message, int code, short? cardId, ReportChannels channels);

        void Warn(string source, string message, string code = null, short? cardId = null);
        void Warn(string source, string message, int code, short? cardId = null);
        void Warn(string source, string message, string code, short? cardId, ReportChannels channels);
        void Warn(string source, string message, int code, short? cardId, ReportChannels channels);

        void Error(string source, string message, string code = null, short? cardId = null);
        void Error(string source, string message, int code, short? cardId = null);
        void Error(string source, string message, string code, short? cardId, ReportChannels channels);
        void Error(string source, string message, int code, short? cardId, ReportChannels channels);

        void Error(string source, Exception ex, string message, string code = null, short? cardId = null);
        void Error(string source, Exception ex, string message, int code, short? cardId = null);
        void Error(string source, Exception ex, string message, string code, short? cardId, ReportChannels channels);
        void Error(string source, Exception ex, string message, int code, short? cardId, ReportChannels channels);

        void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null);

        void Report(string source, ErrorDescriptor error, SystemMessageType type, Exception ex = null, short? cardId = null);
    }
}