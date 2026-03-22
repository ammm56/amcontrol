using SqlSugar;

namespace AM.Model.Entity.Motion.Actuator
{
    /// <summary>
    /// 夹爪对象配置表。
    /// 基于前两层逻辑 IO 点位之上，定义夹爪开合动作、反馈检测、超时与报警规则。
    /// </summary>
    [SugarTable("motion_gripper_config")]
    public class GripperConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 对象名称。
        /// 例如：PickGripper、ClampGripper。
        /// 建议全局唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// 例如：取料夹爪、夹紧夹爪。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 夹爪驱动模式。
        /// Single=单线圈，Double=双线圈。
        /// </summary>
        public string DriveMode { get; set; }

        /// <summary>
        /// 夹紧/闭合输出逻辑位号。
        /// 必须引用 DO 点。
        /// </summary>
        public short CloseOutputBit { get; set; }

        /// <summary>
        /// 打开输出逻辑位号。
        /// 单线圈夹爪可为空。
        /// 必须引用 DO 点。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? OpenOutputBit { get; set; }

        /// <summary>
        /// 夹紧到位反馈逻辑位号。
        /// 可为空。
        /// 必须引用 DI 点。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? CloseFeedbackBit { get; set; }

        /// <summary>
        /// 打开到位反馈逻辑位号。
        /// 可为空。
        /// 必须引用 DI 点。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? OpenFeedbackBit { get; set; }

        /// <summary>
        /// 工件存在检测逻辑位号。
        /// 可为空。
        /// 必须引用 DI 点。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? WorkpiecePresentBit { get; set; }

        /// <summary>
        /// 是否启用反馈校验。
        /// </summary>
        public bool UseFeedbackCheck { get; set; }

        /// <summary>
        /// 是否启用工件检测校验。
        /// </summary>
        public bool UseWorkpieceCheck { get; set; }

        /// <summary>
        /// 夹紧超时时间，单位 ms。
        /// </summary>
        public int CloseTimeoutMs { get; set; }

        /// <summary>
        /// 打开超时时间，单位 ms。
        /// </summary>
        public int OpenTimeoutMs { get; set; }

        /// <summary>
        /// 夹紧超时报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnCloseTimeout { get; set; }

        /// <summary>
        /// 打开超时报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnOpenTimeout { get; set; }

        /// <summary>
        /// 工件丢失报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnWorkpieceLost { get; set; }

        /// <summary>
        /// 是否允许双输出同时关闭。
        /// </summary>
        public bool AllowBothOff { get; set; }

        /// <summary>
        /// 是否允许双输出同时导通。
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