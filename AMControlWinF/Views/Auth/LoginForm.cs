using AM.Core.Context;
using AM.PageModel.Auth;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Auth
{
    /// <summary>
    /// WinForms 登录窗体。
    /// 布局、静态文案、默认初始显示均在 Designer 中定义，
    /// 运行时仅保留最小绑定、交互与主题同步逻辑。
    /// </summary>
    public partial class LoginForm : AntdUI.Window
    {
        private readonly LoginPageModel _model;
        private readonly BindingSource _bindingSource;

        public LoginForm()
        {
            InitializeComponent();

            _model = new LoginPageModel();
            _bindingSource = new BindingSource();

            InitializeBindings();
            InitializeDefaultCredentials();
            ApplyThemeFromConfig();
            BindEvents();
        }

        /// <summary>
        /// 仅绑定动态内容。
        /// </summary>
        private void InitializeBindings()
        {
            _bindingSource.DataSource = _model;

            textBoxLoginName.DataBindings.Add("Text", _bindingSource, "LoginName", false, DataSourceUpdateMode.OnPropertyChanged);
            labelStatusValue.DataBindings.Add("Text", _bindingSource, "StatusText", false, DataSourceUpdateMode.OnPropertyChanged);
            labelErrorValue.DataBindings.Add("Text", _bindingSource, "ErrorMessage", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// 默认管理员账户与系统约定一致。
        /// 保持与 Designer 中的初始显示一致，避免设计期与运行期割裂。
        /// </summary>
        private void InitializeDefaultCredentials()
        {
            _model.LoginName = "am";
            _model.Password = "am123";
            textBoxLoginName.Text = _model.LoginName;
            textBoxPassword.Text = _model.Password;
        }

        /// <summary>
        /// 登录窗体只做最小主题同步：
        /// 1. 同步 AntdUI 默认亮/暗主题；
        /// 2. 同步纹理背景主题。
        /// 不在这里逐个控件手动改色。
        /// </summary>
        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = IsDarkTheme(theme);

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundLogin.SetTheme(isDarkMode);
        }

        /// <summary>
        /// 统一绑定交互事件。
        /// </summary>
        private void BindEvents()
        {
            buttonLogin.Click += async (sender, e) => await LoginAsync();
            buttonCancel.Click += ButtonCancel_Click;
            textBoxPassword.TextChanged += TextBoxPassword_TextChanged;

            Shown += LoginForm_Shown;
            FormClosing += LoginForm_FormClosing;

            KeyPreview = true;
            KeyDown += LoginForm_KeyDown;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextBoxPassword_TextChanged(object sender, EventArgs e)
        {
            _model.Password = textBoxPassword.Text;
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            textBoxLoginName.Focus();
            textBoxLoginName.SelectAll();
        }

        /// <summary>
        /// 非成功登录关闭均视为取消。
        /// </summary>
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Esc 取消，Enter 登录。
        /// </summary>
        private async void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await LoginAsync();
            }
        }

        /// <summary>
        /// 执行登录。
        /// 登录时仅更新模型状态与按钮可用性，不做额外重绘逻辑。
        /// </summary>
        private async Task LoginAsync()
        {
            SetBusyState(true);

            try
            {
                var success = await _model.LoginAsync();
                if (!success)
                {
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        /// <summary>
        /// 登录过程中禁用输入，避免重复触发。
        /// </summary>
        private void SetBusyState(bool isBusy)
        {
            textBoxLoginName.Enabled = !isBusy;
            textBoxPassword.Enabled = !isBusy;
            buttonLogin.Enabled = !isBusy;
            buttonCancel.Enabled = !isBusy;
        }

        private static bool IsDarkTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return false;
            }

            return string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase)
                || string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase);
        }
    }
}