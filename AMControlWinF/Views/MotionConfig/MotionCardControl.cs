using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡信息卡片。
    /// </summary>
    public partial class MotionCardControl : UserControl
    {
        private MotionCardManagementPageModel.MotionCardViewItem _item;

        public MotionCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler EditRequested;

        public event EventHandler DeleteRequested;

        public MotionCardManagementPageModel.MotionCardViewItem CardItem
        {
            get { return _item; }
        }

        public void Bind(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            _item = item;

            labelTitle.Text = item == null ? string.Empty : item.DisplayName;

            buttonTypeTag.Text = item == null ? "-" : item.CardTypeText;
            buttonStatusTag.Text = item == null ? "-" : item.StatusText;
            buttonStatusTag.Type = item != null && item.IsEnabled ? TTypeMini.Success : TTypeMini.Error;

            labelValueCardId.Text = item == null ? "-" : item.CardId.ToString();
            labelValueDisplayName.Text = item == null ? "-" : item.DisplayName;
            labelValueName.Text = item == null ? "-" : item.Name;
            labelValueDriverKey.Text = item == null ? "-" : item.DriverKeyText;
            labelValueStatus.Text = item == null ? "-" : item.StatusText;
            labelValueUpdateTime.Text = item == null ? "-" : item.UpdateTimeText;

            labelValueCore.Text = item == null ? "-" : item.CoreNumberText;
            labelValueAxisCount.Text = item == null ? "-" : item.AxisCountText;
            labelValueModeParam.Text = item == null ? "-" : item.ModeParamText;
            labelValueOpenConfig.Text = item == null ? "-" : item.OpenConfigText;
            labelValueExtModule.Text = item == null ? "-" : item.ExtModuleText;
            labelValueRemark.Text = item == null ? "-" : item.RemarkText;

            labelDescription.Text = item == null ? "-" : item.DescriptionText;
        }

        private void BindEvents()
        {
            buttonEdit.Click += ButtonEdit_Click;
            buttonDelete.Click += ButtonDelete_Click;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            var handler = EditRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            var handler = DeleteRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}