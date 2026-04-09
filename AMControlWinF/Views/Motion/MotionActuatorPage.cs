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
    /// 【第二阶段收口定位】
    /// 页面层只保留 WinForms 最直接的职责：
    /// 1. 页面生命周期；
    /// 2. 事件接线；
    /// 3. 调用页面模型；
    /// 4. 将模型结果 Bind 到控件。
    ///
    /// 与第一阶段相比，本阶段页面不再直接参与动作面板状态组装，
    /// 而是把动作选项同步给 PageModel，再直接绑定：
    /// - _model.PageItems
    /// - _model.SelectedItemKey
    /// - _model.SelectedDetail
    /// - _model.SelectedActionPanel
    ///
    /// 这样更符合 WinForms 简单、直观、事件驱动的实现风格。
    /// </summary>
    public partial class MotionActuatorPage : UserControl
    {
        private readonly MotionActuatorPageModel _model;
        private readonly Timer _refreshTimer;

        /// <summary>
        /// 是否首次加载。
        /// 页面缓存复用场景下，避免重复初始化。
        /// </summary>
        private bool _isFirstLoad;

        /// <summary>
        /// 当前是否正在刷新运行态。
        /// 防止定时器重入。
        /// </summary>
        private bool _isRefreshing;

        /// <summary>
        /// 当前是否正在执行动作。
        /// 动作执行期间禁止定时刷新和重复点击。
        /// </summary>
        private bool _isExecutingAction;

        public MotionActuatorPage()
        {
            InitializeComponent();

            _model = new MotionActuatorPageModel();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            BindEvents();

            Disposed += delegate
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        /// <summary>
        /// 绑定页面级事件。
        /// </summary>
        private void BindEvents()
        {
            Load += MotionActuatorPage_Load;
            VisibleChanged += MotionActuatorPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonFilterAll.Click += delegate { ChangeTypeFilter("All"); };
            buttonFilterCylinder.Click += delegate { ChangeTypeFilter("Cylinder"); };
            buttonFilterVacuum.Click += delegate { ChangeTypeFilter("Vacuum"); };
            buttonFilterGripper.Click += delegate { ChangeTypeFilter("Gripper"); };
            buttonFilterStackLight.Click += delegate { ChangeTypeFilter("StackLight"); };

            inputSearch.TextChanged += InputSearch_TextChanged;
            actuatorVirtualListControl.ItemSelected += ActuatorVirtualListControl_ItemSelected;

            actuatorActionPanelControl.OptionsChanged += ActuatorActionPanelControl_OptionsChanged;
            actuatorActionPanelControl.PrimaryActionRequested += ActuatorActionPanelControl_PrimaryActionRequested;
            actuatorActionPanelControl.SecondaryActionRequested += ActuatorActionPanelControl_SecondaryActionRequested;
            actuatorActionPanelControl.StackLightStateRequested += ActuatorActionPanelControl_StackLightStateRequested;
        }

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

        /// <summary>
        /// 首次进入页面时加载数据。
        /// </summary>
        private async Task InitializePageAsync()
        {
            await ReloadRuntimeAsync(true);
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 首次加载和定时刷新共用同一刷新入口。
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

        /// <summary>
        /// 将页面模型状态同步到界面。
        /// 第二阶段开始页面只负责 Bind，不再直接参与动作面板状态组装。
        /// </summary>
        private void RefreshView()
        {
            BindSummary();

            UpdateActionPanelOptions();

            BindList();
            BindSelection();

            UpdateFilterButtonStyles();
        }

        /// <summary>
        /// 绑定顶部统计信息。
        /// </summary>
        private void BindSummary()
        {
            labelCylinderCount.Text = _model.CylinderCount.ToString();
            labelVacuumCount.Text = _model.VacuumCount.ToString();
            labelGripperCount.Text = _model.GripperCount.ToString();
            labelStackLightCount.Text = _model.StackLightCount.ToString();
        }

        /// <summary>
        /// 绑定左侧列表区。
        /// </summary>
        private void BindList()
        {
            actuatorVirtualListControl.BindItems(
                _model.PageItems,
                _model.SelectedItemKey);
        }

        /// <summary>
        /// 绑定右侧选中对象相关区域。
        /// 包括：
        /// - 动作面板
        /// - 详情区
        /// </summary>
        private void BindSelection()
        {
            actuatorActionPanelControl.Bind(_model.SelectedActionPanel);
            actuatorDetailControl.Bind(_model.SelectedDetail);
        }

        /// <summary>
        /// 将动作区当前选项同步到 PageModel。
        /// 第二阶段开始，动作面板状态由模型内部统一维护。
        /// </summary>
        private void UpdateActionPanelOptions()
        {
            _model.UpdateActionPanelOptions(
                actuatorActionPanelControl.WaitFeedback,
                actuatorActionPanelControl.WaitWorkpiece,
                actuatorActionPanelControl.StackLightWithBuzzer);
        }

        /// <summary>
        /// 切换类型筛选。
        /// 当前筛选属于页面内状态变化，不需要重新构造页面。
        /// </summary>
        private void ChangeTypeFilter(string typeFilter)
        {
            _model.SetTypeFilter(typeFilter);
            RefreshView();
        }

        /// <summary>
        /// 根据当前筛选状态刷新顶部按钮样式。
        /// </summary>
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

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        /// <summary>
        /// 左侧卡片选中后，仅更新当前选中对象相关区域。
        /// 不做整页重载。
        /// </summary>
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

        /// <summary>
        /// 右侧动作区选项变化后，只同步模型并刷新右侧选中区。
        /// 不做整页刷新。
        /// </summary>
        private void ActuatorActionPanelControl_OptionsChanged(object sender, EventArgs e)
        {
            UpdateActionPanelOptions();
            BindSelection();
        }

        private async void ActuatorActionPanelControl_PrimaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(_model.ExecutePrimaryActionAsync);
        }

        private async void ActuatorActionPanelControl_SecondaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(_model.ExecuteSecondaryActionAsync);
        }

        private async void ActuatorActionPanelControl_StackLightStateRequested(
            object sender,
            MotionActuatorActionPanelControl.StackLightStateRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(delegate
            {
                return _model.SetStackLightStateAsync(e.StateKey);
            });
        }

        /// <summary>
        /// 统一动作执行入口。
        ///
        /// 设计说明：
        /// 1. 页面上的所有执行器动作都从这里进入；
        /// 2. 动作执行期间禁止重复触发；
        /// 3. 动作执行后先刷新当前右侧显示；
        /// 4. 成功后再刷新一次运行态，保持页面显示与实际设备状态一致。
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

        /// <summary>
        /// 页面可见时启动定时刷新，不可见时停止。
        /// 缓存页面复用场景下保持逻辑简单直接。
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
            if (!Visible || _isRefreshing || _isExecutingAction)
                return;

            await ReloadRuntimeAsync(false);
        }
    }
}