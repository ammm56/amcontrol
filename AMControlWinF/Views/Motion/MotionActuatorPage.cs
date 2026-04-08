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
    /// 当前第二步实现内容：
    /// 1. 左侧执行器卡片选择；
    /// 2. 右侧上半区控制面板交互；
    /// 3. 主动作 / 副动作 / 灯塔状态切换；
    /// 4. WaitFeedback / WaitWorkpiece / 蜂鸣联动；
    /// 5. 右侧下半区详情按行展示；
    /// 6. 页面统一调度动作执行，避免控件内部直接调用服务。
    /// </summary>
    public partial class MotionActuatorPage : UserControl
    {
        private readonly MotionActuatorPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isExecutingAction;

        public MotionActuatorPage()
        {
            InitializeComponent();

            _model = new MotionActuatorPageModel();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            BindEvents();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
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
        /// </summary>
        private void RefreshView()
        {
            labelCylinderCount.Text = _model.CylinderCount.ToString();
            labelVacuumCount.Text = _model.VacuumCount.ToString();
            labelGripperCount.Text = _model.GripperCount.ToString();
            labelStackLightCount.Text = _model.StackLightCount.ToString();

            actuatorVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            actuatorActionPanelControl.Bind(_model.SelectedItem);
            RefreshActionPanelState();
            actuatorDetailControl.Bind(_model.SelectedItem);

            UpdateFilterButtonStyles();
        }

        /// <summary>
        /// 根据当前选中项和选项状态，刷新右侧上半区联动状态。
        /// 不做整页刷新，避免输入选项切换时影响左侧滚动位置。
        /// </summary>
        private void RefreshActionPanelState()
        {
            actuatorActionPanelControl.ApplyActionState(
                _model.BuildActionPanelState(
                    actuatorActionPanelControl.WaitFeedback,
                    actuatorActionPanelControl.WaitWorkpiece,
                    actuatorActionPanelControl.StackLightWithBuzzer));
        }

        /// <summary>
        /// 切换类型筛选。
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

        private void ActuatorVirtualListControl_ItemSelected(
            object sender,
            MotionActuatorVirtualListControl.MotionActuatorItemSelectedEventArgs e)
        {
            if (e == null)
                return;

            _model.SelectItem(e.ItemKey);
            actuatorVirtualListControl.BindItems(_model.PageItems, _model.SelectedItem);
            actuatorActionPanelControl.Bind(_model.SelectedItem);
            RefreshActionPanelState();
            actuatorDetailControl.Bind(_model.SelectedItem);
        }

        private void ActuatorActionPanelControl_OptionsChanged(object sender, EventArgs e)
        {
            RefreshActionPanelState();
        }

        private async void ActuatorActionPanelControl_PrimaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(() =>
                _model.ExecutePrimaryActionAsync(
                    actuatorActionPanelControl.WaitFeedback,
                    actuatorActionPanelControl.WaitWorkpiece));
        }

        private async void ActuatorActionPanelControl_SecondaryActionRequested(object sender, EventArgs e)
        {
            await ExecuteActionAsync(() =>
                _model.ExecuteSecondaryActionAsync(
                    actuatorActionPanelControl.WaitFeedback));
        }

        private async void ActuatorActionPanelControl_StackLightStateRequested(
            object sender,
            MotionActuatorActionPanelControl.StackLightStateRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(() =>
                _model.SetStackLightStateAsync(
                    e.StateKey,
                    actuatorActionPanelControl.StackLightWithBuzzer));
        }

        /// <summary>
        /// 统一动作执行入口。
        /// 成功后刷新运行态；失败时仅刷新右侧状态与详情文案。
        /// </summary>
        private async Task ExecuteActionAsync(Func<Task<Result>> executeFunc)
        {
            if (_isExecutingAction || executeFunc == null)
                return;

            _isExecutingAction = true;
            try
            {
                var result = await executeFunc();

                actuatorDetailControl.Bind(_model.SelectedItem);
                RefreshActionPanelState();

                if (result != null && result.Success)
                    await ReloadRuntimeAsync(false);
            }
            finally
            {
                _isExecutingAction = false;
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
    }
}