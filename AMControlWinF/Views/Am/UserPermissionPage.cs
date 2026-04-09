using AM.Model.Auth;
using AM.PageModel.Am;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Am
{
    /// <summary>
    /// 用户页面权限分配页。
    /// 被 MainWindow 缓存复用，首次加载采用布尔标记控制。
    /// </summary>
    public partial class UserPermissionPage : UserControl
    {
        private readonly UserPermissionPageModel _model;
        private bool _isFirstLoad;
        private bool _isBusy;

        public UserPermissionPage()
        {
            InitializeComponent();

            _model = new UserPermissionPageModel();

            BindEvents();
            UpdateActionButtons();
        }

        private void BindEvents()
        {
            Load += UserPermissionPage_Load;

            buttonSelectUser.Click += async (s, e) => await SelectUserAsync();
            buttonSelectAll.Click += ButtonSelectAll_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonRestoreDefault.Click += async (s, e) => await RestoreDefaultAsync();
            buttonSave.Click += async (s, e) => await SaveAsync();
        }

        private async void UserPermissionPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            SetBusyState(true);
            try
            {
                var result = await _model.LoadAsync();
                if (!result.Success)
                {
                    labelModuleTitle.Text = "页面权限";
                    labelCurrentUser.Text = "当前用户：权限目录加载失败";
                    BuildModuleButtons();
                    BuildPermissionCards();
                    UpdateActionButtons();
                    return;
                }

                labelCurrentUser.Text = _model.TargetUserDisplayText;
                labelModuleTitle.Text = _model.SelectedModuleDisplayText;

                BuildModuleButtons();
                BuildPermissionCards();
                UpdateActionButtons();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task SelectUserAsync()
        {
            using (var dialog = new UserPermissionUserSelectDialog())
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                var user = dialog.SelectedUser;
                if (user == null)
                    return;

                _model.SetTargetUser(user);
                labelCurrentUser.Text = _model.TargetUserDisplayText;

                SetBusyState(true);
                try
                {
                    await _model.LoadTargetUserPermissionsAsync();
                    labelModuleTitle.Text = _model.SelectedModuleDisplayText;
                    BuildPermissionCards();
                    UpdateActionButtons();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (_isBusy || !_model.HasTargetUser)
                return;

            _model.SelectAllCurrentModule();
            BuildPermissionCards();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            if (_isBusy || !_model.HasTargetUser)
                return;

            _model.ClearCurrentModule();
            BuildPermissionCards();
        }

        private async Task RestoreDefaultAsync()
        {
            if (_isBusy || !_model.HasTargetUser)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.RestoreDefaultAsync();
                if (result.Success && _model.RefreshCurrentUserPermissionContext(false))
                {
                    var mainWindow = FindForm() as AMControlWinF.MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.RefreshNavigationByCurrentUser();
                    }
                }

                BuildPermissionCards();
                UpdateActionButtons();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task SaveAsync()
        {
            if (_isBusy || !_model.HasTargetUser)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.SaveAsync();
                if (result.Success && _model.RefreshCurrentUserPermissionContext(true))
                {
                    var mainWindow = FindForm() as AMControlWinF.MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.RefreshNavigationByCurrentUser();
                    }
                }

                UpdateActionButtons();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            buttonSelectUser.Enabled = !isBusy;
            UpdateActionButtons();
        }

        private void UpdateActionButtons()
        {
            var canOperate = !_isBusy && _model.HasTargetUser;

            buttonSelectAll.Enabled = canOperate;
            buttonClear.Enabled = canOperate;
            buttonRestoreDefault.Enabled = canOperate;
            buttonSave.Enabled = canOperate;
        }

        private void BuildModuleButtons()
        {
            flowModules.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowModules);

                var btnLength = 56;
                var btnMaxLength = 85;

                foreach (var module in _model.Modules)
                {
                    var tempLength = module.DisplayName != null && module.DisplayName.Length >= 4
                        ? btnMaxLength
                        : btnLength;

                    var button = new AntdUI.Button
                    {
                        Margin = new Padding(0),
                        Radius = 8,
                        Size = new Size(tempLength, 38),
                        Text = module.DisplayName,
                        Tag = module.Key,
                        WaveSize = 0,
                        Type = string.Equals(module.Key, _model.SelectedModuleKey, StringComparison.OrdinalIgnoreCase)
                            ? TTypeMini.Primary
                            : TTypeMini.Default
                    };

                    button.Click += ModuleButton_Click;
                    flowModules.Controls.Add(button);
                }
            }
            finally
            {
                flowModules.ResumeLayout();
            }
        }

        private void ModuleButton_Click(object sender, EventArgs e)
        {
            var button = sender as AntdUI.Button;
            if (button == null || button.Tag == null)
                return;

            _model.SelectModule(button.Tag.ToString());
            labelModuleTitle.Text = _model.SelectedModuleDisplayText;
            BuildModuleButtons();
            BuildPermissionCards();
        }

        private void BuildPermissionCards()
        {
            flowPermissionCards.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowPermissionCards);

                foreach (var item in _model.VisiblePermissions)
                {
                    var card = new UserPermissionCardControl();
                    card.Bind(item);
                    flowPermissionCards.Controls.Add(card);
                }
            }
            finally
            {
                flowPermissionCards.ResumeLayout();
            }
        }
    }
}