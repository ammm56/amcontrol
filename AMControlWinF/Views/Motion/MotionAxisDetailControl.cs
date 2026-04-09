using AM.Core.Context;
using AM.PageModel.Motion.Axis;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制右侧实时监视控件。
    ///
    /// 【当前职责】
    /// 1. 负责显示当前选中轴的实时监视信息；
    /// 2. 使用固定标签控件承载详情内容，避免定时刷新时频繁创建控件；
    /// 3. 通过快照去重、暂停布局和双缓冲降低高频刷新闪烁；
    /// 4. 只负责显示监视信息和维护默认参数缓存，不参与动作执行。
    ///
    /// 【层级关系】
    /// - 上游：MotionAxisPage、MotionAxisPageModel；
    /// - 当前层：WinForms 详情显示控件；
    /// - 下游：固定标签控件与空态占位区域。
    ///
    /// 【调用关系】
    /// 1. 页面在轴选中变化或定时刷新后调用 `Bind`；
    /// 2. 控件根据当前项构建快照键并判断是否需要刷新；
    /// 3. 当切换到新轴时同步重置默认参数缓存；
    /// 4. 页面仅读取显示结果和默认参数，不逐项操作标签。
    ///
    /// 【架构设计】
    /// 本控件保持 WinForms 下“整体 Bind + 内部差量刷新”的简单设计：
    /// - 页面模型负责准备显示数据；
    /// - 控件负责显示与刷新优化；
    /// - 不引入额外中间状态对象与复杂绘制逻辑。
    /// </summary>
    public partial class MotionAxisDetailControl : UserControl
    {
        #region 字段

        private string _lastSnapshotKey = string.Empty;
        private short? _lastLogicalAxis;
        private MotionAxisSelectedViewItem _currentAxis;

        private string _velocityText = "10";
        private string _targetPositionText = "0";
        private string _moveDistanceText = "10";

        #endregion

        #region 构造与属性

        /// <summary>
        /// 初始化详情控件并启用双缓冲显示策略。
        /// </summary>
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

        /// <summary>
        /// 当前已绑定的轴项。
        /// 页面层可在需要时读取当前实时快照对象。
        /// </summary>
        public MotionAxisSelectedViewItem CurrentAxis
        {
            get { return _currentAxis; }
        }

        /// <summary>
        /// 当前默认速度文本。
        /// 仅用于兼容页面层读取默认参数。
        /// </summary>
        public string VelocityText
        {
            get { return _velocityText; }
        }

        /// <summary>
        /// 当前默认目标位置文本。
        /// 仅用于兼容页面层读取默认参数。
        /// </summary>
        public string TargetPositionText
        {
            get { return _targetPositionText; }
        }

        /// <summary>
        /// 当前默认相对移动距离文本。
        /// 仅用于兼容页面层读取默认参数。
        /// </summary>
        public string MoveDistanceText
        {
            get { return _moveDistanceText; }
        }

        #endregion

        #region 绑定与界面刷新

        /// <summary>
        /// 绑定当前轴实时信息。
        /// </summary>
        public void Bind(MotionAxisSelectedViewItem axisItem)
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
                SetTagRow(labelTagCmdMmKey, labelTagCmdMmValue, "规划位置(mm)", axisItem.CommandPositionMmText);
                SetTagRow(labelTagEncMmKey, labelTagEncMmValue, "编码器位置(mm)", axisItem.EncoderPositionMmText);
                SetTagRow(labelTagErrorMmKey, labelTagErrorMmValue, "位置误差(mm)", axisItem.PositionErrorMmText);
                SetTagRow(labelTagDefaultVelKey, labelTagDefaultVelValue, "默认速度(mm/s)", axisItem.DefaultVelocityText);
                SetTagRow(labelTagJogVelKey, labelTagJogVelValue, "Jog速度(mm/s)", axisItem.JogVelocityText);

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

        #endregion

        #region 辅助方法

        /// <summary>
        /// 当切换到新轴时，按当前轴运行态重置默认参数缓存。
        /// </summary>
        private void ResetDefaultValues(MotionAxisSelectedViewItem axisItem)
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

        /// <summary>
        /// 获取当前轴扫描任务的运行状态文本。
        /// </summary>
        private static string GetScanStateText()
        {
            var runtime = RuntimeContext.Instance.MotionAxis;
            return runtime.IsScanServiceRunning
                ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                : "已停止";
        }

        /// <summary>
        /// 根据当前轴运行态生成概要状态文本。
        /// </summary>
        private static string GetStateDisplayText(MotionAxisSelectedViewItem axisItem)
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

        /// <summary>
        /// 根据当前轴运行态生成动作联锁提示文本。
        /// </summary>
        private static string GetInterlockText(MotionAxisSelectedViewItem axisItem)
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
        /// 相同快照不重复刷新，从而减少页面闪烁。
        /// </summary>
        private static string BuildSnapshotKey(MotionAxisSelectedViewItem axisItem)
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