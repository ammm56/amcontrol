using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.Axis
{
    /// <summary>
    /// 当前选中轴显示项。
    /// 供右侧实时监视区使用。
    /// </summary>
    public sealed class MotionAxisSelectedViewItem
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
        public DateTime UpdateTime { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "轴-" + LogicalAxis : Name;
            }
        }

        public string CardText
        {
            get
            {
                var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                return "卡#" + CardId + "  " + name;
            }
        }

        public string AxisCategoryText
        {
            get { return string.IsNullOrWhiteSpace(AxisCategory) ? "Other" : AxisCategory; }
        }

        public string PhysicalText
        {
            get { return "核 " + PhysicalCore + " / 轴 " + PhysicalAxis; }
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

        public string EnableText
        {
            get { return IsEnabled ? "已使能" : "未使能"; }
        }

        public string HomeText
        {
            get { return IsAtHome ? "在原点" : "未回原点"; }
        }

        public string DoneText
        {
            get { return IsDone ? "已到位" : "未到位"; }
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

        public string CommandPositionMmText
        {
            get { return CommandPositionMm.ToString("0.###") + " mm"; }
        }

        public string EncoderPositionMmText
        {
            get { return EncoderPositionMm.ToString("0.###") + " mm"; }
        }

        /// <summary>
        /// 位置误差 指令位置 - 编码器位置。
        /// </summary>
        public double PositionErrorMm
        {
            get { return CommandPositionMm - EncoderPositionMm; }
        }

        public string PositionErrorMmText
        {
            get { return PositionErrorMm.ToString("0.###") + " mm"; }
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
                return UpdateTime == default(DateTime)
                    ? "—"
                    : UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
