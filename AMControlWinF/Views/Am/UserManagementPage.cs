using AM.Model.Auth;
using AM.PageModel.Am;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户管理页。
    /// 被 MainWindow 缓存复用，因此仅首次加载时初始化数据。
    /// </summary>
    public partial class UserManagementPage : UserControl
    {
        private readonly UserManagementPageModel _model;
        private AntList<UserTableRow> _tableRows;
        private bool _isInitialized;
        private int _selectedUserId;

        public UserManagementPage()
        {
            InitializeComponent();

            _model = new UserManagementPageModel();
            _tableRows = new AntList<UserTableRow>();

            InitTableColumns();
            BindEvents();
        }

        private void InitTableColumns()
        {
            tableUsers.Columns = new ColumnCollection()
            {
                new Column("", "序号", ColumnAlign.Center)
                {
                    Width = "56",
                    Fixed = true,
                    Render = (value, record, rowindex) => { return rowindex + 1; }
                },
                new Column("LoginName", "登录名", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("DisplayName", "用户名", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("RoleTags", "角色", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("StateBadge", "状态", ColumnAlign.Center)
                {
                    Width = "100"
                },
                new Column("LastLoginTimeText", "最后登录", ColumnAlign.Center)
                {
                    Width = "170"
                },
                new Column("RemarkText", "备注")
                {
                    Width = "220",
                    LineBreak = true
                }
            };
        }

        private void BindEvents()
        {
            Load += UserManagementPage_Load;

            buttonSearch.Click += ButtonSearch_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonRefresh.Click += async (sender, e) => await ReloadAsync(GetSelectedUserId());
            buttonAdd.Click += async (sender, e) => await AddUserAsync();

            buttonEdit.Click += async (sender, e) => await EditUserAsync();
            buttonToggleEnabled.Click += async (sender, e) => await ToggleEnabledAsync();
            buttonResetPassword.Click += async (sender, e) => await ResetPasswordAsync();
            buttonPermission.Click += ButtonPermission_Click;

            tableUsers.CellClick += TableUsers_CellClick;
            tableUsers.SetRowStyle += TableUsers_SetRowStyle;
        }

        private async void UserManagementPage_Load(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            await ReloadAsync(null);
        }

        private async Task ReloadAsync(int? preferredUserId)
        {
            var result = await _model.LoadAsync(preferredUserId);
            BindTable();

            if (!result.Success)
            {
                ShowLocalMessage(result.Message, TType.Warn, 4);
            }
        }

        private void BindTable()
        {
            _tableRows = new AntList<UserTableRow>();
            foreach (var user in _model.Users)
            {
                _tableRows.Add(new UserTableRow(user));
            }

            tableUsers.Binding(_tableRows);

            _selectedUserId = _model.SelectedUser == null ? 0 : _model.SelectedUser.Id;
            UpdateSummary();
            UpdateDetail();
            tableUsers.Invalidate();
        }

        private void UpdateSummary()
        {
            labelTotalValue.Text = _model.TotalUserCount.ToString();
            labelEnabledValue.Text = _model.EnabledUserCount.ToString();
            labelDisabledValue.Text = _model.DisabledUserCount.ToString();
        }

        private void UpdateDetail()
        {
            var user = _model.SelectedUser;

            labelDetailUserNameValue.Text = user == null ? "-" : user.DisplayName;
            labelDetailLoginNameValue.Text = user == null ? "-" : user.LoginName;
            labelDetailRoleValue.Text = user == null ? "-" : user.RoleDisplayName;
            labelDetailStateValue.Text = user == null ? "-" : user.StateText;
            labelDetailLastLoginValue.Text = user == null ? "-" : user.LastLoginTimeText;
            labelDetailRemarkValue.Text = user == null || string.IsNullOrWhiteSpace(user.Remark) ? "-" : user.Remark;

            var enabled = user != null;
            buttonEdit.Enabled = enabled;
            buttonToggleEnabled.Enabled = enabled;
            buttonResetPassword.Enabled = enabled;
            buttonPermission.Enabled = enabled;

            if (user == null)
            {
                buttonToggleEnabled.Text = "启用/禁用";
            }
            else
            {
                buttonToggleEnabled.Text = user.IsEnabled ? "禁用用户" : "启用用户";
            }
        }

        private int? GetSelectedUserId()
        {
            return _model.SelectedUser == null ? (int?)null : _model.SelectedUser.Id;
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            _model.SearchText = inputKeyword.Text == null ? string.Empty : inputKeyword.Text.Trim();
            _model.ApplyFilter(GetSelectedUserId());
            BindTable();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            inputKeyword.Text = string.Empty;
            _model.SearchText = string.Empty;
            _model.ApplyFilter(GetSelectedUserId());
            BindTable();
        }

        private void TableUsers_CellClick(object sender, TableClickEventArgs e)
        {
            var row = e.Record as UserTableRow;
            if (row == null)
            {
                return;
            }

            _selectedUserId = row.Id;
            _model.SelectedUser = row.User;
            UpdateDetail();
            tableUsers.Invalidate();
        }

        private AntdUI.Table.CellStyleInfo TableUsers_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _tableRows.Count)
            {
                return null;
            }

            var row = _tableRows[e.RowIndex];
            if (row == null)
            {
                return null;
            }

            if (row.Id == _selectedUserId)
            {
                return new AntdUI.Table.CellStyleInfo
                {
                    BackColor = Color.FromArgb(232, 242, 255)
                };
            }

            if (e.RowIndex % 2 == 1)
            {
                return new AntdUI.Table.CellStyleInfo
                {
                    BackColor = Color.FromArgb(250, 250, 250)
                };
            }

            return null;
        }

        private async Task AddUserAsync()
        {
            using (var dialog = new UserEditDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var result = await _model.CreateUserAsync(
                    dialog.LoginName,
                    dialog.UserName,
                    dialog.RoleCode,
                    dialog.Password,
                    dialog.IsEnabledUser,
                    dialog.Remark);

                if (!result.Success)
                {
                    ShowLocalMessage(result.Message, TType.Warn, 4);
                    return;
                }
            }

            await ReloadAsync(null);
        }

        private async Task EditUserAsync()
        {
            var user = _model.SelectedUser;
            if (user == null)
            {
                ShowLocalMessage("请先选择一个用户。", TType.Warn, 3);
                return;
            }

            using (var dialog = new UserEditDialog(user))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var result = await _model.UpdateUserAsync(
                    user.Id,
                    dialog.UserName,
                    dialog.RoleCode,
                    dialog.IsEnabledUser,
                    dialog.Remark);

                if (!result.Success)
                {
                    ShowLocalMessage(result.Message, TType.Warn, 4);
                    return;
                }
            }

            await ReloadAsync(user.Id);
        }

        private async Task ToggleEnabledAsync()
        {
            var user = _model.SelectedUser;
            if (user == null)
            {
                ShowLocalMessage("请先选择一个用户。", TType.Warn, 3);
                return;
            }

            var result = await _model.SetUserEnabledAsync(user.Id, !user.IsEnabled);
            if (!result.Success)
            {
                ShowLocalMessage(result.Message, TType.Warn, 4);
                return;
            }

            await ReloadAsync(user.Id);
        }

        private async Task ResetPasswordAsync()
        {
            var user = _model.SelectedUser;
            if (user == null)
            {
                ShowLocalMessage("请先选择一个用户。", TType.Warn, 3);
                return;
            }

            using (var dialog = new ResetUserPasswordDialog(user))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var result = await _model.ResetUserPasswordAsync(user.Id, dialog.NewPassword);
                if (!result.Success)
                {
                    ShowLocalMessage(result.Message, TType.Warn, 4);
                    return;
                }
            }

            await ReloadAsync(user.Id);
        }

        private void ButtonPermission_Click(object sender, EventArgs e)
        {
            var user = _model.SelectedUser;
            if (user == null)
            {
                ShowLocalMessage("请先选择一个用户。", TType.Warn, 3);
                return;
            }

            var mainWindow = FindForm() as MainWindow;
            if (mainWindow == null)
            {
                ShowLocalMessage("未找到主窗口。", TType.Warn, 3);
                return;
            }

            mainWindow.NavigateToPage("System.Permission");
        }

        private void ShowLocalMessage(string message, TType type, int autoClose)
        {
            var window = FindForm() as AntdUI.Window;
            if (window == null || string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            AntdUI.Message.open(new AntdUI.Message.Config(window, message, type)
            {
                Align = TAlignFrom.BL,
                AutoClose = autoClose
            });
        }

        private sealed class UserTableRow
        {
            public UserTableRow(UserSummary user)
            {
                User = user;
                Id = user.Id;
                LoginName = user.LoginName;
                DisplayName = user.DisplayName;
                LastLoginTimeText = user.LastLoginTimeText;
                RemarkText = string.IsNullOrWhiteSpace(user.Remark) ? "-" : user.Remark;
                RoleTags = new CellTag[]
                {
                    new CellTag(user.RoleDisplayName, GetRoleTagType(user.RoleCode))
                };
                StateBadge = new CellBadge(
                    user.IsEnabled ? TState.Success : TState.Error,
                    user.IsEnabled ? "已启用" : "已禁用");
            }

            public int Id { get; private set; }

            public string LoginName { get; private set; }

            public string DisplayName { get; private set; }

            public string LastLoginTimeText { get; private set; }

            public string RemarkText { get; private set; }

            public CellTag[] RoleTags { get; private set; }

            public CellBadge StateBadge { get; private set; }

            public UserSummary User { get; private set; }

            private static TTypeMini GetRoleTagType(string roleCode)
            {
                if (string.Equals(roleCode, "Am", StringComparison.OrdinalIgnoreCase))
                {
                    return TTypeMini.Error;
                }

                if (string.Equals(roleCode, "Engineer", StringComparison.OrdinalIgnoreCase))
                {
                    return TTypeMini.Warn;
                }

                return TTypeMini.Primary;
            }
        }
    }
}