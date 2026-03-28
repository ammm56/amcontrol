using SqlSugar;

namespace AM.Model.Entity.Plc
{
    /// <summary>
    /// PLC 批量读取块配置表。
    /// 用于显式描述高频扫描或协议受限场景下的读块规划。
    /// </summary>
    [SugarTable("plc_read_block")]
    public class PlcReadBlockConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 所属 PLC 名称。
        /// 与 plc_station.Name 关联。
        /// </summary>
        public string PlcName { get; set; }

        /// <summary>
        /// 读块名称。
        /// 在同一 PLC 下唯一。
        /// </summary>
        public string BlockName { get; set; }

        /// <summary>
        /// 地址区域。
        /// 例如：D / DB / HoldingRegister / Coil。
        /// </summary>
        public string AreaType { get; set; }

        /// <summary>
        /// 起始地址。
        /// </summary>
        public string StartAddress { get; set; }

        /// <summary>
        /// 读取长度。
        /// 语义由 ReadUnit 与 DataType 共同决定。
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 读取单位。
        /// 例如：Bit / Byte / Word。
        /// </summary>
        public string ReadUnit { get; set; }

        /// <summary>
        /// 块默认数据类型或解释方式。
        /// 例如：Int / Float / ByteArray / Mixed。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 读取模式。
        /// 例如：BatchByAddress / BatchByWordLength / BatchByByteLength。
        /// </summary>
        public string ReadMode { get; set; }

        /// <summary>
        /// 扫描优先级。
        /// 用于后续区分高频/中频/低频块。
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 描述说明。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }
    }
}