using AM.Model.Common;
using AM.PageModel.Motion;
using AMControlWinF.Views.MotionConfig;
using AntdUI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// DI 监视页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在页面切换时释放；
    /// - 首次加载使用布尔标记控制；
    /// - 通过 500ms 定时刷新运行态，避免整页高频事件驱动。
    /// </summary>
    public partial class DIMotionPage : UserControl
    {
        private readonly DIMotionPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isUpdatingPagination;

        public DIMotionPage()
        {
            InitializeComponent();

            _model = new DIMotionPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 200;

            InitializePagination();
            BindEvents();

            // 页面被释放时，停止定时刷新。
            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        private void BindEvents()
        {
            Load += DIMotionPage_Load;
            VisibleChanged += DIMotionPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectCard.Click += async (s, e) => await SelectCardAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;
            paginationInputs.ValueChanged += PaginationInputs_ValueChanged;
            diMotionVirtualListControl.ItemSelected += DiMotionVirtualListControl_ItemSelected;
        }

        private void InitializePagination()
        {
            paginationInputs.PageSizeOptions = new int[] { 6, 12, 24, 48, 96, 192 };
        }

        private async void DIMotionPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private void DIMotionPage_VisibleChanged(object sender, EventArgs e)
        {
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 首次进入页面时加载数据。
        /// </summary>
        private async Task InitializePageAsync()
        {
            await ReloadRuntimeAsync(true);
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 首次加载和定时刷新共用同一套刷新入口。
        /// </summary>
        private async Task ReloadRuntimeAsync(bool useLoadMethod)
        {
            if (_isRefreshing)
                return;

            _isRefreshing = true;
            try
            {
                Result result = useLoadMethod
                    ? await _model.LoadAsync()
                    : await _model.RefreshAsync();

                RefreshView();

                if (!result.Success)
                    return;
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void RefreshView()
        {
            labelSelectedCard.Text = "当前：" + _model.SelectedCardText;
            labelTotalCount.Text = _model.TotalCount.ToString();
            labelActiveCount.Text = _model.ActiveCount.ToString();
            labelScanValue.Text = _model.ScanStateText;

            diMotionVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            diMotionDetailControl.Bind(_model.SelectedItem);

            SyncPagination();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationInputs.Total = _model.FilteredCount;
                paginationInputs.PageSize = _model.PageSize;
                paginationInputs.Current = _model.PageIndex;
                labelPageSummary.Text = _model.PageSummaryText;
            }
            finally
            {
                _isUpdatingPagination = false;
            }
        }

        /// <summary>
        /// 页面可见时启动刷新，不可见时停止。
        /// </summary>
        private void UpdateRefreshTimerState()
        {
            if (IsDisposed)
                return;

            if (_isFirstLoad && Visible)
                _refreshTimer.Start();
            else
                _refreshTimer.Stop();
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!Visible || _isRefreshing)
                return;

            await ReloadRuntimeAsync(false);
        }

        private async Task SelectCardAsync()
        {
            using (var dialog = new MotionCardSelectDialog(_model.SelectedCardId))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                _model.SetSelectedCardId(dialog.SelectedCardId);
                RefreshView();
            }

            await Task.CompletedTask;
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        private void PaginationInputs_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || e == null)
                return;

            _model.ChangePage(e.Current, e.PageSize);
            RefreshView();
        }

        private void DiMotionVirtualListControl_ItemSelected(object sender, DIMotionVirtualListControl.DIMotionItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectItem(e.LogicalBit);
            diMotionVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            diMotionDetailControl.Bind(_model.SelectedItem);
        }
    }
}