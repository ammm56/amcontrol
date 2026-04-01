using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 管理员重置用户密码对话框。
    /// 当前页面工具栏未接入按钮，但对话框可直接复用。
    /// </summary>
    public partial class ResetUserPasswordDialog : AntdUI.Window
    {
        public ResetUserPasswordDialog()
        {
            InitializeComponent();

            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            KeyPreview = true;
            KeyDown += ResetUserPasswordDialog_KeyDown;
        }

        public string NewPassword
        {
            get { return inputNewPassword.Text ?? string.Empty; }
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

            if (e.KeyCode == Keys.Enter)
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
    }
}