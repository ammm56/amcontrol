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
    /// 【层级定位】
    /// - 所在层：WinForms 页面协调层；
    /// - 上游依赖：MotionActuatorPageModel；
    /// - 下游控件：
    ///   1. MotionActuatorVirtualListControl
    ///   2. MotionActuatorActionPanelControl
    ///   3. MotionActuatorDetailControl
    ///
    /// 【职责】
    /// 1. 管理页面生命周期；
    /// 2. 处理页面级事件驱动逻辑；
    /// 3. 调用 PageModel 加载/刷新/动作执行；
    /// 4. 将页面模型状态绑定到各子控件；
    /// 5. 保持 WinForms 下简单直接的页面协调方式。
    ///
    /// 【本轮适配说明】
    /// 第一轮重构后，页面不再直接依赖旧的 MotionActuatorViewItem：
    /// - 左侧列表改为绑定 MotionActuatorListItem 集合；
    /// - 当前选中对象改为 SelectedSnapshot；
    /// - 右侧详情改为绑定 SelectedDetail；
    /// - 动作面板继续绑定 BuildActionPanelState(...) 的结果。
    ///
    /// 这样页面层的职责会更清晰：
    /// - PageModel 负责状态与动作协调；
    /// - Page 负责事件接线与 Bind；
    /// - 各控件只依赖自己对应的数据对象。
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
        ///
        /// 当前页面拆分后的绑定规则：
        /// - 左侧列表：绑定 PageItems + SelectedSnapshot.ItemKey
        /// - 右上动作区：绑定 BuildActionPanelState(...)
        /// - 右下详情区：绑定 SelectedDetail
        /// </summary>
        private void RefreshView()
        {
            labelCylinderCount.Text = _model.CylinderCount.ToString();
            labelVacuumCount.Text = _model.VacuumCount.ToString();
            labelGripperCount.Text = _model.GripperCount.ToString();
            labelStackLightCount.Text = _model.StackLightCount.ToString();

            actuatorVirtualListControl.BindItems(
                _model.PageItems,
                _model.SelectedSnapshot == null ? null : _model.SelectedSnapshot.ItemKey);

            RefreshActionPanelState();

            actuatorDetailControl.Bind(_model.SelectedDetail);

            UpdateFilterButtonStyles();
        }

        /// <summary>
        /// 根据当前选中项和选项状态，刷新右侧上半区联动状态。
        /// 不做整页刷新，避免选项切换时影响左侧滚动位置。
        /// </summary>
        private void RefreshActionPanelState()
        {
            actuatorActionPanelControl.Bind(
                _model.BuildActionPanelState(
                    actuatorActionPanelControl.WaitFeedback,
                    actuatorActionPanelControl.WaitWorkpiece,
                    actuatorActionPanelControl.StackLightWithBuzzer));
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

            actuatorVirtualListControl.BindItems(
                _model.PageItems,
                _model.SelectedSnapshot == null ? null : _model.SelectedSnapshot.ItemKey);

            RefreshActionPanelState();
            actuatorDetailControl.Bind(_model.SelectedDetail);
        }

        /// <summary>
        /// 右侧动作区选项变化：
        /// - 等待反馈
        /// - 等待工件检测
        /// - 附带蜂鸣
        ///
        /// 当前只需要重算动作区状态，不需要整页刷新。
        /// </summary>
        private void ActuatorActionPanelControl_OptionsChanged(object sender, EventArgs e)
        {
            RefreshActionPanelState();
        }

        private async void ActuatorActionPanelControl_PrimaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(delegate
            {
                return _model.ExecutePrimaryActionAsync(
                    actuatorActionPanelControl.WaitFeedback,
                    actuatorActionPanelControl.WaitWorkpiece);
            });
        }

        private async void ActuatorActionPanelControl_SecondaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(delegate
            {
                return _model.ExecuteSecondaryActionAsync(
                    actuatorActionPanelControl.WaitFeedback);
            });
        }

        private async void ActuatorActionPanelControl_StackLightStateRequested(
            object sender,
            MotionActuatorActionPanelControl.StackLightStateRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(delegate
            {
                return _model.SetStackLightStateAsync(
                    e.StateKey,
                    actuatorActionPanelControl.StackLightWithBuzzer);
            });
        }

        /// <summary>
        /// 统一动作执行入口。
        ///
        /// 设计说明：
        /// 1. 页面上的所有执行器动作都从这里进入；
        /// 2. 动作执行期间禁止重复触发；
        /// 3. 失败时刷新右侧详情与动作区；
        /// 4. 成功时再刷新一次运行态，保持页面显示与实际设备状态一致。
        /// </summary>
        private async Task ExecuteActionAsync(Func<Task<Result>> executeFunc)
        {
            if (_isExecutingAction || executeFunc == null)
                return;

            _isExecutingAction = true;
            try
            {
                var result = await executeFunc();

                // 动作执行后，先立即刷新右侧详情与动作区。
                // 对于失败场景，这一步可以显示最近操作结果和可执行状态变化。
                actuatorDetailControl.Bind(_model.SelectedDetail);
                RefreshActionPanelState();

                // 成功后再刷新运行态，拉取设备最新状态。
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