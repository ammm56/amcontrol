namespace AM.Model.MotionCard.Actuator
{
    /// <summary>
    /// 灯塔状态枚举。
    /// 既支持业务语义状态，也支持单灯色直接控制。
    /// </summary>
    public enum StackLightState
    {
        /// <summary>
        /// 全灭。
        /// </summary>
        Off = 0,

        /// <summary>
        /// 空闲。
        /// 默认映射为绿灯常亮。
        /// </summary>
        Idle = 10,

        /// <summary>
        /// 运行中。
        /// 默认映射为绿灯常亮。
        /// </summary>
        Running = 20,

        /// <summary>
        /// 警告。
        /// 默认映射为黄灯常亮，可选蜂鸣器。
        /// </summary>
        Warning = 30,

        /// <summary>
        /// 报警。
        /// 默认映射为红灯常亮，可选蜂鸣器。
        /// </summary>
        Alarm = 40,

        /// <summary>
        /// 仅红灯。
        /// </summary>
        Red = 100,

        /// <summary>
        /// 仅黄灯。
        /// </summary>
        Yellow = 110,

        /// <summary>
        /// 仅绿灯。
        /// </summary>
        Green = 120,

        /// <summary>
        /// 仅蓝灯。
        /// </summary>
        Blue = 130
    }

    /// <summary>
    /// 灯塔/声光报警对象运行时配置。
    /// 由第三层对象配置表装配而来，引用前两层逻辑 DO 点位。
    /// </summary>
    public class StackLightConfig
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
        /// 红灯输出逻辑位号。
        /// </summary>
        public short? RedOutputBit { get; set; }

        /// <summary>
        /// 黄灯输出逻辑位号。
        /// </summary>
        public short? YellowOutputBit { get; set; }

        /// <summary>
        /// 绿灯输出逻辑位号。
        /// </summary>
        public short? GreenOutputBit { get; set; }

        /// <summary>
        /// 蓝灯输出逻辑位号。
        /// </summary>
        public short? BlueOutputBit { get; set; }

        /// <summary>
        /// 蜂鸣器输出逻辑位号。
        /// </summary>
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
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Remark { get; set; }
    }
}