using AM.DBService.Services.Auth;
using AM.Model.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Am
{
    /// <summary>
    /// 用户管理视图模型。
    /// </summary>
    public class UserManagementViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly List<UserSummary> _allUsers;

        private string _searchText;
        private string _selectedGroup;
        private UserSummary _selectedUser;
        private int _totalUserCount;
        private int _enabledUserCount;
        private int _disabledUserCount;

        public UserManagementViewModel()
        {
            _authService = new AuthService();
            _allUsers = new List<UserSummary>();

            Users = new ObservableCollection<UserSummary>();
            UserGroups = new ObservableCollection<string>();

            UserGroups.Add("全部用户");
            UserGroups.Add("管理员");
            UserGroups.Add("工程师");
            UserGroups.Add("操作员");
            UserGroups.Add("已禁用用户");

            _selectedGroup = "全部用户";
            _searchText = string.Empty;

            RefreshCommand = new AsyncRelayCommand(LoadAsync);
        }

        public ObservableCollection<UserSummary> Users { get; private set; }

        public ObservableCollection<string> UserGroups { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public string SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (SetProperty(ref _selectedGroup, value))
                {
                    ApplyFilter();
                }
            }
        }

        public UserSummary SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public int TotalUserCount
        {
            get { return _totalUserCount; }
            set { SetProperty(ref _totalUserCount, value); }
        }

        public int EnabledUserCount
        {
            get { return _enabledUserCount; }
            set { SetProperty(ref _enabledUserCount, value); }
        }

        public int DisabledUserCount
        {
            get { return _disabledUserCount; }
            set { SetProperty(ref _disabledUserCount, value); }
        }

        public async Task LoadAsync()
        {
            var result = await Task.Run(() => _authService.GetUserSummaries());
            if (!result.Success)
            {
                Users.Clear();
                _allUsers.Clear();
                TotalUserCount = 0;
                EnabledUserCount = 0;
                DisabledUserCount = 0;
                SelectedUser = null;
                return;
            }

            _allUsers.Clear();
            _allUsers.AddRange(result.Items);

            TotalUserCount = _allUsers.Count;
            EnabledUserCount = _allUsers.Count(x => x.IsEnabled);
            DisabledUserCount = _allUsers.Count(x => !x.IsEnabled);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            IEnumerable<UserSummary> query = _allUsers;

            if (!string.IsNullOrWhiteSpace(SelectedGroup))
            {
                switch (SelectedGroup)
                {
                    case "管理员":
                        query = query.Where(x => x.RoleCode == "Am");
                        break;
                    case "工程师":
                        query = query.Where(x => x.RoleCode == "Engineer");
                        break;
                    case "操作员":
                        query = query.Where(x => x.RoleCode == "Operator");
                        break;
                    case "已禁用用户":
                        query = query.Where(x => !x.IsEnabled);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.LoginName) && x.LoginName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.RoleName) && x.RoleName.ToLowerInvariant().Contains(keyword)));
            }

            var list = query.ToList();

            Users.Clear();
            foreach (var item in list)
            {
                Users.Add(item);
            }

            SelectedUser = Users.Count > 0 ? Users[0] : null;
        }
    }
}