using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Entity.Dev;
using System;
using System.Collections.Generic;

namespace AM.Core.Alarm
{
    /// <summary>
    /// 空报警持久化实现。
    /// 用于单元测试或数据库未启用阶段，所有操作为空操作。
    /// </summary>
    public class NullAlarmRecordService : IDevAlarmRecord
    {
        public void SaveRaised(AlarmCode code, AlarmLevel level, string message, string source, short? cardId, DateTime raisedTime, string description, string suggestion)
        {
        }

        public void SaveCleared(AlarmCode code, DateTime clearedTime)
        {
        }

        public Result<DevAlarmRecordEntity> QueryPage(
            int page, int pageSize,
            string levelFilter = null, bool? isCleared = null,
            DateTime? from = null, DateTime? to = null)
        {
            return Result<DevAlarmRecordEntity>.OkList(new List<DevAlarmRecordEntity>())
                .WithNotifyMode(ResultNotifyMode.Silent);
        }

        public int QueryTotalCount(
            string levelFilter = null, bool? isCleared = null,
            DateTime? from = null, DateTime? to = null)
        {
            return 0;
        }

        public Result<DevAlarmRecordEntity> QueryById(int id)
        {
            return Result<DevAlarmRecordEntity>.Fail(-1, "空实现，无数据");
        }
    }
}