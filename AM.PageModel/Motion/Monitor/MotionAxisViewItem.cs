using System;

namespace AM.PageModel.Motion.Monitor
{
    /// <summary>
    /// 多轴总览页面显示项。
    ///
    /// 【当前职责】
    /// 1. 承载轴总览的原始快照字段；
    /// 2. 提供左侧卡片和右侧详情区共用的派生显示属性；
    /// 3. 只表达界面应该如何显示，不承担查询与执行逻辑。
    ///
    /// 【层级关系】
    /// - 上游：MotionMonitorPageModel；
    /// - 下游：MotionMonitorPage、MotionMonitorVirtualListControl、MotionMonitorDetailControl。
    /// </summary>
    public sealed class MotionAxisViewItem
    {
        #region 原始数据属性

        /// <summary>
        /// 逻辑轴编号。
        /// </summary>
        public short LogicalAxis { get; set; }

        /// <summary>
        /// 控制卡编号。
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        /// 卡内轴编号。
        /// </summary>
        public short AxisId { get; set; }

        /// <summary>
        /// 物理核心编号。
        /// </summary>
        public short PhysicalCore { get; set; }

        /// <summary>
        /// 物理轴编号。
        /// </summary>
        public short PhysicalAxis { get; set; }

        /// <summary>
        /// 内部名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 轴分类。
        /// </summary>
        public string AxisCategory { get; set; }

        /// <summary>
        /// 控制卡显示名。
        /// </summary>
        public string CardDisplayName { get; set; }

        /// <summary>
        /// 指令脉冲位置。
        /// </summary>
        public double CommandPositionPulse { get; set; }

        /// <summary>
        /// 编码器脉冲位置。
        /// </summary>
        public double EncoderPositionPulse { get; set; }

        /// <summary>
        /// 指令毫米位置。
        /// </summary>
        public double CommandPositionMm { get; set; }

        /// <summary>
        /// 编码器毫米位置。
        /// </summary>
        public double EncoderPositionMm { get; set; }

        /// <summary>
        /// 默认速度。
        /// </summary>
        public double DefaultVelocityMm { get; set; }

        /// <summary>
        /// 点动速度。
        /// </summary>
        public double JogVelocityMm { get; set; }

        /// <summary>
        /// 当前是否已使能。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 当前是否报警。
        /// </summary>
        public bool IsAlarm { get; set; }

        /// <summary>
        /// 当前是否在原点。
        /// </summary>
        public bool IsAtHome { get; set; }

        /// <summary>
        /// 当前是否触发正限位。
        /// </summary>
        public bool PositiveLimit { get; set; }

        /// <summary>
        /// 当前是否触发负限位。
        /// </summary>
        public bool NegativeLimit { get; set; }

        /// <summary>
        /// 当前是否到位。
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// 当前是否运动中。
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// 最后更新时间。
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        #endregion

        #region 派生显示属性

        /// <summary>
        /// 卡片主标题。
        /// 优先显示显示名，其次显示内部名。
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "轴-" + LogicalAxis : Name;
            }
        }

        /// <summary>
        /// 轴分类显示文本。
        /// </summary>
        public string AxisCategoryText
        {
            get { return string.IsNullOrWhiteSpace(AxisCategory) ? "Other" : AxisCategory; }
        }

        /// <summary>
        /// 控制卡显示文本。
        /// </summary>
        public string CardText
        {
            get
            {
                var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                return "卡#" + CardId + "  " + name;
            }
        }

        /// <summary>
        /// 物理映射显示文本。
        /// </summary>
        public string PhysicalText
        {
            get { return "轴 " + PhysicalAxis; }
        }

        /// <summary>
        /// 使能状态显示文本。
        /// </summary>
        public string EnableText
        {
            get { return IsEnabled ? "Enable" : "Disable"; }
        }

        /// <summary>
        /// 当前状态显示文本。
        /// </summary>
        public string StateText
        {
            get
            {
                if (IsAlarm)
                    return "Alarm";

                if (IsMoving)
                    return "Moving";

                if (!IsEnabled)
                    return "Disabled";

                if (IsDone)
                    return "Ready";

                return "Idle";
            }
        }

        /// <summary>
        /// 是否满足“已就绪”定义。
        /// </summary>
        public bool IsReady
        {
            get { return IsDone && IsEnabled && !IsMoving && !IsAlarm; }
        }

        /// <summary>
        /// 原点状态显示文本。
        /// </summary>
        public string HomeText
        {
            get { return IsAtHome ? "AtHome" : "OffHome"; }
        }

        /// <summary>
        /// 到位状态显示文本。
        /// </summary>
        public string DoneText
        {
            get { return IsDone ? "Done" : "Busy"; }
        }

        /// <summary>
        /// 限位状态显示文本。
        /// </summary>
        public string LimitText
        {
            get
            {
                if (PositiveLimit && NegativeLimit)
                    return "正负限位";

                if (PositiveLimit)
                    return "正限位";

                if (NegativeLimit)
                    return "负限位";

                return "无限位";
            }
        }

        /// <summary>
        /// 位置误差 指令位置 - 编码器位置。
        /// </summary>
        public double PositionErrorMm
        {
            get { return CommandPositionMm - EncoderPositionMm; }
        }

        /// <summary>
        /// 指令位置显示文本。
        /// </summary>
        public string CommandPositionMmText
        {
            get { return CommandPositionMm.ToString("0.###") + " mm"; }
        }

        /// <summary>
        /// 编码器位置显示文本。
        /// </summary>
        public string EncoderPositionMmText
        {
            get { return EncoderPositionMm.ToString("0.###") + " mm"; }
        }

        /// <summary>
        /// 位置误差显示文本。
        /// </summary>
        public string PositionErrorMmText
        {
            get { return PositionErrorMm.ToString("0.###") + " mm"; }
        }

        /// <summary>
        /// 指令脉冲位置显示文本。
        /// </summary>
        public string CommandPositionPulseText
        {
            get { return CommandPositionPulse.ToString("0.###"); }
        }

        /// <summary>
        /// 编码器脉冲位置显示文本。
        /// </summary>
        public string EncoderPositionPulseText
        {
            get { return EncoderPositionPulse.ToString("0.###"); }
        }

        /// <summary>
        /// 默认速度显示文本。
        /// </summary>
        public string DefaultVelocityText
        {
            get { return DefaultVelocityMm.ToString("0.###") + " mm/s"; }
        }

        /// <summary>
        /// 点动速度显示文本。
        /// </summary>
        public string JogVelocityText
        {
            get { return JogVelocityMm.ToString("0.###") + " mm/s"; }
        }

        /// <summary>
        /// 最后更新时间显示文本。
        /// </summary>
        public string UpdateTimeText
        {
            get
            {
                return UpdateTime.HasValue
                    ? UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : "—";
            }
        }

        #endregion
    }
}
