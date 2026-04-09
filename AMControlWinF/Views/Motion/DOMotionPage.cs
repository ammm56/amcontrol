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
    ///
    /// 【当前职责】
    /// 1. 负责页面生命周期、缓存页复用下的首次加载控制与低频定时刷新；
    /// 2. 负责 WinForms 直接事件接线；
    /// 3. 调用 `DOMotionPageModel` 完成数据加载、筛选、分页和选中维护；
    /// 4. 将页面模型结果绑定到顶部摘要区、左侧虚拟列表和右侧详情区。
    ///
    /// 【层级关系】
    /// - 上游：MainWindow 页面缓存与导航切换；
    /// - 当前层：WinForms 页面协调层；
    /// - 下游：DOMotionPageModel、DOMotionVirtualListControl、DOMotionDetailControl。
    ///
    /// 【调用关系】
    /// 1. 页面首次进入时调用 `LoadAsync` 建立初始显示；
    /// 2. 定时器与页面交互事件都回到页面模型重建显示结果；
    /// 3. 页面只负责读取模型属性并调用子控件 `Bind`；
    /// 4. 控制卡选择、搜索、分页、选中切换都保持 WinForms 直接事件驱动。
    ///
    /// 【架构设计】
    /// 本页保持 WinForms 下“事件 + 页面模型 + 绑定”的轻量实现：
    /// - 页面不直接计算筛选与分页；
    /// - 页面模型不直接访问控件；
    /// - 控件只负责显示与回传用户操作。
    /// </summary>
    public partial class DOMotionPage : UserControl
    {
        #region 构造与初始化

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

        #endregion

        #region 页面生命周期

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

        /// <summary>
        /// 页面可见时启动刷新，不可见时停止，避免缓存页在后台持续刷新。
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

        #endregion

        #region 视图绑定

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

        #endregion

        #region 页面事件处理

        private Task SelectCardAsync()
        {
            using (var dialog = new MotionCardSelectDialog(_model.SelectedCardId))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return Task.CompletedTask;

                _model.SetSelectedCardId(dialog.SelectedCardId);
                RefreshView();
            }

            return Task.CompletedTask;
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

        #endregion
    }
}