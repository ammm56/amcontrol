using AM.PageModel.Motion;
using AntdUI;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧下半区详细信息控件。
    /// 当前第一步只负责显示信息，不承担动作执行。
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
                labelTitle.Text = "未选择执行器";
                labelDetail.Text = "请先在左侧选择一个执行器对象。";
                return;
            }

            labelTitle.Text = item.TypeDisplay + " / " + item.DisplayTitle;

            labelDetail.Text =
                "内部名称：" + item.Name + System.Environment.NewLine +
                "模式：" + item.ControlModeText + System.Environment.NewLine +
                item.PrimaryOutputText + System.Environment.NewLine +
                item.SecondaryOutputText + System.Environment.NewLine +
                item.PrimaryFeedbackText + System.Environment.NewLine +
                item.SecondaryFeedbackText + System.Environment.NewLine +
                item.WorkpieceText + System.Environment.NewLine +
                item.TimeoutText + System.Environment.NewLine +
                "状态：" + item.StateText + System.Environment.NewLine +
                "说明：" + item.DetailText + System.Environment.NewLine +
                "摘要：" + item.FooterText + System.Environment.NewLine +
                "更新时间：" + item.RuntimeUpdateTimeText + System.Environment.NewLine +
                item.LastActionMessage;
        }
    }
}