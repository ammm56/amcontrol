using SqlSugar;

namespace AM.Model.Entity
{
    /// <summary>
    /// 轴参数配置实体类。
    /// 采用数值统一 REAL + 类型标记的长期方案。
    /// </summary>
    [SugarTable("configaxisarg")]
    public partial class ConfigAxisArg
    {
        /// <summary>
        /// 初始化默认值。
        /// </summary>
        public ConfigAxisArg()
        {
            ParamValueType = "Double";
            ParamSetVal = 0D;
            ParamDefaultVal = 0D;
            ParamMaxVal = 0D;
            ParamMinVal = 0D;
            ParamStatus1 = 0;
            ParamStatus2 = 0;
            ParamStatus3 = 0;
        }

        /// <summary>
        /// 自增主键 ID。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 轴编号。
        /// 建议统一使用 AxisConfig.LogicalAxis。
        /// </summary>
        public int Axis { get; set; }

        /// <summary>
        /// 参数名称。
        /// 建议统一使用 AxisConfig 属性名。
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数中文名称。
        /// </summary>
        public string ParamName_Cn { get; set; }

        /// <summary>
        /// 参数值类型。
        /// 建议取值：Bool / Int16 / Int32 / Double / Enum。
        /// </summary>
        public string ParamValueType { get; set; }

        /// <summary>
        /// 参数当前设置值。
        /// </summary>
        public double ParamSetVal { get; set; }

        /// <summary>
        /// 参数默认值。
        /// </summary>
        public double ParamDefaultVal { get; set; }

        /// <summary>
        /// 参数最大值。
        /// </summary>
        public double ParamMaxVal { get; set; }

        /// <summary>
        /// 参数最小值。
        /// </summary>
        public double ParamMinVal { get; set; }

        /// <summary>
        /// 参数状态1。
        /// </summary>
        public int ParamStatus1 { get; set; }

        /// <summary>
        /// 参数状态2。
        /// </summary>
        public int ParamStatus2 { get; set; }

        /// <summary>
        /// 参数状态3。
        /// </summary>
        public int ParamStatus3 { get; set; }

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
        /// 轴中文名称。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Axis_Cn { get; set; }

        /// <summary>
        /// 保留字段1。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Reserve1 { get; set; }

        /// <summary>
        /// 保留字段2。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Reserve2 { get; set; }

        /// <summary>
        /// 保留字段3。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Reserve3 { get; set; }

        /// <summary>
        /// 保留字段4。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Reserve4 { get; set; }

        /// <summary>
        /// 保留字段5。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Reserve5 { get; set; }
    }
}
