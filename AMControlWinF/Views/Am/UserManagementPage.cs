using AM.Model.Auth;
using AM.PageModel.Am;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// </summary>
    public partial class UserManagementPage : UserControl
    {
        private readonly UserManagementPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;
        private List<UserSummary> _visibleUsers;

        public UserManagementPage()
        {
            InitializeComponent();

            _model = new UserManagementPageModel();
            _visibleUsers = new List<UserSummary>();

            BindEvents();
            RefreshActionButtons();
        }

        private void BindEvents()
        {
            Load += UserManagementPage_Load;

            inputSearch.TextChanged += InputSearch_TextChanged;

            buttonEditUser.Click += async (s, e) => await EditUserAsync();
            buttonToggleEnabled.Click += async (s, e) => await ToggleUserEnabledAsync();
            buttonDeleteUser.Click += async (s, e) => await DeleteUserAsync();
            buttonAddUser.Click += async (s, e) => await AddUserAsync();

            tableUsers.CellClick += TableUsers_CellClick;
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
                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "加载失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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
            _visibleUsers = _model.Users.ToList();

            var table = new DataTable();
            table.Columns.Add("登录名");
            table.Columns.Add("用户名");
            table.Columns.Add("角色");
            table.Columns.Add("状态");
            table.Columns.Add("最后登录时间");
            table.Columns.Add("备注");

            foreach (var user in _visibleUsers)
            {
                table.Rows.Add(
                    user.LoginName ?? string.Empty,
                    user.DisplayName ?? string.Empty,
                    user.RoleDisplayName ?? string.Empty,
                    user.StateText ?? string.Empty,
                    user.LastLoginTimeText ?? string.Empty,
                    user.Remark ?? string.Empty);
            }

            tableUsers.DataSource = table;
        }

        private void TableUsers_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e == null)
                return;

            if (e.RowIndex < 0 || e.RowIndex >= _visibleUsers.Count)
                return;

            _model.SelectedUser = _visibleUsers[e.RowIndex];
            RefreshActionButtons();
        }

        private void RefreshActionButtons()
        {
            var selected = _model.SelectedUser;
            var hasSelection = selected != null;

            buttonEditUser.Enabled = !_isBusy && hasSelection;
            buttonToggleEnabled.Enabled = !_isBusy && hasSelection;
            buttonDeleteUser.Enabled = !_isBusy && hasSelection;
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

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "新增用户失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

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

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "编辑用户失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                await ReloadAsync(selected.Id);
            }
        }

        private async Task ToggleUserEnabledAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            var newEnabled = !selected.IsEnabled;
            var actionText = newEnabled ? "启用" : "禁用";

            var confirm = MessageBox.Show(
                "确定要" + actionText + "用户“" + selected.LoginName + "”吗？",
                actionText + "用户",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.OK)
                return;

            var result = await _model.SetUserEnabledAsync(selected.Id, newEnabled);
            if (!result.Success)
            {
                MessageBox.Show(
                    result.Message,
                    actionText + "用户失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await ReloadAsync(selected.Id);
        }

        private async Task DeleteUserAsync()
        {
            var selected = _model.SelectedUser;
            if (selected == null)
                return;

            var confirm = MessageBox.Show(
                "确定要删除用户“" + selected.LoginName + "”吗？\r\n此操作不可恢复。",
                "删除用户",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.OK)
                return;

            var result = await _model.DeleteUserAsync(selected.Id);
            if (!result.Success)
            {
                MessageBox.Show(
                    result.Message,
                    "删除用户失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await ReloadAsync(null);
        }
    }
}