using AM.Core.Context;
using AM.DBService.Services.Auth;
using AM.Model.Auth;
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
        private readonly List<RoleFilterItem> _roleFilters;
        private readonly List<PermissionModuleItem> _modules;
        private readonly List<PermissionDisplayItem> _allPermissions;
        private UserSummary _targetUser;

        public UserPermissionView()
        {
            InitializeComponent();

            _authService = new AuthService();
            _allUsers = new List<UserSummary>();
            _roleFilters = new List<RoleFilterItem>();
            _modules = new List<PermissionModuleItem>();
            _allPermissions = new List<PermissionDisplayItem>();

            Loaded += UserPermissionView_Loaded;
        }

        private void UserPermissionView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserPermissionView_Loaded;
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

            _roleFilters.Clear();
            _roleFilters.Add(new RoleFilterItem(string.Empty, "全部用户"));
            _roleFilters.Add(new RoleFilterItem("Am", "管理员"));
            _roleFilters.Add(new RoleFilterItem("Engineer", "工程师"));
            _roleFilters.Add(new RoleFilterItem("Operator", "操作员"));

            ListBoxRoleFilters.ItemsSource = _roleFilters;
            ModuleNavList.ItemsSource = _modules;

            if (_roleFilters.Count > 0)
            {
                ListBoxRoleFilters.SelectedIndex = 0;
            }

            if (_modules.Count > 0)
            {
                ModuleNavList.SelectedIndex = 0;
            }

            ApplyUserFilter();
        }

        private void TextBoxUserSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyUserFilter();
        }

        private void ListBoxRoleFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyUserFilter();
        }

        private void ApplyUserFilter()
        {
            var keyword = TextBoxUserSearch.Text == null
                ? string.Empty
                : TextBoxUserSearch.Text.Trim().ToLowerInvariant();

            var selectedRoleFilter = ListBoxRoleFilters.SelectedItem as RoleFilterItem;
            var selectedUserId = _targetUser == null ? 0 : _targetUser.Id;

            IEnumerable<UserSummary> query = _allUsers;

            if (selectedRoleFilter != null && !string.IsNullOrWhiteSpace(selectedRoleFilter.RoleCode))
            {
                query = query.Where(x => string.Equals(x.RoleCode, selectedRoleFilter.RoleCode, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.LoginName) && x.LoginName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.RoleName) && x.RoleName.ToLowerInvariant().Contains(keyword)));
            }

            var list = query.ToList();
            ListBoxUsers.ItemsSource = list;

            if (selectedUserId > 0)
            {
                var currentUser = list.FirstOrDefault(x => x.Id == selectedUserId);
                if (currentUser != null)
                {
                    ListBoxUsers.SelectedItem = currentUser;
                    return;
                }
            }

            if (list.Count > 0)
            {
                ListBoxUsers.SelectedIndex = 0;
            }
            else
            {
                _targetUser = null;
                UpdateSelectedUserDisplay(null);
                ClearPermissionDetail();
                ListBoxPermissions.ItemsSource = null;
            }
        }

        private void ListBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _targetUser = ListBoxUsers.SelectedItem as UserSummary;
            UpdateSelectedUserDisplay(_targetUser);
            LoadUserPermissions();
        }

        private void ModuleNavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            var selectedModule = ModuleNavList.SelectedItem as PermissionModuleItem;
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
            var selectedModule = ModuleNavList.SelectedItem as PermissionModuleItem;
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
            if (!result.Success)
            {
                HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RefreshCurrentUserPermissionContext(true);

            //HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Information);
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
                ClearPermissionDetail();
                return;
            }

            var result = _authService.GetUserPagePermissions(_targetUser.Id);
            if (!result.Success)
            {
                HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                RefreshPermissionList();
                ClearPermissionDetail();
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
            var selectedModule = ModuleNavList.SelectedItem as PermissionModuleItem;
            if (selectedModule == null)
            {
                ListBoxPermissions.ItemsSource = null;
                TextBlockModuleTitle.Text = "页面权限";
                ClearPermissionDetail();
                return;
            }

            TextBlockModuleTitle.Text = selectedModule.DisplayName + " · 页面权限";

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

        private void ButtonRestoreDefault_OnClick(object sender, RoutedEventArgs e)
        {
            if (_targetUser == null)
            {
                HandyControl.Controls.MessageBox.Show("请先选择一个用户。", "权限分配", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = _authService.RestoreDefaultPagePermissions(_targetUser.Id);
            if (!result.Success)
            {
                HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoadUserPermissions();
            RefreshCurrentUserPermissionContext(false);

            //HandyControl.Controls.MessageBox.Show(result.Message, "权限分配", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshCurrentUserPermissionContext(bool useCustomPagePermission)
        {
            if (!UserContext.Instance.IsLoggedIn ||
                UserContext.Instance.CurrentUser == null ||
                _targetUser == null ||
                UserContext.Instance.CurrentUser.Id != _targetUser.Id)
            {
                return;
            }

            var permissionResult = _authService.GetUserPagePermissions(_targetUser.Id);
            if (!permissionResult.Success)
            {
                return;
            }

            UserContext.Instance.RefreshPagePermissions(permissionResult.Items, useCustomPagePermission);

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.RefreshNavigationByCurrentUser();
            }
        }

        private void ClearPermissionDetail()
        {
            TextBlockPermissionDescription.Text = "请选择一个权限磁贴查看说明。";
        }

        private sealed class RoleFilterItem
        {
            public RoleFilterItem(string roleCode, string displayName)
            {
                RoleCode = roleCode;
                DisplayName = displayName;
            }

            public string RoleCode { get; private set; }

            public string DisplayName { get; private set; }
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