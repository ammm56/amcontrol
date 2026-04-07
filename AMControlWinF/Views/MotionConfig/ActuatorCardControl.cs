using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 执行器信息卡片。
    /// </summary>
    public partial class ActuatorCardControl : UserControl
    {
        private ActuatorManagementPageModel.ActuatorViewItem _item;

        public ActuatorCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler DetailRequested;

        public ActuatorManagementPageModel.ActuatorViewItem ActuatorItem
        {
            get { return _item; }
        }

        public void Bind(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            _item = item;

            labelTitle.Text = item == null ? "-" : item.DisplayTitle;
            labelName.Text = item == null ? "-" : (string.IsNullOrWhiteSpace(item.Name) ? "-" : item.Name);
            
            buttonTypeTag.Text = item == null ? "-" : item.TypeDisplayName;
            buttonTypeTag.Type = ResolveTypeButtonType(item == null ? string.Empty : item.ActuatorType);

            labelStatus.Text = item != null && item.IsEnabled ? "● 启用" : "● 禁用";
            labelStatus.ForeColor = item != null && item.IsEnabled
                ? Color.FromArgb(82, 196, 26)
                : Color.FromArgb(245, 34, 45);
        }

        private void BindEvents()
        {
            buttonDetail.Click += ButtonDetail_Click;
        }

        private void ButtonDetail_Click(object sender, EventArgs e)
        {
            var handler = DetailRequested;
            if (handler != null)
            {
                handler(buttonDetail, EventArgs.Empty);
            }
        }

        private static TTypeMini ResolveTypeButtonType(string actuatorType)
        {
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeCylinder, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Primary;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeVacuum, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Success;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeStackLight, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Warn;
            if (string.Equals(actuatorType, ActuatorManagementPageModel.TypeGripper, StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Error;

            return TTypeMini.Default;
        }
    }
}