namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器左侧虚拟卡片显示对象。
    ///
    /// 【当前职责】
    /// 1. 仅承载左侧卡片渲染所需字段；
    /// 2. 保持列表显示与详情、动作区解耦；
    /// 3. 通过 `ItemKey` 与页面模型中的原始快照建立关联。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorSnapshot、MotionActuatorDisplayBuilder；
    /// - 下游：MotionActuatorVirtualListControl。
    /// </summary>
    public sealed class MotionActuatorListItem
    {
        /// <summary>
        /// 对应原始执行器快照的唯一键。
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        /// 类型显示文本。
        /// 例如：气缸 / 真空 / 夹爪 / 灯塔
        /// </summary>
        public string TypeText { get; set; }

        /// <summary>
        /// 卡片标题。
        /// </summary>
        public string TitleText { get; set; }

        /// <summary>
        /// 内部名称显示文本。
        /// </summary>
        public string NameText { get; set; }

        /// <summary>
        /// 状态显示文本。
        /// </summary>
        public string StateText { get; set; }

        /// <summary>
        /// 状态级别。
        /// 用于决定卡片状态色。
        /// </summary>
        public string StateLevel { get; set; }

        /// <summary>
        /// 卡片第一行摘要。
        /// </summary>
        public string Line1Text { get; set; }

        /// <summary>
        /// 卡片第二行摘要。
        /// </summary>
        public string Line2Text { get; set; }
    }
}