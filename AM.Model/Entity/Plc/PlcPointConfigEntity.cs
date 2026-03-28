using SqlSugar;

namespace AM.Model.Entity.Plc
{
    /// <summary>
    /// PLC 点位配置表。
    /// 描述逻辑点位、地址、数据类型、长度、读写权限与批量读取归属。
    /// </summary>
    [SugarTable("plc_point")]
    public class PlcPointConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 所属 PLC 站名称。
        /// 与 plc_station.Name 关联。
        /// </summary>
        public string PlcName { get; set; }

        /// <summary>
        /// 内部唯一点位名。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 点位分组。
        /// 用于 UI 分类与业务归组。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string GroupName { get; set; }

        /// <summary>
        /// 地址区域。
        /// 例如：X / Y / M / D / DB / Coil / HoldingRegister。
        /// </summary>
        public string AreaType { get; set; }

        /// <summary>
        /// 地址文本。
        /// 例如：100 / DB1.0 / D200 / 40001。
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 位索引。
        /// 字地址内位访问时使用，可空。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? BitIndex { get; set; }

        /// <summary>
        /// 数据类型。
        /// 例如：Bit / Int / Float / String / ByteArray。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字符串长度。
        /// DataType=String 时使用。
        /// </summary>
        public int StringLength { get; set; }

        /// <summary>
        /// 数组长度。
        /// DataType 为数组、块或 ByteArray 时使用。
        /// </summary>
        public int ArrayLength { get; set; }

        /// <summary>
        /// 读取长度。
        /// 用于明确单次读取长度，便于批量块规划。
        /// </summary>
        public int ReadLength { get; set; }

        /// <summary>
        /// 缩放系数。
        /// 原始值 * Scale + Offset = 显示值。
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// 偏移量。
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// 单位。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Unit { get; set; }

        /// <summary>
        /// 访问模式。
        /// ReadOnly / ReadWrite / WriteOnly。
        /// </summary>
        public string AccessMode { get; set; }

        /// <summary>
        /// 读取策略。
        /// Single / BatchByAddress / BatchByDataType / BatchByWordLength / BatchByByteLength。
        /// </summary>
        public string ReadMode { get; set; }

        /// <summary>
        /// 批量读取分组键。
        /// 相同 BatchKey 的点位可优先归并为同一读块。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string BatchKey { get; set; }

        /// <summary>
        /// 字节序。
        /// 例如：LittleEndian / BigEndian。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ByteOrder { get; set; }

        /// <summary>
        /// 字序。
        /// 例如：LowHigh / HighLow。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string WordOrder { get; set; }

        /// <summary>
        /// 字符串编码。
        /// 例如：Ascii / Utf8 / Unicode。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string StringEncoding { get; set; }

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