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
    /// 多轴总览页面。
    /// 第一阶段先完成：
    /// 1. 页面级数据刷新逻辑；
    /// 2. 控制卡筛选、搜索、分页；
    /// 3. 左侧虚拟列表与右侧详情占位控件绑定；
    /// 4. 500ms 定时低频刷新运行态。
    ///
    /// 下一步再补 Designer 和更完整的详情布局。
    /// </summary>
    public partial class MotionMonitorPage : UserControl
    {
        private readonly MotionMonitorPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isUpdatingPagination;

        public MotionMonitorPage()
        {
            InitializeComponent();

            _model = new MotionMonitorPageModel();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            InitializePagination();
            BindEvents();

            // 页面释放时停止后台 UI 刷新定时器。
            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        private void BindEvents()
        {
            Load += MotionMonitorPage_Load;
            VisibleChanged += MotionMonitorPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectCard.Click += async (s, e) => await SelectCardAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;
            paginationAxes.ValueChanged += PaginationAxes_ValueChanged;
            motionMonitorVirtualListControl.ItemSelected += MotionMonitorVirtualListControl_ItemSelected;
        }

        private void InitializePagination()
        {
            paginationAxes.PageSizeOptions = new int[] { 8, 12, 24, 36, 48, 96 };
        }

        private async void MotionMonitorPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private void MotionMonitorPage_VisibleChanged(object sender, EventArgs e)
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
            labelAlarmCount.Text = _model.AlarmCount.ToString();
            labelMovingCount.Text = _model.MovingCount.ToString();
            labelReadyCount.Text = _model.ReadyCount.ToString();
            labelScanValue.Text = _model.ScanStateText;

            motionMonitorVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            motionMonitorDetailControl.Bind(_model.SelectedItem);

            SyncPagination();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationAxes.Total = _model.FilteredCount;
                paginationAxes.PageSize = _model.PageSize;
                paginationAxes.Current = _model.PageIndex;
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

        /// <summary>
        /// 选择控制卡。
        /// 第一阶段先复用现有控制卡选择窗口。
        /// 下一阶段如需复用控制卡配置页 UserControl，可在此替换弹窗实现。
        /// </summary>
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

        private void PaginationAxes_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || e == null)
                return;

            _model.ChangePage(e.Current, e.PageSize);
            RefreshView();
        }

        private void MotionMonitorVirtualListControl_ItemSelected(object sender, MotionMonitorVirtualListControl.MotionAxisItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectItem(e.LogicalAxis);
            motionMonitorVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            motionMonitorDetailControl.Bind(_model.SelectedItem);
        }
    }
}