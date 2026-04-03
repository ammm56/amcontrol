using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡信息卡片。
    /// 简洁摘要布局，仅展示核心信息。
    /// 更详细信息后续由 MotionCardDetailControl 承载。
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

        public event EventHandler DetailRequested;

        public MotionCardManagementPageModel.MotionCardViewItem CardItem
        {
            get { return _item; }
        }

        public void Bind(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            _item = item;

            var displayName = item == null ? "-" : item.DisplayName;
            var name = item == null ? "-" : item.Name;
            var cardTypeText = item == null ? "-" : item.CardTypeText;
            var cardIdText = item == null ? "-" : item.CardId.ToString();
            var coreText = item == null ? "-" : item.CoreNumberText;
            var axisCountText = item == null ? "-" : item.AxisCountText;
            var extModuleText = item == null ? "-" : item.ExtModuleText;
            var statusText = item == null ? "-" : item.StatusText;

            labelTitle.Text = displayName;
            labelSubTitle.Text = name;

            buttonTypeTag.Text = cardTypeText;
            labelValueCardId.Text = "#" + cardIdText;

            labelValueCore.Text = "核: " + coreText;
            labelValueAxisCount.Text = "轴: " + axisCountText;
            labelValueExtModule.Text = "扩展: " + extModuleText;

            labelValueStatus.Text = item != null && item.IsEnabled
                ? "● 启用"
                : "● 禁用";

            labelValueStatus.ForeColor = item != null && item.IsEnabled
                ? System.Drawing.Color.FromArgb(82, 196, 26)
                : System.Drawing.Color.FromArgb(245, 34, 45);

            buttonStatusTag.Text = statusText;
            buttonStatusTag.Type = item != null && item.IsEnabled
                ? TTypeMini.Success
                : TTypeMini.Error;

            buttonTypeTag.Type = ResolveCardTypeTagType(item == null ? 99 : item.CardType);
        }

        private void BindEvents()
        {
            buttonEdit.Click += ButtonEdit_Click;
            buttonDelete.Click += ButtonDelete_Click;
            buttonDetail.Click += ButtonDetail_Click;
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

        private void ButtonDetail_Click(object sender, EventArgs e)
        {
            var handler = DetailRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private static TTypeMini ResolveCardTypeTagType(int cardType)
        {
            switch (cardType)
            {
                case 10:
                    return TTypeMini.Primary;
                case 20:
                    return TTypeMini.Success;
                case 90:
                    return TTypeMini.Primary;
                default:
                    return TTypeMini.Default;
            }
        }
    }
}