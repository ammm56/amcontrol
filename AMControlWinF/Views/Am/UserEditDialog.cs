using AM.Model.Auth;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 新增/编辑用户弹窗。
    /// </summary>
    public partial class UserEditDialog : AntdUI.Window
    {
        private readonly bool _isEditMode;

        public UserEditDialog()
        {
            _isEditMode = false;
            InitializeComponent();
            InitRoleDropdown();
            ApplyAddMode();
        }

        public UserEditDialog(UserSummary user)
        {
            _isEditMode = true;
            InitializeComponent();
            InitRoleDropdown();
            ApplyEditMode(user);
        }

        public string LoginName
        {
            get { return inputLoginName.Text == null ? string.Empty : inputLoginName.Text.Trim(); }
        }

        public string UserName
        {
            get { return inputUserName.Text == null ? string.Empty : inputUserName.Text.Trim(); }
        }

        public string Password
        {
            get { return inputPassword.Text == null ? string.Empty : inputPassword.Text; }
        }

        public string RoleCode
        {
            get
            {
                var value = dropdownRole.SelectedValue == null
                    ? string.Empty
                    : dropdownRole.SelectedValue.ToString();

                if (value == "管理员")
                {
                    return "Am";
                }

                if (value == "工程师")
                {
                    return "Engineer";
                }

                return "Operator";
            }
        }

        public bool IsEnabledUser
        {
            get { return checkBoxEnabled.Checked; }
        }

        public string Remark
        {
            get { return textBoxRemark.Text == null ? string.Empty : textBoxRemark.Text.Trim(); }
        }

        private void InitRoleDropdown()
        {
            dropdownRole.Items.Clear();
            dropdownRole.Items.AddRange(new object[]
            {
                "操作员",
                "工程师",
                "管理员"
            });
        }

        private void ApplyAddMode()
        {
            Text = "新增用户";
            labelTitle.Text = "新增用户";
            dropdownRole.SelectedValue = "操作员";
            checkBoxEnabled.Checked = true;

            Size = new System.Drawing.Size(720, 560);
            MinimumSize = Size;
            MaximumSize = Size;
        }

        private void ApplyEditMode(UserSummary user)
        {
            Text = "编辑用户";
            labelTitle.Text = "编辑用户";

            labelPassword.Visible = false;
            inputPassword.Visible = false;

            if (user != null)
            {
                inputLoginName.Text = user.LoginName;
                inputLoginName.Enabled = false;
                inputUserName.Text = user.UserName;
                textBoxRemark.Text = user.Remark;
                checkBoxEnabled.Checked = user.IsEnabled;

                if (string.Equals(user.RoleCode, "Am", StringComparison.OrdinalIgnoreCase))
                {
                    dropdownRole.SelectedValue = "管理员";
                }
                else if (string.Equals(user.RoleCode, "Engineer", StringComparison.OrdinalIgnoreCase))
                {
                    dropdownRole.SelectedValue = "工程师";
                }
                else
                {
                    dropdownRole.SelectedValue = "操作员";
                }
            }

            Size = new System.Drawing.Size(640, 470);
            MinimumSize = Size;
            MaximumSize = Size;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            labelMessage.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(LoginName))
            {
                labelMessage.Text = "登录名不能为空";
                return;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                labelMessage.Text = "用户名不能为空";
                return;
            }

            if (string.IsNullOrWhiteSpace(RoleCode))
            {
                labelMessage.Text = "请选择角色";
                return;
            }

            if (!_isEditMode && string.IsNullOrWhiteSpace(Password))
            {
                labelMessage.Text = "新增用户时必须输入初始密码";
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}