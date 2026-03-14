using AM.Core.Alarm;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Reporter
{
    public class AppReporter : IAppReporter
    {
        private readonly IMessageBus _messageBus;
        private readonly IAMLogger _logger;
        private readonly AlarmManager _alarmManager;

        public AppReporter(IMessageBus messageBus, IAMLogger logger, AlarmManager alarmManager)
        {
            _messageBus = messageBus;
            _logger = logger;
            _alarmManager = alarmManager;
        }

        public void Info(string source, string message, string code = null, short? cardId = null)
        {
            _logger?.Info(message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Status, source, code, cardId));
        }

        public void Warn(string source, string message, string code = null, short? cardId = null)
        {
            _logger?.Warn(message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Warning, source, code, cardId));
        }

        public void Error(string source, string message, string code = null, short? cardId = null)
        {
            _logger?.Error(message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Error, source, code, cardId));
        }

        public void Error(string source, System.Exception ex, string message, string code = null, short? cardId = null)
        {
            _logger?.Error(ex, message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Error, source, code, cardId));
        }

        public void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null)
        {
            _alarmManager?.RaiseAlarm(code, level, message);
            _messageBus?.Publish(new SystemMessage(message, SystemMessageType.Alarm, source, ((int)code).ToString(), cardId));
        }
    }
}
