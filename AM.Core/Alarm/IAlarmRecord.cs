using AM.Model.Alarm;
using System;

namespace AM.Core.Alarm
{
    /// <summary>
    /// 报警持久化接口。
    /// 负责将报警发生与清除记录写入长期存储。
    /// </summary>
    public interface IAlarmRecord
    {
        /// <summary>
        /// 保存报警发生记录。
        /// </summary>
        void SaveRaised(AlarmCode code, AlarmLevel level, string message, string source, short? cardId, DateTime raisedTime);

        /// <summary>
        /// 保存报警清除记录。
        /// 当前实现按报警码批量清除所有未清除记录。
        /// </summary>
        void SaveCleared(AlarmCode code, DateTime clearedTime);
    }
}