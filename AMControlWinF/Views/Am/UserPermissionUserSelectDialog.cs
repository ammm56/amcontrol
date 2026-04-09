using AM.Model.Auth;
using AM.Core.Context;
using AMControlWinF.Tools;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户选择窗口。
    /// 内容区复用 UserManagementPage。
    /// </summary>
    public partial class UserPermissionUserSelectDialog : AntdUI.Window
    {
        private readonly UserManagementPage _userManagementPage;

        public UserPermissionUserSelectDialog()
        {
            InitializeComponent();

            _userManagementPage = new UserManagementPage();
            _userManagementPage.Dock = DockStyle.Fill;
            _userManagementPage.SetSelectionMode(true);
            panelContentHost.Controls.Add(_userManagementPage);

            BindEvents();
            ApplyThemeFromConfig();
        }

        public UserSummary SelectedUser { get; private set; }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            var selectedUser = _userManagementPage.SelectedUserSummary;
            if (selectedUser == null)
            {
                PageDialogHelper.ShowWarn(this, "选择用户", "请先选择一个用户。");
                return;
            }

            SelectedUser = selectedUser;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = !string.IsNullOrWhiteSpace(theme) &&
                             (string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }
    }
}