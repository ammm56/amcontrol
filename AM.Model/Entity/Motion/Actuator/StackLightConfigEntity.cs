using SqlSugar;

namespace AM.Model.Entity.Motion.Actuator
{
    /// <summary>
    /// 灯塔/声光报警对象配置表。
    /// 基于前两层逻辑 DO 点位之上，定义灯塔各颜色段与蜂鸣器输出的第三层对象配置。
    /// </summary>
    [SugarTable("motion_stacklight_config")]
    public class StackLightConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 对象名称。
        /// 例如：MainTower、OperatorTower。
        /// 建议全局唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// 例如：主灯塔、操作位灯塔。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 红灯输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? RedOutputBit { get; set; }

        /// <summary>
        /// 黄灯输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? YellowOutputBit { get; set; }

        /// <summary>
        /// 绿灯输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? GreenOutputBit { get; set; }

        /// <summary>
        /// 蓝灯输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? BlueOutputBit { get; set; }

        /// <summary>
        /// 蜂鸣器输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? BuzzerOutputBit { get; set; }

        /// <summary>
        /// 警告状态是否默认带蜂鸣器。
        /// </summary>
        public bool EnableBuzzerOnWarning { get; set; }

        /// <summary>
        /// 报警状态是否默认带蜂鸣器。
        /// </summary>
        public bool EnableBuzzerOnAlarm { get; set; }

        /// <summary>
        /// 是否允许多颜色同时点亮。
        /// 首版默认建议为 false。
        /// </summary>
        public bool AllowMultiSegmentOn { get; set; }

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