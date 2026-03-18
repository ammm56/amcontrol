using AM.ViewModel.ViewModels.Auth;
using HandyControl.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMControlWPF.Views.Auth
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : GlowWindow
    {
        private readonly LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            _viewModel.CloseRequested += ViewModel_CloseRequested;
            DataContext = _viewModel;

            _viewModel.LoginName = "am";
            _viewModel.Password = "am123";
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as System.Windows.Controls.PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            _viewModel.Password = passwordBox.Password;
        }

        private void ViewModel_CloseRequested(bool dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.CloseRequested -= ViewModel_CloseRequested;
            base.OnClosed(e);
        }

        private void LoginView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (_viewModel.LoginCommand.CanExecute(null))
                {
                    _viewModel.LoginCommand.Execute(null);
                }
            }
        }
    }
}