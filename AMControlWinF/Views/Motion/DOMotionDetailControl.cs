using AM.PageModel.Motion.DO;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DO 详情展示控件。
    ///
    /// 【当前职责】
    /// 1. 负责显示当前选中 DO 点位的详细信息；
    /// 2. 使用固定标签控件承载详情内容，避免定时刷新时频繁创建控件；
    /// 3. 通过快照去重、暂停布局和双缓冲降低高频刷新闪烁；
    /// 4. 仅负责显示，不参与数据查询、筛选和动作执行。
    ///
    /// 【层级关系】
    /// - 上游：DOMotionPage、DOMotionPageModel；
    /// - 当前层：WinForms 详情显示控件；
    /// - 下游：固定标签控件与空态占位区域。
    ///
    /// 【调用关系】
    /// 1. 页面在列表选中变化或定时刷新后调用 `Bind`；
    /// 2. 控件根据当前项构建快照键并判断是否需要刷新；
    /// 3. 仅当显示内容变化时才批量更新标签文本；
    /// 4. 页面无需逐项设置标签，只需传入当前选中 DO 项对象。
    ///
    /// 【架构设计】
    /// 本控件保持 WinForms 下“整体 Bind + 内部差量刷新”的简单设计：
    /// - 页面模型负责准备显示数据；
    /// - 控件负责显示与刷新优化；
    /// - 不引入额外中间状态对象与复杂绘制逻辑。
    /// </summary>
    public partial class DOMotionDetailControl : UserControl
    {
        #region 字段

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

        #endregion

        #region 构造与绑定

        /// <summary>
        /// 初始化详情控件并启用双缓冲显示策略。
        /// </summary>
        public DOMotionDetailControl()
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
        /// 绑定当前选中 DO 项。
        /// </summary>
        public void Bind(DOMotionIoViewItem item)
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
            if (_lastLogicalBit.HasValue
                && _lastLogicalBit.Value == item.LogicalBit
                && string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
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
                SetTagRow(labelTagOutputModeKey, labelTagOutputModeValue, "输出模式", item.OutputModeText);
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

        #endregion

        #region 辅助方法

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
        private static string BuildSnapshotKey(DOMotionIoViewItem item)
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

            var property = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (property != null && property.CanWrite)
                property.SetValue(control, true, null);
        }

        #endregion
    }
}