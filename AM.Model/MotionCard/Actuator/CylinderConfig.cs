namespace AM.Model.MotionCard.Actuator
{
    /// <summary>
    /// 气缸运行时配置对象。
    /// 由第三层对象配置表装配而来，引用前两层逻辑 IO 点。
    /// </summary>
    public class CylinderConfig
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string DriveMode { get; set; }

        public short ExtendOutputBit { get; set; }

        public short? RetractOutputBit { get; set; }

        public short? ExtendFeedbackBit { get; set; }

        public short? RetractFeedbackBit { get; set; }

        public bool UseFeedbackCheck { get; set; }

        public int ExtendTimeoutMs { get; set; }

        public int RetractTimeoutMs { get; set; }

        public int? AlarmCodeOnExtendTimeout { get; set; }

        public int? AlarmCodeOnRetractTimeout { get; set; }

        public bool AllowBothOff { get; set; }

        public bool AllowBothOn { get; set; }

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }
    }
}