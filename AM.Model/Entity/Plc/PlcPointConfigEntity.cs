using SqlSugar;

namespace AM.Model.Entity.Plc
{
    /// <summary>
    /// PLC 点位配置表。
    /// 当前版本采用最简模型：
    /// - 直接使用 Address 保存完整协议地址；
    /// - 不再拆分 AreaType、BitIndex；
    /// - 不在点位主表中承载缩放、偏移、批量规划等处理逻辑。
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
        /// 完整地址文本。
        /// 直接保存协议可识别地址，例如：
        /// - Modbus: 00001 / 10001 / 30001 / 40001
        /// - S7: DB1.0 / DB1.20 / M10.0
        /// - MC: D200 / M100 / X0 / Y10
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 数据类型。
        /// 例如：Bool / Short / UShort / Int / UInt / Float / Double / String / ByteArray。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字符串长度。
        /// DataType=String 时使用。
        /// </summary>
        public int StringLength { get; set; }

        /// <summary>
        /// 数组长度。
        /// DataType=ByteArray 时使用。
        /// </summary>
        public int ArrayLength { get; set; }

        /// <summary>
        /// 单位。
        /// 仅用于显示，不参与协议读写。
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
        /// 当前阶段推荐统一使用 Single。
        /// </summary>
        public string ReadMode { get; set; }

        /// <summary>
        /// 字符串编码。
        /// 例如：ASCII / UTF8 / Unicode。
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