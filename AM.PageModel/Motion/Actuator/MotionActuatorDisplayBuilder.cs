namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面显示数据组装器。
    ///
    /// 【当前职责】
    /// 1. 将原始快照映射为左侧列表显示对象；
    /// 2. 将原始快照映射为右侧详情显示对象；
    /// 3. 收口列表与详情所需的稳定显示字段，避免页面模型重复拼装。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorSnapshot；
    /// - 当前层：页面显示数据映射；
    /// - 下游：MotionActuatorVirtualListControl、MotionActuatorDetailControl。
    ///
    /// 【设计说明】
    /// 动作面板状态依赖当前页面选项、动作校验结果和运行态，
    /// 因此由 `MotionActuatorPageModel` 直接生成。
    /// 本类只保留“快照 -> 列表/详情显示对象”的纯映射逻辑。
    /// </summary>
    public sealed class MotionActuatorDisplayBuilder
    {
        /// <summary>
        /// 将原始快照映射为左侧列表卡片对象。
        /// </summary>
        public MotionActuatorListItem BuildListItem(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return null;

            return new MotionActuatorListItem
            {
                ItemKey = snapshot.ItemKey,
                TypeText = snapshot.TypeDisplay,
                TitleText = snapshot.DisplayTitle,
                NameText = snapshot.Name,
                StateText = snapshot.StateText,
                StateLevel = snapshot.StateLevel,
                Line1Text = snapshot.CardLine1Text,
                Line2Text = snapshot.CardLine2Text
            };
        }

        /// <summary>
        /// 将原始快照映射为右侧详情数据。
        /// </summary>
        public MotionActuatorDetailData BuildDetailData(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return MotionActuatorDetailData.CreateEmpty();

            return new MotionActuatorDetailData
            {
                StateText = snapshot.StateText,
                ModeText = snapshot.ControlModeText,
                PrimaryOutputText = snapshot.PrimaryOutputText,
                SecondaryOutputText = snapshot.SecondaryOutputText,
                PrimaryFeedbackText = snapshot.PrimaryFeedbackText,
                SecondaryFeedbackText = snapshot.SecondaryFeedbackText,
                WorkpieceText = snapshot.WorkpieceText,
                TimeoutText = snapshot.TimeoutText,
                SummaryText = snapshot.FooterText,
                UpdateTimeText = snapshot.RuntimeUpdateTimeText,
                LastActionText = snapshot.LastActionMessage
            };
        }
    }
}