using AM.Model.Common;
using AM.PageModel.Motion;
using AntdUI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 执行器控制页面。
    ///
    /// 【当前职责】
    /// 1. 负责页面生命周期与页面缓存复用下的初始化控制；
    /// 2. 负责 WinForms 直接事件接线；
    /// 3. 调用 `MotionActuatorPageModel` 完成数据加载、筛选、选择与动作执行；
    /// 4. 将页面模型的显示结果绑定到列表、动作区和详情区控件；
    /// 5. 控制定时刷新与动作执行期间的页面刷新节奏。
    ///
    /// 【层级关系】
    /// - 上游：MainWindow 页面缓存与导航切换；
    /// - 当前层：WinForms 页面协调层；
    /// - 下游：MotionActuatorVirtualListControl、MotionActuatorActionPanelControl、
    ///   MotionActuatorDetailControl。
    ///
    /// 【调用关系】
    /// 1. 页面首次加载时调用 `LoadAsync` 建立初始显示；
    /// 2. 页面内的筛选、搜索、选中、按钮点击都直接由控件事件驱动；
    /// 3. 页面只把当前 UI 选项同步给页面模型，然后直接 Bind 模型结果；
    /// 4. 动作成功后再补一次运行态刷新，保证显示与设备状态一致。
    ///
    /// 【设计说明】
    /// 本页仍保持 WinForms 的直接事件驱动写法：
    /// - 页面不引入 WPF 式命令系统；
    /// - 页面不自行拼装动作面板状态；
    /// - 页面只负责事件、调用与绑定；
    /// - 页面模型负责状态、规则与动作执行。
    /// </summary>
    public partial class MotionActuatorPage : UserControl
    {
        private readonly MotionActuatorPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isExecutingAction;

        #region 构造与初始化

        public MotionActuatorPage()
        {
            InitializeComponent();

            _model = new MotionActuatorPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            BindEvents();

            Disposed += MotionActuatorPage_Disposed;
        }

        private void BindEvents()
        {
            Load += MotionActuatorPage_Load;
            VisibleChanged += MotionActuatorPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonFilterAll.Click += (s, e) => ChangeTypeFilter("All");
            buttonFilterCylinder.Click += (s, e) => ChangeTypeFilter("Cylinder");
            buttonFilterVacuum.Click += (s, e) => ChangeTypeFilter("Vacuum");
            buttonFilterGripper.Click += (s, e) => ChangeTypeFilter("Gripper");
            buttonFilterStackLight.Click += (s, e) => ChangeTypeFilter("StackLight");

            inputSearch.TextChanged += InputSearch_TextChanged;
            actuatorVirtualListControl.ItemSelected += ActuatorVirtualListControl_ItemSelected;

            actuatorActionPanelControl.OptionsChanged += ActuatorActionPanelControl_OptionsChanged;
            actuatorActionPanelControl.PrimaryActionRequested += ActuatorActionPanelControl_PrimaryActionRequested;
            actuatorActionPanelControl.SecondaryActionRequested += ActuatorActionPanelControl_SecondaryActionRequested;
            actuatorActionPanelControl.StackLightStateRequested += ActuatorActionPanelControl_StackLightStateRequested;
        }

        private void MotionActuatorPage_Disposed(object sender, EventArgs e)
        {
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
        }

        #endregion

        #region 页面生命周期

        private async void MotionActuatorPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
                return;

            _isFirstLoad = true;
            await InitializePageAsync();
        }

        private void MotionActuatorPage_VisibleChanged(object sender, EventArgs e)
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
            if (!Visible || _isRefreshing || _isExecutingAction)
                return;

            await ReloadRuntimeAsync(false);
        }

        #endregion

        #region 视图绑定

        private void RefreshView()
        {
            BindSummary();
            UpdateActionPanelOptions();
            BindList();
            BindSelection();
            UpdateFilterButtonStyles();
        }

        private void BindSummary()
        {
            labelCylinderCount.Text = _model.CylinderCount.ToString();
            labelVacuumCount.Text = _model.VacuumCount.ToString();
            labelGripperCount.Text = _model.GripperCount.ToString();
            labelStackLightCount.Text = _model.StackLightCount.ToString();
        }

        private void BindList()
        {
            actuatorVirtualListControl.BindItems(
                _model.PageItems,
                _model.SelectedItemKey);
        }

        private void BindSelection()
        {
            actuatorActionPanelControl.Bind(_model.SelectedActionPanel);
            actuatorDetailControl.Bind(_model.SelectedDetail);
        }

        private void UpdateActionPanelOptions()
        {
            _model.UpdateActionPanelOptions(
                actuatorActionPanelControl.WaitFeedback,
                actuatorActionPanelControl.WaitWorkpiece,
                actuatorActionPanelControl.StackLightWithBuzzer);
        }

        private void UpdateFilterButtonStyles()
        {
            ApplyFilterButtonStyle(buttonFilterAll, string.Equals(_model.TypeFilter, "All", StringComparison.OrdinalIgnoreCase));
            ApplyFilterButtonStyle(buttonFilterCylinder, string.Equals(_model.TypeFilter, "Cylinder", StringComparison.OrdinalIgnoreCase));
            ApplyFilterButtonStyle(buttonFilterVacuum, string.Equals(_model.TypeFilter, "Vacuum", StringComparison.OrdinalIgnoreCase));
            ApplyFilterButtonStyle(buttonFilterGripper, string.Equals(_model.TypeFilter, "Gripper", StringComparison.OrdinalIgnoreCase));
            ApplyFilterButtonStyle(buttonFilterStackLight, string.Equals(_model.TypeFilter, "StackLight", StringComparison.OrdinalIgnoreCase));
        }

        private static void ApplyFilterButtonStyle(AntdUI.Button button, bool selected)
        {
            if (button == null)
                return;

            button.Type = selected ? TTypeMini.Primary : TTypeMini.Default;
        }

        #endregion

        #region 页面事件处理

        private void ChangeTypeFilter(string typeFilter)
        {
            _model.SetTypeFilter(typeFilter);
            RefreshView();
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        private void ActuatorVirtualListControl_ItemSelected(
            object sender,
            MotionActuatorVirtualListControl.MotionActuatorItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectItem(e.ItemKey);
            UpdateActionPanelOptions();

            BindList();
            BindSelection();
        }

        private void ActuatorActionPanelControl_OptionsChanged(object sender, EventArgs e)
        {
            UpdateActionPanelOptions();
            BindSelection();
        }

        private async void ActuatorActionPanelControl_PrimaryActionRequested(object sender, EventArgs e)
        {
            UpdateActionPanelOptions();
            await ExecuteActionAsync(_model.ExecutePrimaryActionAsync);
        }

        private async void ActuatorActionPanelControl_SecondaryActionRequested(object sender, EventArgs e)
        {
            UpdateActionPanelOptions();
            await ExecuteActionAsync(_model.ExecuteSecondaryActionAsync);
        }

        private async void ActuatorActionPanelControl_StackLightStateRequested(
            object sender,
            MotionActuatorActionPanelControl.StackLightStateRequestedEventArgs e)
        {
            if (e == null)
                return;

            UpdateActionPanelOptions();
            await ExecuteActionAsync(() => _model.SetStackLightStateAsync(e.State));
        }

        #endregion

        #region 页面动作执行

        /// <summary>
        /// 页面统一动作执行入口。
        /// 负责控制重复点击、动作后的局部刷新以及成功后的运行态重载。
        /// </summary>
        private async Task ExecuteActionAsync(Func<Task<Result>> executeFunc)
        {
            if (_isExecutingAction || executeFunc == null)
                return;

            _isExecutingAction = true;
            try
            {
                var result = await executeFunc();

                UpdateActionPanelOptions();
                BindSelection();

                if (result != null && result.Success)
                    await ReloadRuntimeAsync(false);
            }
            finally
            {
                _isExecutingAction = false;
            }
        }

        #endregion
    }
}