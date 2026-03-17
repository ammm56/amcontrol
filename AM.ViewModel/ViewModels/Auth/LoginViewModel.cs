using AM.DBService.Services.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Auth
{
    /// <summary>
    /// 登录视图模型。
    /// </summary>
    public class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        private string _loginName;
        private string _password;
        private string _errorMessage;
        private string _statusText;
        private bool _isBusy;

        public event Action<bool> CloseRequested;

        public IAsyncRelayCommand LoginCommand { get; private set; }

        public IRelayCommand CancelCommand { get; private set; }

        public LoginViewModel()
        {
            _authService = new AuthService();

            _statusText = "请输入登录信息";
            _errorMessage = string.Empty;
            _loginName = string.Empty;
            _password = string.Empty;

            LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string LoginName
        {
            get { return _loginName; }
            set
            {
                if (SetProperty(ref _loginName, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(ref _password, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private bool CanLogin()
        {
            return !IsBusy
                && !string.IsNullOrWhiteSpace(LoginName)
                && !string.IsNullOrWhiteSpace(Password);
        }

        private async Task LoginAsync()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            StatusText = "正在校验用户信息...";

            try
            {
                var result = await Task.Run(() => _authService.Login(LoginName, Password, Environment.MachineName));

                if (!result.Success)
                {
                    ErrorMessage = result.Message;
                    StatusText = "登录失败";
                    return;
                }

                StatusText = "登录成功";
                CloseRequested?.Invoke(true);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                StatusText = "登录异常";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}