using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Motion;
using AM.PageModel.Common;
using AM.PageModel.Motion.Axis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms 单轴控制页面模型。
    ///
    /// 【当前职责】
    /// 1. 查询轴静态结构与运行态快照；
    /// 2. 维护当前选中轴；
    /// 3. 维护左侧动作卡片集合；
    /// 4. 按关键字搜索动作卡片；
    /// 5. 根据当前轴运行态更新动作联动可用性；
    /// 6. 接收页面层传入的动作请求并执行对应动作。
    ///
    /// 【层级关系】
    /// - 上游：`MotionAxisPage`；
    /// - 当前层：WinForms 页面模型层；
    /// - 下游：`MotionRuntimeQueryService`、`MotionAxisOperationService`。
    ///
    /// 【设计说明】
    /// 本类保持 WinForms 直接事件驱动下的轻量页面模型定位：
    /// - 页面层负责事件接线与 `Bind`；
    /// - 页面模型负责状态维护、动作校验与动作执行；
    /// - 不引入额外命令系统、验证器类或执行器类。
    /// </summary>
    public class MotionAxisPageModel : BindableBase
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly MotionAxisOperationService _operationService;

        private List<MotionAxisSelectedViewItem> _allAxes;
        private List<MotionAxisActionViewItem> _allActionItems;
        private List<MotionAxisActionViewItem> _filteredActionItems;

        private short? _selectedLogicalAxis;
        private MotionAxisSelectedViewItem _selectedAxis;
        private string _searchText;

        #region 构造与属性

        public MotionAxisPageModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _operationService = new MotionAxisOperationService();

            _allAxes = new List<MotionAxisSelectedViewItem>();
            _allActionItems = CreateActionItems();
            _filteredActionItems = new List<MotionAxisActionViewItem>();

            _searchText = string.Empty;
        }

        /// <summary>
        /// 当前展示的动作卡片集合。
        /// 当前页面固定动作数量不多，不再额外分页，直接交给 `VirtualPanel` 承载。
        /// </summary>
        public IList<MotionAxisActionViewItem> PageItems
        {
            get { return _filteredActionItems; }
        }

        /// <summary>
        /// 当前选中的轴。
        /// 右侧详情区直接绑定该对象。
        /// </summary>
        public MotionAxisSelectedViewItem SelectedAxis
        {
            get { return _selectedAxis; }
            private set { SetProperty(ref _selectedAxis, value); }
        }

        /// <summary>
        /// 当前选中的逻辑轴。
        /// </summary>
        public short? SelectedLogicalAxis
        {
            get { return _selectedLogicalAxis; }
        }

        /// <summary>
        /// 搜索关键字。
        /// 当前页面搜索的是动作卡片，不是轴结构。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
        }

        /// <summary>
        /// 当前动作卡片数量。
        /// </summary>
        public int FilteredCount
        {
            get { return _filteredActionItems.Count; }
        }

        /// <summary>
        /// 当前轴标题文本。
        /// </summary>
        public string SelectedAxisText
        {
            get
            {
                if (SelectedAxis == null)
                    return "未选择轴";

                return "L#" + SelectedAxis.LogicalAxis + "  " + SelectedAxis.DisplayTitle;
            }
        }

        #endregion

        #region 页面状态入口

        /// <summary>
        /// 首次加载。
        /// </summary>
        public async Task<Result> LoadAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 低频刷新运行态。
        /// </summary>
        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 设置搜索关键字。
        /// 搜索范围为动作名称、分类和禁用原因。
        /// </summary>
        public void SetSearchText(string searchText)
        {
            searchText = searchText ?? string.Empty;
            if (string.Equals(_searchText, searchText, StringComparison.Ordinal))
                return;

            _searchText = searchText;
            OnPropertyChanged(nameof(SearchText));

            ApplyActionFilter();
        }

        /// <summary>
        /// 设置当前选中轴。
        /// 页面层从选择窗口返回后调用。
        /// </summary>
        public void SetSelectedLogicalAxis(short? logicalAxis)
        {
            if (_selectedLogicalAxis == logicalAxis)
                return;

            _selectedLogicalAxis = logicalAxis;
            OnPropertyChanged(nameof(SelectedLogicalAxis));

            RefreshSelectedAxis();
            UpdateActionAvailability();
            ApplyActionFilter();
            OnPropertyChanged(nameof(SelectedAxisText));
        }

        #endregion

        #region 页面动作入口

        /// <summary>
        /// 按动作枚举获取原始动作项。
        /// 该方法不受搜索过滤影响，适合底部固定参数卡片使用。
        /// </summary>
        public MotionAxisActionViewItem GetActionItem(MotionAxisActionKey actionKey)
        {
            return _allActionItems.FirstOrDefault(x => x.ActionKey == actionKey);
        }

        /// <summary>
        /// 执行轴动作请求。
        /// 页面层负责收集参数文本，本模型负责校验、解析和动作执行。
        /// </summary>
        public Task<Result> ExecuteActionAsync(MotionAxisActionRequest request)
        {
            if (request == null)
                return Task.FromResult(Result.Fail(-2100, "未识别的轴动作。", ResultSource.Motion));

            if (SelectedAxis == null)
                return Task.FromResult(Result.Fail(-2101, "请先选择轴。", ResultSource.Motion));

            var action = GetActionItem(request.ActionKey);
            if (action == null)
                return Task.FromResult(Result.Fail(-2102, "未识别的轴动作：" + request.ActionKey, ResultSource.Motion));

            if (!action.CanExecute)
                return Task.FromResult(Result.Fail(-2103, action.DisabledReason, ResultSource.Motion));

            return ExecuteActionCoreAsync(request);
        }

        #endregion

        #region 数据刷新与选中轴维护

        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousLogicalAxis = _selectedLogicalAxis;

                var result = _runtimeQueryService.QueryAxisSnapshot();
                if (!result.Success)
                {
                    ClearAll();
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allAxes = result.Items == null
                    ? new List<MotionAxisSelectedViewItem>()
                    : result.Items
                        .OrderBy(x => x.CardId)
                        .ThenBy(x => x.LogicalAxis)
                        .Select(ToAxisViewItem)
                        .ToList();

                if (previousLogicalAxis.HasValue &&
                    _allAxes.Any(x => x.LogicalAxis == previousLogicalAxis.Value))
                {
                    _selectedLogicalAxis = previousLogicalAxis;
                }
                else
                {
                    _selectedLogicalAxis = _allAxes.Count > 0
                        ? (short?)_allAxes[0].LogicalAxis
                        : null;
                }

                RefreshSelectedAxis();
                UpdateActionAvailability();
                ApplyActionFilter();

                OnPropertyChanged(nameof(SelectedLogicalAxis));
                OnPropertyChanged(nameof(SelectedAxisText));

                return Result.Ok("单轴控制页加载成功", ResultSource.Motion);
            });
        }

        private void ClearAll()
        {
            _allAxes = new List<MotionAxisSelectedViewItem>();
            SelectedAxis = null;
            _selectedLogicalAxis = null;

            _allActionItems = CreateActionItems();
            _filteredActionItems = new List<MotionAxisActionViewItem>();

            RaiseUiChanged();
            OnPropertyChanged(nameof(SelectedLogicalAxis));
            OnPropertyChanged(nameof(SelectedAxisText));
        }

        private void RefreshSelectedAxis()
        {
            if (!_selectedLogicalAxis.HasValue)
            {
                SelectedAxis = null;
                return;
            }

            SelectedAxis = _allAxes.FirstOrDefault(x => x.LogicalAxis == _selectedLogicalAxis.Value);
        }

        #endregion

        #region 动作状态与过滤

        /// <summary>
        /// 根据当前轴运行态更新动作卡片联动状态。
        /// </summary>
        private void UpdateActionAvailability()
        {
            foreach (var action in _allActionItems)
            {
                var validate = ValidateAction(action.ActionKey);
                action.CanExecute = validate.Success;
                action.DisabledReason = validate.Success ? "单击立即执行" : validate.Message;
            }
        }

        private void ApplyActionFilter()
        {
            IEnumerable<MotionAxisActionViewItem> query = _allActionItems;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var keyword = _searchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (x.DisplayText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CategoryText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.DisabledReason ?? string.Empty).ToLowerInvariant().Contains(keyword));
            }

            _filteredActionItems = query.ToList();
            RaiseUiChanged();
        }

        private void RaiseUiChanged()
        {
            OnPropertyChanged(nameof(PageItems));
            OnPropertyChanged(nameof(FilteredCount));
        }

        #endregion

        #region 动作校验与执行

        private Result ValidateAction(MotionAxisActionKey actionKey)
        {
            if (SelectedAxis == null)
                return Result.Fail(-2201, "请先选择轴", ResultSource.Motion);

            var axis = SelectedAxis;

            switch (actionKey)
            {
                case MotionAxisActionKey.Enable:
                    if (axis.IsEnabled)
                        return Result.Fail(-2202, "当前轴已使能", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.Disable:
                    if (!axis.IsEnabled)
                        return Result.Fail(-2203, "当前轴未使能", ResultSource.Motion);

                    if (axis.IsMoving)
                        return Result.Fail(-2204, "当前轴运动中，禁止失能", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.ClearStatus:
                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.EmergencyStop:
                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.Stop:
                    if (!axis.IsMoving)
                        return Result.Fail(-2205, "当前轴未运动", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.JogStop:
                    return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (axis.IsAlarm)
                return Result.Fail(-2206, "当前轴报警中，请先清状态", ResultSource.Motion);

            if (actionKey == MotionAxisActionKey.ApplyVelocity)
            {
                if (axis.IsMoving)
                    return Result.Fail(-2214, "当前轴运动中，禁止改速度", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (!axis.IsEnabled)
                return Result.Fail(-2207, "当前轴未使能", ResultSource.Motion);

            switch (actionKey)
            {
                case MotionAxisActionKey.Home:
                    if (axis.IsMoving)
                        return Result.Fail(-2208, "当前轴运动中，禁止回零", ResultSource.Motion);

                    if (axis.PositiveLimit || axis.NegativeLimit)
                        return Result.Fail(-2209, "当前轴存在限位，禁止回零", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.JogNegative:
                    if (axis.IsMoving)
                        return Result.Fail(-2210, "当前轴运动中", ResultSource.Motion);

                    if (axis.NegativeLimit)
                        return Result.Fail(-2211, "当前轴负限位触发", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.JogPositive:
                    if (axis.IsMoving)
                        return Result.Fail(-2212, "当前轴运动中", ResultSource.Motion);

                    if (axis.PositiveLimit)
                        return Result.Fail(-2213, "当前轴正限位触发", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.MoveAbsolute:
                    if (axis.IsMoving)
                        return Result.Fail(-2215, "当前轴运动中", ResultSource.Motion);

                    if (!axis.IsAtHome)
                        return Result.Fail(-2216, "当前轴未回原点", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                case MotionAxisActionKey.MoveRelative:
                    if (axis.IsMoving)
                        return Result.Fail(-2217, "当前轴运动中", ResultSource.Motion);

                    return Result.Ok("允许执行", ResultSource.Motion);

                default:
                    return Result.Fail(-2218, "未识别的动作", ResultSource.Motion);
            }
        }

        private Task<Result> ExecuteActionCoreAsync(MotionAxisActionRequest request)
        {
            var logicalAxis = SelectedAxis.LogicalAxis;

            switch (request.ActionKey)
            {
                case MotionAxisActionKey.Enable:
                    return Task.FromResult(_operationService.Enable(logicalAxis, true));

                case MotionAxisActionKey.Disable:
                    return Task.FromResult(_operationService.Enable(logicalAxis, false));

                case MotionAxisActionKey.Home:
                    return _operationService.HomeAsync(logicalAxis);

                case MotionAxisActionKey.ClearStatus:
                    return Task.FromResult(_operationService.ClearStatus(logicalAxis));

                case MotionAxisActionKey.Stop:
                    return Task.FromResult(_operationService.Stop(logicalAxis, false));

                case MotionAxisActionKey.EmergencyStop:
                    return Task.FromResult(_operationService.Stop(logicalAxis, true));

                case MotionAxisActionKey.JogNegative:
                    {
                        double velocityMm;
                        if (!TryParsePositiveNumber(request.VelocityText, out velocityMm))
                            return Task.FromResult(Result.Fail(-2104, "速度必须是大于 0 的数字。", ResultSource.Motion));

                        return Task.FromResult(_operationService.JogMove(logicalAxis, false, velocityMm));
                    }

                case MotionAxisActionKey.JogStop:
                    return Task.FromResult(_operationService.JogStop(logicalAxis));

                case MotionAxisActionKey.JogPositive:
                    {
                        double velocityMm;
                        if (!TryParsePositiveNumber(request.VelocityText, out velocityMm))
                            return Task.FromResult(Result.Fail(-2105, "速度必须是大于 0 的数字。", ResultSource.Motion));

                        return Task.FromResult(_operationService.JogMove(logicalAxis, true, velocityMm));
                    }

                case MotionAxisActionKey.ApplyVelocity:
                    {
                        double velocityMm;
                        if (!TryParsePositiveNumber(request.VelocityText, out velocityMm))
                            return Task.FromResult(Result.Fail(-2106, "速度必须是大于 0 的数字。", ResultSource.Motion));

                        return Task.FromResult(_operationService.ApplyVelocityMm(logicalAxis, velocityMm));
                    }

                case MotionAxisActionKey.MoveAbsolute:
                    {
                        double positionMm;
                        double velocityMm;

                        if (!TryParseNumber(request.TargetPositionText, out positionMm))
                            return Task.FromResult(Result.Fail(-2107, "目标位置格式无效。", ResultSource.Motion));

                        if (!TryParsePositiveNumber(request.VelocityText, out velocityMm))
                            return Task.FromResult(Result.Fail(-2108, "速度必须是大于 0 的数字。", ResultSource.Motion));

                        return Task.FromResult(_operationService.MoveAbsoluteMm(logicalAxis, positionMm, velocityMm));
                    }

                case MotionAxisActionKey.MoveRelative:
                    {
                        double distanceMm;
                        double velocityMm;

                        if (!TryParseNumber(request.MoveDistanceText, out distanceMm))
                            return Task.FromResult(Result.Fail(-2109, "相对距离格式无效。", ResultSource.Motion));

                        if (!TryParsePositiveNumber(request.VelocityText, out velocityMm))
                            return Task.FromResult(Result.Fail(-2110, "速度必须是大于 0 的数字。", ResultSource.Motion));

                        return Task.FromResult(_operationService.MoveRelativeMm(logicalAxis, distanceMm, velocityMm));
                    }

                default:
                    return Task.FromResult(Result.Fail(-2111, "未识别的轴动作：" + request.ActionKey, ResultSource.Motion));
            }
        }

        #endregion

        #region 参数解析

        private static bool TryParseNumber(string text, out double value)
        {
            value = 0D;

            if (string.IsNullOrWhiteSpace(text))
                return false;

            return double.TryParse(text.Trim(), out value);
        }

        private static bool TryParsePositiveNumber(string text, out double value)
        {
            value = 0D;

            if (!TryParseNumber(text, out value))
                return false;

            return value > 0D;
        }

        #endregion

        #region 动作定义与映射

        /// <summary>
        /// 初始化动作卡片定义。
        /// 左侧卡片统一保持按钮式小卡片风格，只保留分类和名称。
        /// </summary>
        private static List<MotionAxisActionViewItem> CreateActionItems()
        {
            return new List<MotionAxisActionViewItem>
            {
                CreateAction(MotionAxisActionKey.Enable, "使能", "基础操作", "Primary"),
                CreateAction(MotionAxisActionKey.Disable, "失能", "基础操作", "Default"),
                CreateAction(MotionAxisActionKey.Home, "回零", "回零", "Warning"),
                CreateAction(MotionAxisActionKey.ClearStatus, "清状态", "维护", "Default"),
                CreateAction(MotionAxisActionKey.Stop, "平停", "停止", "Success"),
                CreateAction(MotionAxisActionKey.EmergencyStop, "急停", "停止", "Danger"),
                CreateAction(MotionAxisActionKey.JogNegative, "负向运动", "点动", "Primary"),
                CreateAction(MotionAxisActionKey.JogStop, "停止", "点动", "Default"),
                CreateAction(MotionAxisActionKey.JogPositive, "正向运动", "点动", "Primary"),
                CreateAction(MotionAxisActionKey.ApplyVelocity, "应用速度", "参数", "Success"),
                CreateAction(MotionAxisActionKey.MoveAbsolute, "绝对定位", "定位", "Primary"),
                CreateAction(MotionAxisActionKey.MoveRelative, "相对定位", "定位", "Primary")
            };
        }

        private static MotionAxisActionViewItem CreateAction(
            MotionAxisActionKey actionKey,
            string displayText,
            string categoryText,
            string accentType)
        {
            return new MotionAxisActionViewItem
            {
                ActionKey = actionKey,
                DisplayText = displayText,
                CategoryText = categoryText,
                AccentType = accentType,
                CanExecute = false,
                DisabledReason = "请先选择轴"
            };
        }

        private static MotionAxisSelectedViewItem ToAxisViewItem(MotionAxisDisplayItem item)
        {
            return new MotionAxisSelectedViewItem
            {
                LogicalAxis = item == null ? (short)0 : item.LogicalAxis,
                CardId = item == null ? (short)0 : item.CardId,
                AxisId = item == null ? (short)0 : item.AxisId,
                PhysicalCore = item == null ? (short)0 : item.PhysicalCore,
                PhysicalAxis = item == null ? (short)0 : item.PhysicalAxis,
                Name = item == null ? string.Empty : item.Name,
                DisplayName = item == null ? string.Empty : item.DisplayName,
                AxisCategory = item == null ? string.Empty : item.AxisCategory,
                CardDisplayName = item == null ? string.Empty : item.CardDisplayName,
                CommandPositionPulse = item == null ? 0D : item.CommandPositionPulse,
                EncoderPositionPulse = item == null ? 0D : item.EncoderPositionPulse,
                CommandPositionMm = item == null ? 0D : item.CommandPositionMm,
                EncoderPositionMm = item == null ? 0D : item.EncoderPositionMm,
                DefaultVelocityMm = item == null ? 0D : item.DefaultVelocityMm,
                JogVelocityMm = item == null ? 0D : item.JogVelocityMm,
                IsEnabled = item != null && item.IsEnabled,
                IsAlarm = item != null && item.IsAlarm,
                IsAtHome = item != null && item.IsAtHome,
                PositiveLimit = item != null && item.PositiveLimit,
                NegativeLimit = item != null && item.NegativeLimit,
                IsDone = item != null && item.IsDone,
                IsMoving = item != null && item.IsMoving,
                UpdateTime = item == null ? default(DateTime) : item.UpdateTime
            };
        }

        #endregion
    }
}