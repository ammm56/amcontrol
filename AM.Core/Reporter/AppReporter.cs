using AM.Core.Alarm;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Common;
using System;

namespace AM.Core.Reporter
{
    public class AppReporter : IAppReporter
    {
        private readonly IMessageBus _messageBus;
        private readonly IAMLogger _logger;
        private readonly AlarmManager _alarmManager;
        private readonly IErrorCatalog _errorCatalog;

        public AppReporter(IMessageBus messageBus, IAMLogger logger, AlarmManager alarmManager, IErrorCatalog errorCatalog)
        {
            _messageBus = messageBus;
            _logger = logger;
            _alarmManager = alarmManager;
            _errorCatalog = errorCatalog;
        }

        public void Info(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Info(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(finalMessage, SystemMessageType.Status, source, code, cardId));
        }

        public void Info(string source, string message, int code, short? cardId = null)
        {
            Info(source, message, code.ToString(), cardId);
        }

        public void Warn(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Warn(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(finalMessage, SystemMessageType.Warning, source, code, cardId));
        }

        public void Warn(string source, string message, int code, short? cardId = null)
        {
            Warn(source, message, code.ToString(), cardId);
        }

        public void Error(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Error(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(finalMessage, SystemMessageType.Error, source, code, cardId));
        }

        public void Error(string source, string message, int code, short? cardId = null)
        {
            Error(source, message, code.ToString(), cardId);
        }

        public void Error(string source, Exception ex, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Error(ex, BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(finalMessage, SystemMessageType.Error, source, code, cardId));
        }

        public void Error(string source, Exception ex, string message, int code, short? cardId = null)
        {
            Error(source, ex, message, code.ToString(), cardId);
        }

        public void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null)
        {
            // AlarmManager 内部已经负责日志与 Alarm 消消息发布，这里不要重复发第二次
            if (_alarmManager != null)
            {
                _alarmManager.RaiseAlarm(code, level, message);
                return;
            }

            _logger?.Warn("[ALARM][" + source + "] " + message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Alarm, source, ((int)code).ToString(), cardId));
        }

        public void Report(string source, ErrorDescriptor error, SystemMessageType type, Exception ex = null, short? cardId = null)
        {
            if (error == null)
            {
                Error(source, ex, "未提供错误描述对象", null, cardId);
                return;
            }

            var message = MergeMessage(error.Message, error);

            if (ex == null)
            {
                switch (type)
                {
                    case SystemMessageType.Status:
                        Info(source, message, error.Code, cardId);
                        break;
                    case SystemMessageType.Warning:
                        Warn(source, message, error.Code, cardId);
                        break;
                    case SystemMessageType.Error:
                        Error(source, message, error.Code, cardId);
                        break;
                    case SystemMessageType.Alarm:
                        Alarm(source, AlarmCode.Unknown, AlarmLevel.Warning, message, cardId);
                        break;
                    default:
                        Info(source, message, error.Code, cardId);
                        break;
                }
            }
            else
            {
                Error(source, ex, message, error.Code, cardId);
            }
        }

        private ErrorDescriptor Resolve(string code)
        {
            int intCode;
            if (string.IsNullOrWhiteSpace(code)) return null;
            if (!int.TryParse(code, out intCode)) return null;
            if (_errorCatalog == null) return null;

            return _errorCatalog.Get(intCode);
        }

        private static string MergeMessage(string message, ErrorDescriptor descriptor)
        {
            if (descriptor == null)
                return message ?? string.Empty;

            if (string.IsNullOrWhiteSpace(message))
                return descriptor.Message ?? string.Empty;

            if (string.Equals(message, descriptor.Message, StringComparison.OrdinalIgnoreCase))
                return message;

            if (string.IsNullOrWhiteSpace(descriptor.Message))
                return message;

            return message + " | " + descriptor.Message;
        }

        private static string BuildLogMessage(string source, string message, ErrorDescriptor descriptor)
        {
            if (descriptor == null)
                return "[" + source + "] " + message;

            return "[" + source + "] "
                + message
                + " | Code=" + descriptor.Code
                + " | Name=" + (descriptor.Name ?? string.Empty)
                + " | Desc=" + (descriptor.Description ?? string.Empty)
                + " | Suggestion=" + (descriptor.Suggestion ?? string.Empty);
        }
    }
}
