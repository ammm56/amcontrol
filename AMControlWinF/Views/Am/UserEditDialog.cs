using AM.Model.Auth;
using System;
using System.Drawing;
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
            InitDropdowns();
            ApplyAddMode();
        }

        public UserEditDialog(UserSummary user)
        {
            _isEditMode = true;
            InitializeComponent();
            InitDropdowns();
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
            get
            {
                var value = dropdownEnabled.SelectedValue == null
                    ? string.Empty
                    : dropdownEnabled.SelectedValue.ToString();

                return string.Equals(value, "启用", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string Remark
        {
            get { return inputRemark.Text == null ? string.Empty : inputRemark.Text.Trim(); }
        }

        private void InitDropdowns()
        {
            dropdownRole.Items.Clear();
            dropdownRole.Items.AddRange(new object[]
            {
                "操作员",
                "工程师",
                "管理员"
            });

            dropdownEnabled.Items.Clear();
            dropdownEnabled.Items.AddRange(new object[]
            {
                "启用",
                "禁用"
            });
        }

        private void ApplyAddMode()
        {
            Text = "新增用户";
            labelTitle.Text = "新增用户";
            dropdownRole.SelectedValue = "操作员";
            dropdownEnabled.SelectedValue = "启用";

            Size = new Size(760, 430);
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
                inputRemark.Text = user.Remark;

                dropdownEnabled.SelectedValue = user.IsEnabled ? "启用" : "禁用";

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

            Size = new Size(680, 360);
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