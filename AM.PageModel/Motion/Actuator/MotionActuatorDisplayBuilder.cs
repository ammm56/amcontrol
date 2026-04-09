using System;

namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面显示数据组装器。
    ///
    /// 【层级定位】
    /// - 所在层：PageModel 内部的显示数据映射层；
    /// - 上游来源：MotionActuatorSnapshot；
    /// - 下游去向：
    ///   - MotionActuatorListItem
    ///   - MotionActuatorDetailData
    ///   - MotionActuatorActionPanelState
    ///
    /// 【职责】
    /// 1. 把原始快照转换成页面各区域需要的数据对象；
    /// 2. 收口界面显示文本、标题、摘要、按钮基础状态等组装逻辑；
    /// 3. 降低 MotionActuatorPageModel 中“运行时状态 + UI 文本”混杂程度。
    ///
    /// 【当前阶段说明】
    /// 第一阶段仅做最小拆分：
    /// - 列表项映射
    /// - 详情项映射
    /// - 动作面板基础状态映射
    ///
    /// 动作按钮是否最终可执行，仍可继续由 MotionActuatorPageModel
    /// 中的 ValidatePrimaryAction / ValidateSecondaryAction / ValidateStackLightState
    /// 负责补充与覆盖。
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

        /// <summary>
        /// 构建右侧动作面板基础状态。
        ///
        /// 说明：
        /// - 当前只负责结构和基础显示文本；
        /// - 最终 Enabled / 当前按钮文案等可由 PageModel 继续补充；
        /// - 这样 Builder 保持轻量，不抢占动作规则职责。
        /// </summary>
        public MotionActuatorActionPanelState BuildBaseActionPanelState(
            MotionActuatorSnapshot snapshot,
            bool waitFeedback,
            bool waitWorkpiece,
            bool stackLightWithBuzzer)
        {
            var state = MotionActuatorActionPanelState.CreateDefault();

            if (snapshot == null)
                return state;

            state.TitleText = snapshot.TypeDisplay + " / " + snapshot.DisplayTitle;
            state.SubTitleText = snapshot.Name;

            var isStackLight = string.Equals(snapshot.ActuatorType, "StackLight", StringComparison.OrdinalIgnoreCase);
            var canUseWorkpiece =
                string.Equals(snapshot.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase)
                || string.Equals(snapshot.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase);

            state.ShowWaitFeedback = !isStackLight;
            state.ShowWaitWorkpiece = canUseWorkpiece;
            state.ShowStackLightWithBuzzer = isStackLight;

            state.WaitFeedback = waitFeedback;
            state.WaitWorkpiece = waitWorkpiece;
            state.StackLightWithBuzzer = stackLightWithBuzzer;

            if (isStackLight)
            {
                state.PrimaryButton.Visible = false;
                state.PrimaryButton.Enabled = false;
                state.SecondaryButton.Visible = false;
                state.SecondaryButton.Enabled = false;

                state.OffButton.Text = "熄灭";
                state.OffButton.Visible = true;
                state.OffButton.Enabled = true;
                state.OffButton.Type = "Default";

                state.IdleButton.Text = "空闲";
                state.IdleButton.Visible = true;
                state.IdleButton.Enabled = true;
                state.IdleButton.Type = "Success";

                state.RunningButton.Text = "运行";
                state.RunningButton.Visible = true;
                state.RunningButton.Enabled = true;
                state.RunningButton.Type = "Primary";

                state.WarningButton.Text = "警告";
                state.WarningButton.Visible = true;
                state.WarningButton.Enabled = true;
                state.WarningButton.Type = "Warn";

                state.AlarmButton.Text = "报警";
                state.AlarmButton.Visible = true;
                state.AlarmButton.Enabled = true;
                state.AlarmButton.Type = "Error";

                return state;
            }

            state.OffButton.Visible = false;
            state.IdleButton.Visible = false;
            state.RunningButton.Visible = false;
            state.WarningButton.Visible = false;
            state.AlarmButton.Visible = false;

            state.PrimaryButton.Text = ResolvePrimaryActionButtonText(snapshot, waitWorkpiece);
            state.PrimaryButton.Visible = true;
            state.PrimaryButton.Enabled = true;
            state.PrimaryButton.Type = ResolvePrimaryButtonType(snapshot);

            state.SecondaryButton.Text = ResolveSecondaryActionButtonText(snapshot);
            state.SecondaryButton.Visible = snapshot.HasSecondaryAction;
            state.SecondaryButton.Enabled = snapshot.HasSecondaryAction;
            state.SecondaryButton.Type = ResolveSecondaryButtonType(snapshot);

            return state;
        }

        private static string ResolvePrimaryActionButtonText(MotionActuatorSnapshot snapshot, bool waitWorkpiece)
        {
            if (snapshot == null)
                return "主操作";

            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    return snapshot.PrimaryState == true && snapshot.SecondaryState != true
                        ? "伸出（已到位）"
                        : "伸出";

                case "Vacuum":
                    if (snapshot.PrimaryState == true && (!waitWorkpiece || snapshot.WorkpieceState != false))
                        return "吸真空（已建立）";

                    return waitWorkpiece ? "吸真空+检测" : "吸真空";

                case "Gripper":
                    if (snapshot.PrimaryState == true && (!waitWorkpiece || snapshot.WorkpieceState != false))
                        return "夹紧（已到位）";

                    return waitWorkpiece ? "夹紧+检测" : "夹紧";

                default:
                    return "主操作";
            }
        }

        private static string ResolveSecondaryActionButtonText(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "副操作";

            switch (snapshot.ActuatorType)
            {
                case "Cylinder":
                    return snapshot.SecondaryState == true && snapshot.PrimaryState != true
                        ? "缩回（已到位）"
                        : "缩回";

                case "Vacuum":
                    return snapshot.SecondaryState == true || snapshot.PrimaryState == false
                        ? "关闭真空（已释放）"
                        : "关闭真空";

                case "Gripper":
                    return snapshot.SecondaryState == true && snapshot.PrimaryState != true
                        ? "打开（已到位）"
                        : "打开";

                default:
                    return "副操作";
            }
        }

        private static string ResolvePrimaryButtonType(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "Primary";

            if (string.Equals(snapshot.ActuatorType, "Gripper", StringComparison.OrdinalIgnoreCase))
                return "Error";

            return "Primary";
        }

        private static string ResolveSecondaryButtonType(MotionActuatorSnapshot snapshot)
        {
            if (snapshot == null)
                return "Default";

            if (string.Equals(snapshot.ActuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase))
                return "Warn";

            return "Success";
        }
    }
}