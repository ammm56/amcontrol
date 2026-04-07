using AM.PageModel.Motion;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DI 详情展示控件。
    /// 页面只负责传入选中项，详情内部自行分区渲染。
    /// </summary>
    public partial class DIMotionDetailControl : UserControl
    {
        public DIMotionDetailControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定当前选中 DI 项。
        /// </summary>
        public void Bind(DIMotionPageModel.DIMotionIoViewItem item)
        {
            SuspendSectionLayout();
            try
            {
                ClearSection(flowBasic);
                ClearSection(flowMapping);
                ClearSection(flowRuntime);
                ClearSection(flowRemark);

                if (item == null)
                {
                    panelEmpty.Visible = true;
                    gridDetails.Visible = false;
                    return;
                }

                panelEmpty.Visible = false;
                gridDetails.Visible = true;

                AddDetailItem(flowBasic, "显示名称", item.DisplayTitle, item.CurrentValue);
                AddDetailItem(flowBasic, "内部名称", string.IsNullOrWhiteSpace(item.Name) ? "—" : item.Name, null);
                AddDetailItem(flowBasic, "逻辑位号", item.LogicalBit.ToString(), null);
                AddDetailItem(flowBasic, "当前状态", item.ValueText, item.CurrentValue);

                AddDetailItem(flowMapping, "控制卡", item.CardText, null);
                AddDetailItem(flowMapping, "信号类型", item.TypeText, null);
                AddDetailItem(flowMapping, "内核", item.CoreText, null);
                AddDetailItem(flowMapping, "硬件位号", item.HardwareBitText, null);
                AddDetailItem(flowMapping, "板载/扩展", item.ModuleText, null);
                AddDetailItem(flowMapping, "硬件地址", item.HardwareAddressText, null);

                AddDetailItem(flowRuntime, "最后更新时间", item.LastUpdateTimeText, null);
                AddDetailItem(flowRuntime, "关联对象", item.LinkObjectDisplayText, null);

                AddDetailItem(flowRemark, "说明", item.DescriptionText, null);
                AddDetailItem(flowRemark, "备注", item.RemarkText, null);
            }
            finally
            {
                ResumeSectionLayout();
            }
        }

        private void SuspendSectionLayout()
        {
            flowBasic.SuspendLayout();
            flowMapping.SuspendLayout();
            flowRuntime.SuspendLayout();
            flowRemark.SuspendLayout();
        }

        private void ResumeSectionLayout()
        {
            flowBasic.ResumeLayout();
            flowMapping.ResumeLayout();
            flowRuntime.ResumeLayout();
            flowRemark.ResumeLayout();
        }

        private static void ClearSection(AntdUI.FlowPanel host)
        {
            if (host == null)
                return;

            for (var i = host.Controls.Count - 1; i >= 0; i--)
            {
                var control = host.Controls[i];
                host.Controls.RemoveAt(i);
                control.Dispose();
            }
        }

        /// <summary>
        /// 动态创建详情项，减少 Designer 中大量静态 Label。
        /// </summary>
        private static void AddDetailItem(AntdUI.FlowPanel host, string title, string value, bool? state)
        {
            if (host == null)
                return;

            var labelValue = new AntdUI.Label();
            labelValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            labelValue.Margin = new Padding(0);
            labelValue.Size = new Size(172, 36);
            labelValue.Text = string.IsNullOrWhiteSpace(value) ? "—" : value;
            labelValue.TextAlign = ContentAlignment.MiddleLeft;

            if (state.HasValue)
            {
                labelValue.ForeColor = state.Value
                    ? Color.FromArgb(82, 196, 26)
                    : Color.FromArgb(245, 34, 45);
            }

            var labelTitle = new AntdUI.Label();
            labelTitle.ForeColor = Color.FromArgb(120, 120, 120);
            labelTitle.Margin = new Padding(0);
            labelTitle.Size = new Size(172, 20);
            labelTitle.Text = string.IsNullOrWhiteSpace(title) ? "-" : title;
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;

            host.Controls.Add(labelValue);
            host.Controls.Add(labelTitle);
        }
    }
}