namespace AM.Model.MotionCard.Actuator
{
    /// <summary>
    /// 夹爪对象运行时配置。
    /// 由第三层对象配置表装配而来，引用前两层逻辑 IO 点位。
    /// </summary>
    public class GripperConfig
    {
        /// <summary>
        /// 对象名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 驱动模式。
        /// Single=单线圈，Double=双线圈。
        /// </summary>
        public string DriveMode { get; set; }

        /// <summary>
        /// 夹紧/闭合输出逻辑位号。
        /// </summary>
        public short CloseOutputBit { get; set; }

        /// <summary>
        /// 打开输出逻辑位号。
        /// </summary>
        public short? OpenOutputBit { get; set; }

        /// <summary>
        /// 夹紧到位反馈逻辑位号。
        /// </summary>
        public short? CloseFeedbackBit { get; set; }

        /// <summary>
        /// 打开到位反馈逻辑位号。
        /// </summary>
        public short? OpenFeedbackBit { get; set; }

        /// <summary>
        /// 工件存在检测逻辑位号。
        /// </summary>
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
        public int? AlarmCodeOnCloseTimeout { get; set; }

        /// <summary>
        /// 打开超时报警代码。
        /// </summary>
        public int? AlarmCodeOnOpenTimeout { get; set; }

        /// <summary>
        /// 工件丢失报警代码。
        /// </summary>
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
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Remark { get; set; }
    }
}