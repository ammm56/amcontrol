using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 轴拓扑信息卡片。
    /// 简洁摘要布局，仅展示核心轴信息。
    /// </summary>
    public partial class MotionAxisCardControl : UserControl
    {
        private MotionAxisManagementPageModel.MotionAxisViewItem _item;

        public MotionAxisCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler EditRequested;

        public event EventHandler DeleteRequested;

        public event EventHandler DetailRequested;

        public MotionAxisManagementPageModel.MotionAxisViewItem AxisItem
        {
            get { return _item; }
        }

        public void Bind(MotionAxisManagementPageModel.MotionAxisViewItem item)
        {
            _item = item;

            labelTitle.Text = item == null ? "-" : item.DisplayName;
            labelSubTitle.Text = item == null ? "-" : item.Name;
            labelValueLogicalAxis.Text = item == null ? "L-" : "L" + item.LogicalAxisText;

            buttonCategoryTag.Text = item == null ? "-" : item.AxisCategoryText;

            labelValuePhysicalCore.Text = item == null ? "核: -" : "核: " + item.PhysicalCore;
            labelValuePhysicalAxis.Text = item == null ? "轴: -" : "轴: " + item.PhysicalAxis;

            labelValueStatus.Text = item != null && item.IsEnabled
                ? "● 启用"
                : "● 禁用";

            labelValueStatus.ForeColor = item != null && item.IsEnabled
                ? Color.FromArgb(82, 196, 26)
                : Color.FromArgb(245, 34, 45);
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
                handler(this, EventArgs.Empty);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            var handler = DeleteRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ButtonDetail_Click(object sender, EventArgs e)
        {
            var handler = DetailRequested;
            if (handler != null)
                handler(buttonDetail, EventArgs.Empty);
        }

    }
}