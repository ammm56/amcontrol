using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制右侧实时监视控件。
    ///
    /// 设计目标：
    /// 1. 右侧始终显示当前选中轴的实时监视信息，不依赖左侧动作卡片是否被点击；
    /// 2. 提供速度、目标位置、相对距离输入框，供左侧参数动作卡片执行时读取；
    /// 3. 使用固定标签布局和快照去重，降低 500ms 刷新带来的重绘抖动；
    /// 4. 保持代码与 Designer 分离，便于后续继续细调布局。
    /// </summary>
    public partial class MotionAxisDetailControl : UserControl
    {
        private string _lastSnapshotKey = string.Empty;
        private short? _lastLogicalAxis;
        private MotionAxisPageModel.MotionAxisSelectedViewItem _currentAxis;

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

            inputVelocity.Text = "10";
            inputTargetPosition.Text = "0";
            inputMoveDistance.Text = "10";

            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        public MotionAxisPageModel.MotionAxisSelectedViewItem CurrentAxis
        {
            get { return _currentAxis; }
        }

        public string VelocityText
        {
            get { return inputVelocity.Text; }
        }

        public string TargetPositionText
        {
            get { return inputTargetPosition.Text; }
        }

        public string MoveDistanceText
        {
            get { return inputMoveDistance.Text; }
        }

        /// <summary>
        /// 绑定当前轴实时信息。
        /// 右侧不再显示动作详情，因此只关注当前轴本身的监视值和参数输入默认值。
        /// </summary>
        public void Bind(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            if (axisItem == null)
            {
                _lastSnapshotKey = string.Empty;
                _lastLogicalAxis = null;
                _currentAxis = null;

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

            var snapshotKey = BuildSnapshotKey(axisItem);
            var axisChanged = !_lastLogicalAxis.HasValue || _lastLogicalAxis.Value != axisItem.LogicalAxis;

            if (string.Equals(_lastSnapshotKey, snapshotKey, StringComparison.Ordinal) && !axisChanged)
                return;

            _lastSnapshotKey = snapshotKey;
            _lastLogicalAxis = axisItem.LogicalAxis;
            _currentAxis = axisItem;

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

                labelTitle.Text = axisItem.DisplayTitle;
                labelSubTitle.Text = "L#" + axisItem.LogicalAxis + " / " + axisItem.CardText;

                SetTagRow(labelTagAxisLogicKey, labelTagAxisLogicValue, "逻辑轴", "L#" + axisItem.LogicalAxis);
                SetTagRow(labelTagAxisTypeKey, labelTagAxisTypeValue, "轴类型", axisItem.AxisCategoryText);
                SetTagRow(labelTagAxisPhysicalKey, labelTagAxisPhysicalValue, "物理映射", axisItem.PhysicalText);
                SetTagRow(labelTagAxisStateKey, labelTagAxisStateValue, "当前状态", axisItem.StateText);
                SetTagRow(labelTagAxisEnableKey, labelTagAxisEnableValue, "使能状态", axisItem.EnableText);
                SetTagRow(labelTagAxisHomeKey, labelTagAxisHomeValue, "原点状态", axisItem.HomeText);
                SetTagRow(labelTagAxisDoneKey, labelTagAxisDoneValue, "到位状态", axisItem.DoneText);
                SetTagRow(labelTagAxisLimitKey, labelTagAxisLimitValue, "限位状态", axisItem.LimitText);
                SetTagRow(labelTagAxisCommandMmKey, labelTagAxisCommandMmValue, "指令位置", axisItem.CommandPositionMmText);
                SetTagRow(labelTagAxisEncoderMmKey, labelTagAxisEncoderMmValue, "编码器位置", axisItem.EncoderPositionMmText);
                SetTagRow(labelTagAxisDefaultVelKey, labelTagAxisDefaultVelValue, "默认速度", axisItem.DefaultVelocityText);
                SetTagRow(labelTagAxisJogVelKey, labelTagAxisJogVelValue, "点动速度", axisItem.JogVelocityText);

                if (axisChanged)
                {
                    ResetDefaultInputs(axisItem);
                }
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

        private void ResetDefaultInputs(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            if (axisItem == null)
            {
                inputVelocity.Text = "10";
                inputTargetPosition.Text = "0";
                inputMoveDistance.Text = "10";
                return;
            }

            if (axisItem.JogVelocityMm > 0)
                inputVelocity.Text = axisItem.JogVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            else if (axisItem.DefaultVelocityMm > 0)
                inputVelocity.Text = axisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            else
                inputVelocity.Text = "10";

            inputTargetPosition.Text = axisItem.CommandPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            inputMoveDistance.Text = "10";
        }

        private static void SetTagRow(
            AntdUI.Label keyLabel,
            AntdUI.Label valueLabel,
            string keyText,
            string valueText)
        {
            keyLabel.Text = string.IsNullOrWhiteSpace(keyText) ? "-" : keyText;
            valueLabel.Text = string.IsNullOrWhiteSpace(valueText) ? "—" : valueText;
        }

        private static string BuildSnapshotKey(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            return string.Join("|", new[]
            {
                axisItem.LogicalAxis.ToString(),
                axisItem.DisplayTitle ?? string.Empty,
                axisItem.CardText ?? string.Empty,
                axisItem.AxisCategoryText ?? string.Empty,
                axisItem.PhysicalText ?? string.Empty,
                axisItem.StateText ?? string.Empty,
                axisItem.EnableText ?? string.Empty,
                axisItem.HomeText ?? string.Empty,
                axisItem.DoneText ?? string.Empty,
                axisItem.LimitText ?? string.Empty,
                axisItem.CommandPositionMmText ?? string.Empty,
                axisItem.EncoderPositionMmText ?? string.Empty,
                axisItem.DefaultVelocityText ?? string.Empty,
                axisItem.JogVelocityText ?? string.Empty
            });
        }

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
