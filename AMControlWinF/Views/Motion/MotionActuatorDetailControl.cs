using AM.PageModel.Motion.Actuator;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧下半区详细信息控件。
    ///
    /// 【层级定位】
    /// - 所在层：WinForms 显示控件层；
    /// - 上游来源：MotionActuatorPage / MotionActuatorPageModel；
    /// - 下游职责：只负责把详情数据显示到界面标签。
    ///
    /// 【职责】
    /// 1. 显示当前选中执行器的详细信息；
    /// 2. 只依赖专门的详情显示对象 MotionActuatorDetailData；
    /// 3. 不再依赖页面原始快照或动作面板状态；
    /// 4. 不承担动作执行、状态推导、业务校验。
    ///
    /// 【本轮重构意义】
    /// 旧实现直接依赖 MotionActuatorViewItem，一个对象同时承担：
    /// - 原始状态
    /// - 列表显示
    /// - 详情显示
    /// - 动作面板输入
    ///
    /// 第一轮适配后：
    /// - 本控件只依赖 MotionActuatorDetailData；
    /// - 页面层负责把 SelectedSnapshot 转换成 DetailData；
    /// - 详情控件与列表/动作面板的数据对象解耦。
    /// </summary>
    public partial class MotionActuatorDetailControl : UserControl
    {
        public MotionActuatorDetailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定详情显示数据。
        ///
        /// 说明：
        /// - data 为页面模型已组装好的详情对象；
        /// - 控件层不再拼接显示文本；
        /// - 未选中对象时可直接传入 null，内部自动显示空值。
        /// </summary>
        public void Bind(MotionActuatorDetailData data)
        {
            if (data == null)
                data = MotionActuatorDetailData.CreateEmpty();

            labelStateValue.Text = data.StateText;
            labelModeValue.Text = data.ModeText;
            labelPrimaryOutputValue.Text = data.PrimaryOutputText;
            labelSecondaryOutputValue.Text = data.SecondaryOutputText;
            labelPrimaryFeedbackValue.Text = data.PrimaryFeedbackText;
            labelSecondaryFeedbackValue.Text = data.SecondaryFeedbackText;
            labelWorkpieceValue.Text = data.WorkpieceText;
            labelTimeoutValue.Text = data.TimeoutText;
            labelSummaryValue.Text = data.SummaryText;
            labelUpdateTimeValue.Text = data.UpdateTimeText;
            labelLastActionValue.Text = data.LastActionText;
        }
    }
}