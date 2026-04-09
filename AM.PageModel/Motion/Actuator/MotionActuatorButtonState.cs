namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器动作按钮状态对象。
    ///
    /// 【当前职责】
    /// 1. 描述单个按钮的文本、显隐、可用状态和样式；
    /// 2. 作为动作面板状态中的最小重复单元；
    /// 3. 让控件层只做统一渲染，不再区分按钮来源。
    ///
    /// 【字段约定】
    /// Type 目前约定使用字符串：
    /// - Default
    /// - Primary
    /// - Success
    /// - Warn
    /// - Error
    ///
    /// 当前仍使用字符串，便于与现有 AntdUI 按钮类型映射保持轻量实现。
    /// </summary>
    public sealed class MotionActuatorButtonState
    {
        /// <summary>
        /// 按钮文本。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否显示。
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 是否可点击。
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 按钮样式类型。
        /// </summary>
        public string Type { get; set; }

        public MotionActuatorButtonState()
        {
            Text = string.Empty;
            Visible = false;
            Enabled = false;
            Type = "Default";
        }
    }
}