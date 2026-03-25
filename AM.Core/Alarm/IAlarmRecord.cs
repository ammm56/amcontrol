using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Entity.Dev;
using System;

namespace AM.Core.Alarm
{
    //// <summary>
    /// 报警持久化接口。
    /// 负责将报警发生与清除记录写入长期存储。
    /// </summary>
    public interface IAlarmRecord
    {
        /// <summary>
        /// 保存报警发生记录。
        /// </summary>
        void SaveRaised(AlarmCode code, AlarmLevel level, string message, string source, short? cardId, DateTime raisedTime, string description, string suggestion);

        /// <summary>
        /// 保存报警清除记录。
        /// 当前实现按报警码批量清除所有未清除记录。
        /// </summary>
        void SaveCleared(AlarmCode code, DateTime clearedTime);
    }

    /// <summary>
    /// 设备报警持久化与查询接口。
    /// 继承 IAlarmRecord（写入语义），扩展分页查询能力供 UI 历史页使用。
    /// </summary>
    public interface IDevAlarmRecord : IAlarmRecord
    {
        /// <summary>
        /// 分页查询报警记录，按触发时间倒序。
        /// </summary>
        /// <param name="page">页码（从 1 开始）。</param>
        /// <param name="pageSize">每页条数。</param>
        /// <param name="levelFilter">报警级别过滤，null 为全部（"Critical"/"Error"/"Warning"/"Info"）。</param>
        /// <param name="isCleared">是否已清除，null 为全部。</param>
        /// <param name="from">起始时间，null 为不限。</param>
        /// <param name="to">截止时间，null 为不限。</param>
        Result<DevAlarmRecordEntity> QueryPage(
            int page,
            int pageSize,
            string levelFilter = null,
            bool? isCleared = null,
            DateTime? from = null,
            DateTime? to = null);

        /// <summary>
        /// 查询满足过滤条件的报警记录总数，供分页控件使用。
        /// </summary>
        int QueryTotalCount(
            string levelFilter = null,
            bool? isCleared = null,
            DateTime? from = null,
            DateTime? to = null);

        /// <summary>
        /// 按主键查询单条报警记录（行选中后显示详情）。
        /// </summary>
        Result<DevAlarmRecordEntity> QueryById(int id);
    }
}