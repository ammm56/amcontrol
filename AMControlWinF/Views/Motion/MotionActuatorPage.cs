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
    /// 执行器控制页面。
    ///
    /// 当前第一步实现内容：
    /// 1. 三行页面布局；
    /// 2. 顶部类型筛选与搜索；
    /// 3. 第二行统计信息；
    /// 4. 第三行左侧执行器虚拟卡片列表；
    /// 5. 第三行右侧上下两块 UserControl 骨架绑定；
    /// 6. 500ms 低频刷新运行态。
    ///
    /// 下一步再补：
    /// - 右侧动作按钮实际执行；
    /// - 等待反馈 / 等待工件 / 灯塔蜂鸣等联动逻辑；
    /// - 更完整的详情字段与样式细调。
    /// </summary>
    public partial class MotionActuatorPage : UserControl
    {
        private readonly MotionActuatorPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;

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
            actuatorDetailControl.Bind(_model.SelectedItem);

            UpdateFilterButtonStyles();
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
        /// 这里只做最小视觉反馈，不做复杂状态管理。
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
            actuatorDetailControl.Bind(_model.SelectedItem);
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
    }
}