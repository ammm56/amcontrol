namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器右侧详情显示对象。
    ///
    /// 【当前职责】
    /// 1. 承载右侧详情区的稳定显示字段；
    /// 2. 避免详情控件自行拼接文本或访问原始快照；
    /// 3. 让页面刷新时只需整体 Bind 一个对象。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorSnapshot、MotionActuatorDisplayBuilder；
    /// - 下游：MotionActuatorDetailControl。
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