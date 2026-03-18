using AM.DBService.Services.Auth;
using AM.ViewModel.ViewModels.Am;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// UserManagementView.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private readonly UserManagementViewModel _viewModel;
        private readonly AuthService _authService;

        public UserManagementView()
        {
            InitializeComponent();

            _viewModel = new UserManagementViewModel();
            _authService = new AuthService();

            DataContext = _viewModel;
            Loaded += UserManagementView_Loaded;
        }

        private async void UserManagementView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserManagementView_Loaded;
            await _viewModel.LoadAsync();
        }

        private async void ButtonAddUser_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new UserEditDialog
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var result = _authService.CreateUser(
                dialog.LoginName,
                dialog.UserName,
                dialog.RoleCode,
                dialog.Password,
                dialog.IsEnabledUser,
                dialog.Remark);

            MessageBox.Show(result.Message, "新增用户", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

            if (!result.Success)
            {
                return;
            }

            await _viewModel.LoadAsync();
        }

        private async void ButtonEditUser_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedUser = _viewModel.SelectedUser;
            if (selectedUser == null)
            {
                MessageBox.Show("请先选择一个用户。", "编辑用户", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new UserEditDialog(selectedUser)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var updateResult = _authService.UpdateUser(
                dialog.EditingUserId,
                dialog.UserName,
                dialog.RoleCode,
                dialog.IsEnabledUser,
                dialog.Remark);

            if (!updateResult.Success)
            {
                MessageBox.Show(updateResult.Message, "编辑用户", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dialog.ResetPassword)
            {
                var resetResult = _authService.ResetUserPassword(dialog.EditingUserId, dialog.Password);
                MessageBox.Show(resetResult.Message, "编辑用户", MessageBoxButton.OK,
                    resetResult.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

                if (!resetResult.Success)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show(updateResult.Message, "编辑用户", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            await _viewModel.LoadAsync();
        }

        private async void ButtonResetPassword_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedUser = _viewModel.SelectedUser;
            if (selectedUser == null)
            {
                MessageBox.Show("请先选择一个用户。", "重置密码", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new UserEditDialog(selectedUser)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            if (!dialog.ResetPassword)
            {
                MessageBox.Show("请勾选“保存时重置密码”，并输入新密码。", "重置密码", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = _authService.ResetUserPassword(selectedUser.Id, dialog.Password);
            MessageBox.Show(result.Message, "重置密码", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

            if (!result.Success)
            {
                return;
            }

            await _viewModel.LoadAsync();
        }
    }
}