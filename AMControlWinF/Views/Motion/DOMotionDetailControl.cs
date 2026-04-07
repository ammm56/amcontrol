using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DO 详情展示控件。
    /// 使用静态标签控件承载详情，避免定时刷新时频繁创建/销毁控件。
    /// </summary>
    public partial class DOMotionDetailControl : UserControl
    {
        /// <summary>
        /// 上一次已渲染的逻辑位号。
        /// 用于判断当前刷新是否仍然是同一个 DO 点。
        /// </summary>
        private short? _lastLogicalBit;

        /// <summary>
        /// 上一次已渲染的内容快照。
        /// 当本次快照与上次完全一致时，直接跳过 UI 刷新，减少闪烁。
        /// </summary>
        private string _lastSnapshotKey = string.Empty;

        public DOMotionDetailControl()
        {
            InitializeComponent();

            // 为整个详情控件开启双缓冲，降低首次显示和内容切换时的闪烁。
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();

            // 为滚动宿主开启双缓冲。
            // panelScroll 中承载了多行标签，定时刷新时这里最容易出现闪烁。
            EnableDoubleBuffer(panelScroll);

            // 初始状态下显示空态，占位提示更稳定。
            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        /// <summary>
        /// 绑定当前选中 DO 项。
        /// </summary>
        public void Bind(DOMotionPageModel.DOMotionIoViewItem item)
        {
            // 无选中项时显示空态，并清空上一次渲染快照。
            if (item == null)
            {
                _lastLogicalBit = null;
                _lastSnapshotKey = string.Empty;

                // 只有当前确实不是空态时，才执行切换。
                // 避免重复设置 Visible 造成额外重绘。
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

            // 生成当前数据快照。
            // 如果逻辑位相同且快照相同，说明页面显示内容没有变化，直接跳过刷新。
            var snapshotKey = BuildSnapshotKey(item);
            if (_lastLogicalBit.HasValue &&
                _lastLogicalBit.Value == item.LogicalBit &&
                string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastLogicalBit = item.LogicalBit;
            _lastSnapshotKey = snapshotKey;

            // 批量更新前先暂停布局，减少逐个 Label 改值导致的连续重排与闪烁。
            SuspendLayout();
            panelDetail.SuspendLayout();
            panelHeader.SuspendLayout();
            panelScroll.SuspendLayout();
            try
            {
                // 首次切入详情态时，仅切换一次可见性。
                if (!panelDetail.Visible)
                {
                    panelEmpty.Visible = false;
                    panelDetail.Visible = true;
                }

                // 头部标题
                labelTitle.Text = item.DisplayTitle;
                labelSubTitle.Text = string.IsNullOrWhiteSpace(item.Name) ? "—" : item.Name;

                // 标签行
                SetTagRow(labelTagLogicKey, labelTagLogicValue, "逻辑位", item.LogicalBit.ToString());
                SetTagRow(labelTagCategoryKey, labelTagCategoryValue, "分类", item.TypeText);
                SetTagRow(labelTagCardKey, labelTagCardValue, "控制卡", item.CardText);
                SetTagRow(labelTagHardwareKey, labelTagHardwareValue, "硬件地址", item.HardwareAddressText);
                SetTagRow(labelTagStateKey, labelTagStateValue, "当前状态", item.ValueText);
                SetTagRow(labelTagLastUpdateKey, labelTagLastUpdateValue, "最后触发", item.LastUpdateTimeText);
                SetTagRow(labelTagOutputModeKey, labelTagOutputModeValue, "输出模式", item.OutputModeText);
                SetTagRow(labelTagLinkKey, labelTagLinkValue, "使用对象", item.LinkObjectDisplayText);

                // 说明与备注
                labelDescriptionValue.Text = item.DescriptionText;
                labelRemarkValue.Text = item.RemarkText;
            }
            finally
            {
                panelScroll.ResumeLayout(false);
                panelHeader.ResumeLayout(false);
                panelDetail.ResumeLayout(false);
                ResumeLayout(false);

                // 最后统一重绘一次，避免中间多次闪动。
                Invalidate();
            }
        }

        /// <summary>
        /// 设置一行“键 + 值”标签文本。
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
        /// 生成当前详情内容的快照键。
        /// 相同快照不重复刷新，从而减少打开页面和定时刷新时的闪烁。
        /// </summary>
        private static string BuildSnapshotKey(DOMotionPageModel.DOMotionIoViewItem item)
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
                item.OutputModeText ?? string.Empty,
                item.LinkObjectDisplayText ?? string.Empty,
                item.DescriptionText ?? string.Empty,
                item.RemarkText ?? string.Empty
            });
        }

        /// <summary>
        /// 通过反射为指定控件开启双缓冲。
        /// WinForms 某些原生容器未公开 DoubleBuffered，此处统一兼容处理。
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
                // 反射失败时忽略，不影响主流程。
            }
        }
    }
}