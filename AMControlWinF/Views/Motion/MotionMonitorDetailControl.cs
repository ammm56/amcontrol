using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 多轴总览右侧详情控件。
    ///
    /// 设计目标：
    /// 1. 使用固定标签控件显示选中轴的详细信息；
    /// 2. 避免每次刷新都动态创建控件，降低闪烁与内存抖动；
    /// 3. 与 DI/DO 详情区保持一致，使用“快照去重 + SuspendLayout + 双缓冲”策略；
    /// 4. 当用户在左侧切换轴，或运行态发生变化时，仅刷新真正变化的内容。
    ///
    /// 说明：
    /// - 该控件不负责数据查询；
    /// - 只负责将页面模型中的 MotionAxisViewItem 显示到右侧；
    /// - 页面层传入当前选中项即可。
    /// </summary>
    public partial class MotionMonitorDetailControl : UserControl
    {
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
        private MotionMonitorPageModel.MotionAxisViewItem _currentItem;

        public MotionMonitorDetailControl()
        {
            InitializeComponent();

            // 为整个详情控件开启双缓冲。
            // 这样在频繁刷新文本、切换空态/详情态时，闪烁会明显降低。
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();

            // 为滚动宿主开启双缓冲。
            // panelScroll 中承载了多行标签，是最容易发生重绘抖动的区域。
            EnableDoubleBuffer(panelScroll);

            // 初始状态下显示空态。
            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        /// <summary>
        /// 当前已绑定的轴项。
        /// </summary>
        public MotionMonitorPageModel.MotionAxisViewItem CurrentItem
        {
            get { return _currentItem; }
        }

        /// <summary>
        /// 绑定当前选中的轴。
        ///
        /// 刷新策略：
        /// 1. 如果没有选中轴，显示空态；
        /// 2. 如果逻辑轴相同且快照完全一致，则不刷新；
        /// 3. 否则批量更新右侧所有标签。
        /// </summary>
        public void Bind(MotionMonitorPageModel.MotionAxisViewItem item)
        {
            // 没有选中轴时，显示空态并清空缓存快照。
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

            // 生成本次详情快照。
            // 如果当前逻辑轴和快照都与上一次一致，说明右侧内容无变化，直接跳过。
            var snapshotKey = BuildSnapshotKey(item);
            if (_lastLogicalAxis.HasValue &&
                _lastLogicalAxis.Value == item.LogicalAxis &&
                string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
            {
                return;
            }

            _lastLogicalAxis = item.LogicalAxis;
            _lastSnapshotKey = snapshotKey;
            _currentItem = item;

            // 批量更新前暂停布局，避免多次文本赋值导致连续重排。
            SuspendLayout();
            panelDetail.SuspendLayout();
            panelHeader.SuspendLayout();
            panelScroll.SuspendLayout();
            try
            {
                // 首次从空态切到详情态时，只做一次显隐切换。
                if (!panelDetail.Visible)
                {
                    panelEmpty.Visible = false;
                    panelDetail.Visible = true;
                }

                // 标题区
                labelTitle.Text = item.DisplayTitle;
                labelSubTitle.Text = item.CardText;

                // 标签行
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

                // 最后统一重绘一次，避免中间多次闪动。
                Invalidate();
            }
        }

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
        private static string BuildSnapshotKey(MotionMonitorPageModel.MotionAxisViewItem item)
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
                // 忽略反射失败，不影响主流程。
            }
        }
    }
}