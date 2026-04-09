namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器动作按钮状态对象。
    ///
    /// 【层级定位】
    /// - 所在层：页面显示数据层；
    /// - 上游来源：MotionActuatorDisplayBuilder / MotionActuatorPageModel；
    /// - 下游去向：MotionActuatorActionPanelControl。
    ///
    /// 【职责】
    /// 1. 统一描述单个按钮的显示状态；
    /// 2. 避免在动作面板状态中平铺大量重复字段；
    /// 3. 保持按钮渲染逻辑统一。
    ///
    /// 【字段约定】
    /// Type 目前约定使用字符串：
    /// - Default
    /// - Primary
    /// - Success
    /// - Warn
    /// - Error
    ///
    /// 后续如果全项目需要，也可以再统一收敛成枚举。
    /// 当前阶段用字符串更轻量，改动更小。
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