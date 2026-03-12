using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Alarm
{
    public class AlarmManager
    {
        private readonly IMessageBus _bus;
        private readonly IAMLogger _logger;

        private readonly List<AlarmInfo> _alarms = new List<AlarmInfo>();

        public AlarmManager(IMessageBus bus, IAMLogger logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public void RaiseAlarm(AlarmCode code,
                               AlarmLevel level,
                               string message)
        {
            var alarm = new AlarmInfo(code, level, message);

            lock (_alarms)
            {
                _alarms.Add(alarm);
            }

            _logger.Warn($"Alarm {code} {message}");

            _bus.Publish(new SystemMessage(
                message,
                SystemMessageType.Alarm));
        }

        public void ClearAlarm(AlarmCode code)
        {
            lock (_alarms)
            {
                foreach (var alarm in _alarms)
                {
                    if (alarm.Code == code && !alarm.IsCleared)
                    {
                        alarm.IsCleared = true;
                    }
                }
            }
        }

        public List<AlarmInfo> GetActiveAlarms()
        {
            lock (_alarms)
            {
                return _alarms
                    .Where(a => !a.IsCleared)
                    .ToList();
            }
        }
    }
}
