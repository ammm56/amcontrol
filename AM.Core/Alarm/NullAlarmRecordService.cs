using AM.Model.Alarm;
using System;

namespace AM.Core.Alarm
{
    /// <summary>
    /// 空报警持久化实现。
    /// 用于数据库未启用阶段。
    /// </summary>
    public class NullAlarmRecordService : IAlarmRecord
    {
        public void SaveRaised(AlarmCode code, AlarmLevel level, string message, string source, short? cardId, DateTime raisedTime)
        {
        }

        public void SaveCleared(AlarmCode code, DateTime clearedTime)
        {
        }
    }
}