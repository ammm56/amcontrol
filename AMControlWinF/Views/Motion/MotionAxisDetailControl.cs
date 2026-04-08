using AM.Core.Context;
using AM.PageModel.Motion;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制右侧实时监视控件。
    /// 右侧只显示当前轴的运行状态信息，不承载参数输入。
    /// </summary>
    public partial class MotionAxisDetailControl : UserControl
    {
        private string _lastSnapshotKey = string.Empty;
        private short? _lastLogicalAxis;
        private MotionAxisPageModel.MotionAxisSelectedViewItem _currentAxis;

        // 先保留这三个属性，兼容当前页面层调用。
        // 后续把参数输入迁到左侧参数动作卡片后，可一并删除。
        private string _velocityText = "10";
        private string _targetPositionText = "0";
        private string _moveDistanceText = "10";

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

            panelDetail.Visible = false;
            panelEmpty.Visible = true;
        }

        public MotionAxisPageModel.MotionAxisSelectedViewItem CurrentAxis
        {
            get { return _currentAxis; }
        }

        public string VelocityText
        {
            get { return _velocityText; }
        }

        public string TargetPositionText
        {
            get { return _targetPositionText; }
        }

        public string MoveDistanceText
        {
            get { return _moveDistanceText; }
        }

        /// <summary>
        /// 绑定当前轴实时信息。
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

                labelTitle.Text = "当前轴：L#" + axisItem.LogicalAxis + "  " + axisItem.DisplayTitle;
                labelSubTitle.Text = axisItem.CardText;

                SetTagRow(labelTagCardKey, labelTagCardValue, "控制卡", axisItem.CardText);
                SetTagRow(labelTagScanKey, labelTagScanValue, "采样状态", GetScanStateText());
                SetTagRow(labelTagUpdateKey, labelTagUpdateValue, "更新时间", axisItem.UpdateTimeText);
                SetTagRow(labelTagStateKey, labelTagStateValue, "当前状态", GetStateDisplayText(axisItem));
                SetTagRow(labelTagInterlockKey, labelTagInterlockValue, "联锁说明", GetInterlockText(axisItem));
                SetTagRow(labelTagLimitKey, labelTagLimitValue, "限位状态", axisItem.LimitText);
                SetTagRow(labelTagEnableKey, labelTagEnableValue, "使能状态", axisItem.EnableText);
                SetTagRow(labelTagHomeKey, labelTagHomeValue, "原点状态", axisItem.HomeText);
                SetTagRow(labelTagDoneKey, labelTagDoneValue, "到位状态", axisItem.DoneText);
                SetTagRow(labelTagCmdMmKey, labelTagCmdMmValue, "规划位置(mm)", FormatNumber(axisItem.CommandPositionMm));
                SetTagRow(labelTagEncMmKey, labelTagEncMmValue, "编码器位置(mm)", FormatNumber(axisItem.EncoderPositionMm));
                SetTagRow(labelTagErrorMmKey, labelTagErrorMmValue, "位置误差(mm)", FormatNumber(axisItem.CommandPositionMm - axisItem.EncoderPositionMm));
                SetTagRow(labelTagDefaultVelKey, labelTagDefaultVelValue, "默认速度(mm/s)", FormatNumber(axisItem.DefaultVelocityMm));
                SetTagRow(labelTagJogVelKey, labelTagJogVelValue, "Jog速度(mm/s)", FormatNumber(axisItem.JogVelocityMm));

                if (axisChanged)
                {
                    ResetDefaultValues(axisItem);
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

        private void ResetDefaultValues(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            if (axisItem == null)
            {
                _velocityText = "10";
                _targetPositionText = "0";
                _moveDistanceText = "10";
                return;
            }

            _velocityText = axisItem.JogVelocityMm > 0D
                ? axisItem.JogVelocityMm.ToString("0.###")
                : (axisItem.DefaultVelocityMm > 0D ? axisItem.DefaultVelocityMm.ToString("0.###") : "10");

            _targetPositionText = axisItem.CommandPositionMm.ToString("0.###");
            _moveDistanceText = "10";
        }

        private static string GetScanStateText()
        {
            var runtime = RuntimeContext.Instance.MotionAxis;
            return runtime.IsScanServiceRunning
                ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                : "已停止";
        }

        private static string GetStateDisplayText(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            if (axisItem == null)
                return "运行态未建立";

            if (axisItem.IsAlarm)
                return "报警中";

            if (axisItem.IsMoving)
                return "运动中";

            if (!axisItem.IsEnabled)
                return "未使能";

            if (axisItem.IsDone)
                return "已使能待机";

            return "空闲";
        }

        private static string GetInterlockText(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            if (axisItem == null)
                return "运行态未建立，禁止操作";

            if (axisItem.IsAlarm)
                return "当前轴报警中，请先清状态";

            if (!axisItem.IsEnabled)
                return "当前轴未使能，运动前请先伺服上电";

            if (axisItem.PositiveLimit || axisItem.NegativeLimit)
                return "当前存在限位约束，请确认方向后操作";

            if (!axisItem.IsAtHome)
                return "当前轴未回原点，绝对定位前建议先回零";

            if (axisItem.IsMoving)
                return "当前轴正在运动中，请避免重复下发运动命令";

            return "当前轴允许操作";
        }

        private static string FormatNumber(double value)
        {
            return value.ToString("0.###");
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
                axisItem.UpdateTimeText ?? string.Empty,
                axisItem.EnableText ?? string.Empty,
                axisItem.HomeText ?? string.Empty,
                axisItem.DoneText ?? string.Empty,
                axisItem.LimitText ?? string.Empty,
                axisItem.CommandPositionMm.ToString("0.###"),
                axisItem.EncoderPositionMm.ToString("0.###"),
                axisItem.DefaultVelocityMm.ToString("0.###"),
                axisItem.JogVelocityMm.ToString("0.###"),
                axisItem.IsEnabled.ToString(),
                axisItem.IsAlarm.ToString(),
                axisItem.IsAtHome.ToString(),
                axisItem.IsDone.ToString(),
                axisItem.IsMoving.ToString(),
                axisItem.PositiveLimit.ToString(),
                axisItem.NegativeLimit.ToString()
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