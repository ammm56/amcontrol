using SqlSugar;

namespace AM.Model.Entity.Motion
{
    /// <summary>
    /// 运动轴参数表。
    /// 采用“单轴 + 参数名 + 参数值 + 类型标记”的方式保存。
    /// 该表负责保存参数值及其必要元数据，不负责保存界面控件定义。
    /// 唯一约束建议：
    /// 1. (LogicalAxis, ParamName) 唯一
    /// </summary>
    [SugarTable("motion_axis_config")]
    public class MotionAxisConfigEntity
    {
        public MotionAxisConfigEntity()
        {
            ParamGroup = "Motion";
            ParamValueType = "Double";
            ParamSetValue = 0D;
            ParamDefaultValue = 0D;
            ParamMaxValue = 0D;
            ParamMinValue = 0D;
            VendorScope = "All";
        }

        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 逻辑轴号。
        /// 对应 motion_axis.LogicalAxis。
        /// 建议与轴拓扑表统一使用 short。
        /// </summary>
        public short LogicalAxis { get; set; }

        /// <summary>
        /// 参数名。
        /// 与 AxisConfig 属性名保持一致，作为覆盖映射的主键之一。
        /// 例如：Lead / PulsePerRev / HomeTimeoutMs。
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数显示名称。
        /// 用于配置页面和参数说明展示。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ParamDisplayName { get; set; }

        /// <summary>
        /// 参数分组。
        /// 用于按业务语义进行归类。
        /// 推荐值：Hardware / Scale / Motion / Home / SoftLimit / Timing / Safety。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ParamGroup { get; set; }

        /// <summary>
        /// 参数值类型。
        /// 推荐值：Bool / Int16 / Int32 / Double / Enum。
        /// </summary>
        public string ParamValueType { get; set; }

        /// <summary>
        /// 当前设置值。
        /// 所有数值统一以 double 保存，再由映射器按 ParamValueType 转换。
        /// </summary>
        public double ParamSetValue { get; set; }

        /// <summary>
        /// 默认值。
        /// 用于回填、恢复默认参数、批量初始化。
        /// </summary>
        public double ParamDefaultValue { get; set; }

        /// <summary>
        /// 最大值。
        /// 用于参数边界校验；0 不建议再作为通用“未配置”含义滥用。
        /// 关键参数应明确写入上限。
        /// </summary>
        public double ParamMaxValue { get; set; }

        /// <summary>
        /// 最小值。
        /// 用于参数边界校验；关键参数应明确写入下限。
        /// </summary>
        public double ParamMinValue { get; set; }

        /// <summary>
        /// 参数单位。
        /// 例如：mm / pulse / pulse/ms / pulse/ms² / ms / ratio。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Unit { get; set; }

        /// <summary>
        /// 参数说明。
        /// 描述该参数的业务含义和作用。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 取值说明。
        /// 用于补充说明枚举值含义、方向约定、特殊取值含义等。
        /// 例如：-1=双向有效，10=关闭。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ValueDescription { get; set; }

        /// <summary>
        /// 厂商适用范围。
        /// 用于标记该参数适用于哪些控制卡驱动。
        /// 例如：All / GOOGO / LEISAI / GOOGO,LEISAI。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string VendorScope { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 轴显示名称。
        /// 冗余存储，仅用于参数表导出、调试和独立查询时辅助展示。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string AxisDisplayName { get; set; }
    }
}