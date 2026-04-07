using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DI 详情展示控件。
    /// 使用静态标签控件承载详情，避免定时刷新时频繁创建/销毁控件。
    /// </summary>
    public partial class DIMotionDetailControl : UserControl
    {
        private static readonly Color TagKeyBackColor = Color.FromArgb(92, 92, 92);
        private static readonly Color LogicValueBackColor = Color.FromArgb(22, 119, 255);
        private static readonly Color CategoryValueBackColor = Color.FromArgb(0, 121, 107);
        private static readonly Color CardValueBackColor = Color.FromArgb(69, 90, 100);
        private static readonly Color HardwareValueBackColor = Color.FromArgb(245, 124, 0);
        private static readonly Color StateOnValueBackColor = Color.FromArgb(56, 142, 60);
        private static readonly Color StateOffValueBackColor = Color.FromArgb(120, 120, 120);
        private static readonly Color LastUpdateValueBackColor = Color.FromArgb(166, 226, 46);
        private static readonly Color LinkValueBackColor = Color.FromArgb(0, 131, 143);

        public DIMotionDetailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定当前选中 DI 项。
        /// </summary>
        public void Bind(DIMotionPageModel.DIMotionIoViewItem item)
        {
            if (item == null)
            {
                panelEmpty.Visible = true;
                panelDetail.Visible = false;
                return;
            }

            panelEmpty.Visible = false;
            panelDetail.Visible = true;

            labelTitle.Text = item.DisplayTitle;
            labelSubTitle.Text = string.IsNullOrWhiteSpace(item.Name) ? "—" : item.Name;

            SetTagRow(labelTagLogicKey, labelTagLogicValue, "逻辑位", item.LogicalBit.ToString(), TagKeyBackColor, LogicValueBackColor);
            SetTagRow(labelTagCategoryKey, labelTagCategoryValue, "分类", item.TypeText, TagKeyBackColor, CategoryValueBackColor);
            SetTagRow(labelTagCardKey, labelTagCardValue, "控制卡", item.CardText, TagKeyBackColor, CardValueBackColor);
            SetTagRow(labelTagHardwareKey, labelTagHardwareValue, "硬件地址", item.HardwareAddressText, TagKeyBackColor, HardwareValueBackColor);
            SetTagRow(labelTagStateKey, labelTagStateValue, "当前状态", item.ValueText, TagKeyBackColor, item.CurrentValue ? StateOnValueBackColor : StateOffValueBackColor);
            SetTagRow(labelTagLastUpdateKey, labelTagLastUpdateValue, "最后触发", item.LastUpdateTimeText, TagKeyBackColor, LastUpdateValueBackColor);
            SetTagRow(labelTagLinkKey, labelTagLinkValue, "使用对象", item.LinkObjectDisplayText, TagKeyBackColor, LinkValueBackColor);

            labelDescriptionValue.Text = item.DescriptionText;
            labelRemarkValue.Text = item.RemarkText;
        }

        /// <summary>
        /// 设置一行“键 + 值”标签样式。
        /// </summary>
        private static void SetTagRow(
            AntdUI.Label keyLabel,
            AntdUI.Label valueLabel,
            string keyText,
            string valueText,
            Color keyBackColor,
            Color valueBackColor)
        {
            keyLabel.Text = string.IsNullOrWhiteSpace(keyText) ? "-" : keyText;
            keyLabel.BackColor = keyBackColor;
            keyLabel.ForeColor = Color.White;

            valueLabel.Text = string.IsNullOrWhiteSpace(valueText) ? "—" : valueText;
            valueLabel.BackColor = valueBackColor;
            valueLabel.ForeColor = Color.White;
        }
    }
}