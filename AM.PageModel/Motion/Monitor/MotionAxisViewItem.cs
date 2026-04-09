using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.Monitor
{
    /// <summary>
    /// 多轴总览页面显示项。
    /// 供左侧卡片和右侧详情区共同使用。
    /// </summary>
    public sealed class MotionAxisViewItem
    {
        public short LogicalAxis { get; set; }
        public short CardId { get; set; }
        public short AxisId { get; set; }
        public short PhysicalCore { get; set; }
        public short PhysicalAxis { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string AxisCategory { get; set; }
        public string CardDisplayName { get; set; }
        public double CommandPositionPulse { get; set; }
        public double EncoderPositionPulse { get; set; }
        public double CommandPositionMm { get; set; }
        public double EncoderPositionMm { get; set; }
        public double DefaultVelocityMm { get; set; }
        public double JogVelocityMm { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAlarm { get; set; }
        public bool IsAtHome { get; set; }
        public bool PositiveLimit { get; set; }
        public bool NegativeLimit { get; set; }
        public bool IsDone { get; set; }
        public bool IsMoving { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "轴-" + LogicalAxis : Name;
            }
        }

        public string AxisCategoryText
        {
            get { return string.IsNullOrWhiteSpace(AxisCategory) ? "Other" : AxisCategory; }
        }

        public string CardText
        {
            get
            {
                var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                return "卡#" + CardId + "  " + name;
            }
        }

        public string PhysicalText
        {
            //get { return "核 " + PhysicalCore + " / 轴 " + PhysicalAxis; }
            get { return "轴 " + PhysicalAxis; }
        }

        public string EnableText
        {
            get { return IsEnabled ? "Enable" : "Disable"; }
        }

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

        public bool IsReady
        {
            get { return IsDone && IsEnabled && !IsMoving && !IsAlarm; }
        }

        public string HomeText
        {
            get { return IsAtHome ? "AtHome" : "OffHome"; }
        }

        public string DoneText
        {
            get { return IsDone ? "Done" : "Busy"; }
        }

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

        public string CommandPositionMmText
        {
            get { return CommandPositionMm.ToString("0.###") + " mm"; }
        }

        public string EncoderPositionMmText
        {
            get { return EncoderPositionMm.ToString("0.###") + " mm"; }
        }

        public string PositionErrorMmText
        {
            get { return PositionErrorMm.ToString("0.###") + " mm"; }
        }

        public string CommandPositionPulseText
        {
            get { return CommandPositionPulse.ToString("0.###"); }
        }

        public string EncoderPositionPulseText
        {
            get { return EncoderPositionPulse.ToString("0.###"); }
        }

        public string DefaultVelocityText
        {
            get { return DefaultVelocityMm.ToString("0.###") + " mm/s"; }
        }

        public string JogVelocityText
        {
            get { return JogVelocityMm.ToString("0.###") + " mm/s"; }
        }

        public string UpdateTimeText
        {
            get
            {
                return UpdateTime.HasValue
                    ? UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : "—";
            }
        }
    }
}
