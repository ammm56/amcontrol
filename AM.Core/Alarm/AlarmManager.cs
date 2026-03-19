using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Core.Alarm
{
    /// <summary>
    /// 报警管理器。
    /// 负责报警状态维护、报警清除和活动报警查询。
    /// 报警属于系统运行时状态，而不是普通一次性消息。
    /// </summary>
    public class AlarmManager
    {
        /// <summary>
        /// 统一状态变更事件
        /// </summary>
        public event Action AlarmStateChanged;

        /// <summary>
        /// 消息总线。
        /// 用于向 UI 和其他模块广播报警消息。
        /// </summary>
        private readonly IMessageBus _bus;

        /// <summary>
        /// 日志记录器。
        /// 用于记录报警产生过程。
        /// </summary>
        private readonly IAMLogger _logger;

        /// <summary>
        /// 报警记录持久化器。
        /// </summary>
        private readonly IAlarmRecord _alarmrecord;

        /// <summary>
        /// 当前报警集合。
        /// 保存所有曾产生过的报警，是否清除由 <see cref="AlarmInfo.IsCleared"/> 标识。
        /// </summary>
        private readonly List<AlarmInfo> _alarms = new List<AlarmInfo>();

        /// <summary>
        /// 初始化报警管理器。
        /// </summary>
        /// <param name="bus">消息总线。</param>
        /// <param name="logger">日志记录器。</param>
        /// <param name="alarmrecord">报警持久化器。</param>
        public AlarmManager(IMessageBus bus, IAMLogger logger, IAlarmRecord alarmrecord = null)
        {
            _bus = bus;
            _logger = logger;
            _alarmrecord = alarmrecord;
        }

        private void NotifyAlarmStateChanged()
        {
            var handler = AlarmStateChanged;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// 触发报警。
        /// 使用默认来源 <c>Alarm</c>，不附带控制卡编号。
        /// </summary>
        /// <param name="code">报警代码。</param>
        /// <param name="level">报警等级。</param>
        /// <param name="message">报警消息。</param>
        public void RaiseAlarm(AlarmCode code, AlarmLevel level, string message)
        {
            RaiseAlarm(code, level, message, "Alarm", null, null, null);
        }

        /// <summary>
        /// 触发报警，并携带来源和设备编号。
        /// </summary>
        /// <param name="code">报警代码。</param>
        /// <param name="level">报警等级。</param>
        /// <param name="message">报警消息。</param>
        /// <param name="source">报警来源，例如 MotionCard、DB、PLC。</param>
        /// <param name="cardId">控制卡编号，非控制卡来源可为空。</param>
        public void RaiseAlarm(AlarmCode code, AlarmLevel level, string message, string source, short? cardId)
        {
            RaiseAlarm(code, level, message, source, cardId, null, null);
        }

        /// <summary>
        /// 触发报警
        /// </summary>
        /// <param name="code"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="cardId"></param>
        /// <param name="description"></param>
        /// <param name="suggestion"></param>
        public void RaiseAlarm(AlarmCode code,AlarmLevel level,string message,string source,short? cardId,string description,string suggestion)
        {
            var finalMessage = string.IsNullOrWhiteSpace(message) ? "未提供报警消息" : message;
            var finalSource = string.IsNullOrWhiteSpace(source) ? "Alarm" : source;

            var alarm = new AlarmInfo(
                code,
                level,
                finalMessage,
                finalSource,
                cardId,
                description,
                suggestion);

            lock (_alarms)
            {
                _alarms.Add(alarm);
            }

            _logger?.Warn("Alarm " + code + " " + finalSource + " " + finalMessage);

            try
            {
                _alarmrecord?.SaveRaised(code, level, finalMessage, finalSource, cardId, alarm.Time);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Alarm record save raised failed");
            }

            _bus?.Publish(new SystemMessage(
                finalMessage,
                SystemMessageType.Alarm,
                finalSource,
                ((int)code).ToString(),
                description,
                suggestion,
                cardId));

            NotifyAlarmStateChanged();
        }

        /// <summary>
        /// 清除指定报警代码的所有未清除报警。
        /// </summary>
        public void ClearAlarm(AlarmCode code)
        {
            var hasCleared = false;

            lock (_alarms)
            {
                foreach (var alarm in _alarms)
                {
                    if (alarm.Code == code && !alarm.IsCleared)
                    {
                        alarm.IsCleared = true;
                        hasCleared = true;
                    }
                }
            }

            if (!hasCleared)
            {
                return;
            }

            try
            {
                _alarmrecord?.SaveCleared(code, DateTime.Now);
                NotifyAlarmStateChanged();
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Alarm record save cleared failed");
            }
        }

        public void ClearAlarm(AlarmInfo target)
        {
            if (target == null)
            {
                return;
            }

            var hasCleared = false;

            lock (_alarms)
            {
                var alarm = _alarms.FirstOrDefault(x =>
                    !x.IsCleared &&
                    x.Code == target.Code &&
                    x.Time == target.Time &&
                    string.Equals(x.Message, target.Message, StringComparison.Ordinal));

                if (alarm != null)
                {
                    alarm.IsCleared = true;
                    hasCleared = true;
                }
            }

            if (!hasCleared)
            {
                return;
            }

            try
            {
                _alarmrecord?.SaveCleared(target.Code, DateTime.Now);
                NotifyAlarmStateChanged();
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Alarm record save cleared failed");
            }
        }

        /// <summary>
        /// 获取当前所有未清除报警。
        /// </summary>
        /// <returns>未清除报警列表副本。</returns>
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
