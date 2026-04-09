namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器左侧虚拟卡片显示对象。
    ///
    /// 【层级定位】
    /// - 所在层：页面显示数据层；
    /// - 上游来源：MotionActuatorDisplayBuilder；
    /// - 下游去向：MotionActuatorVirtualListControl。
    ///
    /// 【职责】
    /// 1. 仅为左侧列表卡片提供最小必要显示数据；
    /// 2. 不混入详情区字段；
    /// 3. 不混入动作面板字段；
    /// 4. 只保留列表渲染和选择所需的信息。
    ///
    /// 【设计关系】
    /// - 由 MotionActuatorSnapshot 映射而来；
    /// - 可视为 Snapshot 的“列表视图”；
    /// - 页面切换选中时通过 ItemKey 回到 Snapshot。
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