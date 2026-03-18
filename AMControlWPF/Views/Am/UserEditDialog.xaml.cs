using AM.Model.Auth;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// UserEditDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserEditDialog : HandyControl.Controls.GlowWindow
    {
        public UserEditDialog()
        {
            InitializeComponent();

            IsEditMode = false;
            TextBlockTitle.Text = "新增用户";
            TextBlockPasswordLabel.Visibility = Visibility.Visible;
            PasswordBoxPassword.Visibility = Visibility.Visible;

            Height = 620;
        }

        public UserEditDialog(UserSummary user) : this()
        {
            if (user == null)
            {
                return;
            }

            IsEditMode = true;
            EditingUserId = user.Id;
            TextBlockTitle.Text = "编辑用户";

            TextBoxLoginName.Text = user.LoginName;
            TextBoxUserName.Text = user.UserName;
            TextBoxRemark.Text = user.Remark;
            CheckBoxIsEnabled.IsChecked = user.IsEnabled;

            SetRole(user.RoleCode);
            TextBoxLoginName.IsEnabled = false;

            TextBlockPasswordLabel.Visibility = Visibility.Collapsed;
            PasswordBoxPassword.Visibility = Visibility.Collapsed;

            PasswordTopSpacingRow.Height = new GridLength(0);
            PasswordLabelRow.Height = new GridLength(0);
            PasswordInputRow.Height = new GridLength(0);
            PasswordBottomSpacingRow.Height = new GridLength(0);

            Height = 560;
        }

        public bool IsEditMode { get; private set; }

        public int EditingUserId { get; private set; }

        public string LoginName
        {
            get { return TextBoxLoginName.Text == null ? string.Empty : TextBoxLoginName.Text.Trim(); }
        }

        public string UserName
        {
            get { return TextBoxUserName.Text == null ? string.Empty : TextBoxUserName.Text.Trim(); }
        }

        public string RoleCode
        {
            get
            {
                var item = ComboBoxRole.SelectedItem as ComboBoxItem;
                if (item == null || item.Tag == null)
                {
                    return string.Empty;
                }

                return item.Tag.ToString();
            }
        }

        public string Password
        {
            get { return PasswordBoxPassword.Password == null ? string.Empty : PasswordBoxPassword.Password; }
        }

        public bool IsEnabledUser
        {
            get { return CheckBoxIsEnabled.IsChecked == true; }
        }

        public string Remark
        {
            get { return TextBoxRemark.Text == null ? string.Empty : TextBoxRemark.Text.Trim(); }
        }

        private void SetRole(string roleCode)
        {
            foreach (var item in ComboBoxRole.Items)
            {
                var comboBoxItem = item as ComboBoxItem;
                if (comboBoxItem == null || comboBoxItem.Tag == null)
                {
                    continue;
                }

                if (comboBoxItem.Tag.ToString() == roleCode)
                {
                    ComboBoxRole.SelectedItem = comboBoxItem;
                    return;
                }
            }

            ComboBoxRole.SelectedIndex = 2;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockMessage.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(LoginName))
            {
                TextBlockMessage.Text = "登录名不能为空";
                return;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                TextBlockMessage.Text = "用户名不能为空";
                return;
            }

            if (string.IsNullOrWhiteSpace(RoleCode))
            {
                TextBlockMessage.Text = "请选择角色";
                return;
            }

            if (!IsEditMode && string.IsNullOrWhiteSpace(Password))
            {
                TextBlockMessage.Text = "新增用户时必须输入初始密码";
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}