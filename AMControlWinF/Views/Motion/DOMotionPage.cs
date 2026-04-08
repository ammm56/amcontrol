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
    /// DO 监视页面。
    /// 被 MainWindow 页面缓存复用：
    /// - 不在页面切换时释放；
    /// - 首次加载使用布尔标记控制；
    /// - 通过 500ms 定时刷新运行态。
    /// </summary>
    public partial class DOMotionPage : UserControl
    {
        private readonly DOMotionPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isUpdatingPagination;

        public DOMotionPage()
        {
            InitializeComponent();

            _model = new DOMotionPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 200;

            InitializePagination();
            BindEvents();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        private void BindEvents()
        {
            Load += DOMotionPage_Load;
            VisibleChanged += DOMotionPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectCard.Click += async (s, e) => await SelectCardAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;
            paginationInputs.ValueChanged += PaginationInputs_ValueChanged;
            doMotionVirtualListControl.ItemSelected += DoMotionVirtualListControl_ItemSelected;
        }

        private void InitializePagination()
        {
            paginationInputs.Current = 1;
            paginationInputs.Total = 0;
            paginationInputs.PageSize = 48;
            paginationInputs.PageSizeOptions = new int[] { 6, 12, 24, 48, 96, 192 };
            paginationInputs.ShowSizeChanger = true;
            paginationInputs.SizeChangerWidth = 72;
            paginationInputs.RightToLeft = RightToLeft.Yes;
            paginationInputs.Gap = 8;
            paginationInputs.Radius = 8;
        }

        private async void DOMotionPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private void DOMotionPage_VisibleChanged(object sender, EventArgs e)
        {
            UpdateRefreshTimerState();
        }

        private async Task InitializePageAsync()
        {
            await ReloadRuntimeAsync(true);
            UpdateRefreshTimerState();
        }

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

            doMotionVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            doMotionDetailControl.Bind(_model.SelectedItem);

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

        private void DoMotionVirtualListControl_ItemSelected(object sender, DOMotionVirtualListControl.DOMotionItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectItem(e.LogicalBit);
            doMotionVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            doMotionDetailControl.Bind(_model.SelectedItem);
        }
    }
}