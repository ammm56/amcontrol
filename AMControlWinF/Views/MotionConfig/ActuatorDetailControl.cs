using AM.PageModel.MotionConfig;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 执行器详情展示控件。
    /// </summary>
    public partial class ActuatorDetailControl : UserControl
    {
        public ActuatorDetailControl()
        {
            InitializeComponent();
        }

        public void Bind(ActuatorManagementPageModel.ActuatorViewItem item)
        {
            SuspendSectionLayout();
            try
            {
                ClearSectionContent();

                if (item == null)
                {
                    AddDetailItem(flowBasic, "执行器类型", "-");
                    AddDetailItem(flowBasic, "对象名称", "-");
                    AddDetailItem(flowBasic, "显示名称", "-");
                    return;
                }

                var lines = item.DetailLines == null
                    ? Enumerable.Empty<ActuatorManagementPageModel.ActuatorDetailLine>()
                    : item.DetailLines.Where(x => x != null);

                foreach (var line in lines)
                {
                    var section = ResolveSection(line.Title);
                    AddDetailItem(section, line.Title, line.Value);
                }
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
            flowState.SuspendLayout();
            flowRemark.SuspendLayout();
        }

        private void ResumeSectionLayout()
        {
            flowBasic.ResumeLayout();
            flowMapping.ResumeLayout();
            flowState.ResumeLayout();
            flowRemark.ResumeLayout();
        }

        private void ClearSectionContent()
        {
            ClearSection(flowBasic);
            ClearSection(flowMapping);
            ClearSection(flowState);
            ClearSection(flowRemark);
        }

        private static void ClearSection(AntdUI.FlowPanel section)
        {
            if (section == null)
                return;

            for (var i = section.Controls.Count - 1; i >= 0; i--)
            {
                var control = section.Controls[i];
                section.Controls.RemoveAt(i);
                control.Dispose();
            }
        }

        private AntdUI.FlowPanel ResolveSection(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return flowBasic;

            if (string.Equals(title, "描述", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(title, "备注", StringComparison.OrdinalIgnoreCase))
            {
                return flowRemark;
            }

            if (title.IndexOf("输出位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("反馈位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("检测位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("驱动模式", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return flowMapping;
            }

            if (title.IndexOf("超时", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("启用", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("排序", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("校验", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("蜂鸣", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("允许", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("保持", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return flowState;
            }

            return flowBasic;
        }

        private static void AddDetailItem(AntdUI.FlowPanel host, string title, string value)
        {
            if (host == null)
                return;

            var labelValue = new AntdUI.Label();
            labelValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            labelValue.Margin = new Padding(0);
            labelValue.Size = new Size(267, 22);
            labelValue.Text = string.IsNullOrWhiteSpace(value) ? "-" : value;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > 22)
            {
                labelValue.AutoEllipsis = false;
                labelValue.Size = new Size(267, 44);
                labelValue.TextAlign = ContentAlignment.TopLeft;
            }

            var labelTitle = new AntdUI.Label();
            labelTitle.ForeColor = Color.FromArgb(110, 110, 110);
            labelTitle.Margin = new Padding(0, 0, 0, 0);
            labelTitle.Size = new Size(267, 22);
            labelTitle.Text = string.IsNullOrWhiteSpace(title) ? "-" : title;

            host.Controls.Add(labelValue);
            host.Controls.Add(labelTitle);
        }
    }
}