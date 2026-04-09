using AM.Model.Common;
using AM.PageModel.Motion;
using AM.PageModel.Motion.Axis;
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
    /// 【当前职责】
    /// 1. 负责页面生命周期与低频定时刷新；
    /// 2. 负责选择轴、搜索动作、参数输入等 WinForms 直接事件接线；
    /// 3. 负责将页面模型状态绑定到左侧简单动作区、下方参数动作区和右侧详情区；
    /// 4. 负责收集页面输入并构建动作请求，交由页面模型执行。
    ///
    /// 【层级关系】
    /// - 上游：`MainWindow` 页面缓存与导航；
    /// - 当前层：WinForms 单轴控制页面；
    /// - 下游：`MotionAxisPageModel`、`MotionAxisVirtualListControl`、
    ///   `MotionAxisParameterActionControl`、`MotionAxisDetailControl`。
    ///
    /// 【调用关系】
    /// 1. 页面首次加载和定时刷新时调用 `LoadAsync` / `RefreshAsync`；
    /// 2. 页面从模型读取动作卡片、参数卡片和右侧详情数据直接 Bind；
    /// 3. 页面负责收集参数输入并构建 `MotionAxisActionRequest`；
    /// 4. 所有动作执行最终都回到 `MotionAxisPageModel` 统一校验和执行。
    ///
    /// 【架构设计】
    /// 本页保持 WinForms 直接事件驱动模式：
    /// - 页面只做事件、调用和 Bind；
    /// - 页面模型负责轴状态维护、动作校验与动作执行；
    /// - 参数输入仍留在页面控件层，避免过度抽象。
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

        #region 构造与初始化

        public MotionAxisPage()
        {
            InitializeComponent();

            _model = new MotionAxisPageModel();
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 500;

            InitializeParameterCards();
            BindEvents();

            Disposed += MotionAxisPage_Disposed;
        }

        /// <summary>
        /// 初始化下方 3 个固定参数卡片的静态信息。
        /// </summary>
        private void InitializeParameterCards()
        {
            parameterCardApplyVelocity.Configure(
                MotionAxisActionKey.ApplyVelocity,
                "参数",
                "应用速度",
                "速度(mm/s)",
                "Success");

            parameterCardMoveAbsolute.Configure(
                MotionAxisActionKey.MoveAbsolute,
                "定位",
                "绝对定位",
                "目标位置(mm)",
                "Primary");

            parameterCardMoveRelative.Configure(
                MotionAxisActionKey.MoveRelative,
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

        private void MotionAxisPage_Disposed(object sender, EventArgs e)
        {
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
        }

        #endregion

        #region 页面生命周期

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

        /// <summary>
        /// 将页面模型状态刷新到界面。
        /// </summary>
        private void RefreshView()
        {
            labelSelectedAxis.Text = "当前：" + _model.SelectedAxisText;

            var simpleActionItems = _model.PageItems
                .Where(x => !IsParameterAction(x.ActionKey))
                .ToList();

            motionAxisVirtualListControl.BindItems(simpleActionItems);

            parameterCardApplyVelocity.BindItem(_model.GetActionItem(MotionAxisActionKey.ApplyVelocity));
            parameterCardMoveAbsolute.BindItem(_model.GetActionItem(MotionAxisActionKey.MoveAbsolute));
            parameterCardMoveRelative.BindItem(_model.GetActionItem(MotionAxisActionKey.MoveRelative));

            ApplyParameterDefaultsIfAxisChanged(_model.SelectedAxis);
            motionAxisDetailControl.Bind(_model.SelectedAxis);
        }

        /// <summary>
        /// 当选中轴变化时，将默认参数值写入底部参数卡片。
        /// 定时刷新不覆盖用户手工输入。
        /// </summary>
        private void ApplyParameterDefaultsIfAxisChanged(MotionAxisSelectedViewItem axisItem)
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

        #endregion

        #region 页面事件处理

        /// <summary>
        /// 选择轴。
        /// 当前先复用现有轴选择窗口。
        /// </summary>
        private Task SelectAxisAsync()
        {
            using (var dialog = new MotionAxisSelectDialog(_model.SelectedLogicalAxis))
            {
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return Task.CompletedTask;

                _model.SetSelectedLogicalAxis(dialog.SelectedLogicalAxis);
                RefreshView();
            }

            return Task.CompletedTask;
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

        #endregion

        #region 页面动作执行

        /// <summary>
        /// 统一动作执行入口。
        /// 页面层负责收集输入并构建动作请求。
        /// </summary>
        private async Task ExecuteActionAsync(MotionAxisActionKey actionKey)
        {
            if (_isExecutingAction)
                return;

            _isExecutingAction = true;
            try
            {
                await _model.ExecuteActionAsync(BuildActionRequest(actionKey));
                await ReloadRuntimeAsync(false);
            }
            finally
            {
                _isExecutingAction = false;
            }
        }

        private MotionAxisActionRequest BuildActionRequest(MotionAxisActionKey actionKey)
        {
            return new MotionAxisActionRequest
            {
                ActionKey = actionKey,
                TargetPositionText = parameterCardMoveAbsolute.InputText,
                MoveDistanceText = parameterCardMoveRelative.InputText,
                VelocityText = parameterCardApplyVelocity.InputText
            };
        }

        #endregion

        #region 辅助方法

        private static bool IsParameterAction(MotionAxisActionKey actionKey)
        {
            return actionKey == MotionAxisActionKey.ApplyVelocity
                || actionKey == MotionAxisActionKey.MoveAbsolute
                || actionKey == MotionAxisActionKey.MoveRelative;
        }

        #endregion
    }
}