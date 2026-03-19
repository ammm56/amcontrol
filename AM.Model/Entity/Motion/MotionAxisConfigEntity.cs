using SqlSugar;

namespace AM.Model.Entity.Motion
{
    /// <summary>
    /// 运动轴参数表。
    /// 按“单轴 + 参数名 + 数值 + 类型标记”的方式保存。
    /// </summary>
    [SugarTable("motion_axis_config")]
    public class MotionAxisConfigEntity
    {
        public MotionAxisConfigEntity()
        {
            ParamValueType = "Double";
            ParamSetValue = 0D;
            ParamDefaultValue = 0D;
            ParamMaxValue = 0D;
            ParamMinValue = 0D;
            Status1 = 0;
            Status2 = 0;
            Status3 = 0;
        }

        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 逻辑轴号。
        /// </summary>
        public int LogicalAxis { get; set; }

        /// <summary>
        /// 参数名。
        /// 与 AxisConfig 属性名一致。
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数显示名。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ParamDisplayName { get; set; }

        /// <summary>
        /// 参数值类型。
        /// Bool / Int16 / Int32 / Double / Enum
        /// </summary>
        public string ParamValueType { get; set; }

        /// <summary>
        /// 当前设置值。
        /// </summary>
        public double ParamSetValue { get; set; }

        /// <summary>
        /// 默认值。
        /// </summary>
        public double ParamDefaultValue { get; set; }

        /// <summary>
        /// 最大值。
        /// </summary>
        public double ParamMaxValue { get; set; }

        /// <summary>
        /// 最小值。
        /// </summary>
        public double ParamMinValue { get; set; }

        /// <summary>
        /// 状态位1。
        /// </summary>
        public int Status1 { get; set; }

        /// <summary>
        /// 状态位2。
        /// </summary>
        public int Status2 { get; set; }

        /// <summary>
        /// 状态位3。
        /// </summary>
        public int Status3 { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 轴显示名。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string AxisDisplayName { get; set; }

        /// <summary>
        /// 扩展字段1。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Extension1 { get; set; }

        /// <summary>
        /// 扩展字段2。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Extension2 { get; set; }

        /// <summary>
        /// 扩展字段3。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Extension3 { get; set; }
    }
}