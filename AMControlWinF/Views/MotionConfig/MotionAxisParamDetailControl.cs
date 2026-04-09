using AM.PageModel.MotionConfig;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴参数详情展示控件。
    /// </summary>
    public partial class MotionAxisParamDetailControl : UserControl
    {
        public MotionAxisParamDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionAxisParamManagementPageModel.AxisParamViewItem item)
        {
            labelValueGroup.Text = item == null ? "-" : item.GroupDisplayName;
            labelValueName.Text = item == null ? "-" : item.ParamDisplayName;
            labelValueParamName.Text = item == null ? "-" : item.ParamName;
            labelValueType.Text = item == null ? "-" : item.TypeLabel;

            labelValueCurrent.Text = item == null ? "-" : item.ParamValueText;
            labelValueDefault.Text = item == null ? "-" : item.ParamDefaultValueText;
            labelValueRange.Text = item == null ? "-" : item.RangeText;
            labelValueUnit.Text = item == null ? "-" : item.UnitText;

            labelValueVendor.Text = item == null ? "-" : item.VendorScope;
            labelValueValueDescription.Text = item == null ? "-" : item.ValueDescriptionText;
            labelValueDescription.Text = item == null ? "-" : item.DescriptionText;
            labelValueRemark.Text = item == null ? "-" : item.RemarkText;
        }
    }
}