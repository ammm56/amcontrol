using AM.Model.Common;
using AM.PageModel.Motion;
using AMControlWinF.Views.MotionConfig;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制页面。
    ///
    /// 页面布局：
    /// 1. 第一行工具栏：左侧选择轴，右侧搜索动作；
    /// 2. 第二行预留空白区：保持和现有监视页一致的三行结构；
    /// 3. 第三行主内容：左侧虚拟动作卡片，右侧实时监视信息。
    ///
    /// 交互策略：
    /// - 左侧动作卡片外观是卡片，交互语义按按钮处理；
    /// - 普通动作卡片单击即执行；
    /// - 需要参数的动作从右侧参数输入区读取数值；
    /// - 右侧始终显示当前轴实时信息，不依赖动作卡片选中。
    /// </summary>
    public partial class MotionAxisPage : UserControl
    {
        private readonly MotionAxisPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isExecutingAction;

        public MotionAxisPage()
        {
            InitializeComponent();

            _model = new MotionAxisPageModel();

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
            Load += MotionAxisPage_Load;
            VisibleChanged += MotionAxisPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectAxis.Click += async (s, e) => await SelectAxisAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;
            motionAxisVirtualListControl.ActionExecuteRequested += MotionAxisVirtualListControl_ActionExecuteRequested;
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
            labelSelectedAxis.Text = "当前：" + _model.SelectedAxisText;

            motionAxisVirtualListControl.BindItems(_model.PageItems);
            motionAxisDetailControl.Bind(_model.SelectedAxis);
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

        /// <summary>
        /// 选择轴。
        /// 当前先复用现有轴选择窗口，后续如需切换为直接嵌入轴拓扑页，可在此替换实现。
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

        /// <summary>
        /// 左侧动作卡片单击即执行。
        /// 执行参数统一从右侧实时监视区读取。
        /// </summary>
        private async void MotionAxisVirtualListControl_ActionExecuteRequested(object sender, MotionAxisVirtualListControl.MotionAxisActionExecuteRequestedEventArgs e)
        {
            if (e == null || _isExecutingAction)
                return;

            _isExecutingAction = true;
            try
            {
                await _model.ExecuteActionAsync(
                    e.ActionKey,
                    motionAxisDetailControl.TargetPositionText,
                    motionAxisDetailControl.MoveDistanceText,
                    motionAxisDetailControl.VelocityText);

                await ReloadRuntimeAsync(false);
            }
            finally
            {
                _isExecutingAction = false;
            }
        }
    }
}
