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

            Disposed += delegate
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

        private void ChangeTypeFilter(string typeFilter)
        {
            _model.SetTypeFilter(typeFilter);
            RefreshView();
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

        private async void ActuatorActionPanelControl_StackLightStateRequested(object sender, MotionActuatorActionPanelControl.StackLightStateRequestedEventArgs e)
        {
            if (e == null)
                return;

            UpdateActionPanelOptions();

            await ExecuteActionAsync(()=>
            {
                return _model.SetStackLightStateAsync(e.State);
            });
        }

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