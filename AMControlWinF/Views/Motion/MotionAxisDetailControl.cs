using AM.PageModel.Motion;
using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制右侧详情控件。
    ///
    /// 第一阶段职责：
    /// 1. 展示“当前选中轴 + 当前选中动作”的详细说明；
    /// 2. 使用快照去重，避免 500ms 刷新时频繁重绘；
    /// 3. 为下一步 Designer 明细布局做好准备。
    ///
    /// 当前阶段先使用“单标签承载详细文本”的方式实现，
    /// 后续再拆为更细的标签行布局。
    /// </summary>
    public partial class MotionAxisDetailControl : UserControl
    {
        private string _lastSnapshotKey = string.Empty;
        private MotionAxisPageModel.MotionAxisSelectedViewItem _currentAxis;
        private MotionAxisPageModel.MotionAxisActionViewItem _currentAction;

        public MotionAxisDetailControl()
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
        }

        public MotionAxisPageModel.MotionAxisSelectedViewItem CurrentAxis
        {
            get { return _currentAxis; }
        }

        public MotionAxisPageModel.MotionAxisActionViewItem CurrentAction
        {
            get { return _currentAction; }
        }

        /// <summary>
        /// 绑定当前选中轴与动作项。
        /// 第一阶段先使用单个说明区域承载详情文本。
        /// </summary>
        public void Bind(
            MotionAxisPageModel.MotionAxisSelectedViewItem axisItem,
            MotionAxisPageModel.MotionAxisActionViewItem actionItem)
        {
            if (actionItem == null)
            {
                _lastSnapshotKey = string.Empty;
                _currentAxis = null;
                _currentAction = null;
                Invalidate();
                return;
            }

            var snapshotKey = BuildSnapshotKey(axisItem, actionItem);
            if (string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
                return;

            _lastSnapshotKey = snapshotKey;
            _currentAxis = axisItem;
            _currentAction = actionItem;

            Invalidate();
        }

        /// <summary>
        /// 构建详情快照键。
        /// 只要轴状态和动作说明都未变化，就不重复刷新。
        /// </summary>
        private static string BuildSnapshotKey(
            MotionAxisPageModel.MotionAxisSelectedViewItem axisItem,
            MotionAxisPageModel.MotionAxisActionViewItem actionItem)
        {
            return string.Join("|", new[]
            {
                actionItem == null ? string.Empty : (actionItem.ActionKey ?? string.Empty),
                actionItem == null ? string.Empty : (actionItem.DisplayText ?? string.Empty),
                actionItem == null ? string.Empty : (actionItem.DescriptionText ?? string.Empty),
                actionItem == null ? string.Empty : (actionItem.ParameterHintText ?? string.Empty),
                axisItem == null ? string.Empty : axisItem.LogicalAxis.ToString(),
                axisItem == null ? string.Empty : (axisItem.DisplayTitle ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.CardText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.StateText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.EnableText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.HomeText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.DoneText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.LimitText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.CommandPositionMmText ?? string.Empty),
                axisItem == null ? string.Empty : (axisItem.EncoderPositionMmText ?? string.Empty)
            });
        }

        /// <summary>
        /// 第一阶段的详情文本。
        /// 下一步可以拆成更标准的标签布局。
        /// </summary>
        private static string BuildDetailText(
            MotionAxisPageModel.MotionAxisSelectedViewItem axisItem,
            MotionAxisPageModel.MotionAxisActionViewItem actionItem)
        {
            var builder = new StringBuilder();

            if (actionItem != null)
            {
                builder.AppendLine("动作名称： " + actionItem.DisplayText);
                builder.AppendLine("动作分组： " + actionItem.CategoryText);
                builder.AppendLine("动作说明： " + actionItem.DescriptionText);
                builder.AppendLine("参数要求： " + actionItem.ParameterHintText);
                builder.AppendLine();
            }

            if (axisItem == null)
            {
                builder.AppendLine("当前轴：未选择");
                builder.AppendLine("提示：请先点击顶部“选择轴”按钮。");
                return builder.ToString();
            }

            builder.AppendLine("当前轴：L#" + axisItem.LogicalAxis + "  " + axisItem.DisplayTitle);
            builder.AppendLine("控制卡： " + axisItem.CardText);
            builder.AppendLine("轴类型： " + axisItem.AxisCategoryText);
            builder.AppendLine("物理映射： " + axisItem.PhysicalText);
            builder.AppendLine("当前状态： " + axisItem.StateText);
            builder.AppendLine("使能状态： " + axisItem.EnableText);
            builder.AppendLine("原点状态： " + axisItem.HomeText);
            builder.AppendLine("到位状态： " + axisItem.DoneText);
            builder.AppendLine("限位状态： " + axisItem.LimitText);
            builder.AppendLine();
            builder.AppendLine("指令位置： " + axisItem.CommandPositionMmText);
            builder.AppendLine("编码器位置： " + axisItem.EncoderPositionMmText);
            builder.AppendLine("默认速度： " + axisItem.DefaultVelocityText);
            builder.AppendLine("点动速度： " + axisItem.JogVelocityText);

            return builder.ToString();
        }

        /// <summary>
        /// 滚动区域双缓冲处理。
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