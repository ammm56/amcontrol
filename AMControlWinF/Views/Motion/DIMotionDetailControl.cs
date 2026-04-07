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

            SetTagRow(labelTagLogicKey, labelTagLogicValue, "逻辑位", item.LogicalBit.ToString());
            SetTagRow(labelTagCategoryKey, labelTagCategoryValue, "分类", item.TypeText);
            SetTagRow(labelTagCardKey, labelTagCardValue, "控制卡", item.CardText);
            SetTagRow(labelTagHardwareKey, labelTagHardwareValue, "硬件地址", item.HardwareAddressText);
            SetTagRow(labelTagStateKey, labelTagStateValue, "当前状态", item.ValueText);
            SetTagRow(labelTagLastUpdateKey, labelTagLastUpdateValue, "最后触发", item.LastUpdateTimeText);
            SetTagRow(labelTagLinkKey, labelTagLinkValue, "使用对象", item.LinkObjectDisplayText);

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
            string valueText)
        {
            keyLabel.Text = string.IsNullOrWhiteSpace(keyText) ? "-" : keyText;

            valueLabel.Text = string.IsNullOrWhiteSpace(valueText) ? "—" : valueText;
        }
    }
}