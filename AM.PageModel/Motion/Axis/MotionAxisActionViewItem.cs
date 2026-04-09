namespace AM.PageModel.Motion.Axis
{
    /// <summary>
    /// 单轴控制页左侧动作卡片显示项。
    ///
    /// 【当前职责】
    /// 1. 描述单个动作卡片的显示信息；
    /// 2. 承载动作枚举键、标题、分类、强调色和当前可执行状态；
    /// 3. 作为页面模型与左侧动作虚拟列表之间的轻量显示对象。
    ///
    /// 【层级关系】
    /// - 上游：MotionAxisPageModel；
    /// - 下游：MotionAxisVirtualListControl。
    ///
    /// 【设计说明】
    /// 当前页动作数量不多，因此仍直接使用轻量显示项集合，
    /// 不再额外引入动作面板状态模型。
    /// </summary>
    public sealed class MotionAxisActionViewItem
    {
        /// <summary>
        /// 动作键。
        /// 页面模型内部和页面控件统一使用枚举，避免字符串中转。
        /// </summary>
        public MotionAxisActionKey ActionKey { get; set; }

        /// <summary>
        /// 卡片主标题。
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 分类文本。
        /// 例如：基础操作 / 停止 / 点动 / 定位。
        /// </summary>
        public string CategoryText { get; set; }

        /// <summary>
        /// 强调色类型。
        /// 目前仍保持字符串，便于映射到 AntdUI 显示样式。
        /// </summary>
        public string AccentType { get; set; }

        /// <summary>
        /// 当前是否允许执行。
        /// </summary>
        public bool CanExecute { get; set; }

        /// <summary>
        /// 当前不可执行原因。
        /// 用于页面搜索和后续状态说明。
        /// </summary>
        public string DisabledReason { get; set; }
    }
}