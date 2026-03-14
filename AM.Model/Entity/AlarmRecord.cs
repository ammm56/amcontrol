using SqlSugar;
using System;

namespace AM.Model.Entity
{
    /// <summary>
    /// 报警历史记录。
    /// </summary>
    [SugarTable("alarmrecord")]
    public class AlarmRecord
    {
        /// <summary>
        /// 自增主键 ID。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 报警码。
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 报警等级。
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 报警来源。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Source { get; set; }

        /// <summary>
        /// 报警消息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// 控制卡编号。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? CardId { get; set; }

        /// <summary>
        /// 发生时间。
        /// </summary>
        public DateTime RaisedTime { get; set; }

        /// <summary>
        /// 是否已清除。
        /// </summary>
        public bool IsCleared { get; set; }

        /// <summary>
        /// 清除时间。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ClearedTime { get; set; }
    }
}