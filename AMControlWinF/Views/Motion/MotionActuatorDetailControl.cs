using AM.PageModel.Motion;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧下半区详细信息控件。
    ///
    /// 当前实现：
    /// 1. 采用分行紧凑布局替代原来的纯文本大段拼接；
    /// 2. 重点展示当前对象最常用的结构与运行态信息；
    /// 3. 不承担动作执行，只负责数据显示。
    /// </summary>
    public partial class MotionActuatorDetailControl : UserControl
    {
        public MotionActuatorDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionActuatorPageModel.MotionActuatorViewItem item)
        {
            if (item == null)
            {
                labelStateValue.Text = "—";
                labelModeValue.Text = "—";
                labelPrimaryOutputValue.Text = "—";
                labelSecondaryOutputValue.Text = "—";
                labelPrimaryFeedbackValue.Text = "—";
                labelSecondaryFeedbackValue.Text = "—";
                labelWorkpieceValue.Text = "—";
                labelTimeoutValue.Text = "—";
                labelSummaryValue.Text = "—";
                labelUpdateTimeValue.Text = "—";
                labelLastActionValue.Text = "—";
                return;
            }

            labelStateValue.Text = item.StateText;
            labelModeValue.Text = item.ControlModeText;
            labelPrimaryOutputValue.Text = item.PrimaryOutputText;
            labelSecondaryOutputValue.Text = item.SecondaryOutputText;
            labelPrimaryFeedbackValue.Text = item.PrimaryFeedbackText;
            labelSecondaryFeedbackValue.Text = item.SecondaryFeedbackText;
            labelWorkpieceValue.Text = item.WorkpieceText;
            labelTimeoutValue.Text = item.TimeoutText;
            labelSummaryValue.Text = item.FooterText;
            labelUpdateTimeValue.Text = item.RuntimeUpdateTimeText;
            labelLastActionValue.Text = item.LastActionMessage;
        }
    }
}