using AM.Core.Alarm;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Common;
using System;

namespace AM.Core.Reporter
{
    /// <summary>
    /// 应用统一报告器。
    /// 负责将日志、消息通知、报警和错误目录解析统一编排。
    /// </summary>
    public class AppReporter : IAppReporter
    {
        /// <summary>
        /// 消息总线。
        /// </summary>
        private readonly IMessageBus _messageBus;

        /// <summary>
        /// 日志记录器。
        /// </summary>
        private readonly IAMLogger _logger;

        /// <summary>
        /// 报警管理器。
        /// </summary>
        private readonly AlarmManager _alarmManager;

        /// <summary>
        /// 错误目录。
        /// </summary>
        private readonly IErrorCatalog _errorCatalog;

        /// <summary>
        /// 初始化统一报告器。
        /// </summary>
        /// <param name="messageBus">消息总线。</param>
        /// <param name="logger">日志记录器。</param>
        /// <param name="alarmManager">报警管理器。</param>
        /// <param name="errorCatalog">错误目录。</param>
        public AppReporter(IMessageBus messageBus, IAMLogger logger, AlarmManager alarmManager, IErrorCatalog errorCatalog)
        {
            _messageBus = messageBus;
            _logger = logger;
            _alarmManager = alarmManager;
            _errorCatalog = errorCatalog;
        }

        /// <summary>
        /// 记录信息级消息。
        /// </summary>
        public void Info(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Info(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Status,
                source,
                code,
                descriptor == null ? null : descriptor.Description,
                descriptor == null ? null : descriptor.Suggestion,
                cardId));
        }

        /// <summary>
        /// 记录信息级消息。
        /// </summary>
        public void Info(string source, string message, int code, short? cardId = null)
        {
            Info(source, message, code.ToString(), cardId);
        }

        /// <summary>
        /// 记录警告级消息。
        /// </summary>
        public void Warn(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Warn(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Warning,
                source,
                code,
                descriptor == null ? null : descriptor.Description,
                descriptor == null ? null : descriptor.Suggestion,
                cardId));
        }

        /// <summary>
        /// 记录警告级消息。
        /// </summary>
        public void Warn(string source, string message, int code, short? cardId = null)
        {
            Warn(source, message, code.ToString(), cardId);
        }

        /// <summary>
        /// 记录错误级消息。
        /// </summary>
        public void Error(string source, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Error(BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Error,
                source,
                code,
                descriptor == null ? null : descriptor.Description,
                descriptor == null ? null : descriptor.Suggestion,
                cardId));
        }

        /// <summary>
        /// 记录错误级消息。
        /// </summary>
        public void Error(string source, string message, int code, short? cardId = null)
        {
            Error(source, message, code.ToString(), cardId);
        }

        /// <summary>
        /// 记录异常级错误消息。
        /// </summary>
        public void Error(string source, Exception ex, string message, string code = null, short? cardId = null)
        {
            var descriptor = Resolve(code);
            var finalMessage = MergeMessage(message, descriptor);

            _logger?.Error(ex, BuildLogMessage(source, finalMessage, descriptor));
            _messageBus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Error,
                source,
                code,
                descriptor == null ? null : descriptor.Description,
                descriptor == null ? null : descriptor.Suggestion,
                cardId));
        }

        /// <summary>
        /// 记录异常级错误消息。
        /// </summary>
        public void Error(string source, Exception ex, string message, int code, short? cardId = null)
        {
            Error(source, ex, message, code.ToString(), cardId);
        }

        /// <summary>
        /// 触发报警。
        /// </summary>
        public void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null)
        {
            var codeText = ((int)code).ToString();
            var descriptor = Resolve(codeText);
            var finalMessage = MergeMessage(message, descriptor);

            // AlarmManager 负责报警状态维护与消息发布，这里不重复发布第二次 Alarm 消息。
            if (_alarmManager != null)
            {
                _alarmManager.RaiseAlarm(
                    code,
                    level,
                    finalMessage,
                    source,
                    cardId,
                    descriptor == null ? null : descriptor.Description,
                    descriptor == null ? null : descriptor.Suggestion);
                return;
            }

            _logger?.Warn("[ALARM][" + source + "] " + finalMessage);
            _messageBus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Alarm,
                source,
                codeText,
                descriptor == null ? null : descriptor.Description,
                descriptor == null ? null : descriptor.Suggestion,
                cardId));
        }

        /// <summary>
        /// 依据错误描述对象统一输出消息。
        /// </summary>
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

        /// <summary>
        /// 依据错误码解析错误目录。
        /// </summary>
        private ErrorDescriptor Resolve(string code)
        {
            int intCode;
            if (string.IsNullOrWhiteSpace(code)) return null;
            if (!int.TryParse(code, out intCode)) return null;
            if (_errorCatalog == null) return null;

            return _errorCatalog.Get(intCode);
        }

        /// <summary>
        /// 合并运行时消息与错误目录默认消息。
        /// </summary>
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

        /// <summary>
        /// 生成用于日志输出的完整文本。
        /// </summary>
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
