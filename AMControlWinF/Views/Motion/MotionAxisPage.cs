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
    /// 3. 第三行主内容：左侧分上下两行，
    ///    上方简单动作卡片，
    ///    下方 3 个参数动作卡片；
    ///    右侧为实时监视信息。
    ///
    /// 交互原则：
    /// 1. 上方简单动作卡片单击即执行；
    /// 2. 下方参数动作卡片用于输入参数并单独触发执行；
    /// 3. 搜索只影响上方简单动作卡片，不影响下方固定参数卡片；
    /// 4. 页面层统一调度动作执行，避免各控件内部直接操作业务层。
    /// </summary>
    public partial class MotionAxisPage : UserControl
    {
        private readonly MotionAxisPageModel _model;
        private readonly Timer _refreshTimer;

        private bool _isFirstLoad;
        private bool _isRefreshing;
        private bool _isExecutingAction;

        /// <summary>
        /// 记录上一次应用默认参数的轴编号。
        /// 只在切轴时刷新底部参数卡片默认值，避免定时刷新覆盖用户输入。
        /// </summary>
        private short? _lastParameterLogicalAxis;

        public MotionAxisPage()
        {
            InitializeComponent();

            _model = new MotionAxisPageModel();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 100;

            InitializeParameterCards();
            BindEvents();

            Disposed += (s, e) =>
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            };
        }

        /// <summary>
        /// 初始化下方 3 个固定参数卡片的静态信息。
        ///
        /// 说明：
        /// - 这里配置的是卡片身份，不是运行态；
        /// - 运行态（能否执行、颜色、按钮文案更新）由 BindItem(...) 刷新。
        /// </summary>
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

        /// <summary>
        /// 绑定页面事件。
        /// </summary>
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

        /// <summary>
        /// 页面首次加载入口。
        /// </summary>
        private async Task InitializePageAsync()
        {
            await ReloadRuntimeAsync(true);
            UpdateRefreshTimerState();
        }

        /// <summary>
        /// 首次加载与定时刷新共用的运行态刷新入口。
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
        /// 将页面模型状态刷新到界面。
        ///
        /// 刷新策略：
        /// 1. 上方简单动作卡片使用过滤后的 PageItems；
        /// 2. 下方参数动作卡片直接从 PageModel 的原始动作集中取值，不受搜索过滤影响；
        /// 3. 参数默认值只在切轴时刷新；
        /// 4. 右侧详情区始终绑定当前轴。
        /// </summary>
        private void RefreshView()
        {
            labelSelectedAxis.Text = "当前：" + _model.SelectedAxisText;

            var simpleActionItems = _model.PageItems
                .Where(x => !IsParameterAction(x == null ? null : x.ActionKey))
                .ToList();

            motionAxisVirtualListControl.BindItems(simpleActionItems);

            parameterCardApplyVelocity.BindItem(_model.GetActionItem("ApplyVelocity"));
            parameterCardMoveAbsolute.BindItem(_model.GetActionItem("MoveAbsolute"));
            parameterCardMoveRelative.BindItem(_model.GetActionItem("MoveRelative"));

            ApplyParameterDefaultsIfAxisChanged(_model.SelectedAxis);
            motionAxisDetailControl.Bind(_model.SelectedAxis);
        }

        /// <summary>
        /// 当选中轴变化时，将默认参数值写入底部参数卡片。
        ///
        /// 规则：
        /// 1. 只在切换逻辑轴时更新；
        /// 2. 定时刷新不覆盖用户手工输入；
        /// 3. 应用速度优先取 Jog 速度，其次取默认速度；
        /// 4. 绝对定位默认值取当前规划位置；
        /// 5. 相对移动默认值固定回到 10。
        /// </summary>
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

        /// <summary>
        /// 判断是否为参数动作。
        /// 参数动作不进入上方简单动作卡片区。
        /// </summary>
        private static bool IsParameterAction(string actionKey)
        {
            return string.Equals(actionKey, "ApplyVelocity", StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, "MoveAbsolute", StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, "MoveRelative", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 根据页面可见状态控制定时刷新器启停。
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

        /// <summary>
        /// 搜索动作卡片。
        /// 仅影响上方简单动作区，不影响下方固定参数卡片。
        /// </summary>
        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            _model.SetSearchText(inputSearch.Text);
            RefreshView();
        }

        /// <summary>
        /// 简单动作卡片执行入口。
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
        /// 参数动作卡片执行入口。
        /// 三张卡片共用同一个处理方法。
        /// </summary>
        private async void ParameterCard_ExecuteRequested(
            object sender,
            MotionAxisParameterActionControl.MotionAxisParameterActionExecuteRequestedEventArgs e)
        {
            if (e == null)
                return;

            await ExecuteActionAsync(e.ActionKey);
        }

        /// <summary>
        /// 统一动作执行入口。
        ///
        /// 参数来源约定：
        /// - 绝对定位：读取 parameterCardMoveAbsolute.InputText
        /// - 相对移动：读取 parameterCardMoveRelative.InputText
        /// - 应用速度 / 运动相关速度：读取 parameterCardApplyVelocity.InputText
        ///
        /// 所有动作执行后统一触发一次运行态刷新。
        /// </summary>
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