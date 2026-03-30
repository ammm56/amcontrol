using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 左下角用户头像菜单控件。
    /// 仅显示头像，点击后弹出用户操作菜单。
    /// </summary>
    public partial class UserAvatarMenuControl : UserControl
    {
        private string _userDisplayName;
        private string _roleDisplayName;
        private string _language;
        private bool _isDarkMode;

        public event EventHandler SwitchUserRequested;
        public event EventHandler ChangePasswordRequested;
        public event EventHandler LogoutRequested;

        public UserAvatarMenuControl()
        {
            InitializeComponent();

            _userDisplayName = "未登录";
            _roleDisplayName = "用户";
            _language = "zh-CN";
            _isDarkMode = false;

            BindEvents();
            RefreshAvatarText();
            ApplyLanguage(_language);
            ApplyTheme(_isDarkMode);
        }

        public string UserDisplayName
        {
            get { return _userDisplayName; }
            set
            {
                _userDisplayName = string.IsNullOrWhiteSpace(value) ? "未登录" : value.Trim();
                RefreshAvatarText();
            }
        }

        public string RoleDisplayName
        {
            get { return _roleDisplayName; }
            set
            {
                _roleDisplayName = string.IsNullOrWhiteSpace(value) ? "用户" : value.Trim();
            }
        }

        public void SetUserInfo(string userDisplayName, string roleDisplayName)
        {
            _userDisplayName = string.IsNullOrWhiteSpace(userDisplayName) ? "未登录" : userDisplayName.Trim();
            _roleDisplayName = string.IsNullOrWhiteSpace(roleDisplayName) ? "用户" : roleDisplayName.Trim();
            RefreshAvatarText();
        }

        public void ApplyLanguage(string language)
        {
            _language = string.IsNullOrWhiteSpace(language) ? "zh-CN" : language;
        }

        public void ApplyTheme(bool isDarkMode)
        {
            _isDarkMode = isDarkMode;

            panelRoot.Back = Color.Transparent;
            BackColor = Color.Transparent;

            avatarCurrentUser.BackColor = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;

            avatarCurrentUser.ForeColor = isDarkMode
                ? Color.Gainsboro
                : Color.FromArgb(24, 39, 58);
        }

        private void BindEvents()
        {
            avatarCurrentUser.Click += AvatarCurrentUser_Click;
            panelRoot.Click += AvatarCurrentUser_Click;
        }

        private void AvatarCurrentUser_Click(object sender, EventArgs e)
        {
            var switchUserText = IsEnglishLanguage(_language) ? "Switch User" : "切换用户";
            var changePasswordText = IsEnglishLanguage(_language) ? "Change Password" : "修改密码";
            var logoutText = IsEnglishLanguage(_language) ? "Logout" : "退出登录";

            var items = new IContextMenuStripItem[]
            {
                new ContextMenuStripItem(switchUserText) { IconSvg = "SwapOutlined" },
                new ContextMenuStripItem(changePasswordText) { IconSvg = "LockOutlined" },
                new ContextMenuStripItem(logoutText) { IconSvg = "LogoutOutlined" }
            };

            AntdUI.ContextMenuStrip.open(avatarCurrentUser, item =>
            {
                if (item == null || string.IsNullOrWhiteSpace(item.Text))
                {
                    return;
                }

                if (string.Equals(item.Text, switchUserText, StringComparison.Ordinal))
                {
                    var handler = SwitchUserRequested;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                    return;
                }

                if (string.Equals(item.Text, changePasswordText, StringComparison.Ordinal))
                {
                    var handler = ChangePasswordRequested;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                    return;
                }

                if (string.Equals(item.Text, logoutText, StringComparison.Ordinal))
                {
                    var handler = LogoutRequested;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }, items);
        }

        private void RefreshAvatarText()
        {
            var text = string.IsNullOrWhiteSpace(_userDisplayName) ? "A" : _userDisplayName;
            avatarCurrentUser.Text = text.Substring(0, 1).ToUpperInvariant();
        }

        private static bool IsEnglishLanguage(string language)
        {
            return !string.IsNullOrWhiteSpace(language)
                && language.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        }
    }
}