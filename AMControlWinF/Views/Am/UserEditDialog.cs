using System;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户新增/编辑共用对话框。
    /// 新增模式窗口更大，编辑模式更紧凑。
    /// </summary>
    public partial class UserEditDialog : AntdUI.Window
    {
        private static readonly RoleItem[] Roles = new[]
        {
            new RoleItem("Operator", "操作员"),
            new RoleItem("Engineer", "工程师"),
            new RoleItem("Am", "管理员")
        };

        private bool _isCreateMode = true;

        public UserEditDialog()
        {
            InitializeComponent();
            InitializeRoleDropdown();
            BindEvents();
            ApplyMode();
        }

        public bool IsCreateMode
        {
            get { return _isCreateMode; }
            set
            {
                _isCreateMode = value;
                ApplyMode();
            }
        }

        public string LoginName
        {
            get { return inputLoginName.Text == null ? string.Empty : inputLoginName.Text.Trim(); }
            set { inputLoginName.Text = value ?? string.Empty; }
        }

        public string UserDisplayName
        {
            get { return inputUserName.Text == null ? string.Empty : inputUserName.Text.Trim(); }
            set { inputUserName.Text = value ?? string.Empty; }
        }

        public string RoleCode
        {
            get
            {
                var selected = dropdownRole.SelectedValue == null
                    ? string.Empty
                    : dropdownRole.SelectedValue.ToString();

                var role = Roles.FirstOrDefault(x =>
                    string.Equals(x.DisplayName, selected, StringComparison.OrdinalIgnoreCase));

                return role == null ? string.Empty : role.Code;
            }
            set
            {
                var role = Roles.FirstOrDefault(x =>
                    string.Equals(x.Code, value, StringComparison.OrdinalIgnoreCase));

                dropdownRole.SelectedValue = role == null
                    ? Roles[0].DisplayName
                    : role.DisplayName;
            }
        }

        public string Password
        {
            get { return inputPassword.Text ?? string.Empty; }
            set { inputPassword.Text = value ?? string.Empty; }
        }

        public bool IsEnabled
        {
            get { return checkEnabled.Checked; }
            set { checkEnabled.Checked = value; }
        }

        public string Remark
        {
            get { return inputRemark.Text == null ? string.Empty : inputRemark.Text.Trim(); }
            set { inputRemark.Text = value ?? string.Empty; }
        }

        private void InitializeRoleDropdown()
        {
            dropdownRole.Items.Clear();
            dropdownRole.Items.AddRange(Roles.Select(x => (object)x.DisplayName).ToArray());
            dropdownRole.SelectedValue = Roles[0].DisplayName;
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;

            KeyPreview = true;
            KeyDown += UserEditDialog_KeyDown;
        }

        private void ApplyMode()
        {
            if (_isCreateMode)
            {
                Height = 430;
                inputLoginName.Enabled = true;
                labelPassword.Visible = true;
                inputPassword.Visible = true;
                checkEnabled.Checked = true;
            }
            else
            {
                Height = 360;
                inputLoginName.Enabled = false;
                labelPassword.Visible = false;
                inputPassword.Visible = false;
            }
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

        private void UserEditDialog_KeyDown(object sender, KeyEventArgs e)
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
            if (_isCreateMode && string.IsNullOrWhiteSpace(LoginName))
            {
                MessageBox.Show("登录名不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputLoginName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDisplayName))
            {
                MessageBox.Show("用户名不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputUserName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(RoleCode))
            {
                MessageBox.Show("角色不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dropdownRole.Focus();
                return false;
            }

            if (_isCreateMode && string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("初始密码不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputPassword.Focus();
                return false;
            }

            return true;
        }

        private sealed class RoleItem
        {
            public RoleItem(string code, string displayName)
            {
                Code = code;
                DisplayName = displayName;
            }

            public string Code { get; private set; }
            public string DisplayName { get; private set; }
        }
    }
}