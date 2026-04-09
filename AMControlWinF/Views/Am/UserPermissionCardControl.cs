using AM.PageModel.Am;
using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户权限卡片控件。
    /// 仅负责展示和编辑单个页面权限项。
    /// </summary>
    public partial class UserPermissionCardControl : UserControl
    {
        private UserPermissionPageModel.PermissionCardItem _item;
        private bool _isBinding;

        public UserPermissionCardControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public void Bind(UserPermissionPageModel.PermissionCardItem item)
        {
            _item = item;
            _isBinding = true;
            try
            {
                labelTitle.Text = item == null ? string.Empty : item.DisplayName ?? string.Empty;
                labelDescription.Text = item == null || string.IsNullOrWhiteSpace(item.Description)
                    ? "暂无说明。"
                    : item.Description;
                labelRecommendedRoles.Text = "建议角色：" + (
                    item == null || string.IsNullOrWhiteSpace(item.RecommendedRoles)
                        ? "-"
                        : item.RecommendedRoles);
                checkPermission.Checked = item != null && item.IsChecked;

                ApplyRiskStyle(item == null ? string.Empty : item.RiskLevel);
            }
            finally
            {
                _isBinding = false;
            }
        }

        private void BindEvents()
        {
            checkPermission.CheckedChanged += CheckPermission_CheckedChanged;
        }

        private void CheckPermission_CheckedChanged(object sender, EventArgs e)
        {
            if (_isBinding || _item == null)
                return;

            _item.IsChecked = checkPermission.Checked;
        }

        private void ApplyRiskStyle(string riskLevel)
        {
            var text = string.IsNullOrWhiteSpace(riskLevel) ? "未定义" : riskLevel;
            buttonRisk.Text = "权限：" + text;

            if (string.Equals(text, "高", StringComparison.OrdinalIgnoreCase))
            {
                buttonRisk.Type = TTypeMini.Error;
                return;
            }

            if (string.Equals(text, "中", StringComparison.OrdinalIgnoreCase))
            {
                buttonRisk.Type = TTypeMini.Warn;
                return;
            }

            buttonRisk.Type = TTypeMini.Success;
        }
    }
}