using AM.DBService.Services.Auth;
using AM.PageModel.Common;
using System;
using System.Threading.Tasks;

namespace AM.PageModel.Auth
{
    /// <summary>
    /// WinForms 登录页模型。
    /// </summary>
    public class LoginPageModel : BindableBase
    {
        private readonly AuthService _authService;

        private string _loginName;
        private string _password;
        private string _statusText;
        private string _errorMessage;
        private bool _isBusy;

        public LoginPageModel()
        {
            _authService = new AuthService();
            _loginName = "am";
            _password = "am123";
            _statusText = "请输入登录信息";
            _errorMessage = string.Empty;
        }

        public string LoginName
        {
            get { return _loginName; }
            set { SetProperty(ref _loginName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public async Task<bool> LoginAsync()
        {
            if (IsBusy)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(LoginName))
            {
                ErrorMessage = "登录名不能为空";
                StatusText = "登录失败";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "密码不能为空";
                StatusText = "登录失败";
                return false;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;
            StatusText = "正在校验用户信息...";

            try
            {
                var result = await Task.Run(() =>
                    _authService.Login(LoginName.Trim(), Password, Environment.MachineName));

                if (!result.Success)
                {
                    ErrorMessage = result.Message;
                    StatusText = "登录失败";
                    return false;
                }

                StatusText = "登录成功";
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                StatusText = "登录异常";
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}