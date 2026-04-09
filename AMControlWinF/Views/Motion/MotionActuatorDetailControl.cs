using AM.PageModel.Motion.Actuator;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧下半区详情控件。
    ///
    /// 【当前职责】
    /// 1. 负责显示当前选中执行器的详情信息；
    /// 2. 只依赖 `MotionActuatorDetailData` 进行整体绑定；
    /// 3. 不访问原始快照，不参与动作规则与业务校验。
    ///
    /// 【层级关系】
    /// - 上游：MotionActuatorPage、MotionActuatorPageModel；
    /// - 当前层：WinForms 详情显示控件。
    /// </summary>
    public partial class MotionActuatorDetailControl : UserControl
    {
        #region 构造与绑定

        public MotionActuatorDetailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定详情显示数据。
        ///
        /// 页面只需传入页面模型已组装好的详情对象，
        /// 控件层不再自行拼接文本。
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

        #endregion
    }
}