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
    /// 当前实现原则：
    /// 1. 结构尽量简单，避免多层嵌套与复杂提示区；
    /// 2. 整体固定为三行：
    ///    - 第一行：标题 / 副标题；
    ///    - 第二行：控制选项；
    ///    - 第三行：动作按钮区；
    /// 3. 所有动作按钮统一放在同一个 FlowPanel 中，
    ///    不同执行器只通过 Visible / Enabled 控制显示与禁用；
    /// 4. 控件本身不直接调用服务，只抛出事件给页面层统一处理。
    /// </summary>
    public partial class MotionActuatorActionPanelControl : UserControl
    {
        private MotionActuatorPageModel.MotionActuatorViewItem _item;

        public MotionActuatorActionPanelControl()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// 当前是否勾选等待反馈。
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
        /// 绑定当前选中对象。
        /// 这里只负责基础结构切换，不负责动作可执行状态计算。
        /// </summary>
        public void Bind(MotionActuatorPageModel.MotionActuatorViewItem item)
        {
            _item = item;

            if (item == null)
            {
                labelTitle.Text = "当前对象：未选择";
                labelSubTitle.Text = "内部名称：—";

                checkStackLightWithBuzzer.Visible = false;
                checkWaitWorkpiece.Visible = false;
                checkWaitFeedback.Visible = true;
                checkWaitFeedback.Enabled = false;

                SetButtonVisible(buttonPrimary, true);
                SetButtonVisible(buttonSecondary, true);

                SetButtonVisible(buttonStateOff, false);
                SetButtonVisible(buttonStateIdle, false);
                SetButtonVisible(buttonStateRunning, false);
                SetButtonVisible(buttonStateWarning, false);
                SetButtonVisible(buttonStateAlarm, false);

                buttonPrimary.Text = "主操作";
                buttonSecondary.Text = "副操作";
                buttonPrimary.Enabled = false;
                buttonSecondary.Enabled = false;

                UpdateButtonWidths();
                RefreshLayoutState();
                return;
            }

            labelTitle.Text = item.TypeDisplay + " / " + item.DisplayTitle;
            labelSubTitle.Text = "内部名称：" + item.Name;

            var isStackLight = string.Equals(item.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase);
            var canUseWorkpiece =
                string.Equals(item.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase)
                || string.Equals(item.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase);

            checkStackLightWithBuzzer.Visible = isStackLight;
            checkStackLightWithBuzzer.Enabled = isStackLight;

            checkWaitWorkpiece.Visible = canUseWorkpiece;
            checkWaitWorkpiece.Enabled = canUseWorkpiece;

            checkWaitFeedback.Visible = !isStackLight;
            checkWaitFeedback.Enabled = !isStackLight;

            SetButtonVisible(buttonPrimary, !isStackLight);
            SetButtonVisible(buttonSecondary, !isStackLight);

            SetButtonVisible(buttonStateOff, isStackLight);
            SetButtonVisible(buttonStateIdle, isStackLight);
            SetButtonVisible(buttonStateRunning, isStackLight);
            SetButtonVisible(buttonStateWarning, isStackLight);
            SetButtonVisible(buttonStateAlarm, isStackLight);

            UpdateButtonWidths();
            RefreshLayoutState();
        }

        /// <summary>
        /// 应用页面模型计算出的控制状态。
        /// 这里只做显示，不再保留额外文字提示区。
        /// </summary>
        public void ApplyActionState(MotionActuatorPageModel.MotionActuatorActionPanelState state)
        {
            if (state == null)
                return;

            labelTitle.Text = string.IsNullOrWhiteSpace(state.TitleText)
                ? "当前对象：未选择"
                : state.TitleText;

            labelSubTitle.Text = string.IsNullOrWhiteSpace(state.SubTitleText)
                ? "内部名称：—"
                : "内部名称：" + state.SubTitleText.Replace("内部名称：", string.Empty);

            checkStackLightWithBuzzer.Visible = state.ShowStackLightWithBuzzer;
            checkWaitWorkpiece.Visible = state.ShowWaitWorkpiece;
            checkWaitFeedback.Visible = state.ShowWaitFeedback;

            SetCheckedSilently(checkStackLightWithBuzzer, state.StackLightWithBuzzer);
            SetCheckedSilently(checkWaitWorkpiece, state.WaitWorkpiece);
            SetCheckedSilently(checkWaitFeedback, state.WaitFeedback);

            SetButtonVisible(buttonPrimary, state.ShowNormalActions);
            SetButtonVisible(buttonSecondary, state.ShowNormalActions);

            SetButtonVisible(buttonStateOff, state.ShowStackLightActions);
            SetButtonVisible(buttonStateIdle, state.ShowStackLightActions);
            SetButtonVisible(buttonStateRunning, state.ShowStackLightActions);
            SetButtonVisible(buttonStateWarning, state.ShowStackLightActions);
            SetButtonVisible(buttonStateAlarm, state.ShowStackLightActions);

            buttonPrimary.Text = string.IsNullOrWhiteSpace(state.PrimaryButtonText)
                ? "主操作"
                : state.PrimaryButtonText;
            buttonPrimary.Enabled = state.PrimaryButtonEnabled;

            buttonSecondary.Text = string.IsNullOrWhiteSpace(state.SecondaryButtonText)
                ? "副操作"
                : state.SecondaryButtonText;
            buttonSecondary.Enabled = state.SecondaryButtonEnabled;

            ApplyNormalActionButtonStyle();

            ApplyStackLightButton(buttonStateOff, state.OffButtonText, state.OffButtonEnabled, state.IsOffCurrent, "Off");
            ApplyStackLightButton(buttonStateIdle, state.IdleButtonText, state.IdleButtonEnabled, state.IsIdleCurrent, "Idle");
            ApplyStackLightButton(buttonStateRunning, state.RunningButtonText, state.RunningButtonEnabled, state.IsRunningCurrent, "Running");
            ApplyStackLightButton(buttonStateWarning, state.WarningButtonText, state.WarningButtonEnabled, state.IsWarningCurrent, "Warning");
            ApplyStackLightButton(buttonStateAlarm, state.AlarmButtonText, state.AlarmButtonEnabled, state.IsAlarmCurrent, "Alarm");

            UpdateButtonWidths();
            RefreshLayoutState();
        }

        private void BindEvents()
        {
            buttonPrimary.Click += ButtonPrimary_Click;
            buttonSecondary.Click += ButtonSecondary_Click;

            buttonStateOff.Click += (s, e) => RaiseStackLightStateRequested("Off");
            buttonStateIdle.Click += (s, e) => RaiseStackLightStateRequested("Idle");
            buttonStateRunning.Click += (s, e) => RaiseStackLightStateRequested("Running");
            buttonStateWarning.Click += (s, e) => RaiseStackLightStateRequested("Warning");
            buttonStateAlarm.Click += (s, e) => RaiseStackLightStateRequested("Alarm");

            checkStackLightWithBuzzer.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitWorkpiece.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitFeedback.CheckedChanged += OptionControl_CheckedChanged;
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

        private void OptionControl_CheckedChanged(object sender, EventArgs e)
        {
            var handler = OptionsChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// 普通执行器主/副按钮样式。
        /// 这里保持简单：
        /// - 真空：主蓝、副黄
        /// - 夹爪：主紫、副绿（用 Error/Success 近似表达）
        /// - 气缸：主蓝、副绿
        /// </summary>
        private void ApplyNormalActionButtonStyle()
        {
            if (_item == null)
            {
                buttonPrimary.Type = TTypeMini.Primary;
                buttonSecondary.Type = TTypeMini.Default;
                return;
            }

            if (string.Equals(_item.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase))
            {
                buttonPrimary.Type = TTypeMini.Primary;
                buttonSecondary.Type = TTypeMini.Warn;
                return;
            }

            if (string.Equals(_item.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase))
            {
                buttonPrimary.Type = TTypeMini.Error;
                buttonSecondary.Type = TTypeMini.Success;
                return;
            }

            buttonPrimary.Type = TTypeMini.Primary;
            buttonSecondary.Type = TTypeMini.Success;
        }

        private static void ApplyStackLightButton(
            AntdUI.Button button,
            string text,
            bool enabled,
            bool isCurrent,
            string stateKey)
        {
            if (button == null)
                return;

            button.Text = string.IsNullOrWhiteSpace(text) ? "状态" : text;
            button.Enabled = enabled;

            if (!isCurrent)
            {
                switch (stateKey)
                {
                    case "Warning":
                        button.Type = TTypeMini.Warn;
                        break;

                    case "Alarm":
                        button.Type = TTypeMini.Error;
                        break;

                    case "Idle":
                    case "Running":
                        button.Type = TTypeMini.Primary;
                        break;

                    default:
                        button.Type = TTypeMini.Default;
                        break;
                }

                return;
            }

            switch (stateKey)
            {
                case "Warning":
                    button.Type = TTypeMini.Warn;
                    break;

                case "Alarm":
                    button.Type = TTypeMini.Error;
                    break;

                case "Idle":
                case "Running":
                    button.Type = TTypeMini.Success;
                    break;

                default:
                    button.Type = TTypeMini.Default;
                    break;
            }
        }

        private static void SetButtonVisible(Control control, bool visible)
        {
            if (control == null)
                return;

            control.Visible = visible;
        }

        private static void SetCheckedSilently(Checkbox checkbox, bool value)
        {
            if (checkbox == null)
                return;

            if (checkbox.Checked != value)
                checkbox.Checked = value;
        }

        /// <summary>
        /// 根据按钮文本自动调整宽度，避免“当前”文案变长后显示不全。
        /// 第三行使用 FlowPanel，可自动换行。
        /// </summary>
        private void UpdateButtonWidths()
        {
            ResizeButton(buttonPrimary, 92, 160);
            ResizeButton(buttonSecondary, 92, 160);

            ResizeButton(buttonStateOff, 60, 110);
            ResizeButton(buttonStateIdle, 60, 110);
            ResizeButton(buttonStateRunning, 60, 110);
            ResizeButton(buttonStateWarning, 60, 110);
            ResizeButton(buttonStateAlarm, 60, 110);
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
        /// 控件显隐切换后，主动触发布局刷新。
        /// 页面缓存复用时这样更稳。
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