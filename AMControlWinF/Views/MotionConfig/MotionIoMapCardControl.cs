using AM.PageModel.MotionConfig;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// IO 映射信息卡片。
    /// 简洁摘要布局，仅展示核心信息。
    /// </summary>
    public partial class MotionIoMapCardControl : UserControl
    {
        private MotionIoMapManagementPageModel.MotionIoMapViewItem _item;

        public MotionIoMapCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public event EventHandler EditRequested;

        public event EventHandler DeleteRequested;

        public event EventHandler DetailRequested;

        public MotionIoMapManagementPageModel.MotionIoMapViewItem IoMapItem
        {
            get { return _item; }
        }

        public void Bind(MotionIoMapManagementPageModel.MotionIoMapViewItem item)
        {
            _item = item;

            labelTitle.Text = item == null ? "-" : item.Name;
            labelValueLogicalBit.Text = item == null ? "L-" : "L" + item.LogicalBitText;
            labelValueExtModule.Text = item == null ? "-" : item.ExtModuleText;

            buttonIoTypeTag.Text = item == null ? "-" : item.IoTypeText;
            //buttonIoTypeTag.Type = ResolveIoTypeTagType(item == null ? null : item.IoType);

            labelValueCore.Text = item == null ? "核: -" : "核: " + item.Core;
            labelValueHardwareBit.Text = item == null ? "位: -" : "位: " + item.HardwareBitText;

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

        private static TTypeMini ResolveIoTypeTagType(string ioType)
        {
            if (string.Equals(ioType, "DO", StringComparison.OrdinalIgnoreCase))
                return TTypeMini.Warn;

            return TTypeMini.Primary;
        }
    }
}