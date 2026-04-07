using AM.PageModel.Motion;
using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 多轴总览右侧详情控件。
    ///
    /// 当前版本职责：
    /// 1. 根据选中轴显示右侧详情；
    /// 2. 通过快照去重，避免 500ms 刷新时重复重绘；
    /// 3. 使用当前第一阶段 Designer 中的标题、副标题、说明文本完成真实绑定。
    ///
    /// 后续若要进一步升级，可把 labelPlaceholder 替换成多行标签区，
    /// 但当前版本已经可以稳定展示真实内容，并与 DI/DO 页面防闪策略保持一致。
    /// </summary>
    public partial class MotionMonitorDetailControl : UserControl
    {
        /// <summary>
        /// 上一次已渲染的逻辑轴。
        /// 用于判断当前是否仍然是同一张轴详情。
        /// </summary>
        private short? _lastLogicalAxis;

        /// <summary>
        /// 上一次已渲染的内容快照。
        /// 相同快照不重复刷新，减少闪烁。
        /// </summary>
        private string _lastSnapshotKey = string.Empty;

        /// <summary>
        /// 当前已绑定的轴项。
        /// 后续如果详情区拆成更多控件，可继续复用该对象。
        /// </summary>
        private MotionMonitorPageModel.MotionAxisViewItem _currentItem;

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
        public MotionMonitorPageModel.MotionAxisViewItem CurrentItem
        {
            get { return _currentItem; }
        }

        /// <summary>
        /// 绑定当前选中的轴。
        /// </summary>
        public void Bind(MotionMonitorPageModel.MotionAxisViewItem item)
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
            if (_lastLogicalAxis.HasValue &&
                _lastLogicalAxis.Value == item.LogicalAxis &&
                string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal))
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

                // 标题区
                labelTitle.Text = item.DisplayTitle;
                labelSubTitle.Text = item.CardText + " / " + item.PhysicalText;

                // 第一阶段详情文本区：
                // 先使用单个说明标签承载详细信息，后续再升级成分行标签布局。
                labelPlaceholder.Text = BuildDetailText(item);
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
        /// 构建当前详情区显示文本。
        /// 第一阶段先用单个标签展示完整信息，便于快速落地。
        /// </summary>
        private static string BuildDetailText(MotionMonitorPageModel.MotionAxisViewItem item)
        {
            var builder = new StringBuilder();

            builder.AppendLine("逻辑轴：L#" + item.LogicalAxis);
            builder.AppendLine("控制卡：" + item.CardText);
            builder.AppendLine("轴类型：" + item.AxisCategoryText);
            builder.AppendLine("物理映射：" + item.PhysicalText);
            builder.AppendLine("当前状态：" + item.StateText);
            builder.AppendLine("使能状态：" + item.EnableText);
            builder.AppendLine("原点状态：" + item.HomeText);
            builder.AppendLine("到位状态：" + item.DoneText);
            builder.AppendLine("限位状态：" + item.LimitText);
            builder.AppendLine();
            builder.AppendLine("指令位置(mm)： " + item.CommandPositionMmText);
            builder.AppendLine("编码器位置(mm)： " + item.EncoderPositionMmText);
            builder.AppendLine("位置误差(mm)： " + item.PositionErrorMmText);
            builder.AppendLine();
            builder.AppendLine("指令位置(pulse)： " + item.CommandPositionPulseText);
            builder.AppendLine("编码器位置(pulse)： " + item.EncoderPositionPulseText);
            builder.AppendLine();
            builder.AppendLine("默认速度： " + item.DefaultVelocityText);
            builder.AppendLine("点动速度： " + item.JogVelocityText);

            return builder.ToString();
        }

        /// <summary>
        /// 生成当前详情快照键。
        /// 相同内容不重复刷新。
        /// </summary>
        private static string BuildSnapshotKey(MotionMonitorPageModel.MotionAxisViewItem item)
        {
            return string.Join("|", new[]
            {
                item.DisplayTitle ?? string.Empty,
                item.LogicalAxis.ToString(),
                item.CardText ?? string.Empty,
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
                item.JogVelocityText ?? string.Empty
            });
        }

        /// <summary>
        /// 为滚动宿主开启双缓冲，降低文本刷新时的闪烁。
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