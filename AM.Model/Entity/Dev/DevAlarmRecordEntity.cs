using SqlSugar;
using System;

namespace AM.Model.Entity.Dev
{
    /// <summary>
    /// 报警持久化记录实体。
    /// 对应数据库表 dev_alarm_record。
    /// 
    /// 可空列必须显式标注 IsNullable = true，SqlSugar + SQLite 不会从 C# 可空类型自动推断。
    /// </summary>
    [SugarTable("dev_alarm_record")]
    public class DevAlarmRecordEntity
    {
        /// <summary>自增主键。</summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>报警码整型值（AlarmCode 枚举）。</summary>
        public int AlarmCode { get; set; }

        /// <summary>报警级别文本：Critical / Error / Warning / Info。</summary>
        [SugarColumn(IsNullable = true)]
        public string AlarmLevel { get; set; }

        /// <summary>报警消息正文。</summary>
        [SugarColumn(IsNullable = true)]
        public string Message { get; set; }

        /// <summary>触发来源（服务名或模块名）。</summary>
        [SugarColumn(IsNullable = true)]
        public string Source { get; set; }

        /// <summary>关联控制卡编号，非运动类报警为 null。</summary>
        [SugarColumn(IsNullable = true)]
        public short? CardId { get; set; }

        /// <summary>报警触发时间。</summary>
        public DateTime RaisedTime { get; set; }

        /// <summary>报警清除时间，未清除时为 null。</summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ClearedTime { get; set; }

        /// <summary>是否已清除。</summary>
        public bool IsCleared { get; set; }
    }
}
