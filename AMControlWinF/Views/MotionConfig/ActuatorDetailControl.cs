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
            ClearSectionContent();

            if (item == null)
            {
                AddDetailItem(stackBasic, "执行器类型", "-");
                AddDetailItem(stackBasic, "对象名称", "-");
                AddDetailItem(stackBasic, "显示名称", "-");
                return;
            }

            var lines = item.DetailLines == null
                ? Enumerable.Empty<ActuatorManagementPageModel.ActuatorDetailLine>()
                : item.DetailLines;

            foreach (var line in lines)
            {
                if (line == null)
                    continue;

                var section = ResolveSection(line.Title);
                AddDetailItem(section, line.Title, line.Value);
            }
        }

        private void ClearSectionContent()
        {
            ClearSection(stackBasic, dividerSectionBasic);
            ClearSection(stackMapping, dividerSectionMapping);
            ClearSection(stackState, dividerSectionState);
            ClearSection(stackRemark, dividerSectionRemark);
        }

        private static void ClearSection(AntdUI.StackPanel section, Control divider)
        {
            if (section == null)
                return;

            for (var i = section.Controls.Count - 1; i >= 0; i--)
            {
                var control = section.Controls[i];
                if (ReferenceEquals(control, divider))
                    continue;

                section.Controls.RemoveAt(i);
                control.Dispose();
            }
        }

        private Control ResolveSection(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return stackBasic;

            if (string.Equals(title, "描述", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(title, "备注", StringComparison.OrdinalIgnoreCase))
            {
                return stackRemark;
            }

            if (title.IndexOf("输出位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("反馈位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("检测位", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("驱动模式", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return stackMapping;
            }

            if (title.IndexOf("超时", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("启用", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("排序", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("校验", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("蜂鸣", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("允许", StringComparison.OrdinalIgnoreCase) >= 0 ||
                title.IndexOf("保持", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return stackState;
            }

            return stackBasic;
        }

        private static void AddDetailItem(Control host, string title, string value)
        {
            var stack = host as AntdUI.StackPanel;
            if (stack == null)
                return;

            var labelTitle = new AntdUI.Label();
            labelTitle.ForeColor = Color.FromArgb(110, 110, 110);
            labelTitle.Margin = new Padding(0);
            labelTitle.Size = new Size(267, 22);
            labelTitle.Text = string.IsNullOrWhiteSpace(title) ? "-" : title;

            var labelValue = new AntdUI.Label();
            labelValue.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            labelValue.Margin = new Padding(0, 0, 0, 6);
            labelValue.Size = new Size(267, 24);
            labelValue.Text = string.IsNullOrWhiteSpace(value) ? "-" : value;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > 24)
            {
                labelValue.AutoEllipsis = false;
                labelValue.Size = new Size(267, 48);
                labelValue.TextAlign = ContentAlignment.TopLeft;
            }

            stack.Controls.Add(labelTitle);
            stack.Controls.Add(labelValue);
        }
    }
}