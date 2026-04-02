using AM.Core.Context;
using AM.DBService.Services.Auth;
using AM.Model.Auth;
using AM.Model.Common;
using AM.PageModel.Common;
using AM.PageModel.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Am
{
    /// <summary>
    /// 用户权限分配页面模型。
    /// </summary>
    public class UserPermissionPageModel : BindableBase
    {
        private readonly AuthService _authService;
        private List<PermissionModuleItem> _modules;
        private List<PermissionCardItem> _allPermissions;
        private List<PermissionCardItem> _visiblePermissions;
        private UserSummary _targetUser;
        private string _selectedModuleKey;

        public UserPermissionPageModel()
        {
            _authService = new AuthService();
            _modules = new List<PermissionModuleItem>();
            _allPermissions = new List<PermissionCardItem>();
            _visiblePermissions = new List<PermissionCardItem>();
            _selectedModuleKey = string.Empty;
        }

        public IList<PermissionModuleItem> Modules
        {
            get { return _modules; }
        }

        public IList<PermissionCardItem> VisiblePermissions
        {
            get { return _visiblePermissions; }
        }

        public UserSummary TargetUser
        {
            get { return _targetUser; }
            private set
            {
                if (SetProperty(ref _targetUser, value))
                {
                    OnPropertyChanged(nameof(TargetUserDisplayText));
                    OnPropertyChanged(nameof(HasTargetUser));
                }
            }
        }

        public bool HasTargetUser
        {
            get { return TargetUser != null; }
        }

        public string TargetUserDisplayText
        {
            get
            {
                if (TargetUser == null)
                    return "当前用户：未选择";

                return "当前用户：" + (TargetUser.LoginName ?? "-") + " / " + (TargetUser.RoleDisplayName ?? "-");
            }
        }

        public string SelectedModuleKey
        {
            get { return _selectedModuleKey; }
            private set { SetProperty(ref _selectedModuleKey, value ?? string.Empty); }
        }

        public string SelectedModuleDisplayText
        {
            get
            {
                var module = _modules.FirstOrDefault(x => string.Equals(x.Key, SelectedModuleKey, StringComparison.OrdinalIgnoreCase));
                return module == null ? "页面权限" : module.DisplayName + " · 页面权限";
            }
        }

        public async Task<Result> LoadAsync()
        {
            var permissionResult = await Task.Run(() => _authService.GetPagePermissions());
            if (!permissionResult.Success)
            {
                _modules = new List<PermissionModuleItem>();
                _allPermissions = new List<PermissionCardItem>();
                _visiblePermissions = new List<PermissionCardItem>();
                OnPropertyChanged(nameof(Modules));
                OnPropertyChanged(nameof(VisiblePermissions));
                OnPropertyChanged(nameof(SelectedModuleDisplayText));
                return Result.Fail(permissionResult.Code, permissionResult.Message, permissionResult.Source);
            }

            _allPermissions = permissionResult.Items == null
                ? new List<PermissionCardItem>()
                : permissionResult.Items
                    .Where(x => x != null && x.IsEnabled)
                    .Select(x => new PermissionCardItem
                    {
                        ModuleKey = x.ModuleKey ?? string.Empty,
                        ModuleName = x.ModuleName ?? string.Empty,
                        PageKey = x.PageKey ?? string.Empty,
                        DisplayName = x.DisplayName ?? string.Empty,
                        Description = x.Description ?? string.Empty,
                        RecommendedRoles = x.RecommendedRoles ?? string.Empty,
                        RiskLevel = x.RiskLevel ?? string.Empty,
                        SortOrder = x.SortOrder,
                        IsChecked = false
                    })
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.DisplayName)
                    .ToList();

            BuildModules();
            if (_modules.Count > 0 && string.IsNullOrWhiteSpace(SelectedModuleKey))
            {
                SelectedModuleKey = _modules[0].Key;
            }

            RefreshVisiblePermissions();
            return Result.Ok("加载页面权限成功");
        }

        public void SetTargetUser(UserSummary user)
        {
            TargetUser = user;
        }

        public async Task<Result> LoadTargetUserPermissionsAsync()
        {
            foreach (var item in _allPermissions)
            {
                item.IsChecked = false;
            }

            if (TargetUser == null)
            {
                RefreshVisiblePermissions();
                return Result.Ok("未选择用户");
            }

            var result = await Task.Run(() => _authService.GetUserPagePermissions(TargetUser.Id));
            if (!result.Success)
            {
                RefreshVisiblePermissions();
                return Result.Fail(result.Code, result.Message, result.Source);
            }

            var grantedSet = new HashSet<string>(
                result.Items == null ? new List<string>() : result.Items,
                StringComparer.OrdinalIgnoreCase);

            foreach (var item in _allPermissions)
            {
                item.IsChecked = grantedSet.Contains(item.PageKey);
            }

            RefreshVisiblePermissions();
            return Result.Ok("加载用户权限成功");
        }

        public void SelectModule(string moduleKey)
        {
            if (string.IsNullOrWhiteSpace(moduleKey))
                return;

            SelectedModuleKey = moduleKey;
            RefreshVisiblePermissions();
        }

        public void SelectAllCurrentModule()
        {
            if (string.IsNullOrWhiteSpace(SelectedModuleKey))
                return;

            foreach (var item in _allPermissions.Where(x => string.Equals(x.ModuleKey, SelectedModuleKey, StringComparison.OrdinalIgnoreCase)))
            {
                item.IsChecked = true;
            }

            RefreshVisiblePermissions();
        }

        public void ClearCurrentModule()
        {
            if (string.IsNullOrWhiteSpace(SelectedModuleKey))
                return;

            foreach (var item in _allPermissions.Where(x => string.Equals(x.ModuleKey, SelectedModuleKey, StringComparison.OrdinalIgnoreCase)))
            {
                item.IsChecked = false;
            }

            RefreshVisiblePermissions();
        }

        public async Task<Result> RestoreDefaultAsync()
        {
            if (TargetUser == null)
                return Result.Fail(-1, "未选择用户");

            var result = await Task.Run(() => _authService.RestoreDefaultPagePermissions(TargetUser.Id));
            if (!result.Success)
                return result;

            var reloadResult = await LoadTargetUserPermissionsAsync();
            if (!reloadResult.Success)
                return reloadResult;

            return result;
        }

        public async Task<Result> SaveAsync()
        {
            if (TargetUser == null)
                return Result.Fail(-1, "未选择用户");

            var selectedKeys = _allPermissions
                .Where(x => x.IsChecked)
                .Select(x => x.PageKey)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return await Task.Run(() => _authService.SaveUserPagePermissions(TargetUser.Id, selectedKeys));
        }

        public bool RefreshCurrentUserPermissionContext(bool useCustomPagePermission)
        {
            if (!UserContext.Instance.IsLoggedIn ||
                UserContext.Instance.CurrentUser == null ||
                TargetUser == null ||
                UserContext.Instance.CurrentUser.Id != TargetUser.Id)
            {
                return false;
            }

            var permissionResult = _authService.GetUserPagePermissions(TargetUser.Id);
            if (!permissionResult.Success)
                return false;

            UserContext.Instance.RefreshPagePermissions(permissionResult.Items, useCustomPagePermission);
            return true;
        }

        private void BuildModules()
        {
            var primaryOrder = NavigationCatalog.GetPrimaryItems()
                .Select((x, index) => new { x.Key, Index = index })
                .ToDictionary(x => x.Key, x => x.Index, StringComparer.OrdinalIgnoreCase);

            _modules = _allPermissions
                .GroupBy(x => x.ModuleKey)
                .Select(x => new PermissionModuleItem(
                    x.Key,
                    x.First().ModuleName,
                    primaryOrder.ContainsKey(x.Key) ? primaryOrder[x.Key] : int.MaxValue))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DisplayName)
                .ToList();

            OnPropertyChanged(nameof(Modules));
        }

        private void RefreshVisiblePermissions()
        {
            _visiblePermissions = _allPermissions
                .Where(x => string.Equals(x.ModuleKey, SelectedModuleKey, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DisplayName)
                .ToList();

            OnPropertyChanged(nameof(VisiblePermissions));
            OnPropertyChanged(nameof(SelectedModuleDisplayText));
        }

        public sealed class PermissionModuleItem
        {
            public PermissionModuleItem(string key, string displayName, int sortOrder)
            {
                Key = key;
                DisplayName = displayName;
                SortOrder = sortOrder;
            }

            public string Key { get; private set; }

            public string DisplayName { get; private set; }

            public int SortOrder { get; private set; }
        }

        public sealed class PermissionCardItem
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