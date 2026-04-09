using System;

namespace AM.PageModel.Motion.Actuator
{
    /// <summary>
    /// 执行器页面原始快照模型。
    ///
    /// 【当前职责】
    /// 1. 承载页面所需的执行器完整状态与配置快照；
    /// 2. 作为列表、详情和动作面板的统一数据源；
    /// 3. 集中保存运行态、配置态与少量必要衍生字段。
    ///
    /// 【层级关系】
    /// - 上游：MachineContext、RuntimeContext、第三层执行器服务；
    /// - 当前层：页面模型内部原始状态对象；
    /// - 下游：MotionActuatorDisplayBuilder、MotionActuatorPageModel。
    ///
    /// 【调用关系】
    /// 页面模型先从 MachineContext 构建该对象，再刷新运行态，
    /// 然后根据当前页面需要映射为列表显示对象、详情显示对象和动作面板状态。
    /// </summary>
    public sealed class MotionActuatorSnapshot
    {
        /// <summary>
        /// 执行器类型。
        /// 取值约定：
        /// Cylinder / Vacuum / Gripper / StackLight
        /// </summary>
        public string ActuatorType { get; set; }

        /// <summary>
        /// 类型显示文本。
        /// 例如：气缸 / 真空 / 夹爪 / 灯塔
        /// </summary>
        public string TypeDisplay { get; set; }

        /// <summary>
        /// 执行器内部名称。
        /// 在 MachineContext 中通常按 Name 唯一索引。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 执行器显示名称。
        /// 用于页面标题、列表显示等。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序值。
        /// 页面显示时先按类型，再按 SortOrder，再按 Name 排序。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 说明描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 控制模式文本。
        /// 例如：双线圈 / 单线圈 / 吸附保持 / 单段控制
        /// </summary>
        public string ControlModeText { get; set; }

        /// <summary>
        /// 主输出位。
        /// 气缸：伸出输出
        /// 真空：吸真空输出
        /// 夹爪：夹紧输出
        /// </summary>
        public short? PrimaryOutputBit { get; set; }

        /// <summary>
        /// 副输出位。
        /// 气缸：缩回输出
        /// 真空：破真空输出
        /// 夹爪：打开输出
        /// </summary>
        public short? SecondaryOutputBit { get; set; }

        /// <summary>
        /// 主反馈位。
        /// </summary>
        public short? PrimaryFeedbackBit { get; set; }

        /// <summary>
        /// 副反馈位。
        /// </summary>
        public short? SecondaryFeedbackBit { get; set; }

        /// <summary>
        /// 工件检测位。
        /// 真空、夹爪可能使用。
        /// </summary>
        public short? WorkpieceBit { get; set; }

        /// <summary>
        /// 灯塔红灯输出位。
        /// </summary>
        public short? RedOutputBit { get; set; }

        /// <summary>
        /// 灯塔黄灯输出位。
        /// </summary>
        public short? YellowOutputBit { get; set; }

        /// <summary>
        /// 灯塔绿灯输出位。
        /// </summary>
        public short? GreenOutputBit { get; set; }

        /// <summary>
        /// 灯塔蓝灯输出位。
        /// </summary>
        public short? BlueOutputBit { get; set; }

        /// <summary>
        /// 灯塔蜂鸣器输出位。
        /// </summary>
        public short? BuzzerOutputBit { get; set; }

        /// <summary>
        /// 主状态。
        /// 气缸：伸出到位
        /// 真空：真空建立
        /// 夹爪：夹紧到位
        /// </summary>
        public bool? PrimaryState { get; set; }

        /// <summary>
        /// 副状态。
        /// 气缸：缩回到位
        /// 真空：释放到位
        /// 夹爪：打开到位
        /// </summary>
        public bool? SecondaryState { get; set; }

        /// <summary>
        /// 工件状态。
        /// </summary>
        public bool? WorkpieceState { get; set; }

        /// <summary>
        /// 灯塔红灯是否点亮。
        /// </summary>
        public bool? RedOn { get; set; }

        /// <summary>
        /// 灯塔黄灯是否点亮。
        /// </summary>
        public bool? YellowOn { get; set; }

        /// <summary>
        /// 灯塔绿灯是否点亮。
        /// </summary>
        public bool? GreenOn { get; set; }

        /// <summary>
        /// 灯塔蓝灯是否点亮。
        /// </summary>
        public bool? BlueOn { get; set; }

        /// <summary>
        /// 灯塔蜂鸣器是否开启。
        /// </summary>
        public bool? BuzzerOn { get; set; }

        /// <summary>
        /// 动作时是否启用反馈校验。
        /// </summary>
        public bool UseFeedbackCheck { get; set; }

        /// <summary>
        /// 动作时是否启用工件检测校验。
        /// </summary>
        public bool UseWorkpieceCheck { get; set; }

        /// <summary>
        /// 主动作原始名称。
        /// 例如：伸出 / 吸真空 / 夹紧
        /// </summary>
        public string PrimaryActionText { get; set; }

        /// <summary>
        /// 副动作原始名称。
        /// 例如：缩回 / 关闭真空 / 打开
        /// </summary>
        public string SecondaryActionText { get; set; }

        /// <summary>
        /// 是否存在副动作。
        /// 灯塔通常为 false。
        /// </summary>
        public bool HasSecondaryAction { get; set; }

        /// <summary>
        /// 主输出显示文本。
        /// </summary>
        public string PrimaryOutputText { get; set; }

        /// <summary>
        /// 副输出显示文本。
        /// </summary>
        public string SecondaryOutputText { get; set; }

        /// <summary>
        /// 主反馈显示文本。
        /// </summary>
        public string PrimaryFeedbackText { get; set; }

        /// <summary>
        /// 副反馈显示文本。
        /// </summary>
        public string SecondaryFeedbackText { get; set; }

        /// <summary>
        /// 工件检测显示文本。
        /// </summary>
        public string WorkpieceText { get; set; }

        /// <summary>
        /// 超时或联动说明文本。
        /// </summary>
        public string TimeoutText { get; set; }

        /// <summary>
        /// 列表第一行摘要文本。
        /// </summary>
        public string CardLine1Text { get; set; }

        /// <summary>
        /// 列表第二行摘要文本。
        /// </summary>
        public string CardLine2Text { get; set; }

        /// <summary>
        /// 当前状态文本。
        /// 例如：伸出到位 / 已吸附 / 反馈冲突 / 全灭
        /// </summary>
        public string StateText { get; set; }

        /// <summary>
        /// 当前状态级别。
        /// 约定取值：
        /// Danger / Warning / Success / Primary / Secondary
        /// </summary>
        public string StateLevel { get; set; }

        /// <summary>
        /// 详细说明文本。
        /// </summary>
        public string DetailText { get; set; }

        /// <summary>
        /// 运行摘要文本。
        /// </summary>
        public string FooterText { get; set; }

        /// <summary>
        /// 最近一次运行态刷新时间文本。
        /// </summary>
        public string RuntimeUpdateTimeText { get; set; }

        /// <summary>
        /// 最近动作结果文本。
        /// </summary>
        public string LastActionMessage { get; set; }

        /// <summary>
        /// 最近动作级别。
        /// 约定取值：
        /// Danger / Warning / Success / Primary / Secondary
        /// </summary>
        public string LastActionLevel { get; set; }

        /// <summary>
        /// 当前对象是否带故障。
        /// </summary>
        public bool HasFault { get; set; }

        /// <summary>
        /// 当前对象唯一键。
        /// 页面列表、选中恢复、控件选择统一依赖该值。
        /// </summary>
        public string ItemKey
        {
            get { return (ActuatorType ?? string.Empty) + "|" + (Name ?? string.Empty); }
        }

        /// <summary>
        /// 当前对象标题显示文本。
        /// 优先 DisplayName，回退到 Name。
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                    return DisplayName;

                return string.IsNullOrWhiteSpace(Name) ? "未命名对象" : Name;
            }
        }

        /// <summary>
        /// 当前灯塔是否配置了任意输出位。
        /// </summary>
        public bool HasAnyStackLightOutput
        {
            get
            {
                return RedOutputBit.HasValue
                    || YellowOutputBit.HasValue
                    || GreenOutputBit.HasValue
                    || BlueOutputBit.HasValue
                    || BuzzerOutputBit.HasValue;
            }
        }
    }
}