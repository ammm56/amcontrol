using AM.DBService.Services.Motion.App;
using AM.PageModel.MotionConfig;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// IO 映射配置页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在离开页面时释放 ViewModel；
    /// - 首次加载使用布尔标记控制，避免重复初始化。
    /// </summary>
    public sealed partial class MotionIoMapManagementPage : UserControl
    {
        private readonly MotionIoMapManagementPageModel _model;
        private readonly MachineConfigReloadService _machineConfigReloadService;

        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isUpdatingPagination;

        public MotionIoMapManagementPage()
        {
            InitializeComponent();

            _model = new MotionIoMapManagementPageModel();
            _machineConfigReloadService = new MachineConfigReloadService();

            BindEvents();
            UpdateActionButtons();
            UpdateIoTypeButtons();
        }

        private void BindEvents()
        {
            Load += MotionIoMapManagementPage_Load;

            buttonRefresh.Click += async (s, e) => await ReloadAsync();
            buttonSelectCard.Click += ButtonSelectCard_Click;
            buttonFilterAll.Click += ButtonFilterAll_Click;
            buttonFilterDI.Click += ButtonFilterDI_Click;
            buttonFilterDO.Click += ButtonFilterDO_Click;
            buttonAddIoMap.Click += async (s, e) => await AddIoMapAsync();
            paginationCards.ValueChanged += PaginationCards_ValueChanged;
        }

        private async void MotionIoMapManagementPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
                return;

            SetBusyState(true);
            try
            {
                await _model.LoadAsync();
                labelSelectedCard.Text = _model.SelectedCardText;
                UpdateIoTypeButtons();
                RefreshPaginationUi();
                BuildCards();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        /// <summary>
        /// IO 映射变更后立即热重载设备配置。
        /// 这样 DI/DO 监视页和运行时路由才能立刻使用最新映射。
        /// </summary>
        private async Task<bool> ReloadMachineConfigAsync()
        {
            var result = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
            return result.Success;
        }

        private void PaginationCards_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || _isBusy || e == null)
                return;

            _model.ChangePage(e.Current, e.PageSize);
            RefreshPaginationUi();
            BuildCards();
        }

        private void RefreshPaginationUi()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationCards.Total = _model.TotalCount;
                paginationCards.PageSize = _model.PageSize;
                paginationCards.Current = _model.CurrentPage;
                labelPageSummary.Text = _model.PageSummaryText;
            }
            finally
            {
                _isUpdatingPagination = false;
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            UpdateActionButtons();
        }

        private void UpdateActionButtons()
        {
            buttonRefresh.Enabled = !_isBusy;
            buttonAddIoMap.Enabled = !_isBusy;
            buttonSelectCard.Enabled = !_isBusy;
            buttonFilterAll.Enabled = !_isBusy;
            buttonFilterDI.Enabled = !_isBusy;
            buttonFilterDO.Enabled = !_isBusy;
            paginationCards.Enabled = !_isBusy;
        }

        private void UpdateIoTypeButtons()
        {
            buttonFilterAll.Type = string.Equals(_model.SelectedIoType, "All", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterDI.Type = string.Equals(_model.SelectedIoType, "DI", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;

            buttonFilterDO.Type = string.Equals(_model.SelectedIoType, "DO", StringComparison.OrdinalIgnoreCase)
                ? TTypeMini.Primary
                : TTypeMini.Default;
        }

        private void BuildCards()
        {
            flowCards.SuspendLayout();
            try
            {
                ControlDisposeHelper.ClearControlsSafely(flowCards);

                foreach (var item in _model.Items)
                {
                    var card = new MotionIoMapCardControl();
                    card.Bind(item);
                    card.Margin = new Padding(0);
                    card.EditRequested += async (s, e) => await EditIoMapAsync(card.IoMapItem);
                    card.DeleteRequested += async (s, e) => await DeleteIoMapAsync(card.IoMapItem);
                    card.DetailRequested += (s, e) => ShowDetail(s as Control ?? card, card.IoMapItem);
                    flowCards.Controls.Add(card);
                }
            }
            finally
            {
                flowCards.ResumeLayout();
            }
        }

        private async void ButtonSelectCard_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            using (var dialog = new MotionCardSelectDialog(_model.SelectedCardId))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                _model.SelectedCardId = dialog.SelectedCardId;
                await ReloadAsync();
            }
        }

        private async void ButtonFilterAll_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            _model.SelectedIoType = "All";
            await ReloadAsync();
        }

        private async void ButtonFilterDI_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            _model.SelectedIoType = "DI";
            await ReloadAsync();
        }

        private async void ButtonFilterDO_Click(object sender, EventArgs e)
        {
            if (_isBusy)
                return;

            _model.SelectedIoType = "DO";
            await ReloadAsync();
        }

        private async Task AddIoMapAsync()
        {
            if (_isBusy)
                return;

            if (!_model.Cards.Any())
            {
                PageDialogHelper.ShowWarn(this, "新增 IO 映射", "当前没有可用控制卡，请先新增控制卡。");
                return;
            }

            var entity = _model.CreateDefaultEntity();
            using (var dialog = new MotionIoMapEditDialog(entity, true, _model.Cards.ToList()))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    if (!await ReloadMachineConfigAsync())
                        return;

                    await ReloadAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task EditIoMapAsync(MotionIoMapManagementPageModel.MotionIoMapViewItem item)
        {
            if (_isBusy || item == null)
                return;

            using (var dialog = new MotionIoMapEditDialog(item.ToEntity(), false, _model.Cards.ToList()))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                SetBusyState(true);
                try
                {
                    var result = await _model.SaveAsync(dialog.ResultEntity);
                    if (!result.Success)
                        return;

                    if (!await ReloadMachineConfigAsync())
                        return;

                    await ReloadAsync();
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task DeleteIoMapAsync(MotionIoMapManagementPageModel.MotionIoMapViewItem item)
        {
            if (_isBusy || item == null)
                return;

            var ok = PageDialogHelper.Confirm(
                this,
                "删除 IO 映射",
                "确定删除 " + item.IoTypeText + " 逻辑位 " + item.LogicalBit + "（" + item.Name + "）吗？");

            if (!ok)
                return;

            SetBusyState(true);
            try
            {
                var result = await _model.DeleteAsync(item.LogicalBit, item.IoType);
                if (!result.Success)
                    return;

                if (!await ReloadMachineConfigAsync())
                    return;

                await ReloadAsync();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void ShowDetail(Control anchorControl, MotionIoMapManagementPageModel.MotionIoMapViewItem item)
        {
            if (anchorControl == null || item == null)
                return;

            var detail = new MotionIoMapDetailControl();
            detail.Bind(item);

            PageDialogHelper.ShowDetailPopover(this, anchorControl, detail, new Size(500, 420));
        }
    }
}