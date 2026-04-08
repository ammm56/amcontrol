using AM.Model.Common;
using AM.PageModel.Motion;
using AMControlWinF.Views.MotionConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Motion
{
    /// <summary>
    /// 单轴控制页面。
    ///
    /// 页面布局：
    /// 1. 第一行工具栏：左侧选择轴，右侧搜索动作；
    /// 2. 第二行保留极小间隔；
    /// 3. 第三行主内容：左侧分上下两行，上方简单动作卡片，下方 3 个参数动作卡片；右侧实时监视。
    /// </summary>
    public partial class MotionAxisPage : UserControl
    {
        private readonly MotionAxisPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isExecutingAction;
        private short? _lastParameterLogicalAxis;

        public MotionAxisPage()
        {
            InitializeComponent();

            _model = new MotionAxisPageModel();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            InitializeParameterCards();
            BindEvents();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        private void InitializeParameterCards()
        {
            parameterCardApplyVelocity.Configure(
                "ApplyVelocity",
                "参数",
                "应用速度",
                "速度(mm/s)",
                "Success");

            parameterCardMoveAbsolute.Configure(
                "MoveAbsolute",
                "定位",
                "绝对定位",
                "目标位置(mm)",
                "Primary");

            parameterCardMoveRelative.Configure(
                "MoveRelative",
                "定位",
                "相对移动",
                "相对距离(mm)",
                "Primary");
        }

        private void BindEvents()
        {
            Load += MotionAxisPage_Load;
            VisibleChanged += MotionAxisPage_VisibleChanged;

            _refreshTimer.Tick += RefreshTimer_Tick;

            buttonSelectAxis.Click += async (s, e) => await SelectAxisAsync();
            inputSearch.TextChanged += InputSearch_TextChanged;

            motionAxisVirtualListControl.ActionExecuteRequested += MotionAxisVirtualListControl_ActionExecuteRequested;
            parameterCardApplyVelocity.ExecuteRequested += ParameterCard_ExecuteRequested;
            parameterCardMoveAbsolute.ExecuteRequested += ParameterCard_ExecuteRequested;
            parameterCardMoveRelative.ExecuteRequested += ParameterCard_ExecuteRequested;
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

            var simpleActionItems = _model.PageItems
                .Where(x => !IsParameterAction(x == null ? null : x.ActionKey))
                .ToList();

            motionAxisVirtualListControl.BindItems(simpleActionItems);

            parameterCardApplyVelocity.BindItem(FindActionItem("ApplyVelocity"));
            parameterCardMoveAbsolute.BindItem(FindActionItem("MoveAbsolute"));
            parameterCardMoveRelative.BindItem(FindActionItem("MoveRelative"));

            ApplyParameterDefaultsIfAxisChanged(_model.SelectedAxis);
            motionAxisDetailControl.Bind(_model.SelectedAxis);
        }

        private MotionAxisPageModel.MotionAxisActionViewItem FindActionItem(string actionKey)
        {
            return _model.PageItems.FirstOrDefault(
                x => string.Equals(x.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase));
        }

        private void ApplyParameterDefaultsIfAxisChanged(MotionAxisPageModel.MotionAxisSelectedViewItem axisItem)
        {
            var logicalAxis = axisItem == null ? (short?)null : axisItem.LogicalAxis;
            if (_lastParameterLogicalAxis == logicalAxis)
                return;

            _lastParameterLogicalAxis = logicalAxis;

            if (axisItem == null)
            {
                parameterCardApplyVelocity.InputText = "10";
                parameterCardMoveAbsolute.InputText = "0";
                parameterCardMoveRelative.InputText = "10";
                return;
            }

            if (axisItem.JogVelocityMm > 0D)
                parameterCardApplyVelocity.InputText = axisItem.JogVelocityMm.ToString("0.###");
            else if (axisItem.DefaultVelocityMm > 0D)
                parameterCardApplyVelocity.InputText = axisItem.DefaultVelocityMm.ToString("0.###");
            else
                parameterCardApplyVelocity.InputText = "10";

            parameterCardMoveAbsolute.InputText = axisItem.CommandPositionMm.ToString("0.###");
            parameterCardMoveRelative.InputText = "10";
        }

        private static bool IsParameterAction(string actionKey)
        {
            return string.Equals(actionKey, "ApplyVelocity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, "MoveAbsolute", StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, "MoveRelative", StringComparison.OrdinalIgnoreCase);
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
        /// 当前先复用现有轴选择窗口。
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
        /// 简单动作卡片执行。
        /// </summary>
        private async void MotionAxisVirtualListControl_ActionExecuteRequested(
            object sender,
            MotionAxisVirtualListControl.MotionAxisActionExecuteRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(e.ActionKey);
        }

        /// <summary>
        /// 参数动作卡片执行。
        /// </summary>
        private async void ParameterCard_ExecuteRequested(
            object sender,
            MotionAxisParameterActionControl.MotionAxisParameterActionExecuteRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(e.ActionKey);
        }

        private async Task ExecuteActionAsync(string actionKey)
        {
            if (_isExecutingAction || string.IsNullOrWhiteSpace(actionKey))
                return;

            _isExecutingAction = true;
            try
            {
                await _model.ExecuteActionAsync(
                    actionKey,
                    parameterCardMoveAbsolute.InputText,
                    parameterCardMoveRelative.InputText,
                    parameterCardApplyVelocity.InputText);

                await ReloadRuntimeAsync(false);
            }
            finally
            {
                _isExecutingAction = false;
            }
        }
    }
}