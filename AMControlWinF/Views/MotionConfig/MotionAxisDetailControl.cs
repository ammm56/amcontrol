using AM.PageModel.MotionConfig;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴详情展示控件。
    /// </summary>
    public partial class MotionAxisDetailControl : UserControl
    {
        public MotionAxisDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            labelValueLogicalAxis.Text = item == null ? "-" : item.LogicalAxisText;
            labelValueAxisCategory.Text = item == null ? "-" : item.AxisCategoryText;
            labelValueDisplayName.Text = item == null ? "-" : item.DisplayName;
            labelValueName.Text = item == null ? "-" : item.Name;
            labelValueCard.Text = item == null ? "-" : item.CardText;

            labelValueAxisId.Text = item == null ? "-" : item.AxisId.ToString();
            labelValuePhysicalCore.Text = item == null ? "-" : item.PhysicalCore.ToString();
            labelValuePhysicalAxis.Text = item == null ? "-" : item.PhysicalAxis.ToString();

            labelValueEnabled.Text = item == null ? "-" : item.StatusText;
            labelValueEnabled.ForeColor = item != null && item.IsEnabled
                ? Color.FromArgb(82, 196, 26)
                : Color.FromArgb(245, 34, 45);

            labelValueSortOrder.Text = item == null ? "-" : item.SortOrder.ToString();
            labelValueUpdateTime.Text = item == null ? "-" : item.UpdateTimeText;

            labelValueDescription.Text = item == null ? "-" : item.DescriptionText;
            labelValueRemark.Text = item == null ? "-" : item.RemarkText;
        }
    }
}