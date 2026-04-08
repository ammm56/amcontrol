using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧上半区控制面板。
    ///
    /// 当前职责：
    /// 1. 显示选中对象标题、副标题；
    /// 2. 提供普通执行器主/副动作按钮；
    /// 3. 提供灯塔状态按钮组；
    /// 4. 提供等待反馈、等待工件、蜂鸣联动三个选项；
    /// 5. 把用户操作通过事件抛给页面层统一处理。
    ///
    /// 说明：
    /// - 控件本身不直接调用服务；
    /// - 联动校验与真实执行统一由 MotionActuatorPage + MotionActuatorPageModel 处理；
    /// - 本控件只负责界面状态呈现与用户输入采集。
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
        /// 这里只处理显示结构切换，不处理动作可执行状态。
        /// </summary>
        public void Bind(MotionActuatorPageModel.MotionActuatorViewItem item)
        {
            _item = item;

            if (item == null)
            {
                labelTitle.Text = "当前对象：未选择";
                labelSubTitle.Text = "—";
                labelHint.Text = "请先在左侧选择一个执行器对象。";

                panelNormalActions.Visible = true;
                panelStackLightActions.Visible = false;

                checkWaitFeedback.Visible = true;
                checkWaitFeedback.Enabled = false;
                checkWaitWorkpiece.Visible = false;
                checkStackLightWithBuzzer.Visible = false;

                buttonPrimary.Text = "主操作";
                buttonSecondary.Text = "副操作";
                buttonPrimary.Enabled = false;
                buttonSecondary.Enabled = false;

                ApplyStackLightButton(buttonStateOff, "熄灭", false, false, "Off");
                ApplyStackLightButton(buttonStateIdle, "空闲", false, false, "Idle");
                ApplyStackLightButton(buttonStateRunning, "运行", false, false, "Running");
                ApplyStackLightButton(buttonStateWarning, "警告", false, false, "Warning");
                ApplyStackLightButton(buttonStateAlarm, "报警", false, false, "Alarm");
                return;
            }

            labelTitle.Text = item.TypeDisplay + " / " + item.DisplayTitle;
            labelSubTitle.Text = item.Name;

            var isStackLight = string.Equals(item.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase);
            var canUseWorkpiece = string.Equals(item.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase)
                || string.Equals(item.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase);

            panelNormalActions.Visible = !isStackLight;
            panelStackLightActions.Visible = isStackLight;

            checkWaitFeedback.Visible = !isStackLight;
            checkWaitFeedback.Enabled = !isStackLight;

            checkWaitWorkpiece.Visible = canUseWorkpiece;
            checkWaitWorkpiece.Enabled = canUseWorkpiece;

            checkStackLightWithBuzzer.Visible = isStackLight;
            checkStackLightWithBuzzer.Enabled = isStackLight;
        }

        /// <summary>
        /// 应用页面模型计算得到的控制面板状态。
        /// </summary>
        public void ApplyActionState(MotionActuatorPageModel.MotionActuatorActionPanelState state)
        {
            if (state == null)
                return;

            labelTitle.Text = state.TitleText ?? "当前对象：未选择";
            labelSubTitle.Text = state.SubTitleText ?? "—";
            labelHint.Text = state.HintText ?? string.Empty;

            panelNormalActions.Visible = state.ShowNormalActions;
            panelStackLightActions.Visible = state.ShowStackLightActions;

            checkWaitFeedback.Visible = state.ShowWaitFeedback;
            checkWaitWorkpiece.Visible = state.ShowWaitWorkpiece;
            checkStackLightWithBuzzer.Visible = state.ShowStackLightWithBuzzer;

            buttonPrimary.Text = string.IsNullOrWhiteSpace(state.PrimaryButtonText) ? "主操作" : state.PrimaryButtonText;
            buttonPrimary.Enabled = state.PrimaryButtonEnabled;

            buttonSecondary.Text = string.IsNullOrWhiteSpace(state.SecondaryButtonText) ? "副操作" : state.SecondaryButtonText;
            buttonSecondary.Enabled = state.SecondaryButtonEnabled;

            ApplyStackLightButton(buttonStateOff, state.OffButtonText, state.OffButtonEnabled, state.IsOffCurrent, "Off");
            ApplyStackLightButton(buttonStateIdle, state.IdleButtonText, state.IdleButtonEnabled, state.IsIdleCurrent, "Idle");
            ApplyStackLightButton(buttonStateRunning, state.RunningButtonText, state.RunningButtonEnabled, state.IsRunningCurrent, "Running");
            ApplyStackLightButton(buttonStateWarning, state.WarningButtonText, state.WarningButtonEnabled, state.IsWarningCurrent, "Warning");
            ApplyStackLightButton(buttonStateAlarm, state.AlarmButtonText, state.AlarmButtonEnabled, state.IsAlarmCurrent, "Alarm");
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

            checkWaitFeedback.CheckedChanged += OptionControl_CheckedChanged;
            checkWaitWorkpiece.CheckedChanged += OptionControl_CheckedChanged;
            checkStackLightWithBuzzer.CheckedChanged += OptionControl_CheckedChanged;
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
                button.Type = TTypeMini.Default;
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
                    button.Type = TTypeMini.Primary;
                    break;
            }
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
        /// 页面层收到后重新计算按钮启用状态与提示文案。
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