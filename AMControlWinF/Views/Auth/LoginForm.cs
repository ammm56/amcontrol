using AM.Core.Context;
using AM.PageModel.Auth;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Auth
{
    /// <summary>
    /// WinForms 登录窗体。
    /// 结构参考 WPF 版登录页，但视觉优先对齐当前 AMControlWinF。
    /// 背景纹理复用 MainWindow 同款 TextureBackgroundControl。
    /// </summary>
    public partial class LoginForm : AntdUI.Window
    {
        private readonly LoginPageModel _model;
        private readonly BindingSource _bindingSource;
        private bool _isDarkMode;

        public LoginForm()
        {
            InitializeComponent();

            _model = new LoginPageModel();
            _bindingSource = new BindingSource();

            InitializeBindings();
            InitializeDefaultCredentials();
            BindEvents();
            ApplyThemeFromConfig();
        }

        /// <summary>
        /// 绑定视图与页面模型。
        /// </summary>
        private void InitializeBindings()
        {
            _bindingSource.DataSource = _model;

            textBoxLoginName.DataBindings.Add("Text", _bindingSource, "LoginName", false, DataSourceUpdateMode.OnPropertyChanged);
            labelStatusValue.DataBindings.Add("Text", _bindingSource, "StatusText", false, DataSourceUpdateMode.OnPropertyChanged);
            labelErrorValue.DataBindings.Add("Text", _bindingSource, "ErrorMessage", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        /// 默认管理员账户与 WPF 版保持一致。
        /// </summary>
        private void InitializeDefaultCredentials()
        {
            _model.LoginName = "am";
            _model.Password = "am123";
            textBoxPassword.Text = _model.Password;
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

        /// <summary>
        /// 从全局配置读取主题，保持与主窗体一致。
        /// </summary>
        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            _isDarkMode = IsDarkTheme(theme);
            ApplyTheme(_isDarkMode);
        }

        /// <summary>
        /// 应用登录窗体主题。
        /// 背景纹理与卡片层次对齐当前 WinForms 主界面。
        /// </summary>
        private void ApplyTheme(bool isDarkMode)
        {
            _isDarkMode = isDarkMode;
            textureBackgroundLogin.SetTheme(isDarkMode);

            BackColor = isDarkMode
                ? Color.FromArgb(31, 31, 31)
                : Color.FromArgb(245, 247, 250);

            panelShell.Back = isDarkMode
                ? Color.FromArgb(39, 39, 39)
                : Color.White;

            panelIntro.Back = isDarkMode
                ? Color.FromArgb(46, 46, 46)
                : Color.FromArgb(250, 251, 253);

            panelIntroCard.Back = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;

            panelLogin.Back = isDarkMode
                ? Color.FromArgb(39, 39, 39)
                : Color.White;

            var primaryText = isDarkMode
                ? Color.FromArgb(236, 236, 236)
                : Color.FromArgb(24, 39, 58);

            var secondaryText = isDarkMode
                ? Color.FromArgb(188, 188, 188)
                : Color.FromArgb(98, 108, 122);

            labelIntroTitle.ForeColor = isDarkMode
                ? Color.FromArgb(110, 180, 255)
                : Color.FromArgb(22, 119, 255);

            labelLoginTitle.ForeColor = primaryText;
            labelIntroSubtitle.ForeColor = secondaryText;
            labelIntroBottom.ForeColor = secondaryText;
            labelIntroCardTitle.ForeColor = primaryText;
            labelIntroOperator.ForeColor = primaryText;
            labelIntroEngineer.ForeColor = primaryText;
            labelIntroAdmin.ForeColor = primaryText;
            labelLoginName.ForeColor = primaryText;
            labelPassword.ForeColor = primaryText;
            labelStatusValue.ForeColor = secondaryText;
            labelErrorValue.ForeColor = isDarkMode
                ? Color.FromArgb(255, 138, 138)
                : Color.Firebrick;

            buttonLoginNameIcon.BackColor = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;

            buttonPasswordIcon.BackColor = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;

            textBoxLoginName.BackColor = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;

            textBoxPassword.BackColor = isDarkMode
                ? Color.FromArgb(56, 56, 56)
                : Color.White;
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
        /// 与 WPF 版一致：
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