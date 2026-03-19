using AM.DBService.Services.Auth;
using AM.Model.Auth;
using AM.ViewModel.ViewModels.Am;
using System;
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

            var result = _authService.UpdateUser(
                dialog.EditingUserId,
                dialog.UserName,
                dialog.RoleCode,
                dialog.IsEnabledUser,
                dialog.Remark);

            MessageBox.Show(result.Message, "编辑用户", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

            if (!result.Success)
            {
                return;
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

            var dialog = new ResetUserPasswordDialog(selectedUser)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var result = _authService.ResetUserPassword(selectedUser.Id, dialog.NewPassword);
            MessageBox.Show(result.Message, "重置密码", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

            if (!result.Success)
            {
                return;
            }

            await _viewModel.LoadAsync();
        }

        private async void ButtonToggleEnabled_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedUser = _viewModel.SelectedUser;
            if (selectedUser == null)
            {
                MessageBox.Show("请先选择一个用户。", "启用/禁用", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var enableTarget = !selectedUser.IsEnabled;
            var result = _authService.SetUserEnabled(selectedUser.Id, enableTarget);

            MessageBox.Show(result.Message, "启用/禁用", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

            if (!result.Success)
            {
                return;
            }

            await _viewModel.LoadAsync();
        }

        private void ButtonAssignPermission_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedUser = _viewModel.SelectedUser;
            if (selectedUser == null)
            {
                MessageBox.Show("请先选择一个用户。", "分配权限", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
    }
}