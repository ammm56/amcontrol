using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 用户头像弹出操作卡片。
    /// 使用最简单的 WinForms 布局，按钮使用 AntdUI。
    /// </summary>
    public partial class UserAvatarPopoverCard : UserControl
    {
        private string _userDisplayName;
        private string _roleDisplayName;
        private string _language;

        private int _topActionButtonHeight;
        private int _bottomActionButtonHeight;
        private int _sectionGapHeight;
        private int _actionColumnGap;

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

            _topActionButtonHeight = 36;
            _bottomActionButtonHeight = 36;
            _sectionGapHeight = 18;
            _actionColumnGap = 8;

            BindEvents();
            RefreshDisplay();
            LayoutControlsCore();
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

        public int TopActionButtonHeight
        {
            get { return _topActionButtonHeight; }
            set
            {
                _topActionButtonHeight = value < 28 ? 28 : value;
                LayoutControlsCore();
            }
        }

        public int BottomActionButtonHeight
        {
            get { return _bottomActionButtonHeight; }
            set
            {
                _bottomActionButtonHeight = value < 28 ? 28 : value;
                LayoutControlsCore();
            }
        }

        public int SectionGapHeight
        {
            get { return _sectionGapHeight; }
            set
            {
                _sectionGapHeight = value < 0 ? 0 : value;
                LayoutControlsCore();
            }
        }

        public int ActionColumnGap
        {
            get { return _actionColumnGap; }
            set
            {
                _actionColumnGap = value < 0 ? 0 : value;
                LayoutControlsCore();
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
            // 不手动指定颜色，交由 AntdUI 原生主题处理。
        }

        private void BindEvents()
        {
            buttonSwitchUser.Click += ButtonSwitchUser_Click;
            buttonChangePassword.Click += ButtonChangePassword_Click;
            buttonLogout.Click += ButtonLogout_Click;
            Resize += UserAvatarPopoverCard_Resize;
        }

        private void UserAvatarPopoverCard_Resize(object sender, EventArgs e)
        {
            LayoutControlsCore();
        }

        private void LayoutControlsCore()
        {
            SuspendLayout();

            try
            {
                const int paddingLeft = 16;
                const int paddingTop = 14;
                const int paddingRight = 16;
                const int paddingBottom = 16;
                const int avatarSize = 42;
                const int avatarTextGap = 10;
                const int userNameHeight = 22;
                const int roleHeight = 18;

                var contentWidth = Width - paddingLeft - paddingRight;
                if (contentWidth < 180)
                {
                    contentWidth = 180;
                }

                var textX = paddingLeft + avatarSize + avatarTextGap;
                var textWidth = contentWidth - avatarSize - avatarTextGap;
                if (textWidth < 80)
                {
                    textWidth = 80;
                }

                avatarPopupUser.SetBounds(
                    paddingLeft,
                    paddingTop,
                    avatarSize,
                    avatarSize);

                labelUserDisplayName.SetBounds(
                    textX,
                    paddingTop,
                    textWidth,
                    userNameHeight);

                labelRoleDisplayName.SetBounds(
                    textX,
                    paddingTop + userNameHeight + 4,
                    textWidth,
                    roleHeight);

                var buttonTopY = paddingTop + avatarSize + _sectionGapHeight;
                var leftButtonWidth = (contentWidth - _actionColumnGap) / 2;
                if (leftButtonWidth < 80)
                {
                    leftButtonWidth = 80;
                }

                var rightButtonX = paddingLeft + leftButtonWidth + _actionColumnGap;
                var rightButtonWidth = contentWidth - leftButtonWidth - _actionColumnGap;
                if (rightButtonWidth < 80)
                {
                    rightButtonWidth = 80;
                }

                buttonSwitchUser.SetBounds(
                    paddingLeft,
                    buttonTopY,
                    leftButtonWidth,
                    _topActionButtonHeight);

                buttonChangePassword.SetBounds(
                    rightButtonX,
                    buttonTopY,
                    rightButtonWidth,
                    _topActionButtonHeight);

                buttonLogout.SetBounds(
                    paddingLeft,
                    buttonTopY + _topActionButtonHeight + 8,
                    leftButtonWidth,
                    _bottomActionButtonHeight);

                var totalHeight = buttonLogout.Bottom + paddingBottom;
                if (Height != totalHeight)
                {
                    Height = totalHeight;
                }

                MinimumSize = new System.Drawing.Size(240, totalHeight);
            }
            finally
            {
                ResumeLayout();
            }
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