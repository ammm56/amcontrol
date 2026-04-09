using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 用户头像弹出操作卡片。
    /// 使用设计器中定义好的静态布局，不再做运行时尺寸和位置计算。
    /// </summary>
    public partial class UserAvatarPopoverCard : UserControl
    {
        private string _userDisplayName;
        private string _roleDisplayName;
        private string _language;

        /// <summary>
        /// 防止同一个弹层在关闭过程中被重复点击。
        /// </summary>
        private bool _isClosing;

        public event EventHandler SwitchUserRequested;
        public event EventHandler ChangePasswordRequested;
        public event EventHandler LogoutRequested;

        public UserAvatarPopoverCard()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            _userDisplayName = "未登录";
            _roleDisplayName = "用户";
            _language = "zh-CN";

            BindEvents();
            RefreshDisplay();
        }

        public string UserDisplayName
        {
            get { return _userDisplayName; }
            set
            {
                _userDisplayName = string.IsNullOrWhiteSpace(value)
                    ? GetDefaultUserDisplayName()
                    : value.Trim();

                RefreshDisplay();
            }
        }

        public string RoleDisplayName
        {
            get { return _roleDisplayName; }
            set
            {
                _roleDisplayName = string.IsNullOrWhiteSpace(value)
                    ? GetDefaultRoleDisplayName()
                    : value.Trim();

                RefreshDisplay();
            }
        }

        public void SetUserInfo(string userDisplayName, string roleDisplayName)
        {
            _userDisplayName = string.IsNullOrWhiteSpace(userDisplayName)
                ? GetDefaultUserDisplayName()
                : userDisplayName.Trim();

            _roleDisplayName = string.IsNullOrWhiteSpace(roleDisplayName)
                ? GetDefaultRoleDisplayName()
                : roleDisplayName.Trim();

            RefreshDisplay();
        }

        public void ApplyLanguage(string language)
        {
            _language = string.IsNullOrWhiteSpace(language) ? "zh-CN" : language;
            RefreshDisplay();
        }

        public void ApplyTheme(bool isDarkMode)
        {
            // 不手动指定颜色，交由 AntdUI 原生主题处理。
        }

        private void BindEvents()
        {
            buttonSwitchUser.Click += ButtonSwitchUser_Click;
            buttonChangePassword.Click += ButtonChangePassword_Click;
            buttonLogout.Click += ButtonLogout_Click;
        }

        private void ButtonSwitchUser_Click(object sender, EventArgs e)
        {
            ClosePopoverThenNotify(SwitchUserRequested);
        }

        private void ButtonChangePassword_Click(object sender, EventArgs e)
        {
            ClosePopoverThenNotify(ChangePasswordRequested);
        }

        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            ClosePopoverThenNotify(LogoutRequested);
        }

        /// <summary>
        /// 先关闭弹层，再在宿主窗体的下一轮消息中通知外层。
        /// 避免在当前按钮点击链路中直接触发模态对话框。
        /// </summary>
        private void ClosePopoverThenNotify(EventHandler handler)
        {
            if (_isClosing)
                return;

            _isClosing = true;

            buttonSwitchUser.Enabled = false;
            buttonChangePassword.Enabled = false;
            buttonLogout.Enabled = false;

            // 必须在 Dispose 之前获取宿主窗体引用。
            var ownerForm = FindForm();

            // 隐藏 + 释放卡片内容，触发 AntdUI Popover 关闭浮层。
            Visible = false;
            Dispose();

            // 延迟到下一轮消息通知，确保弹层完全关闭后再执行后续逻辑。
            // 不传 this（已释放），sender 传 null — 下游事件链不依赖 sender。
            if (handler != null && ownerForm != null && !ownerForm.IsDisposed && ownerForm.IsHandleCreated)
            {
                ownerForm.BeginInvoke(new Action(() => handler(null, EventArgs.Empty)));
            }
        }

        private void RefreshDisplay()
        {
            labelUserDisplayName.Text = string.IsNullOrWhiteSpace(_userDisplayName)
                ? GetDefaultUserDisplayName()
                : _userDisplayName;

            labelRoleDisplayName.Text = string.IsNullOrWhiteSpace(_roleDisplayName)
                ? GetDefaultRoleDisplayName()
                : _roleDisplayName;

            if (IsEnglishLanguage(_language))
            {
                buttonSwitchUser.Text = "Switch User";
                buttonChangePassword.Text = "Change Password";
                buttonLogout.Text = "Logout";
            }
            else
            {
                buttonSwitchUser.Text = "切换用户";
                buttonChangePassword.Text = "修改密码";
                buttonLogout.Text = "退出登录";
            }

            RefreshAvatarText();
        }

        private void RefreshAvatarText()
        {
            if (!string.IsNullOrWhiteSpace(avatarPopupUser.ImageSvg))
            {
                avatarPopupUser.Text = string.Empty;
                return;
            }

            var text = string.IsNullOrWhiteSpace(_userDisplayName) ? "A" : _userDisplayName.Trim();
            avatarPopupUser.Text = text.Substring(0, 1).ToUpperInvariant();
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