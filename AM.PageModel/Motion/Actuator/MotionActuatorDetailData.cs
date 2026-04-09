namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器右侧详情显示对象。
    ///
    /// 【层级定位】
    /// - 所在层：页面显示数据层；
    /// - 上游来源：MotionActuatorDisplayBuilder；
    /// - 下游去向：MotionActuatorDetailControl。
    ///
    /// 【职责】
    /// 1. 仅承载右侧详情区需要显示的数据；
    /// 2. 保持字段语义稳定，避免控件层拼接复杂文本；
    /// 3. 让详情控件只负责 Bind 和显示，不负责状态推导。
    ///
    /// 【设计关系】
    /// - 由 MotionActuatorSnapshot 映射得到；
    /// - 是 Snapshot 的“详情视图”；
    /// - 不承担动作执行与联动校验职责。
    /// </summary>
    public sealed class MotionActuatorDetailData
    {
        /// <summary>
        /// 状态文本。
        /// </summary>
        public string StateText { get; set; }

        /// <summary>
        /// 模式文本。
        /// </summary>
        public string ModeText { get; set; }

        /// <summary>
        /// 主输出文本。
        /// </summary>
        public string PrimaryOutputText { get; set; }

        /// <summary>
        /// 副输出文本。
        /// </summary>
        public string SecondaryOutputText { get; set; }

        /// <summary>
        /// 主反馈文本。
        /// </summary>
        public string PrimaryFeedbackText { get; set; }

        /// <summary>
        /// 副反馈文本。
        /// </summary>
        public string SecondaryFeedbackText { get; set; }

        /// <summary>
        /// 工件检测文本。
        /// </summary>
        public string WorkpieceText { get; set; }

        /// <summary>
        /// 超时配置文本。
        /// </summary>
        public string TimeoutText { get; set; }

        /// <summary>
        /// 运行摘要文本。
        /// </summary>
        public string SummaryText { get; set; }

        /// <summary>
        /// 更新时间文本。
        /// </summary>
        public string UpdateTimeText { get; set; }

        /// <summary>
        /// 最近操作文本。
        /// </summary>
        public string LastActionText { get; set; }

        /// <summary>
        /// 空详情对象。
        /// 便于页面未选中时直接绑定。
        /// </summary>
        public static MotionActuatorDetailData CreateEmpty()
        {
            return new MotionActuatorDetailData
            {
                StateText = "—",
                ModeText = "—",
                PrimaryOutputText = "—",
                SecondaryOutputText = "—",
                PrimaryFeedbackText = "—",
                SecondaryFeedbackText = "—",
                WorkpieceText = "—",
                TimeoutText = "—",
                SummaryText = "—",
                UpdateTimeText = "—",
                LastActionText = "—"
            };
        }
    }
}