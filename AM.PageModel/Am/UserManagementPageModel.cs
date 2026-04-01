using AM.DBService.Services.Auth;
using AM.Model.Auth;
using AM.Model.Common;
using AM.PageModel.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Am
{
    /// <summary>
    /// 用户管理页面模型。
    /// </summary>
    public class UserManagementPageModel : BindableBase
    {
        private readonly AuthService _authService;
        private List<UserSummary> _allUsers;
        private List<UserSummary> _users;
        private UserSummary _selectedUser;
        private string _searchText;
        private int _totalUserCount;
        private int _enabledUserCount;
        private int _disabledUserCount;

        public UserManagementPageModel()
        {
            _authService = new AuthService();
            _allUsers = new List<UserSummary>();
            _users = new List<UserSummary>();
            _searchText = string.Empty;
        }

        public IList<UserSummary> Users
        {
            get { return _users; }
        }

        public UserSummary SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value ?? string.Empty); }
        }

        public int TotalUserCount
        {
            get { return _totalUserCount; }
            private set { SetProperty(ref _totalUserCount, value); }
        }

        public int EnabledUserCount
        {
            get { return _enabledUserCount; }
            private set { SetProperty(ref _enabledUserCount, value); }
        }

        public int DisabledUserCount
        {
            get { return _disabledUserCount; }
            private set { SetProperty(ref _disabledUserCount, value); }
        }

        public async Task<Result> LoadAsync(int? preferredUserId = null)
        {
            var result = await Task.Run(() => _authService.GetUserSummaries());
            if (!result.Success)
            {
                _allUsers = new List<UserSummary>();
                _users = new List<UserSummary>();
                SelectedUser = null;
                TotalUserCount = 0;
                EnabledUserCount = 0;
                DisabledUserCount = 0;
                OnPropertyChanged(nameof(Users));
                return Result.Fail(result.Code, result.Message, result.Source);
            }

            _allUsers = result.Items == null
                ? new List<UserSummary>()
                : result.Items.ToList();

            TotalUserCount = _allUsers.Count;
            EnabledUserCount = _allUsers.Count(x => x.IsEnabled);
            DisabledUserCount = _allUsers.Count(x => !x.IsEnabled);

            ApplyFilter(preferredUserId);
            return Result.Ok("加载用户列表成功");
        }

        public void ApplyFilter(int? preferredUserId = null)
        {
            IEnumerable<UserSummary> query = _allUsers;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.LoginName) && x.LoginName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.UserName) && x.UserName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.RoleName) && x.RoleName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.Remark) && x.Remark.ToLowerInvariant().Contains(keyword)));
            }

            _users = query
                .OrderByDescending(x => x.IsEnabled)
                .ThenBy(x => x.LoginName)
                .ToList();

            OnPropertyChanged(nameof(Users));

            UserSummary selected = null;

            if (preferredUserId.HasValue)
            {
                selected = _users.FirstOrDefault(x => x.Id == preferredUserId.Value);
            }

            if (selected == null && SelectedUser != null)
            {
                selected = _users.FirstOrDefault(x => x.Id == SelectedUser.Id);
            }

            if (selected == null && _users.Count > 0)
            {
                selected = _users[0];
            }

            SelectedUser = selected;
        }

        public async Task<Result> CreateUserAsync(
            string loginName,
            string userName,
            string roleCode,
            string password,
            bool isEnabled,
            string remark)
        {
            return await Task.Run(() =>
                _authService.CreateUser(loginName, userName, roleCode, password, isEnabled, remark));
        }

        public async Task<Result> UpdateUserAsync(
            int userId,
            string userName,
            string roleCode,
            bool isEnabled,
            string remark)
        {
            return await Task.Run(() =>
                _authService.UpdateUser(userId, userName, roleCode, isEnabled, remark));
        }

        public async Task<Result> SetUserEnabledAsync(int userId, bool isEnabled)
        {
            return await Task.Run(() => _authService.SetUserEnabled(userId, isEnabled));
        }

        public async Task<Result> ResetUserPasswordAsync(int userId, string newPassword)
        {
            return await Task.Run(() => _authService.ResetUserPassword(userId, newPassword));
        }
    }
}