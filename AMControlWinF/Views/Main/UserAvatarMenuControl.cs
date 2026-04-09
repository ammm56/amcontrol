using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 左下角用户头像菜单控件。
    /// 仅显示头像，点击后弹出用户操作卡片。
    /// 使用 AntdUI 默认原生样式，自动跟随主题切换。
    /// </summary>
    public partial class UserAvatarMenuControl : UserControl
    {
        private string _userDisplayName;
        private string _roleDisplayName;
        private string _language;

        public event EventHandler SwitchUserRequested;
        public event EventHandler ChangePasswordRequested;
        public event EventHandler LogoutRequested;

        public UserAvatarMenuControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);

            _userDisplayName = "未登录";
            _roleDisplayName = "用户";
            _language = "zh-CN";

            ConfigureAvatarLayout();
            BindEvents();
            RefreshAvatarText();
            ApplyLanguage(_language);
            ApplyTheme(false);
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
            // 不做任何手动颜色覆盖。
            // 由 AntdUI 原生主题系统自动接管头像颜色与样式。
        }

        private void BindEvents()
        {
            avatarCurrentUser.Click += AvatarCurrentUser_Click;
            panelRoot.Click += AvatarCurrentUser_Click;
            SizeChanged += UserAvatarMenuControl_SizeChanged;
            panelRoot.SizeChanged += UserAvatarMenuControl_SizeChanged;
        }

        private void ConfigureAvatarLayout()
        {
            panelRoot.Padding = new Padding(4);
            avatarCurrentUser.Dock = DockStyle.Fill;
            avatarCurrentUser.Margin = Padding.Empty;
            avatarCurrentUser.ImageFit = TFit.Contain;
            UpdateAvatarMetrics();
        }

        private void UserAvatarMenuControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateAvatarMetrics();
        }

        private void UpdateAvatarMetrics()
        {
            var diameter = Math.Min(avatarCurrentUser.Width, avatarCurrentUser.Height);
            var innerPadding = diameter >= 56 ? 6 : Math.Max(3, diameter / 8);

            avatarCurrentUser.Padding = new Padding(innerPadding);
            avatarCurrentUser.Radius = diameter > 0 ? diameter / 2 : 0;
        }

        private void AvatarCurrentUser_Click(object sender, EventArgs e)
        {
            ShowUserPopoverCard();
        }

        private void ShowUserPopoverCard()
        {
            var card = new UserAvatarPopoverCard();
            card.SetUserInfo(_userDisplayName, _roleDisplayName);
            card.ApplyLanguage(_language);

            card.SwitchUserRequested += Card_SwitchUserRequested;
            card.ChangePasswordRequested += Card_ChangePasswordRequested;
            card.LogoutRequested += Card_LogoutRequested;

            Popover.open(new Popover.Config(avatarCurrentUser, card)
            {
                ArrowAlign = TAlign.Left,
                ArrowSize = 10
            });
        }

        private void Card_SwitchUserRequested(object sender, EventArgs e)
        {
            var handler = SwitchUserRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Card_ChangePasswordRequested(object sender, EventArgs e)
        {
            var handler = ChangePasswordRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void Card_LogoutRequested(object sender, EventArgs e)
        {
            var handler = LogoutRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RefreshAvatarText()
        {
            if (!string.IsNullOrWhiteSpace(avatarCurrentUser.ImageSvg))
            {
                avatarCurrentUser.Text = string.Empty;
                return;
            }

            var text = string.IsNullOrWhiteSpace(_userDisplayName) ? "A" : _userDisplayName;
            avatarCurrentUser.Text = text.Substring(0, 1).ToUpperInvariant();
        }
    }
}