namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器右侧动作面板显示状态。
    ///
    /// 【当前职责】
    /// 1. 承载动作面板标题、副标题、选项状态与按钮状态；
    /// 2. 只表达界面应该如何显示，不承担执行逻辑；
    /// 3. 作为 `MotionActuatorPageModel` 与 `MotionActuatorActionPanelControl` 之间的稳定绑定对象。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorPageModel；
    /// - 下游：MotionActuatorActionPanelControl。
    /// </summary>
    public sealed class MotionActuatorActionPanelState
    {
        /// <summary>
        /// 标题文本。
        /// 例如：气缸 / 上料气缸
        /// </summary>
        public string TitleText { get; set; }

        /// <summary>
        /// 副标题文本。
        /// 一般使用执行器内部名称。
        /// </summary>
        public string SubTitleText { get; set; }

        /// <summary>
        /// 是否显示“等待反馈”选项。
        /// </summary>
        public bool ShowWaitFeedback { get; set; }

        /// <summary>
        /// 是否显示“等待工件检测”选项。
        /// </summary>
        public bool ShowWaitWorkpiece { get; set; }

        /// <summary>
        /// 是否显示“附带蜂鸣”选项。
        /// </summary>
        public bool ShowStackLightWithBuzzer { get; set; }

        /// <summary>
        /// 当前是否勾选等待反馈。
        /// </summary>
        public bool WaitFeedback { get; set; }

        /// <summary>
        /// 当前是否勾选等待工件检测。
        /// </summary>
        public bool WaitWorkpiece { get; set; }

        /// <summary>
        /// 当前是否勾选附带蜂鸣。
        /// </summary>
        public bool StackLightWithBuzzer { get; set; }

        /// <summary>
        /// 普通执行器主按钮。
        /// </summary>
        public MotionActuatorButtonState PrimaryButton { get; private set; }

        /// <summary>
        /// 普通执行器副按钮。
        /// </summary>
        public MotionActuatorButtonState SecondaryButton { get; private set; }

        /// <summary>
        /// 灯塔：熄灭。
        /// </summary>
        public MotionActuatorButtonState OffButton { get; private set; }

        /// <summary>
        /// 灯塔：空闲。
        /// </summary>
        public MotionActuatorButtonState IdleButton { get; private set; }

        /// <summary>
        /// 灯塔：运行。
        /// </summary>
        public MotionActuatorButtonState RunningButton { get; private set; }

        /// <summary>
        /// 灯塔：警告。
        /// </summary>
        public MotionActuatorButtonState WarningButton { get; private set; }

        /// <summary>
        /// 灯塔：报警。
        /// </summary>
        public MotionActuatorButtonState AlarmButton { get; private set; }

        public MotionActuatorActionPanelState()
        {
            PrimaryButton = new MotionActuatorButtonState();
            SecondaryButton = new MotionActuatorButtonState();

            OffButton = new MotionActuatorButtonState();
            IdleButton = new MotionActuatorButtonState();
            RunningButton = new MotionActuatorButtonState();
            WarningButton = new MotionActuatorButtonState();
            AlarmButton = new MotionActuatorButtonState();
        }

        /// <summary>
        /// 创建默认空状态。
        /// 用于未选择对象时绑定右侧动作面板。
        /// </summary>
        public static MotionActuatorActionPanelState CreateDefault()
        {
            var state = new MotionActuatorActionPanelState();

            state.TitleText = "当前对象：未选择";
            state.SubTitleText = "—";

            state.ShowWaitFeedback = true;
            state.WaitFeedback = true;

            state.PrimaryButton.Text = "主操作";
            state.PrimaryButton.Visible = true;
            state.PrimaryButton.Enabled = false;
            state.PrimaryButton.Type = "Primary";

            state.SecondaryButton.Text = "副操作";
            state.SecondaryButton.Visible = true;
            state.SecondaryButton.Enabled = false;
            state.SecondaryButton.Type = "Default";

            return state;
        }
    }
}