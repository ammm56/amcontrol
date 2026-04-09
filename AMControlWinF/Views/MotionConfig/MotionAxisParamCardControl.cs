using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴参数信息卡片。
    /// </summary>
    public partial class MotionAxisParamCardControl : UserControl
    {
        private MotionAxisParamManagementPageModel.AxisParamViewItem _item;

        public MotionAxisParamCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler DetailRequested;

        public MotionAxisParamManagementPageModel.AxisParamViewItem ParamItem
        {
            get { return _item; }
        }

        public void Bind(MotionAxisParamManagementPageModel.AxisParamViewItem item)
        {
            _item = item;

            labelTitle.Text = item == null ? "-" : item.ParamDisplayName;
            labelValue.Text = item == null ? "-" : item.ParamValueText;
            labelParamName.Text = item == null ? "-" : item.ParamName;

            buttonTypeTag.Text = item == null ? "-" : item.TypeLabel;
        }

        private void BindEvents()
        {
            buttonDetail.Click += ButtonDetail_Click;
        }

        private void ButtonDetail_Click(object sender, EventArgs e)
        {
            var handler = DetailRequested;
            if (handler != null)
                handler(buttonDetail, EventArgs.Empty);
        }

        private static TTypeMini ResolveGroupTagType(string groupKey)
        {
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupHardware, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Default;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupScale, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Primary;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupMotion, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Success;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupHome, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Warn;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupSoftLimit, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Error;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupTiming, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Primary;
            if (string.Equals(groupKey, MotionAxisParamManagementPageModel.GroupSafety, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Error;

            return TTypeMini.Default;
        }
    }
}