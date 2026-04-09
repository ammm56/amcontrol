using System;

namespace AM.Model.Motion
{
    /// <summary>
    /// 运动轴显示项。
    /// 供多轴总览页和单轴控制页复用。
    /// </summary>
    public class MotionAxisDisplayItem
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
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(Name) ? "L#" + LogicalAxis : Name;
            }
        }

        public string StateText
        {
            get
            {
                if (IsAlarm)
                {
                    return "Alarm";
                }

                if (IsMoving)
                {
                    return "Moving";
                }

                if (!IsEnabled)
                {
                    return "Disabled";
                }

                if (IsDone)
                {
                    return "Ready";
                }

                return "Idle";
            }
        }

        public string LimitStateText
        {
            get
            {
                if (PositiveLimit && NegativeLimit)
                {
                    return "正负限位";
                }

                if (PositiveLimit)
                {
                    return "正限位";
                }

                if (NegativeLimit)
                {
                    return "负限位";
                }

                return "无限位";
            }
        }

        public string SignalSummaryText
        {
            get
            {
                return "使能:" + (IsEnabled ? "Y" : "N")
                    + " / 原点:" + (IsAtHome ? "Y" : "N")
                    + " / 到位:" + (IsDone ? "Y" : "N");
            }
        }
    }
}