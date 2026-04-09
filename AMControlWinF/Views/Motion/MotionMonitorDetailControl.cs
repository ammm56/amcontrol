using AM.PageModel.Motion.Monitor;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 多轴总览右侧详情控件。
    ///
    /// 【当前职责】
    /// 1. 负责显示当前选中轴的详细信息；
    /// 2. 使用固定标签控件承载详情内容，避免定时刷新时频繁创建控件；
    /// 3. 通过快照去重、暂停布局和双缓冲降低高频刷新闪烁；
    /// 4. 仅负责显示，不参与数据查询、筛选和动作执行。
    ///
    /// 【层级关系】
    /// - 上游：MotionMonitorPage、MotionMonitorPageModel；
    /// - 当前层：WinForms 详情显示控件；
    /// - 下游：固定标签控件与空态占位区域。
    ///
    /// 【调用关系】
    /// 1. 页面在轴选中变化或定时刷新后调用 `Bind`；
    /// 2. 控件根据当前项构建快照键并判断是否需要刷新；
    /// 3. 仅当显示内容变化时才批量更新标签文本；
    /// 4. 页面无需逐项设置标签，只需传入当前选中轴对象。
    ///
    /// 【架构设计】
    /// 本控件保持 WinForms 下“整体 Bind + 内部差量刷新”的简单设计：
    /// - 页面模型负责准备显示数据；
    /// - 控件负责显示与刷新优化；
    /// - 不引入额外中间状态对象与复杂绘制逻辑。
    /// </summary>
    public partial class MotionMonitorDetailControl : UserControl
    {
        #region 字段

        /// <summary>
        /// 上一次已渲染的逻辑轴编号。
        /// 用于判断本次刷新是否仍然是同一条轴详情。
        /// </summary>
        private short? _lastLogicalAxis;

        /// <summary>
        /// 上一次已渲染的内容快照。
        /// 当快照完全一致时，直接跳过 UI 重绘，减少闪烁。
        /// </summary>
        private string _lastSnapshotKey = string.Empty;

        /// <summary>
        /// 当前已绑定的轴项。
        /// 主要用于外部调试或后续扩展。
        /// </summary>
        private MotionAxisViewItem _currentItem;

        #endregion

        #region 构造与属性

        /// <summary>
        /// 初始化详情控件并启用双缓冲显示策略。
        /// </summary>
        public MotionMonitorDetailControl()
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
        /// 当前已绑定的轴项。
        /// </summary>
        public MotionAxisViewItem CurrentItem
        {
            get { return _currentItem; }
        }

        #endregion

        #region 绑定与界面刷新

        /// <summary>
        /// 绑定当前选中的轴。
        /// </summary>
        public void Bind(MotionAxisViewItem item)
        {
            if (item == null)
            {
                _lastLogicalAxis = null;
                _lastSnapshotKey = string.Empty;
                _currentItem = null;

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
            if (_lastLogicalAxis.HasValue
                && _lastLogicalAxis.Value == item.LogicalAxis
                && string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastLogicalAxis = item.LogicalAxis;
            _lastSnapshotKey = snapshotKey;
            _currentItem = item;

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
                labelSubTitle.Text = item.CardText;

                SetTagRow(labelTagLogicalAxisKey, labelTagLogicalAxisValue, "逻辑轴", "L#" + item.LogicalAxis);
                SetTagRow(labelTagAxisTypeKey, labelTagAxisTypeValue, "轴类型", item.AxisCategoryText);
                SetTagRow(labelTagPhysicalKey, labelTagPhysicalValue, "物理映射", item.PhysicalText);
                SetTagRow(labelTagStateKey, labelTagStateValue, "当前状态", item.StateText);
                SetTagRow(labelTagEnableKey, labelTagEnableValue, "使能状态", item.EnableText);
                SetTagRow(labelTagHomeKey, labelTagHomeValue, "原点状态", item.HomeText);
                SetTagRow(labelTagDoneKey, labelTagDoneValue, "到位状态", item.DoneText);
                SetTagRow(labelTagLimitKey, labelTagLimitValue, "限位状态", item.LimitText);
                SetTagRow(labelTagCmdMmKey, labelTagCmdMmValue, "指令位置(mm)", item.CommandPositionMmText);
                SetTagRow(labelTagEncMmKey, labelTagEncMmValue, "编码器位置(mm)", item.EncoderPositionMmText);
                SetTagRow(labelTagErrorMmKey, labelTagErrorMmValue, "位置误差(mm)", item.PositionErrorMmText);
                SetTagRow(labelTagCmdPulseKey, labelTagCmdPulseValue, "指令位置(pulse)", item.CommandPositionPulseText);
                SetTagRow(labelTagEncPulseKey, labelTagEncPulseValue, "编码器位置(pulse)", item.EncoderPositionPulseText);
                SetTagRow(labelTagDefaultVelKey, labelTagDefaultVelValue, "默认速度", item.DefaultVelocityText);
                SetTagRow(labelTagJogVelKey, labelTagJogVelValue, "点动速度", item.JogVelocityText);
                SetTagRow(labelTagLastUpdateKey, labelTagLastUpdateValue, "最后更新", item.UpdateTimeText);
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
        /// 为空时统一回退到占位文本。
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
        /// 只要这些文本都没有变化，就不重复刷新右侧详情区。
        /// </summary>
        private static string BuildSnapshotKey(MotionAxisViewItem item)
        {
            return string.Join("|", new[]
            {
                item.DisplayTitle ?? string.Empty,
                item.CardText ?? string.Empty,
                item.LogicalAxis.ToString(),
                item.AxisCategoryText ?? string.Empty,
                item.PhysicalText ?? string.Empty,
                item.StateText ?? string.Empty,
                item.EnableText ?? string.Empty,
                item.HomeText ?? string.Empty,
                item.DoneText ?? string.Empty,
                item.LimitText ?? string.Empty,
                item.CommandPositionMmText ?? string.Empty,
                item.EncoderPositionMmText ?? string.Empty,
                item.PositionErrorMmText ?? string.Empty,
                item.CommandPositionPulseText ?? string.Empty,
                item.EncoderPositionPulseText ?? string.Empty,
                item.DefaultVelocityText ?? string.Empty,
                item.JogVelocityText ?? string.Empty,
                item.UpdateTimeText ?? string.Empty
            });
        }

        /// <summary>
        /// 为指定控件开启双缓冲。
        /// 某些 WinForms 原生容器没有公开 DoubleBuffered，这里统一通过反射处理。
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