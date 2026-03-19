using System;

namespace AM.Model.Alarm
{
    /// <summary>
    /// 活动报警信息。
    /// </summary>
    public class AlarmInfo
    {
        public AlarmCode Code { get; private set; }

        public AlarmLevel Level { get; private set; }

        public string Message { get; private set; }

        public DateTime Time { get; private set; }

        public string Source { get; private set; }

        public short? CardId { get; private set; }

        public string Description { get; private set; }

        public string Suggestion { get; private set; }

        public bool IsCleared { get; set; }

        public AlarmInfo(
            AlarmCode code,
            AlarmLevel level,
            string message,
            string source,
            short? cardId,
            string description,
            string suggestion)
        {
            Code = code;
            Level = level;
            Message = string.IsNullOrWhiteSpace(message) ? "未提供报警消息" : message;
            Source = string.IsNullOrWhiteSpace(source) ? "Alarm" : source;
            CardId = cardId;
            Description = description;
            Suggestion = suggestion;
            Time = DateTime.Now;
            IsCleared = false;
        }
    }
}