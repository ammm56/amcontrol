using SqlSugar;

namespace AM.Model.Entity.Motion.Actuator
{
    /// <summary>
    /// 气缸对象配置表。
    /// 基于前两层逻辑 IO 点位之上，定义完整气缸对象的动作、反馈、超时与报警规则。
    /// </summary>
    [SugarTable("motion_cylinder_config")]
    public class CylinderConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 对象名称。
        /// 例如：LoadClampCylinder、XAxisStopperCylinder。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// 例如：上料夹紧气缸、X轴挡停气缸。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 气缸驱动模式。
        /// Single=单线圈，Double=双线圈。
        /// </summary>
        public string DriveMode { get; set; }

        /// <summary>
        /// 伸出输出逻辑位号。
        /// 必须引用 DO 点。
        /// 指向 motion_io_map 中 IoType = DO
        /// </summary>
        public short ExtendOutputBit { get; set; }

        /// <summary>
        /// 缩回输出逻辑位号。
        /// 单线圈气缸可为空。
        /// 指向 motion_io_map 中 IoType = DO
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? RetractOutputBit { get; set; }

        /// <summary>
        /// 伸出到位反馈逻辑位号。
        /// 必须引用 DI 点。
        /// 指向 motion_io_map 中 IoType = DI
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? ExtendFeedbackBit { get; set; }

        /// <summary>
        /// 缩回到位反馈逻辑位号。
        /// 必须引用 DI 点。
        /// 指向 motion_io_map 中 IoType = DI
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? RetractFeedbackBit { get; set; }

        /// <summary>
        /// 是否启用反馈校验。
        /// </summary>
        public bool UseFeedbackCheck { get; set; }

        /// <summary>
        /// 伸出超时时间，单位 ms。
        /// </summary>
        public int ExtendTimeoutMs { get; set; }

        /// <summary>
        /// 缩回超时时间，单位 ms。
        /// </summary>
        public int RetractTimeoutMs { get; set; }

        /// <summary>
        /// 伸出超时报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnExtendTimeout { get; set; }

        /// <summary>
        /// 缩回超时报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnRetractTimeout { get; set; }

        /// <summary>
        /// 是否允许双输出同时关闭。
        /// 某些单线圈或保压策略可能需要。
        /// </summary>
        public bool AllowBothOff { get; set; }

        /// <summary>
        /// 是否允许双输出同时导通。
        /// 一般应为 false。
        /// </summary>
        public bool AllowBothOn { get; set; }

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