using AM.Core.Context;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户新增/编辑共用对话框。
    /// 布局参考 LoginForm：
    /// - 纹理背景
    /// - 中央大卡片
    /// - 顶部固定标题说明（单行左右结构）
    /// - 中间 StackPanel 滚动表单区
    /// - 底部固定右对齐按钮栏
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
            ApplyThemeFromConfig();
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

            Shown += UserEditDialog_Shown;

            KeyPreview = true;
            KeyDown += UserEditDialog_KeyDown;
        }

        private void UserEditDialog_Shown(object sender, EventArgs e)
        {
            if (_isCreateMode)
            {
                inputLoginName.Focus();
                inputLoginName.SelectAll();
            }
            else
            {
                inputUserName.Focus();
                inputUserName.SelectAll();
            }
        }

        private void ApplyMode()
        {
            if (_isCreateMode)
            {
                Text = "新增用户";
                labelDialogTitle.Text = "新增用户";
                labelDialogDescription.Text = "创建用户账号并设置角色、状态和备注信息。";

                Size = new System.Drawing.Size(560, 650);

                panelRowPassword.Visible = true;
                inputLoginName.Enabled = true;
                checkEnabled.Checked = true;
                buttonOk.Text = "保存";
            }
            else
            {
                Text = "编辑用户";
                labelDialogTitle.Text = "编辑用户";
                labelDialogDescription.Text = "修改用户显示信息、角色、状态和备注信息。";

                Size = new System.Drawing.Size(560, 580);

                panelRowPassword.Visible = false;
                inputLoginName.Enabled = false;
                buttonOk.Text = "保存";
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

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private bool ValidateInput()
        {
            if (_isCreateMode && string.IsNullOrWhiteSpace(LoginName))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "登录名不能为空。");
                inputLoginName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDisplayName))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "用户名不能为空。");
                inputUserName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(RoleCode))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "角色不能为空。");
                dropdownRole.Focus();
                return false;
            }

            if (_isCreateMode && string.IsNullOrWhiteSpace(Password))
            {
                PageDialogHelper.ShowWarn(this, "输入提示", "初始密码不能为空。");
                inputPassword.Focus();
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