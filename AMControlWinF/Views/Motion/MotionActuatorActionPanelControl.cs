using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧上半区控制面板。
    ///
    /// 设计原则：
    /// 1. 结构固定为三行：
    ///    - 第一行：当前对象标题 + 副标题；
    ///    - 第二行：控制选项；
    ///    - 第三行：动作按钮区；
    /// 2. 所有动作按钮统一放在同一个 FlowPanel 中；
    /// 3. 不同执行器只通过 Visible / Enabled / Text / Type 控制显示；
    /// 4. 控件本身不直接执行业务动作，只抛出事件给页面层统一处理；
    /// 5. 外部只通过 Bind(...) 单入口刷新，避免重复状态同步。
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
        public void Bind(MotionActuatorPageModel.MotionActuatorActionPanelState state)
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

                // 根据按钮文本重新计算宽度，适配“（当前）”等文案变化。
                UpdateButtonWidths();

                // 主动触发布局，保证缓存页面复用时显示稳定。
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

            buttonStateOff.Click += delegate { RaiseStackLightStateRequested("Off"); };
            buttonStateIdle.Click += delegate { RaiseStackLightStateRequested("Idle"); };
            buttonStateRunning.Click += delegate { RaiseStackLightStateRequested("Running"); };
            buttonStateWarning.Click += delegate { RaiseStackLightStateRequested("Warning"); };
            buttonStateAlarm.Click += delegate { RaiseStackLightStateRequested("Alarm"); };

            checkStackLightWithBuzzer.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitWorkpiece.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitFeedback.CheckedChanged += OptionControl_CheckedChanged;
        }

        /// <summary>
        /// 应用单个按钮状态。
        /// </summary>
        private static void ApplyButtonState(
            AntdUI.Button button,
            MotionActuatorPageModel.MotionActuatorButtonState state)
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
            var handler = PrimaryActionRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ButtonSecondary_Click(object sender, EventArgs e)
        {
            var handler = SecondaryActionRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void RaiseStackLightStateRequested(string stateKey)
        {
            var handler = StackLightStateRequested;
            if (handler != null)
                handler(this, new StackLightStateRequestedEventArgs(stateKey));
        }

        /// <summary>
        /// 选项变化时通知页面层重新计算按钮启用状态。
        /// 应用状态阶段会被 _suppressOptionChanged 抑制，避免重复刷新。
        /// </summary>
        private void OptionControl_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressOptionChanged)
                return;

            var handler = OptionsChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
            public StackLightStateRequestedEventArgs(string stateKey)
            {
                StateKey = stateKey;
            }

            public string StateKey { get; private set; }
        }
    }
}