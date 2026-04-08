using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Motion;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms 单轴控制页页面模型。
    /// 负责：
    /// 1. 查询轴静态结构与运行态快照；
    /// 2. 维护当前选中轴；
    /// 3. 维护左侧动作卡片列表；
    /// 4. 按关键字搜索动作卡片；
    /// 5. 根据当前轴运行态更新动作联动可用性；
    /// 6. 接收页面层传入的参数并执行对应动作。
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
        /// 当前页面固定动作数量不多，不再额外分页，直接交给 VirtualPanel 承载。
        /// </summary>
        public IList<MotionAxisActionViewItem> PageItems
        {
            get { return _filteredActionItems; }
        }

        /// <summary>
        /// 当前选中的轴。
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

        /// <summary>
        /// 按动作键获取原始动作项。
        /// 该方法不受搜索过滤影响，适合底部固定参数卡片使用。
        /// </summary>
        public MotionAxisActionViewItem GetActionItem(string actionKey)
        {
            if (string.IsNullOrWhiteSpace(actionKey))
                return null;

            return _allActionItems.FirstOrDefault(
                x => string.Equals(x.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 执行左侧动作卡片对应的轴动作。
        /// 普通卡片点击即执行；需要参数的动作从右侧实时监视区读取输入值。
        /// </summary>
        public Task<Result> ExecuteActionAsync(
            string actionKey,
            string targetPositionText,
            string moveDistanceText,
            string velocityText)
        {
            if (string.IsNullOrWhiteSpace(actionKey))
            {
                return Task.FromResult(Result.Fail(-2100, "未识别的轴动作。", ResultSource.Motion));
            }

            if (SelectedAxis == null)
            {
                return Task.FromResult(Result.Fail(-2101, "请先选择轴。", ResultSource.Motion));
            }

            var action = _allActionItems.FirstOrDefault(
                x => string.Equals(x.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase));
            if (action == null)
            {
                return Task.FromResult(Result.Fail(-2102, "未识别的轴动作：" + actionKey, ResultSource.Motion));
            }

            if (!action.CanExecute)
            {
                return Task.FromResult(Result.Fail(-2103, action.DisabledReason, ResultSource.Motion));
            }

            var logicalAxis = SelectedAxis.LogicalAxis;

            if (string.Equals(actionKey, "Enable", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.Enable(logicalAxis, true));

            if (string.Equals(actionKey, "Disable", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.Enable(logicalAxis, false));

            if (string.Equals(actionKey, "Home", StringComparison.OrdinalIgnoreCase))
                return _operationService.HomeAsync(logicalAxis);

            if (string.Equals(actionKey, "ClearStatus", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.ClearStatus(logicalAxis));

            if (string.Equals(actionKey, "Stop", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.Stop(logicalAxis, false));

            if (string.Equals(actionKey, "EmergencyStop", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.Stop(logicalAxis, true));

            if (string.Equals(actionKey, "JogNegative", StringComparison.OrdinalIgnoreCase))
            {
                double velocityMm;
                if (!TryParsePositiveNumber(velocityText, out velocityMm))
                    return Task.FromResult(Result.Fail(-2104, "速度必须是大于 0 的数字。", ResultSource.Motion));

                return Task.FromResult(_operationService.JogMove(logicalAxis, false, velocityMm));
            }

            if (string.Equals(actionKey, "JogStop", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(_operationService.JogStop(logicalAxis));

            if (string.Equals(actionKey, "JogPositive", StringComparison.OrdinalIgnoreCase))
            {
                double velocityMm;
                if (!TryParsePositiveNumber(velocityText, out velocityMm))
                    return Task.FromResult(Result.Fail(-2105, "速度必须是大于 0 的数字。", ResultSource.Motion));

                return Task.FromResult(_operationService.JogMove(logicalAxis, true, velocityMm));
            }

            if (string.Equals(actionKey, "ApplyVelocity", StringComparison.OrdinalIgnoreCase))
            {
                double velocityMm;
                if (!TryParsePositiveNumber(velocityText, out velocityMm))
                    return Task.FromResult(Result.Fail(-2106, "速度必须是大于 0 的数字。", ResultSource.Motion));

                return Task.FromResult(_operationService.ApplyVelocityMm(logicalAxis, velocityMm));
            }

            if (string.Equals(actionKey, "MoveAbsolute", StringComparison.OrdinalIgnoreCase))
            {
                double positionMm;
                double velocityMm;

                if (!TryParseNumber(targetPositionText, out positionMm))
                    return Task.FromResult(Result.Fail(-2107, "目标位置格式无效。", ResultSource.Motion));

                if (!TryParsePositiveNumber(velocityText, out velocityMm))
                    return Task.FromResult(Result.Fail(-2108, "速度必须是大于 0 的数字。", ResultSource.Motion));

                return Task.FromResult(_operationService.MoveAbsoluteMm(logicalAxis, positionMm, velocityMm));
            }

            if (string.Equals(actionKey, "MoveRelative", StringComparison.OrdinalIgnoreCase))
            {
                double distanceMm;
                double velocityMm;

                if (!TryParseNumber(moveDistanceText, out distanceMm))
                    return Task.FromResult(Result.Fail(-2109, "相对距离格式无效。", ResultSource.Motion));

                if (!TryParsePositiveNumber(velocityText, out velocityMm))
                    return Task.FromResult(Result.Fail(-2110, "速度必须是大于 0 的数字。", ResultSource.Motion));

                return Task.FromResult(_operationService.MoveRelativeMm(logicalAxis, distanceMm, velocityMm));
            }

            return Task.FromResult(Result.Fail(-2111, "未识别的轴动作：" + actionKey, ResultSource.Motion));
        }

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

        /// <summary>
        /// 根据当前轴运行态更新动作卡片联动状态。
        /// 这里直接按 WPF 页面现有联动约束收敛，不额外再包装新抽象。
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

        private Result ValidateAction(string actionKey)
        {
            if (SelectedAxis == null)
                return Result.Fail(-2201, "请先选择轴", ResultSource.Motion);

            var axis = SelectedAxis;

            if (string.Equals(actionKey, "Enable", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsEnabled)
                    return Result.Fail(-2202, "当前轴已使能", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "Disable", StringComparison.OrdinalIgnoreCase))
            {
                if (!axis.IsEnabled)
                    return Result.Fail(-2203, "当前轴未使能", ResultSource.Motion);

                if (axis.IsMoving)
                    return Result.Fail(-2204, "当前轴运动中，禁止失能", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "ClearStatus", StringComparison.OrdinalIgnoreCase))
                return Result.Ok("允许执行", ResultSource.Motion);

            if (string.Equals(actionKey, "EmergencyStop", StringComparison.OrdinalIgnoreCase))
                return Result.Ok("允许执行", ResultSource.Motion);

            if (string.Equals(actionKey, "Stop", StringComparison.OrdinalIgnoreCase))
            {
                if (!axis.IsMoving)
                    return Result.Fail(-2205, "当前轴未运动", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "JogStop", StringComparison.OrdinalIgnoreCase))
                return Result.Ok("允许执行", ResultSource.Motion);

            if (axis.IsAlarm)
                return Result.Fail(-2206, "当前轴报警中，请先清状态", ResultSource.Motion);

            if (string.Equals(actionKey, "ApplyVelocity", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2214, "当前轴运动中，禁止改速度", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (!axis.IsEnabled)
                return Result.Fail(-2207, "当前轴未使能", ResultSource.Motion);

            if (string.Equals(actionKey, "Home", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2208, "当前轴运动中，禁止回零", ResultSource.Motion);

                if (axis.PositiveLimit || axis.NegativeLimit)
                    return Result.Fail(-2209, "当前轴存在限位，禁止回零", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "JogNegative", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2210, "当前轴运动中", ResultSource.Motion);

                if (axis.NegativeLimit)
                    return Result.Fail(-2211, "当前轴负限位触发", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "JogPositive", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2212, "当前轴运动中", ResultSource.Motion);

                if (axis.PositiveLimit)
                    return Result.Fail(-2213, "当前轴正限位触发", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "MoveAbsolute", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2215, "当前轴运动中", ResultSource.Motion);

                if (!axis.IsAtHome)
                    return Result.Fail(-2216, "当前轴未回原点", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            if (string.Equals(actionKey, "MoveRelative", StringComparison.OrdinalIgnoreCase))
            {
                if (axis.IsMoving)
                    return Result.Fail(-2217, "当前轴运动中", ResultSource.Motion);

                return Result.Ok("允许执行", ResultSource.Motion);
            }

            return Result.Fail(-2218, "未识别的动作", ResultSource.Motion);
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

        /// <summary>
        /// 初始化动作卡片定义。
        /// 左侧卡片统一保持按钮式小卡片风格，只保留分类和名称。
        /// </summary>
        private static List<MotionAxisActionViewItem> CreateActionItems()
        {
            return new List<MotionAxisActionViewItem>
            {
                CreateAction("Enable", "使能", "基础操作", "Primary"),
                CreateAction("Disable", "失能", "基础操作", "Default"),
                CreateAction("Home", "回零", "回零", "Warning"),
                CreateAction("ClearStatus", "清状态", "维护", "Default"),
                CreateAction("Stop", "平停", "停止", "Success"),
                CreateAction("EmergencyStop", "急停", "停止", "Danger"),
                CreateAction("JogNegative", "负向运动", "点动", "Primary"),
                CreateAction("JogStop", "停止", "点动", "Default"),
                CreateAction("JogPositive", "正向运动", "点动", "Primary"),
                CreateAction("ApplyVelocity", "应用速度", "参数", "Success"),
                CreateAction("MoveAbsolute", "绝对定位", "定位", "Primary"),
                CreateAction("MoveRelative", "相对定位", "定位", "Primary")
            };
        }

        private static MotionAxisActionViewItem CreateAction(
            string actionKey,
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

        /// <summary>
        /// 当前选中轴显示项。
        /// 供右侧实时监视区使用。
        /// </summary>
        public sealed class MotionAxisSelectedViewItem
        {
            public short LogicalAxis { get; set; }
            public short CardId { get; set; }
            public short AxisId { get; set; }
            public short PhysicalCore { get; set; }
            public short PhysicalAxis { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string AxisCategory { get; set; }
            public string CardDisplayName { get; set; }
            public double CommandPositionPulse { get; set; }
            public double EncoderPositionPulse { get; set; }
            public double CommandPositionMm { get; set; }
            public double EncoderPositionMm { get; set; }
            public double DefaultVelocityMm { get; set; }
            public double JogVelocityMm { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsAlarm { get; set; }
            public bool IsAtHome { get; set; }
            public bool PositiveLimit { get; set; }
            public bool NegativeLimit { get; set; }
            public bool IsDone { get; set; }
            public bool IsMoving { get; set; }
            public DateTime UpdateTime { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                        return DisplayName;

                    return string.IsNullOrWhiteSpace(Name) ? "轴-" + LogicalAxis : Name;
                }
            }

            public string CardText
            {
                get
                {
                    var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                    return "卡#" + CardId + "  " + name;
                }
            }

            public string AxisCategoryText
            {
                get { return string.IsNullOrWhiteSpace(AxisCategory) ? "Other" : AxisCategory; }
            }

            public string PhysicalText
            {
                get { return "核 " + PhysicalCore + " / 轴 " + PhysicalAxis; }
            }

            public string StateText
            {
                get
                {
                    if (IsAlarm)
                        return "Alarm";

                    if (IsMoving)
                        return "Moving";

                    if (!IsEnabled)
                        return "Disabled";

                    if (IsDone)
                        return "Ready";

                    return "Idle";
                }
            }

            public string EnableText
            {
                get { return IsEnabled ? "已使能" : "未使能"; }
            }

            public string HomeText
            {
                get { return IsAtHome ? "在原点" : "未回原点"; }
            }

            public string DoneText
            {
                get { return IsDone ? "已到位" : "未到位"; }
            }

            public string LimitText
            {
                get
                {
                    if (PositiveLimit && NegativeLimit)
                        return "正负限位";

                    if (PositiveLimit)
                        return "正限位";

                    if (NegativeLimit)
                        return "负限位";

                    return "无限位";
                }
            }

            public string CommandPositionMmText
            {
                get { return CommandPositionMm.ToString("0.###") + " mm"; }
            }

            public string EncoderPositionMmText
            {
                get { return EncoderPositionMm.ToString("0.###") + " mm"; }
            }

            /// <summary>
            /// 位置误差 指令位置 - 编码器位置。
            /// </summary>
            public double PositionErrorMm
            {
                get { return CommandPositionMm - EncoderPositionMm; }
            }

            public string PositionErrorMmText
            {
                get { return PositionErrorMm.ToString("0.###") + " mm"; }
            }

            public string DefaultVelocityText
            {
                get { return DefaultVelocityMm.ToString("0.###") + " mm/s"; }
            }

            public string JogVelocityText
            {
                get { return JogVelocityMm.ToString("0.###") + " mm/s"; }
            }

            public string UpdateTimeText
            {
                get
                {
                    return UpdateTime == default(DateTime)
                        ? "—"
                        : UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// 左侧动作卡片显示项。
        /// 卡片外观保持统一按钮式小卡片风格，只承载当前动作的执行状态。
        /// </summary>
        public sealed class MotionAxisActionViewItem
        {
            public string ActionKey { get; set; }
            public string DisplayText { get; set; }
            public string CategoryText { get; set; }
            public string AccentType { get; set; }
            public bool CanExecute { get; set; }
            public string DisabledReason { get; set; }
        }
    }
}