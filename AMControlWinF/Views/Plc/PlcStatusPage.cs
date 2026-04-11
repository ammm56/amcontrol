using AM.Model.Common;
using AM.PageModel.Plc;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Plc
{
    /// <summary>
    /// PLC 运行状态页面。
    /// 当前职责：
    /// 1. 负责页面生命周期、首次加载与低频定时刷新；
    /// 2. 负责将 `PlcStatusPageModel` 结果绑定到工具栏、统计区、左侧虚拟列表与右侧详情区；
    /// 3. 负责扫描启停、单轮扫描等页面交互；
    /// 4. 负责按照当前用户权限控制扫描动作按钮可见性。
    /// </summary>
    public partial class PlcStatusPage : UserControl
    {
        private readonly PlcStatusPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isBusy;

        public PlcStatusPage()
        {
            InitializeComponent();

            _model = new PlcStatusPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 1000;

            BindEvents();
            ApplyPermissionState();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        /// <summary>
        /// 绑定页面事件。
        /// </summary>
        private void BindEvents()
        {
            Load += PlcStatusPage_Load;
            VisibleChanged += PlcStatusPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            inputSearch.TextChanged += InputSearch_TextChanged;

            buttonRefresh.Click += async (s, e) => await ReloadAsync(false);
            buttonScanOnce.Click += async (s, e) => await ExecuteAsync(() => _model.ScanOnce());
            buttonStartScan.Click += async (s, e) => await ExecuteAsync(() => _model.StartScan());
            buttonStopScan.Click += async (s, e) => await ExecuteAsync(() => _model.StopScan());

            plcStatusVirtualListControl.ItemSelected += PlcStatusVirtualListControl_ItemSelected;
        }

        /// <summary>
        /// 页面首次加载。
        /// </summary>
        private async void PlcStatusPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync(true);
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 页面可见性变化时启停低频刷新。
        /// </summary>
        private void PlcStatusPage_VisibleChanged(object sender, EventArgs e)
        {
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 控制页面低频刷新定时器。
        /// 页面缓存复用时，仅在页面可见时刷新。
        /// </summary>
        private void UpdateRefreshTimerState()
        {
            if (IsDisposed)
            {
                return;
            }

            if (_isFirstLoad && Visible)
            {
                _refreshTimer.Start();
            }
            else
            {
                _refreshTimer.Stop();
            }
        }

        /// <summary>
        /// 定时刷新运行态。
        /// </summary>
        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!Visible || _isBusy)
            {
                return;
            }

            await ReloadAsync(false);
        }

        /// <summary>
        /// 搜索条件变化后重建左侧站列表。
        /// </summary>
        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        /// <summary>
        /// 左侧虚拟列表项选中后，驱动右侧详情刷新。
        /// </summary>
        private void PlcStatusVirtualListControl_ItemSelected(object sender, PlcStatusVirtualListControl.PlcStationSelectedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            _model.SelectStationByName(e.PlcName);
            RefreshStationSelection();
        }

        /// <summary>
        /// 首次加载或普通刷新入口。
        /// </summary>
        private async Task ReloadAsync(bool useLoadMethod)
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                Result result = useLoadMethod
                    ? await _model.LoadAsync()
                    : await _model.RefreshAsync();

                RefreshView();

                if (!result.Success)
                {
                    return;
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        /// <summary>
        /// 执行扫描控制动作后刷新页面。
        /// </summary>
        private async Task ExecuteAsync(Func<Result> action)
        {
            if (_isBusy || action == null || !_model.CanControlScanOperations)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await Task.Run(action);
                await _model.RefreshAsync();
                RefreshView();

                if (!result.Success)
                {
                    return;
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        /// <summary>
        /// 刷新页面全部显示区域。
        /// </summary>
        private void RefreshView()
        {
            labelRuntimeSummary.Text = _model.RuntimeSummaryText;

            labelStationTotalCount.Text = _model.TotalStationCount.ToString();
            labelOnlineStationCount.Text = _model.OnlineStationCount.ToString();
            labelOfflineStationCount.Text = _model.OfflineStationCount.ToString();
            labelErrorStationCount.Text = _model.ErrorStationCount.ToString();

            RefreshStationSelection();
            ApplyPermissionState();
        }

        /// <summary>
        /// 刷新当前选中站显示与左右区域。
        /// </summary>
        private void RefreshStationSelection()
        {
            plcStatusVirtualListControl.BindItems(_model.Stations, _model.SelectedStation);
            plcStatusDetailControl.Bind(_model.SelectedStation);
        }

        /// <summary>
        /// 设置忙碌状态。
        /// </summary>
        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;

            inputSearch.Enabled = !isBusy;
            buttonRefresh.Enabled = !isBusy;

            bool canControl = _model.CanControlScanOperations;
            buttonScanOnce.Enabled = !isBusy && canControl;
            buttonStartScan.Enabled = !isBusy && canControl;
            buttonStopScan.Enabled = !isBusy && canControl;
        }

        /// <summary>
        /// 按当前用户权限控制扫描动作按钮显示。
        /// 操作员仅保留刷新；工程师/管理员可执行扫描控制。
        /// </summary>
        private void ApplyPermissionState()
        {
            bool canControl = _model.CanControlScanOperations;

            buttonScanOnce.Visible = canControl;
            buttonStartScan.Visible = canControl;
            buttonStopScan.Visible = canControl;
        }
    }
}