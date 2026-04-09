using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.DI
{
    /// <summary>
    /// DI 监视页显示项。
    /// </summary>
    public sealed class DIMotionIoViewItem
    {
        public string IoType { get; set; }

        public short LogicalBit { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string SignalCategoryDisplayName { get; set; }

        public short CardId { get; set; }

        public string CardDisplayName { get; set; }

        public short Core { get; set; }

        public short HardwareBit { get; set; }

        public bool IsExtModule { get; set; }

        public bool CurrentValue { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string LinkObjectName { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "未命名输入点" : Name;
            }
        }

        public string ValueText
        {
            get { return CurrentValue ? "ON" : "OFF"; }
        }

        public string TypeText
        {
            get
            {
                return string.IsNullOrWhiteSpace(SignalCategoryDisplayName)
                    ? "数字输入"
                    : SignalCategoryDisplayName;
            }
        }

        public string ModuleText
        {
            get { return IsExtModule ? "扩展" : "板载"; }
        }

        public string CardText
        {
            get
            {
                var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                return "卡#" + CardId + "  " + name;
            }
        }

        public string CoreText
        {
            get { return "Core " + Core; }
        }

        public string HardwareBitText
        {
            get { return HardwareBit.ToString(); }
        }

        public string HardwareAddressText
        {
            get { return CoreText + " / Bit " + HardwareBit + " / " + ModuleText; }
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

        public string DescriptionText
        {
            get { return string.IsNullOrWhiteSpace(Description) ? "—" : Description; }
        }

        public string RemarkText
        {
            get { return string.IsNullOrWhiteSpace(Remark) ? "—" : Remark; }
        }

        public string LinkObjectDisplayText
        {
            get { return string.IsNullOrWhiteSpace(LinkObjectName) ? "—" : LinkObjectName; }
        }
    }
}
