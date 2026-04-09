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
    /// 【当前职责】
    /// 1. 负责显示当前选中执行器的标题、副标题与动作选项；
    /// 2. 负责显示普通执行器主副按钮或灯塔状态按钮组；
    /// 3. 负责将按钮点击与选项变化以事件形式回传给页面层；
    /// 4. 不直接访问服务层，不直接持有页面原始状态对象。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorPage、MotionActuatorPageModel；
    /// - 当前层：WinForms 动作面板显示控件；
    /// - 下游：按钮点击事件、选项变化事件。
    ///
    /// 【调用关系】
    /// 1. 页面把 `MotionActuatorActionPanelState` 整体绑定到本控件；
    /// 2. 控件按状态刷新标题、选项和按钮显隐；
    /// 3. 用户操作按钮后，本控件只抛出事件；
    /// 4. 页面接收事件后再调用页面模型执行动作。
    ///
    /// 【设计说明】
    /// 本控件保持 WinForms 常见的“Bind + 事件回调”模式：
    /// - 显示逻辑集中在控件；
    /// - 状态推导集中在页面模型；
    /// - 设备动作集中在服务层；
    /// - 不引入额外命令对象或中间抽象。
    /// </summary>
    public partial class MotionActuatorActionPanelControl : UserControl
    {
        private bool _suppressOptionChanged;

        #region 构造与属性

        public MotionActuatorActionPanelControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 当前是否勾选等待反馈。
        /// 页面层从这里读取用户当前动作选项。
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

        #endregion

        #region 公开事件

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

        #endregion

        #region 绑定与界面刷新

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

                checkStackLightWithBuzzer.Visible = state.ShowStackLightWithBuzzer;
                checkWaitWorkpiece.Visible = state.ShowWaitWorkpiece;
                checkWaitFeedback.Visible = state.ShowWaitFeedback;

                checkStackLightWithBuzzer.Checked = state.StackLightWithBuzzer;
                checkWaitWorkpiece.Checked = state.WaitWorkpiece;
                checkWaitFeedback.Checked = state.WaitFeedback;

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

        private static void ApplyButtonState(AntdUI.Button button, MotionActuatorButtonState state)
        {
            if (button == null || state == null)
                return;

            button.Text = string.IsNullOrWhiteSpace(state.Text) ? "按钮" : state.Text;
            button.Visible = state.Visible;
            button.Enabled = state.Enabled;
            button.Type = ResolveButtonType(state.Type);
        }

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

        private void RefreshLayoutState()
        {
            flowOptions.PerformLayout();
            flowActionButtons.PerformLayout();
            gridRoot.PerformLayout();
            panelRoot.PerformLayout();
        }

        #endregion

        #region 控件事件转发

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

        private void OptionControl_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressOptionChanged)
                return;

            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region 事件参数

        /// <summary>
        /// 灯塔状态切换请求参数。
        /// 页面层直接使用枚举值调用页面模型，避免字符串中转。
        /// </summary>
        public sealed class StackLightStateRequestedEventArgs : EventArgs
        {
            public StackLightStateRequestedEventArgs(StackLightState state)
            {
                State = state;
            }

            public StackLightState State { get; private set; }
        }

        #endregion
    }
}