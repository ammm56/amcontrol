using AM.DBService.Services.Auth;
using AM.Model.Auth;
using AM.Model.Entity.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Am
{
    /// <summary>
    /// UserPermissionView.xaml 的交互逻辑
    /// </summary>
    public partial class UserPermissionView : UserControl
    {
        private readonly AuthService _authService;
        private readonly List<UserSummary> _allUsers;
        private readonly List<PermissionModuleItem> _modules;
        private readonly List<PermissionDisplayItem> _allPermissions;
        private UserSummary _targetUser;

        public UserPermissionView()
        {
            InitializeComponent();

            _authService = new AuthService();
            _allUsers = new List<UserSummary>();
            _modules = new List<PermissionModuleItem>();
            _allPermissions = new List<PermissionDisplayItem>();

            Loaded += UserPermissionView_Loaded;
        }

        private async void UserPermissionView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserPermissionView_Loaded;
            await System.Threading.Tasks.Task.Run(() => { });
            LoadData();
        }

        private void LoadData()
        {
            var userResult = _authService.GetUserSummaries();
            if (!userResult.Success)
            {
                HandyControl.Controls.MessageBox.Show(userResult.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var permissionResult = _authService.GetPagePermissions();
            if (!permissionResult.Success)
            {
                HandyControl.Controls.MessageBox.Show(permissionResult.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _allUsers.Clear();
            _allUsers.AddRange(userResult.Items.OrderBy(x => x.LoginName));

            _allPermissions.Clear();
            _allPermissions.AddRange(permissionResult.Items.Select(x => new PermissionDisplayItem
            {
                ModuleKey = x.ModuleKey,
                ModuleName = x.ModuleName,
                PageKey = x.PageKey,
                DisplayName = x.DisplayName,
                Description = x.Description,
                RecommendedRoles = x.RecommendedRoles,
                RiskLevel = x.RiskLevel,
                SortOrder = x.SortOrder,
                IsChecked = false
            }));

            _modules.Clear();
            _modules.AddRange(_allPermissions
                .GroupBy(x => x.ModuleKey)
                .Select(x => new PermissionModuleItem(x.Key, x.First().ModuleName))
                .OrderBy(x => x.DisplayName));

            ListBoxUsers.ItemsSource = _allUsers;
            ComboBoxModules.ItemsSource = _modules;

            if (_modules.Count > 0)
            {
                ComboBoxModules.SelectedIndex = 0;
            }

            if (_allUsers.Count > 0)
            {
                ListBoxUsers.SelectedIndex = 0;
            }
            else
            {
                UpdateSelectedUserDisplay(null);
            }
        }

        private void TextBoxUserSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = TextBoxUserSearch.Text == null ? string.Empty : TextBoxUserSearch.Text.Trim().ToLowerInvariant();

            var list = string.IsNullOrWhiteSpace(keyword)
                ? _allUsers
                : _allUsers.Where(x =>
                        (!string.IsNullOrWhiteSpace(x.LoginName) && x.LoginName.ToLowerInvariant().Contains(keyword)) ||
                        (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToLowerInvariant().Contains(keyword)) ||
                        (!string.IsNullOrWhiteSpace(x.RoleName) && x.RoleName.ToLowerInvariant().Contains(keyword)))
                    .ToList();

            ListBoxUsers.ItemsSource = list;
        }

        private void ListBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = ListBoxUsers.SelectedItem as UserSummary;
            _targetUser = user;

            UpdateSelectedUserDisplay(user);
            LoadUserPermissions();
        }

        private void ComboBoxModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshPermissionList();
        }

        private void ListBoxPermissions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ListBoxPermissions.SelectedItem as PermissionDisplayItem;
            if (item == null)
            {
                ClearPermissionDetail();
                return;
            }

            TextBlockPermissionName.Text = item.DisplayName;
            TextBlockPermissionKey.Text = "页面键值：" + item.PageKey;
            TextBlockPermissionRoles.Text = "建议角色：" + (string.IsNullOrWhiteSpace(item.RecommendedRoles) ? "-" : item.RecommendedRoles);
            TextBlockPermissionRisk.Text = "风险级别：" + (string.IsNullOrWhiteSpace(item.RiskLevel) ? "-" : item.RiskLevel);
            TextBlockPermissionDescription.Text = string.IsNullOrWhiteSpace(item.Description)
                ? "暂无说明。"
                : item.Description;
        }

        private void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ButtonSelectAll_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedModule = ComboBoxModules.SelectedItem as PermissionModuleItem;
            if (selectedModule == null)
            {
                return;
            }

            foreach (var item in _allPermissions.Where(x => x.ModuleKey == selectedModule.Key))
            {
                item.IsChecked = true;
            }

            RefreshPermissionList();
        }

        private void ButtonClearAll_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedModule = ComboBoxModules.SelectedItem as PermissionModuleItem;
            if (selectedModule == null)
            {
                return;
            }

            foreach (var item in _allPermissions.Where(x => x.ModuleKey == selectedModule.Key))
            {
                item.IsChecked = false;
            }

            RefreshPermissionList();
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (_targetUser == null)
            {
                HandyControl.Controls.MessageBox.Show("请先选择一个用户。", "权限分配", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedKeys = _allPermissions
                .Where(x => x.IsChecked)
                .Select(x => x.PageKey)
                .ToList();

            var result = _authService.SaveUserPagePermissions(_targetUser.Id, selectedKeys);
            HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK,
                result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);
        }

        private void LoadUserPermissions()
        {
            foreach (var item in _allPermissions)
            {
                item.IsChecked = false;
            }

            if (_targetUser == null)
            {
                RefreshPermissionList();
                return;
            }

            var result = _authService.GetUserPagePermissions(_targetUser.Id);
            if (!result.Success)
            {
                HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                RefreshPermissionList();
                return;
            }

            var grantedSet = new HashSet<string>(result.Items, StringComparer.OrdinalIgnoreCase);
            foreach (var item in _allPermissions)
            {
                item.IsChecked = grantedSet.Contains(item.PageKey);
            }

            RefreshPermissionList();
        }

        private void RefreshPermissionList()
        {
            var selectedModule = ComboBoxModules.SelectedItem as PermissionModuleItem;
            if (selectedModule == null)
            {
                ListBoxPermissions.ItemsSource = null;
                ClearPermissionDetail();
                return;
            }

            var list = _allPermissions
                .Where(x => x.ModuleKey == selectedModule.Key)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DisplayName)
                .ToList();

            ListBoxPermissions.ItemsSource = list;
            ListBoxPermissions.SelectedIndex = list.Count > 0 ? 0 : -1;
        }

        private void UpdateSelectedUserDisplay(UserSummary user)
        {
            if (user == null)
            {
                TextBlockSelectedUser.Text = "未选择用户";
                return;
            }

            TextBlockSelectedUser.Text = "当前用户：" + user.LoginName + " / " + user.RoleDisplayName;
        }

        private void ClearPermissionDetail()
        {
            TextBlockPermissionName.Text = "-";
            TextBlockPermissionKey.Text = "页面键值：-";
            TextBlockPermissionRoles.Text = "建议角色：-";
            TextBlockPermissionRisk.Text = "风险级别：-";
            TextBlockPermissionDescription.Text = "请选择一个页面权限以查看说明。";
        }

        private sealed class PermissionModuleItem
        {
            public PermissionModuleItem(string key, string displayName)
            {
                Key = key;
                DisplayName = displayName;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }
        }

        private sealed class PermissionDisplayItem
        {
            public string ModuleKey { get; set; }

            public string ModuleName { get; set; }

            public string PageKey { get; set; }

            public string DisplayName { get; set; }

            public string Description { get; set; }

            public string RecommendedRoles { get; set; }

            public string RiskLevel { get; set; }

            public int SortOrder { get; set; }

            public bool IsChecked { get; set; }
        }
    }
}