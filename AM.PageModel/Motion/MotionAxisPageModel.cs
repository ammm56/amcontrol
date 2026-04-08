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
    /// 1. 查询轴静态结构 + 运行态快照；
    /// 2. 维护当前选中轴；
    /// 3. 维护动作卡片列表；
    /// 4. 按关键字搜索动作卡片；
    /// 5. 计算分页并维护当前选中的动作项。
    /// </summary>
    public class MotionAxisPageModel : BindableBase
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;

        private List<MotionAxisSelectedViewItem> _allAxes;
        private List<MotionAxisActionViewItem> _allActionItems;
        private List<MotionAxisActionViewItem> _filteredActionItems;
        private List<MotionAxisActionViewItem> _pageActionItems;

        private short? _selectedLogicalAxis;
        private MotionAxisSelectedViewItem _selectedAxis;
        private MotionAxisActionViewItem _selectedAction;
        private string _searchText;
        private int _pageIndex;
        private int _pageSize;

        public MotionAxisPageModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();

            _allAxes = new List<MotionAxisSelectedViewItem>();
            _allActionItems = CreateActionItems();
            _filteredActionItems = new List<MotionAxisActionViewItem>();
            _pageActionItems = new List<MotionAxisActionViewItem>();

            _searchText = string.Empty;
            _pageIndex = 1;
            _pageSize = 12;
        }

        /// <summary>
        /// 当前页动作卡片数据。
        /// </summary>
        public IList<MotionAxisActionViewItem> PageItems
        {
            get { return _pageActionItems; }
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
        /// 当前选中的动作卡片。
        /// </summary>
        public MotionAxisActionViewItem SelectedAction
        {
            get { return _selectedAction; }
            private set { SetProperty(ref _selectedAction, value); }
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
        /// 当前页面搜索的是“动作卡片”，不是轴结构。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
        }

        public int FilteredCount
        {
            get { return _filteredActionItems.Count; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            private set { SetProperty(ref _pageIndex, value <= 0 ? 1 : value); }
        }

        public int PageSize
        {
            get { return _pageSize; }
            private set { SetProperty(ref _pageSize, value <= 0 ? 12 : value); }
        }

        public int PageCount
        {
            get
            {
                if (FilteredCount <= 0 || PageSize <= 0)
                    return 1;

                return (int)Math.Ceiling(FilteredCount * 1.0 / PageSize);
            }
        }

        public string SelectedAxisText
        {
            get
            {
                if (SelectedAxis == null)
                    return "未选择轴";

                return "L#" + SelectedAxis.LogicalAxis + "  " + SelectedAxis.DisplayTitle;
            }
        }

        public string PageSummaryText
        {
            get
            {
                if (FilteredCount <= 0)
                    return "共 0 项";

                var start = ((PageIndex - 1) * PageSize) + 1;
                var end = Math.Min(PageIndex * PageSize, FilteredCount);
                return "第 " + start + " - " + end + " 项，共 " + FilteredCount + " 项";
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
        /// 定时刷新运行态。
        /// 单轴控制页与监视页保持一致，使用低频刷新。
        /// </summary>
        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 设置搜索关键字。
        /// 搜索范围为动作名称、分组、描述和参数说明。
        /// </summary>
        public void SetSearchText(string searchText)
        {
            searchText = searchText ?? string.Empty;
            if (string.Equals(_searchText, searchText, StringComparison.Ordinal))
                return;

            _searchText = searchText;
            OnPropertyChanged(nameof(SearchText));

            PageIndex = 1;
            ApplyActionFilterAndPaging();
        }

        /// <summary>
        /// 切换分页。
        /// </summary>
        public void ChangePage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? PageSize : pageSize;

            NormalizePage();
            RebuildPageItems();
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
            ApplyActionFilterAndPaging();
            OnPropertyChanged(nameof(SelectedAxisText));
        }

        /// <summary>
        /// 选中指定动作卡片。
        /// </summary>
        public void SelectAction(string actionKey)
        {
            if (string.IsNullOrWhiteSpace(actionKey))
                return;

            var selected = _pageActionItems.FirstOrDefault(
                x => string.Equals(x.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
                return;

            SelectedAction = selected;
        }

        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousLogicalAxis = _selectedLogicalAxis;
                var previousActionKey = SelectedAction == null ? null : SelectedAction.ActionKey;

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
                ApplyActionFilterAndPaging();
                RestoreSelectedAction(previousActionKey);

                OnPropertyChanged(nameof(SelectedLogicalAxis));
                OnPropertyChanged(nameof(SelectedAxisText));

                return Result.Ok("单轴控制页加载成功");
            });
        }

        private void ClearAll()
        {
            _allAxes = new List<MotionAxisSelectedViewItem>();
            SelectedAxis = null;
            _selectedLogicalAxis = null;

            _allActionItems = CreateActionItems();
            _filteredActionItems = new List<MotionAxisActionViewItem>();
            _pageActionItems = new List<MotionAxisActionViewItem>();
            SelectedAction = null;

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
        /// 更新动作卡片是否具备可执行前提。
        /// 第一阶段先只判断“是否已选择轴”。
        /// </summary>
        private void UpdateActionAvailability()
        {
            var hasAxis = SelectedAxis != null;

            foreach (var action in _allActionItems)
            {
                action.HasSelectedAxis = hasAxis;
            }
        }

        private void ApplyActionFilterAndPaging()
        {
            IEnumerable<MotionAxisActionViewItem> query = _allActionItems;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var keyword = _searchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (x.DisplayText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CategoryText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.DescriptionText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.ParameterHintText ?? string.Empty).ToLowerInvariant().Contains(keyword));
            }

            _filteredActionItems = query.ToList();

            NormalizePage();
            RebuildPageItems();
        }

        private void NormalizePage()
        {
            if (PageIndex <= 0)
                PageIndex = 1;

            var pageCount = PageCount;
            if (PageIndex > pageCount)
                PageIndex = pageCount;
        }

        private void RebuildPageItems()
        {
            var previousActionKey = SelectedAction == null ? null : SelectedAction.ActionKey;

            var skip = (PageIndex - 1) * PageSize;
            _pageActionItems = _filteredActionItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();

            if (!string.IsNullOrWhiteSpace(previousActionKey))
            {
                SelectedAction = _pageActionItems.FirstOrDefault(
                    x => string.Equals(x.ActionKey, previousActionKey, StringComparison.OrdinalIgnoreCase))
                    ?? (_pageActionItems.Count > 0 ? _pageActionItems[0] : null);
            }
            else
            {
                SelectedAction = _pageActionItems.Count > 0 ? _pageActionItems[0] : null;
            }

            RaiseUiChanged();
        }

        private void RestoreSelectedAction(string actionKey)
        {
            if (string.IsNullOrWhiteSpace(actionKey))
            {
                SelectedAction = _pageActionItems.Count > 0 ? _pageActionItems[0] : null;
                return;
            }

            SelectedAction = _pageActionItems.FirstOrDefault(
                x => string.Equals(x.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase))
                ?? (_pageActionItems.Count > 0 ? _pageActionItems[0] : null);
        }

        private void RaiseUiChanged()
        {
            OnPropertyChanged(nameof(PageItems));
            OnPropertyChanged(nameof(FilteredCount));
            OnPropertyChanged(nameof(PageIndex));
            OnPropertyChanged(nameof(PageSize));
            OnPropertyChanged(nameof(PageCount));
            OnPropertyChanged(nameof(PageSummaryText));
        }

        /// <summary>
        /// 初始化动作卡片定义。
        /// 第一阶段先构建动作元数据，不在页面模型内直接执行设备动作。
        /// 真正执行可在下一阶段接入 MotionAxisOperationService。
        /// </summary>
        private static List<MotionAxisActionViewItem> CreateActionItems()
        {
            return new List<MotionAxisActionViewItem>
            {
                CreateAction("Enable", "使能", "基础操作", "给当前选中轴上伺服使能。", "无需额外参数", "Primary"),
                CreateAction("Disable", "失能", "基础操作", "关闭当前选中轴的使能状态。", "无需额外参数", "Default"),
                CreateAction("Home", "回零", "回零", "执行当前选中轴回零。", "无需额外参数", "Warning"),
                CreateAction("ClearStatus", "清状态", "维护", "清除轴当前状态和部分故障标记。", "无需额外参数", "Default"),
                CreateAction("Stop", "平停", "停止", "执行普通停止，按减速过程停止轴。", "无需额外参数", "Success"),
                CreateAction("EmergencyStop", "急停", "停止", "执行急停，立即打断当前运动。", "无需额外参数", "Danger"),
                CreateAction("JogNegative", "负向运动", "点动", "按当前速度做负方向连续点动。", "需要速度参数", "Primary"),
                CreateAction("JogStop", "停止", "点动", "停止当前点动运动。", "无需额外参数", "Default"),
                CreateAction("JogPositive", "正向运动", "点动", "按当前速度做正方向连续点动。", "需要速度参数", "Primary"),
                CreateAction("ApplyVelocity", "应用速度", "参数", "将当前输入速度应用到轴运行参数。", "需要速度参数", "Success"),
                CreateAction("MoveAbsolute", "绝对定位", "定位", "移动到指定绝对位置。", "需要位置与速度参数", "Primary"),
                CreateAction("MoveRelative", "相对移动", "定位", "按指定距离做相对位移。", "需要距离与速度参数", "Primary")
            };
        }

        private static MotionAxisActionViewItem CreateAction(
            string actionKey,
            string displayText,
            string categoryText,
            string descriptionText,
            string parameterHintText,
            string accentType)
        {
            return new MotionAxisActionViewItem
            {
                ActionKey = actionKey,
                DisplayText = displayText,
                CategoryText = categoryText,
                DescriptionText = descriptionText,
                ParameterHintText = parameterHintText,
                AccentType = accentType,
                HasSelectedAxis = false
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
                IsMoving = item != null && item.IsMoving
            };
        }

        /// <summary>
        /// 当前选中轴显示项。
        /// 供右侧详情区使用。
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

            public string DefaultVelocityText
            {
                get { return DefaultVelocityMm.ToString("0.###") + " mm/s"; }
            }

            public string JogVelocityText
            {
                get { return JogVelocityMm.ToString("0.###") + " mm/s"; }
            }
        }

        /// <summary>
        /// 单轴控制动作卡片显示项。
        /// 当前阶段先承载动作元数据，后续再接入真实执行逻辑。
        /// </summary>
        public sealed class MotionAxisActionViewItem
        {
            public string ActionKey { get; set; }
            public string DisplayText { get; set; }
            public string CategoryText { get; set; }
            public string DescriptionText { get; set; }
            public string ParameterHintText { get; set; }
            public string AccentType { get; set; }
            public bool HasSelectedAxis { get; set; }

            public string AxisStateHintText
            {
                get { return HasSelectedAxis ? "已选择轴" : "需先选轴"; }
            }
        }
    }
}