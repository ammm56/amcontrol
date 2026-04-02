using AM.Core.Context;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 管理员重置用户密码对话框。
    /// 布局与 UserEditDialog 一致：
    /// - 纹理背景
    /// - 中央大卡片
    /// - 顶部固定标题说明（单行左右结构）
    /// - 中间 StackPanel 表单区
    /// - 底部固定右对齐按钮栏
    /// </summary>
    public partial class ResetUserPasswordDialog : AntdUI.Window
    {
        public ResetUserPasswordDialog()
        {
            InitializeComponent();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyHeaderText();
        }

        public string TargetLoginName { get; set; }

        public string TargetDisplayName { get; set; }

        public string NewPassword
        {
            get { return inputNewPassword.Text ?? string.Empty; }
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            Shown += ResetUserPasswordDialog_Shown;

            KeyPreview = true;
            KeyDown += ResetUserPasswordDialog_KeyDown;
        }

        private void ResetUserPasswordDialog_Shown(object sender, EventArgs e)
        {
            ApplyHeaderText();
            inputNewPassword.Focus();
            inputNewPassword.SelectAll();
        }

        private void ApplyHeaderText()
        {
            labelDialogTitle.Text = "重置用户密码";

            var loginName = string.IsNullOrWhiteSpace(TargetLoginName) ? "-" : TargetLoginName;
            var displayName = string.IsNullOrWhiteSpace(TargetDisplayName) ? "-" : TargetDisplayName;

            labelDialogDescription.Text = "目标用户：" + loginName + " / " + displayName;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ResetUserPasswordDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private bool ValidateInput()
        {
            var newPassword = inputNewPassword.Text ?? string.Empty;
            var confirmPassword = inputConfirmPassword.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("新密码不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputNewPassword.Focus();
                return false;
            }

            if (!string.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                MessageBox.Show("两次输入的新密码不一致。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputConfirmPassword.Focus();
                return false;
            }

            return true;
        }

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

            textureBackgroundDialog.SetTheme(isDarkMode);
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