using AM.PageModel.Motion;
using AntdUI;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器右侧上半区控制面板骨架。
    ///
    /// 当前第一步仅完成：
    /// 1. 选中对象标题显示；
    /// 2. 主/副动作按钮文字切换；
    /// 3. 灯塔状态按钮区域与普通执行器动作区域切换；
    ///
    /// 下一步再补：
    /// - 实际动作执行事件；
    /// - 等待反馈 / 等待工件 / 蜂鸣联动；
    /// - 更细的联动禁用逻辑。
    /// </summary>
    public partial class MotionActuatorActionPanelControl : UserControl
    {
        public MotionActuatorActionPanelControl()
        {
            InitializeComponent();
        }

        public void Bind(MotionActuatorPageModel.MotionActuatorViewItem item)
        {
            if (item == null)
            {
                labelTitle.Text = "当前对象：未选择";
                labelSubTitle.Text = "—";

                panelNormalActions.Visible = true;
                panelStackLightActions.Visible = false;

                buttonPrimary.Text = "主操作";
                buttonSecondary.Text = "副操作";
                buttonPrimary.Enabled = false;
                buttonSecondary.Enabled = false;
                return;
            }

            labelTitle.Text = item.TypeDisplay + " / " + item.DisplayTitle;
            labelSubTitle.Text = item.Name;

            if (item.ActuatorType == "StackLight")
            {
                panelNormalActions.Visible = false;
                panelStackLightActions.Visible = true;
            }
            else
            {
                panelNormalActions.Visible = true;
                panelStackLightActions.Visible = false;

                buttonPrimary.Text = string.IsNullOrWhiteSpace(item.PrimaryActionText) ? "主操作" : item.PrimaryActionText;
                buttonSecondary.Text = string.IsNullOrWhiteSpace(item.SecondaryActionText) ? "副操作" : item.SecondaryActionText;
                buttonPrimary.Enabled = true;
                buttonSecondary.Enabled = item.HasSecondaryAction;
            }
        }
    }
}