using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 用户头像弹出操作卡片。
    /// 使用 AntdUI 默认原生样式，自动跟随主题切换。
    /// </summary>
    public partial class UserAvatarPopoverCard : UserControl
    {
        private string _userDisplayName;
        private string _roleDisplayName;
        private string _language;

        public event EventHandler SwitchUserRequested;
        public event EventHandler ChangePasswordRequested;
        public event EventHandler LogoutRequested;

        public UserAvatarPopoverCard()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);

            _userDisplayName = "未登录";
            _roleDisplayName = "用户";
            _language = "zh-CN";

            BindEvents();
            RefreshDisplay();
            ApplyLanguage(_language);
            ApplyTheme(false);
        }

        public string UserDisplayName
        {
            get { return _userDisplayName; }
            set
            {
                _userDisplayName = string.IsNullOrWhiteSpace(value) ? GetDefaultUserDisplayName() : value.Trim();
                RefreshDisplay();
            }
        }

        public string RoleDisplayName
        {
            get { return _roleDisplayName; }
            set
            {
                _roleDisplayName = string.IsNullOrWhiteSpace(value) ? GetDefaultRoleDisplayName() : value.Trim();
                RefreshDisplay();
            }
        }

        public void SetUserInfo(string userDisplayName, string roleDisplayName)
        {
            _userDisplayName = string.IsNullOrWhiteSpace(userDisplayName) ? GetDefaultUserDisplayName() : userDisplayName.Trim();
            _roleDisplayName = string.IsNullOrWhiteSpace(roleDisplayName) ? GetDefaultRoleDisplayName() : roleDisplayName.Trim();
            RefreshDisplay();
        }

        public void ApplyLanguage(string language)
        {
            _language = string.IsNullOrWhiteSpace(language) ? "zh-CN" : language;
            RefreshDisplay();
        }

        public void ApplyTheme(bool isDarkMode)
        {
            // 不做任何手动颜色覆盖。
            // 由 AntdUI 原生主题系统自动接管颜色与样式。
        }

        private void BindEvents()
        {
            buttonSwitchUser.Click += ButtonSwitchUser_Click;
            buttonChangePassword.Click += ButtonChangePassword_Click;
            buttonLogout.Click += ButtonLogout_Click;
        }

        private void ButtonSwitchUser_Click(object sender, EventArgs e)
        {
            var handler = SwitchUserRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            Dispose();
        }

        private void ButtonChangePassword_Click(object sender, EventArgs e)
        {
            var handler = ChangePasswordRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            Dispose();
        }

        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            var handler = LogoutRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            Dispose();
        }

        private void RefreshDisplay()
        {
            labelUserDisplayName.Text = string.IsNullOrWhiteSpace(_userDisplayName)
                ? GetDefaultUserDisplayName()
                : _userDisplayName;

            labelRoleDisplayName.Text = string.IsNullOrWhiteSpace(_roleDisplayName)
                ? GetDefaultRoleDisplayName()
                : _roleDisplayName;

            buttonSwitchUser.Text = IsEnglishLanguage(_language) ? "Switch User" : "切换用户";
            buttonChangePassword.Text = IsEnglishLanguage(_language) ? "Change Password" : "修改密码";
            buttonLogout.Text = IsEnglishLanguage(_language) ? "Logout" : "退出登录";
        }

        private string GetDefaultUserDisplayName()
        {
            return IsEnglishLanguage(_language) ? "Not Signed In" : "未登录";
        }

        private string GetDefaultRoleDisplayName()
        {
            return IsEnglishLanguage(_language) ? "User" : "用户";
        }

        private static bool IsEnglishLanguage(string language)
        {
            return !string.IsNullOrWhiteSpace(language)
                && language.StartsWith("en", StringComparison.OrdinalIgnoreCase);
        }
    }
}