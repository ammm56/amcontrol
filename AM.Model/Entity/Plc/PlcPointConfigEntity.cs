using SqlSugar;

namespace AM.Model.Entity.Plc
{
    /// <summary>
    /// PLC 点位配置表。
    /// 当前版本采用最简模型：
    /// - 直接使用 Address 保存完整协议地址；
    /// - 长度统一使用 Length；
    /// - 不在点位主表中承载批量规划与显示扩展字段。
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
        /// - Modbus: 00001 / 40001 / 40040
        /// - S7: DB1.0 / DB1.20 / M10.0
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 数据类型。
        /// 统一使用字符串表示，例如：
        /// bool、uint8、int8、uint16、int16、uint32、int32、uint64、int64、float、double、string。
        /// 数组类型使用 type[] 表示。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 长度。
        /// - 标量：1
        /// - 字符串：字符长度
        /// - 数组：元素个数
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 访问模式。
        /// ReadOnly / ReadWrite / WriteOnly。
        /// </summary>
        public string AccessMode { get; set; }

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