using AM.Model.Auth;
using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// ResetUserPasswordDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ResetUserPasswordDialog : GlowWindow
    {
        private string _newPassword;
        private string _confirmPassword;

        public ResetUserPasswordDialog(UserSummary user)
        {
            InitializeComponent();

            TargetUserId = user == null ? 0 : user.Id;
            TargetLoginName = user == null ? string.Empty : user.LoginName;

            _newPassword = string.Empty;
            _confirmPassword = string.Empty;

            TextBlockUserInfo.Text = "目标用户：" + (string.IsNullOrWhiteSpace(TargetLoginName) ? "-" : TargetLoginName);
        }

        public int TargetUserId { get; private set; }

        public string TargetLoginName { get; private set; }

        public string NewPassword
        {
            get { return _newPassword; }
        }

        private void PasswordBoxNewPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            _newPassword = passwordBox == null ? string.Empty : passwordBox.Password;
        }

        private void PasswordBoxConfirmPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            _confirmPassword = passwordBox == null ? string.Empty : passwordBox.Password;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockMessage.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(_newPassword))
            {
                TextBlockMessage.Text = "新密码不能为空";
                return;
            }

            if (_newPassword.Length < 1)
            {
                TextBlockMessage.Text = "新密码长度不能少于 1 位";
                return;
            }

            if (_newPassword != _confirmPassword)
            {
                TextBlockMessage.Text = "两次输入的新密码不一致";
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}