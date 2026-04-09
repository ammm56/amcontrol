using AM.PageModel.MotionConfig;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡详情展示控件。
    /// </summary>
    public partial class MotionCardDetailControl : UserControl
    {
        public MotionCardDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            labelValueCardId.Text = item == null ? "-" : item.CardId.ToString();
            labelValueCardType.Text = item == null ? "-" : item.CardTypeText;
            labelValueDisplayName.Text = item == null ? "-" : item.DisplayName;
            labelValueName.Text = item == null ? "-" : item.Name;
            labelValueDriverKey.Text = item == null ? "-" : item.DriverKeyText;

            labelValueCoreNumber.Text = item == null ? "-" : item.CoreNumberText;
            labelValueAxisCount.Text = item == null ? "-" : item.AxisCountText;
            labelValueModeParam.Text = item == null ? "-" : item.ModeParamText;
            labelValueInitOrder.Text = item == null ? "-" : item.InitOrder.ToString();
            labelValueOpenConfig.Text = item == null ? "-" : item.OpenConfigText;
            labelValueUseExtModule.Text = item == null ? "-" : item.ExtModuleText;

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
