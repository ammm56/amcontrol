using AM.Model.Auth;
using AM.PageModel.Am;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户管理页面。
    ///
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放 ViewModel；
    /// - 首次加载使用布尔标记控制，避免重复初始化。
    /// - 数据处理成功/失败提示由系统消息总线统一负责；
    /// - 页面仅保留必要的交互确认类对话框。
    /// </summary>
    public partial class UserManagementPage : UserControl
    {
        private readonly UserManagementPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private AntList<UserTableRow> _tableRows;
        private bool _selectionMode;

        public UserManagementPage()
        {
            InitializeComponent();

            _model = new UserManagementPageModel();
            _tableRows = new AntList<UserTableRow>();

            InitializeTableColumns();
            BindEvents();
            RefreshActionButtons();

            ApplySelectionMode();
        }

        public UserSummary SelectedUserSummary
        {
            get { return _model.SelectedUser; }
        }

        private void BindEvents()
        {
            Load += UserManagementPage_Load;

            inputSearch.TextChanged += InputSearch_TextChanged;

            buttonEditUser.Click += async (s, e) => await EditUserAsync();
            buttonToggleEnabled.Click += async (s, e) => await ToggleUserEnabledAsync();
            buttonDeleteUser.Click += async (s, e) => await DeleteUserAsync();
            buttonAddUser.Click += async (s, e) => await AddUserAsync();
            buttonResetPwd.Click += async (s, e) => await ResetUserPasswordAsync();

            tableUsers.CellClick += TableUsers_CellClick;
        }

        private void InitializeTableColumns()
        {
            tableUsers.Columns = new ColumnCollection()
            {
                new Column("", "序号", ColumnAlign.Center)
                {
                    Width = "60",
                    Fixed = true,
                    Render = (value, record, rowindex) => rowindex + 1
                },
                new Column("LoginName", "登录名", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("DisplayName", "用户名", ColumnAlign.Left)
                {
                    Width = "140",
                    Fixed = true
                },
                new Column("RoleTag", "角色", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("StateTag", "状态", ColumnAlign.Center)
                {
                    Width = "110"
                },
                new Column("LastLoginText", "最后登录时间", ColumnAlign.Center)
                {
                    Width = "180"
                },
                new Column("RemarkText", "备注", ColumnAlign.Left)
                {
                    LineBreak = true
                }
            };

            tableUsers.StackedHeaderRows = new StackedHeaderRow[]
            {
                new StackedHeaderRow(
                    new StackedColumn("LoginName,DisplayName,RoleTag", "用户信息"),
                    new StackedColumn("StateTag,LastLoginText,RemarkText", "状态与说明"))
            };
        }

        private async void UserManagementPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await ReloadAsync(null);
        }

        private async Task ReloadAsync(int? preferredUserId)
        {
            if (_isBusy)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync(preferredUserId);

                // 加载失败消息由系统消息总线统一负责，这里不再重复弹页面对话框。
                if (!result.Success)
                {
                    RefreshStatCards();
                    RebindTable();
                    RefreshActionButtons();
                    return;
                }

                RefreshStatCards();
                RebindTable();
                RefreshActionButtons();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;

            inputSearch.Enabled = !isBusy;
            buttonEditUser.Enabled = !isBusy && _model.SelectedUser != null;
            buttonToggleEnabled.Enabled = !isBusy && _model.SelectedUser != null;
            buttonDeleteUser.Enabled = !isBusy && _model.SelectedUser != null;
            buttonResetPwd.Enabled = !isBusy && _model.SelectedUser != null;
            buttonAddUser.Enabled = !isBusy;
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SearchText = inputSearch.Text;
            _model.ApplyFilter();
            RebindTable();
            RefreshActionButtons();
        }

        private void RefreshStatCards()
        {
            labelTotalCount.Text = _model.TotalUserCount.ToString();
            labelEnabledCount.Text = _model.EnabledUserCount.ToString();
            labelDisabledCount.Text = _model.DisabledUserCount.ToString();
        }

        private void RebindTable()
        {
            _tableRows = new AntList<UserTableRow>();

            foreach (var user in _model.Users)
            {
                _tableRows.Add(new UserTableRow
                {
                    User = user,
                    LoginName = user.LoginName ?? string.Empty,
                    DisplayName = new CellText(user.DisplayName ?? string.Empty),
                    RoleTag = BuildRoleTag(user),
                    StateTag = new CellTag(
                        user.IsEnabled ? "已启用" : "已禁用",
                        user.IsEnabled ? TTypeMini.Success : TTypeMini.Error),
                    LastLoginText = new CellText(user.LastLoginTimeText ?? "-"),
                    RemarkText = new CellText(string.IsNullOrWhiteSpace(user.Remark) ? "-" : user.Remark)
                });
            }

            tableUsers.Binding(_tableRows);
        }

        private static CellTag BuildRoleTag(UserSummary user)
        {
            var roleCode = user == null || string.IsNullOrWhiteSpace(user.RoleCode)
                ? string.Empty
                : user.RoleCode.Trim();

            switch (roleCode)
            {
                case "Am":
                    return new CellTag("管理员", TTypeMini.Error);
                case "Engineer":
                    return new CellTag("工程师", TTypeMini.Warn);
                case "Operator":
                    return new CellTag("操作员", TTypeMini.Primary);
                default:
                    return new CellTag(
                        string.IsNullOrWhiteSpace(user == null ? null : user.RoleDisplayName) ? "未分配" : user.RoleDisplayName,
                        TTypeMini.Default);
            }
        }

        private void TableUsers_CellClick(object sender, TableClickEventArgs e)
        {
            if (e == null)
                return;

            var row = e.Record as UserTableRow;
            if (row == null || row.User == null)
                return;

            _model.SelectedUser = row.User;
            RefreshActionButtons();
        }

        private void RefreshActionButtons()
        {
            var selected = _model.SelectedUser;
            var hasSelection = selected != null;

            buttonEditUser.Enabled = !_isBusy && hasSelection;
            buttonToggleEnabled.Enabled = !_isBusy && hasSelection;
            buttonDeleteUser.Enabled = !_isBusy && hasSelection;
            buttonResetPwd.Enabled = !_isBusy && hasSelection;
            buttonAddUser.Enabled = !_isBusy;

            if (hasSelection)
            {
                buttonToggleEnabled.Text = selected.IsEnabled ? "禁用用户" : "启用用户";
                buttonToggleEnabled.IconSvg = selected.IsEnabled ? "StopOutlined" : "CheckCircleOutlined";
            }
            else
            {
                buttonToggleEnabled.Text = "启用/禁用";
                buttonToggleEnabled.IconSvg = "SwapOutlined";
            }
        }

        private async Task AddUserAsync()
        {
            using (var dialog = new UserEditDialog())
            {
                dialog.IsCreateMode = true;
                dialog.Text = "新增用户";

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var result = await _model.CreateUserAsync(
                    dialog.LoginName,
                    dialog.UserDisplayName,
                    dialog.RoleCode,
                    dialog.Password,
                    dialog.IsEnabled,
                    dialog.Remark);

                // 成功/失败均由系统消息总线统一提示。
                if (result.Success)
                    await ReloadAsync(null);
            }
        }

        private async Task EditUserAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            using (var dialog = new UserEditDialog())
            {
                dialog.IsCreateMode = false;
                dialog.Text = "编辑用户";
                dialog.LoginName = selected.LoginName;
                dialog.UserDisplayName = selected.UserName;
                dialog.RoleCode = selected.RoleCode;
                dialog.IsEnabled = selected.IsEnabled;
                dialog.Remark = selected.Remark;

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var result = await _model.UpdateUserAsync(
                    selected.Id,
                    dialog.UserDisplayName,
                    dialog.RoleCode,
                    dialog.IsEnabled,
                    dialog.Remark);

                // 成功/失败均由系统消息总线统一提示。
                if (result.Success)
                    await ReloadAsync(selected.Id);
            }
        }

        private async Task ResetUserPasswordAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            using (var dialog = new ResetUserPasswordDialog())
            {
                dialog.Text = "重置用户密码";
                dialog.TargetLoginName = selected.LoginName;
                dialog.TargetDisplayName = selected.DisplayName;

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                await _model.ResetUserPasswordAsync(selected.Id, dialog.NewPassword);

                // 重置密码结果由系统消息总线统一提示。
            }
        }

        private async Task ToggleUserEnabledAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            var newEnabled = !selected.IsEnabled;
            var actionText = newEnabled ? "启用" : "禁用";

            if (!PageDialogHelper.Confirm(
                this,
                actionText + "用户",
                "确定要" + actionText + "用户“" + selected.LoginName + "”吗？",
                TType.Warn))
            {
                return;
            }

            var result = await _model.SetUserEnabledAsync(selected.Id, newEnabled);

            // 执行结果由系统消息总线统一提示。
            if (result.Success)
                await ReloadAsync(selected.Id);
        }

        private async Task DeleteUserAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            if (!PageDialogHelper.Confirm(
                this,
                "删除用户",
                "确定要删除用户“" + selected.LoginName + "”吗？\r\n此操作不可恢复。",
                TType.Error))
            {
                return;
            }

            var result = await _model.DeleteUserAsync(selected.Id);

            // 执行结果由系统消息总线统一提示。
            if (result.Success)
                await ReloadAsync(null);
        }

        public void SetSelectionMode(bool selectionMode)
        {
            _selectionMode = selectionMode;
            ApplySelectionMode();
        }

        private void ApplySelectionMode()
        {
            if (flowStats == null || flowActionsRight == null)
                return;

            flowStats.Visible = !_selectionMode;
            flowActionsRight.Visible = !_selectionMode;
        }

        private sealed class UserTableRow
        {
            public UserSummary User { get; set; }

            public string LoginName { get; set; }

            public CellText DisplayName { get; set; }

            public CellTag RoleTag { get; set; }

            public CellTag StateTag { get; set; }

            public CellText LastLoginText { get; set; }

            public CellText RemarkText { get; set; }
        }
    }
}