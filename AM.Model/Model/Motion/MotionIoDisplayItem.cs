using System;

namespace AM.Model.Model.Motion
{
    /// <summary>
    /// Motion IO 监视显示项。
    /// </summary>
    public class MotionIoDisplayItem
    {
        public string IoType { get; set; }

        public short LogicalBit { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string SignalCategory { get; set; }

        public string SignalCategoryDisplayName { get; set; }

        public short CardId { get; set; }

        public string CardDisplayName { get; set; }

        public short Core { get; set; }

        public short HardwareBit { get; set; }

        public bool IsExtModule { get; set; }

        public bool IsEnabled { get; set; }

        public bool Invert { get; set; }

        public bool IsNormallyClosed { get; set; }

        public bool CanManualOperate { get; set; }

        public bool DefaultOutputState { get; set; }

        public string OutputMode { get; set; }

        public int DebounceMs { get; set; }

        public int FilterMs { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string LinkObjectName { get; set; }

        public bool CurrentValue { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(Name) ? "未命名点位" : Name;
            }
        }

        public string HardwareAddressText
        {
            get
            {
                return "核" + Core + " / Bit" + HardwareBit + (IsExtModule ? " / 扩展" : " / 板载");
            }
        }

        public string CardDisplayText
        {
            get
            {
                return "卡#" + CardId + "  " + (string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName);
            }
        }

        public string LastUpdateTimeText
        {
            get
            {
                return LastUpdateTime.HasValue
                    ? LastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : "—";
            }
        }

        public string StateText
        {
            get
            {
                if (!IsEnabled)
                {
                    return "禁用";
                }

                return CurrentValue ? "ON" : "OFF";
            }
        }

        public string TypeBadgeText
        {
            get
            {
                return string.IsNullOrWhiteSpace(IoType) ? "IO" : IoType;
            }
        }

        public string LinkObjectDisplayText
        {
            get
            {
                return string.IsNullOrWhiteSpace(LinkObjectName) ? "—" : LinkObjectName;
            }
        }
    }
}