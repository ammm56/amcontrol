using AM.PageModel.MotionConfig;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// IO 映射详情展示控件。
    /// </summary>
    public partial class MotionIoMapDetailControl : UserControl
    {
        public MotionIoMapDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionIoMapManagementPageModel.MotionIoMapViewItem item)
        {
            labelValueIoType.Text = item == null ? "-" : item.IoTypeDisplayText;
            labelValueName.Text = item == null ? "-" : item.Name;
            labelValueLogicalBit.Text = item == null ? "-" : item.LogicalBitText;
            labelValueCard.Text = item == null ? "-" : item.CardText;

            labelValueCore.Text = item == null ? "-" : item.Core.ToString();
            labelValueHardwareBit.Text = item == null ? "-" : item.HardwareBitText;
            labelValueExtModule.Text = item == null ? "-" : item.ExtModuleText;

            labelValueEnabled.Text = item == null ? "-" : item.StatusText;
            labelValueEnabled.ForeColor = item != null && item.IsEnabled
                ? Color.FromArgb(82, 196, 26)
                : Color.FromArgb(245, 34, 45);

            labelValueSortOrder.Text = item == null ? "-" : item.SortOrder.ToString();
            labelValueRemark.Text = item == null ? "-" : (string.IsNullOrWhiteSpace(item.Remark) ? "-" : item.Remark);
        }
    }
}