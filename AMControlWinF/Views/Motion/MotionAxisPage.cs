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
    /// 单轴控制页面。
    ///
    /// 第一阶段先完成：
    /// 1. 页面级数据刷新逻辑；
    /// 2. 轴选择、动作搜索、分页；
    /// 3. 左侧虚拟动作卡片列表与右侧详情占位绑定；
    /// 4. 500ms 定时低频刷新选中轴运行态。
    ///
    /// 当前阶段说明：
    /// - 左侧卡片用于“选择动作”；
    /// - 右侧详情区显示“当前轴 + 当前动作”的说明信息；
    /// - 真正动作执行入口在下一阶段补充。
    /// </summary>
    public partial class MotionAxisPage : UserControl
    {
        private readonly MotionAxisPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isUpdatingPagination;

        public MotionAxisPage()
        {
            InitializeComponent();

            _model = new MotionAxisPageModel();

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
            Load += MotionAxisPage_Load;
            VisibleChanged += MotionAxisPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectAxis.Click += async (s, e) => await SelectAxisAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;
            paginationActions.ValueChanged += PaginationActions_ValueChanged;
            motionAxisVirtualListControl.ItemSelected += MotionAxisVirtualListControl_ItemSelected;
        }

        private void InitializePagination()
        {
            paginationActions.PageSizeOptions = new int[] { 6, 12, 24, 36, 48, 96 };
        }

        private async void MotionAxisPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private void MotionAxisPage_VisibleChanged(object sender, EventArgs e)
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
        /// 首次加载和定时刷新共用同一套入口。
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
            labelSelectedAxis.Text = "当前：" + _model.SelectedAxisText;

            motionAxisVirtualListControl.BindItems(_model.PageItems, _model.SelectedAction);
            motionAxisDetailControl.Bind(_model.SelectedAxis, _model.SelectedAction);

            SyncPagination();
        }

        private void SyncPagination()
        {
            _isUpdatingPagination = true;
            try
            {
                paginationActions.Total = _model.FilteredCount;
                paginationActions.PageSize = _model.PageSize;
                paginationActions.Current = _model.PageIndex;
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
        /// 选择轴。
        ///
        /// 当前第一阶段先复用现有 `MotionAxisSelectDialog`。
        /// 下一阶段如需完全对齐你的要求，可替换成“复用轴拓扑配置页 UserControl”的选择窗口。
        /// </summary>
        private async Task SelectAxisAsync()
        {
            using (var dialog = new MotionAxisSelectDialog(_model.SelectedLogicalAxis))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                _model.SetSelectedLogicalAxis(dialog.SelectedLogicalAxis);
                RefreshView();
            }

            await Task.CompletedTask;
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        private void PaginationActions_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isUpdatingPagination || e == null)
                return;

            _model.ChangePage(e.Current, e.PageSize);
            RefreshView();
        }

        private void MotionAxisVirtualListControl_ItemSelected(object sender, MotionAxisVirtualListControl.MotionAxisActionItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectAction(e.ActionKey);
            motionAxisVirtualListControl.BindItems(_model.PageItems, _model.SelectedAction);
            motionAxisDetailControl.Bind(_model.SelectedAxis, _model.SelectedAction);
        }
    }
}