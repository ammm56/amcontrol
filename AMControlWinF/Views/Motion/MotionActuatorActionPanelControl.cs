using AM.Model.MotionCard.Actuator;
using AM.PageModel.Motion.Actuator;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧上半区控制面板。
    ///
    /// 【层级定位】
    /// - 所在层：WinForms 显示控件层；
    /// - 上游来源：MotionActuatorPage / MotionActuatorPageModel；
    /// - 下游输出：按钮点击事件、选项变化事件。
    ///
    /// 【职责】
    /// 1. 负责右侧上半区动作面板显示；
    /// 2. 显示标题 / 副标题；
    /// 3. 显示等待反馈、等待工件检测、附带蜂鸣三个选项；
    /// 4. 显示普通执行器动作按钮或灯塔状态按钮；
    /// 5. 不直接调用服务层，只抛出事件给页面层处理。
    ///
    /// 【当前收口说明】
    /// - 动作面板只负责显示状态和抛出事件；
    /// - 灯塔按钮事件已从 string 改为 StackLightState 枚举；
    /// - 这样避免字符串中转、TryParse 和额外桥接逻辑。
    /// </summary>
    public partial class MotionActuatorActionPanelControl : UserControl
    {
        /// <summary>
        /// 应用界面状态时，临时抑制复选框事件，
        /// 避免 Checked 赋值再次触发 OptionsChanged，造成重复刷新。
        /// </summary>
        private bool _suppressOptionChanged;

        public MotionActuatorActionPanelControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 当前是否勾选等待反馈。
        /// 页面层统一从这里读取用户选项。
        /// </summary>
        public bool WaitFeedback
        {
            get { return checkWaitFeedback.Checked; }
        }

        /// <summary>
        /// 当前是否勾选等待工件检测。
        /// </summary>
        public bool WaitWorkpiece
        {
            get { return checkWaitWorkpiece.Checked; }
        }

        /// <summary>
        /// 当前是否勾选灯塔蜂鸣联动。
        /// </summary>
        public bool StackLightWithBuzzer
        {
            get { return checkStackLightWithBuzzer.Checked; }
        }

        /// <summary>
        /// 绑定页面模型计算出的控制面板状态。
        /// 这是控件唯一的外部刷新入口。
        /// </summary>
        public void Bind(MotionActuatorActionPanelState state)
        {
            if (state == null)
                return;

            _suppressOptionChanged = true;
            try
            {
                labelTitle.Text = string.IsNullOrWhiteSpace(state.TitleText)
                    ? "当前对象：未选择"
                    : state.TitleText;

                labelSubTitle.Text = string.IsNullOrWhiteSpace(state.SubTitleText)
                    ? "内部名称：—"
                    : "内部名称：" + state.SubTitleText.Replace("内部名称：", string.Empty);

                // 第二行：选项区显隐与取值。
                checkStackLightWithBuzzer.Visible = state.ShowStackLightWithBuzzer;
                checkWaitWorkpiece.Visible = state.ShowWaitWorkpiece;
                checkWaitFeedback.Visible = state.ShowWaitFeedback;

                checkStackLightWithBuzzer.Checked = state.StackLightWithBuzzer;
                checkWaitWorkpiece.Checked = state.WaitWorkpiece;
                checkWaitFeedback.Checked = state.WaitFeedback;

                // 第三行：统一按钮区。
                ApplyButtonState(buttonPrimary, state.PrimaryButton);
                ApplyButtonState(buttonSecondary, state.SecondaryButton);
                ApplyButtonState(buttonStateOff, state.OffButton);
                ApplyButtonState(buttonStateIdle, state.IdleButton);
                ApplyButtonState(buttonStateRunning, state.RunningButton);
                ApplyButtonState(buttonStateWarning, state.WarningButton);
                ApplyButtonState(buttonStateAlarm, state.AlarmButton);

                UpdateButtonWidths();
                RefreshLayoutState();
            }
            finally
            {
                _suppressOptionChanged = false;
            }
        }

        /// <summary>
        /// 绑定内部控件事件。
        /// </summary>
        private void BindEvents()
        {
            buttonPrimary.Click += ButtonPrimary_Click;
            buttonSecondary.Click += ButtonSecondary_Click;

            buttonStateOff.Click += (s, e) => RaiseStackLightStateRequested(StackLightState.Off);
            buttonStateIdle.Click += (s, e) => RaiseStackLightStateRequested(StackLightState.Idle);
            buttonStateRunning.Click += (s, e) => RaiseStackLightStateRequested(StackLightState.Running);
            buttonStateWarning.Click += (s, e) => RaiseStackLightStateRequested(StackLightState.Warning);
            buttonStateAlarm.Click += (s, e) => RaiseStackLightStateRequested(StackLightState.Alarm);
            checkStackLightWithBuzzer.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitWorkpiece.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitFeedback.CheckedChanged += OptionControl_CheckedChanged;
        }

        /// <summary>
        /// 应用单个按钮状态。
        /// 控件层只负责显示，不负责推导状态。
        /// </summary>
        private static void ApplyButtonState(
            AntdUI.Button button,
            MotionActuatorButtonState state)
        {
            if (button == null || state == null)
                return;

            button.Text = string.IsNullOrWhiteSpace(state.Text) ? "按钮" : state.Text;
            button.Visible = state.Visible;
            button.Enabled = state.Enabled;
            button.Type = ResolveButtonType(state.Type);
        }

        /// <summary>
        /// 页面模型中的轻量字符串类型映射为 AntdUI 内置按钮类型。
        /// </summary>
        private static TTypeMini ResolveButtonType(string type)
        {
            switch (type)
            {
                case "Primary":
                    return TTypeMini.Primary;

                case "Success":
                    return TTypeMini.Success;

                case "Warn":
                    return TTypeMini.Warn;

                case "Error":
                    return TTypeMini.Error;

                default:
                    return TTypeMini.Default;
            }
        }

        private void ButtonPrimary_Click(object sender, EventArgs e)
        {
            PrimaryActionRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonSecondary_Click(object sender, EventArgs e)
        {
            SecondaryActionRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseStackLightStateRequested(StackLightState state)
        {
            StackLightStateRequested?.Invoke(this, new StackLightStateRequestedEventArgs(state));
        }

        /// <summary>
        /// 选项变化时通知页面层重新计算按钮启用状态。
        /// 应用状态阶段会被 _suppressOptionChanged 抑制，避免重复刷新。
        /// </summary>
        private void OptionControl_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressOptionChanged)
                return;

            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 根据当前按钮文本自动调整按钮宽度。
        /// 第三行使用 FlowPanel，可自动换行，因此只需要保证单按钮宽度合理。
        /// </summary>
        private void UpdateButtonWidths()
        {
            ResizeButton(buttonPrimary, 92, 160);
            ResizeButton(buttonSecondary, 92, 160);

            ResizeButton(buttonStateOff, 60, 120);
            ResizeButton(buttonStateIdle, 60, 120);
            ResizeButton(buttonStateRunning, 60, 120);
            ResizeButton(buttonStateWarning, 60, 120);
            ResizeButton(buttonStateAlarm, 60, 120);
        }

        private static void ResizeButton(AntdUI.Button button, int minWidth, int maxWidth)
        {
            if (button == null)
                return;

            var text = string.IsNullOrWhiteSpace(button.Text) ? "按钮" : button.Text;
            var textWidth = TextRenderer.MeasureText(text, button.Font).Width + 28;

            if (textWidth < minWidth)
                textWidth = minWidth;

            if (textWidth > maxWidth)
                textWidth = maxWidth;

            button.Width = textWidth;
            button.Height = 36;
        }

        /// <summary>
        /// 控件显隐切换后主动刷新布局。
        /// 页面缓存复用时，这样能减少局部未重排问题。
        /// </summary>
        private void RefreshLayoutState()
        {
            flowOptions.PerformLayout();
            flowActionButtons.PerformLayout();
            gridRoot.PerformLayout();
            panelRoot.PerformLayout();
        }

        /// <summary>
        /// 普通执行器主动作请求事件。
        /// </summary>
        public event EventHandler PrimaryActionRequested;

        /// <summary>
        /// 普通执行器副动作请求事件。
        /// </summary>
        public event EventHandler SecondaryActionRequested;

        /// <summary>
        /// 灯塔状态切换请求事件。
        /// </summary>
        public event EventHandler<StackLightStateRequestedEventArgs> StackLightStateRequested;

        /// <summary>
        /// 控制选项变化事件。
        /// 页面层收到后重新计算按钮启用状态。
        /// </summary>
        public event EventHandler OptionsChanged;

        public sealed class StackLightStateRequestedEventArgs : EventArgs
        {
            public StackLightStateRequestedEventArgs(StackLightState state)
            {
                State = state;
            }

            public StackLightState State { get; private set; }
        }
    }
}