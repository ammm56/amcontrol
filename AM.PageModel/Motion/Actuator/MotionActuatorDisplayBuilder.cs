namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面显示数据组装器。
    ///
    /// 【最终收口定位】
    /// 第二阶段开始，本类只负责：
    /// 1. 左侧列表显示对象映射；
    /// 2. 右侧详情显示对象映射。
    ///
    /// 动作面板相关逻辑不再放在这里，
    /// 而是直接收口到 `MotionActuatorPageModel` 中统一维护。
    ///
    /// 【这样做的原因】
    /// 动作面板并不是纯显示映射，它依赖：
    /// - 当前选中对象；
    /// - 当前页面选项；
    /// - 当前动作校验结果；
    /// - 当前运行态；
    ///
    /// 因此更适合由页面模型直接生成最终动作面板状态，
    /// 而不是继续通过 Builder 间接组装。
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