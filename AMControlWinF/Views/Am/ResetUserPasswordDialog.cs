using AM.Model.Auth;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 重置用户密码弹窗。
    /// </summary>
    public partial class ResetUserPasswordDialog : AntdUI.Window
    {
        private readonly UserSummary _user;

        public ResetUserPasswordDialog(UserSummary user)
        {
            _user = user;
            InitializeComponent();
            InitData();
        }

        public string NewPassword
        {
            get { return inputNewPassword.Text == null ? string.Empty : inputNewPassword.Text; }
        }

        private void InitData()
        {
            labelLoginNameValue.Text = _user == null ? "-" : _user.LoginName;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            labelMessage.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                labelMessage.Text = "新密码不能为空";
                return;
            }

            if (!string.Equals(inputNewPassword.Text, inputConfirmPassword.Text, StringComparison.Ordinal))
            {
                labelMessage.Text = "两次输入的新密码不一致";
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}