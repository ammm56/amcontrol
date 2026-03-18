using AM.DBService.Services.Auth;
using HandyControl.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMControlWPF.Views.Auth
{
    /// <summary>
    /// ChangePasswordDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePasswordDialog : GlowWindow
    {
        private readonly AuthService _authService;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;

        public ChangePasswordDialog()
        {
            InitializeComponent();

            _authService = new AuthService();
            _oldPassword = string.Empty;
            _newPassword = string.Empty;
            _confirmPassword = string.Empty;
        }

        private void PasswordBoxOld_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            _oldPassword = passwordBox == null ? string.Empty : passwordBox.Password;
        }

        private void PasswordBoxNew_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            _newPassword = passwordBox == null ? string.Empty : passwordBox.Password;
        }

        private void PasswordBoxConfirm_OnPasswordChanged(object sender, RoutedEventArgs e)
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

            var result = _authService.ChangeCurrentUserPassword(_oldPassword, _newPassword, _confirmPassword);
            if (!result.Success)
            {
                TextBlockMessage.Text = result.Message;
                return;
            }

            HandyControl.Controls.MessageBox.Show(result.Message, "修改密码", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }

        private void ChangePasswordDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                return;
            }

            if (e.Key == Key.Enter)
            {
                ButtonSubmit_OnClick(this, new RoutedEventArgs());
            }
        }
    }
}