using AM.PageModel.Motion;
using AntdUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DI 详情展示控件。
    /// 使用静态标签控件承载详情，避免定时刷新时频繁创建/销毁控件。
    /// </summary>
    public partial class DIMotionDetailControl : UserControl
    {
        private short? _lastLogicalBit;
        private string _lastSnapshotKey = string.Empty;

        public DIMotionDetailControl()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();

            EnableDoubleBuffer(panelScroll);

            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        /// <summary>
        /// 绑定当前选中 DI 项。
        /// </summary>
        public void Bind(DIMotionPageModel.DIMotionIoViewItem item)
        {
            if (item == null)
            {
                _lastLogicalBit = null;
                _lastSnapshotKey = string.Empty;

                if (!panelEmpty.Visible)
                {
                    SuspendLayout();
                    panelDetail.SuspendLayout();
                    try
                    {
                        panelDetail.Visible = false;
                        panelEmpty.Visible = true;
                    }
                    finally
                    {
                        panelDetail.ResumeLayout(false);
                        ResumeLayout(false);
                    }
                }

                return;
            }

            var snapshotKey = BuildSnapshotKey(item);
            if (_lastLogicalBit.HasValue &&
                _lastLogicalBit.Value == item.LogicalBit &&
                string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastLogicalBit = item.LogicalBit;
            _lastSnapshotKey = snapshotKey;

            SuspendLayout();
            panelDetail.SuspendLayout();
            panelHeader.SuspendLayout();
            panelScroll.SuspendLayout();
            try
            {
                if (!panelDetail.Visible)
                {
                    panelEmpty.Visible = false;
                    panelDetail.Visible = true;
                }

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
            finally
            {
                panelScroll.ResumeLayout(false);
                panelHeader.ResumeLayout(false);
                panelDetail.ResumeLayout(false);
                ResumeLayout(false);
                Invalidate();
            }
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

        /// <summary>
        /// 生成当前详情快照键。
        /// 相同内容不重复刷新，减少打开页面时的闪烁。
        /// </summary>
        private static string BuildSnapshotKey(DIMotionPageModel.DIMotionIoViewItem item)
        {
            return string.Join("|", new[]
            {
                item.DisplayTitle ?? string.Empty,
                item.Name ?? string.Empty,
                item.LogicalBit.ToString(),
                item.TypeText ?? string.Empty,
                item.CardText ?? string.Empty,
                item.HardwareAddressText ?? string.Empty,
                item.ValueText ?? string.Empty,
                item.LastUpdateTimeText ?? string.Empty,
                item.LinkObjectDisplayText ?? string.Empty,
                item.DescriptionText ?? string.Empty,
                item.RemarkText ?? string.Empty
            });
        }

        /// <summary>
        /// 为滚动宿主开启双缓冲，降低滚动与文本重排时闪烁。
        /// </summary>
        private static void EnableDoubleBuffer(Control control)
        {
            if (control == null)
                return;

            try
            {
                var property = typeof(Control).GetProperty(
                    "DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                if (property != null)
                    property.SetValue(control, true, null);
            }
            catch
            {
            }
        }
    }
}