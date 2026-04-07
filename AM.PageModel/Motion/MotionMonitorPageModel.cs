using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Motion;
using AM.Model.MotionCard;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms 多轴总览页面模型。
    /// 负责：
    /// 1. 查询轴静态结构 + 运行态快照；
    /// 2. 按控制卡与关键字筛选；
    /// 3. 计算分页与统计信息；
    /// 4. 维护当前选中轴。
    /// </summary>
    public class MotionMonitorPageModel : BindableBase
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;

        private List<MotionAxisViewItem> _allItems;
        private List<MotionAxisViewItem> _filteredItems;
        private List<MotionAxisViewItem> _pageItems;
        private List<MotionCardFilterItem> _cardFilters;

        private short? _selectedCardId;
        private string _searchText;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;
        private int _alarmCount;
        private int _movingCount;
        private int _readyCount;
        private string _scanStateText;
        private MotionAxisViewItem _selectedItem;

        public MotionMonitorPageModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();

            _allItems = new List<MotionAxisViewItem>();
            _filteredItems = new List<MotionAxisViewItem>();
            _pageItems = new List<MotionAxisViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();

            _searchText = string.Empty;
            _pageIndex = 1;
            _pageSize = 24;
            _scanStateText = "未启动";
        }

        /// <summary>
        /// 当前页轴数据。
        /// </summary>
        public IList<MotionAxisViewItem> PageItems
        {
            get { return _pageItems; }
        }

        /// <summary>
        /// 控制卡筛选项。
        /// </summary>
        public IList<MotionCardFilterItem> CardFilters
        {
            get { return _cardFilters; }
        }

        /// <summary>
        /// 当前选中的控制卡。
        /// null 表示全部控制卡。
        /// </summary>
        public short? SelectedCardId
        {
            get { return _selectedCardId; }
        }

        /// <summary>
        /// 当前搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
        }

        /// <summary>
        /// 当前选中的轴。
        /// </summary>
        public MotionAxisViewItem SelectedItem
        {
            get { return _selectedItem; }
            private set { SetProperty(ref _selectedItem, value); }
        }

        /// <summary>
        /// 当前控制卡范围内轴总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        /// <summary>
        /// 当前控制卡范围内报警轴数量。
        /// </summary>
        public int AlarmCount
        {
            get { return _alarmCount; }
            private set { SetProperty(ref _alarmCount, value); }
        }

        /// <summary>
        /// 当前控制卡范围内运动中轴数量。
        /// </summary>
        public int MovingCount
        {
            get { return _movingCount; }
            private set { SetProperty(ref _movingCount, value); }
        }

        /// <summary>
        /// 当前控制卡范围内就绪轴数量。
        /// </summary>
        public int ReadyCount
        {
            get { return _readyCount; }
            private set { SetProperty(ref _readyCount, value); }
        }

        /// <summary>
        /// 搜索过滤后的总数。
        /// 分页控件使用这个值。
        /// </summary>
        public int FilteredCount
        {
            get { return _filteredItems.Count; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            private set { SetProperty(ref _pageIndex, value <= 0 ? 1 : value); }
        }

        public int PageSize
        {
            get { return _pageSize; }
            private set { SetProperty(ref _pageSize, value <= 0 ? 24 : value); }
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

        public string ScanStateText
        {
            get { return _scanStateText; }
            private set { SetProperty(ref _scanStateText, value ?? string.Empty); }
        }

        public string SelectedCardText
        {
            get
            {
                if (!_selectedCardId.HasValue)
                    return "全部控制卡";

                var card = _cardFilters.FirstOrDefault(x => x.CardId == _selectedCardId.Value);
                if (card == null)
                    return "控制卡 #" + _selectedCardId.Value;

                return card.DisplayText;
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
        /// 多轴总览与 DI/DO 监视页保持一致，使用低频刷新。
        /// </summary>
        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

        /// <summary>
        /// 更新搜索关键字。
        /// 页面内搜索不重新查库，只重建筛选与分页。
        /// </summary>
        public void SetSearchText(string searchText)
        {
            searchText = searchText ?? string.Empty;
            if (string.Equals(_searchText, searchText, StringComparison.Ordinal))
                return;

            _searchText = searchText;
            OnPropertyChanged(nameof(SearchText));

            PageIndex = 1;
            ApplyFilterAndPaging();
        }

        /// <summary>
        /// 设置控制卡筛选。
        /// </summary>
        public void SetSelectedCardId(short? selectedCardId)
        {
            if (_selectedCardId == selectedCardId)
                return;

            _selectedCardId = selectedCardId;
            OnPropertyChanged(nameof(SelectedCardId));
            OnPropertyChanged(nameof(SelectedCardText));

            PageIndex = 1;
            ApplyFilterAndPaging();
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
        /// 选中指定逻辑轴。
        /// </summary>
        public void SelectItem(short logicalAxis)
        {
            var selected = _pageItems.FirstOrDefault(x => x.LogicalAxis == logicalAxis);
            if (selected == null)
                return;

            SelectedItem = selected;
        }

        /// <summary>
        /// 重新加载轴静态结构与运行态快照。
        /// </summary>
        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousSelectedLogicalAxis = SelectedItem == null
                    ? (short?)null
                    : SelectedItem.LogicalAxis;

                var result = _runtimeQueryService.QueryAxisSnapshot();
                if (!result.Success)
                {
                    ClearAll();
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allItems = result.Items == null
                    ? new List<MotionAxisViewItem>()
                    : result.Items
                        .Select(ToViewItem)
                        .OrderBy(x => x.CardId)
                        .ThenBy(x => x.LogicalAxis)
                        .ToList();

                RefreshCardFilters();
                EnsureSelectedCardStillValid();
                ApplyFilterAndPaging();
                RestoreSelection(previousSelectedLogicalAxis);

                return Result.Ok("多轴总览加载成功");
            });
        }

        private void ClearAll()
        {
            _allItems = new List<MotionAxisViewItem>();
            _filteredItems = new List<MotionAxisViewItem>();
            _pageItems = new List<MotionAxisViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();

            SelectedItem = null;
            TotalCount = 0;
            AlarmCount = 0;
            MovingCount = 0;
            ReadyCount = 0;
            ScanStateText = "未启动";

            RaiseUiChanged();
        }

        /// <summary>
        /// 控制卡筛选项基于系统全部控制卡，而不是仅基于当前已有轴配置的控制卡。
        /// 这样新增控制卡后，即使暂时没有轴，也仍然可以被选择。
        /// </summary>
        private void RefreshCardFilters()
        {
            var cards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<MotionCardConfig>();

            _cardFilters = cards
                .Where(x => x != null)
                .OrderBy(x => x.InitOrder)
                .ThenBy(x => x.CardId)
                .Select(x => new MotionCardFilterItem
                {
                    CardId = x.CardId,
                    DisplayName = string.IsNullOrWhiteSpace(x.DisplayName) ? x.Name : x.DisplayName
                })
                .GroupBy(x => x.CardId)
                .Select(x => x.First())
                .ToList();

            OnPropertyChanged(nameof(CardFilters));
        }

        private void EnsureSelectedCardStillValid()
        {
            if (!_selectedCardId.HasValue)
                return;

            var exists = _cardFilters.Any(x => x.CardId == _selectedCardId.Value);
            if (exists)
                return;

            _selectedCardId = null;
            OnPropertyChanged(nameof(SelectedCardId));
            OnPropertyChanged(nameof(SelectedCardText));
        }

        private void ApplyFilterAndPaging()
        {
            var scopedItems = GetCardScopedItems();

            // 第二行统计按“当前控制卡范围”统计，不受搜索框影响。
            TotalCount = scopedItems.Count;
            AlarmCount = scopedItems.Count(x => x.IsAlarm);
            MovingCount = scopedItems.Count(x => x.IsMoving);
            ReadyCount = scopedItems.Count(x => x.IsReady);

            UpdateScanState();

            IEnumerable<MotionAxisViewItem> query = scopedItems;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var keyword = _searchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (x.DisplayTitle ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.Name ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.AxisCategoryText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CardText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.StateText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.EnableText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    x.LogicalAxis.ToString().Contains(keyword) ||
                    x.PhysicalAxis.ToString().Contains(keyword) ||
                    x.PhysicalCore.ToString().Contains(keyword));
            }

            _filteredItems = query
                .OrderBy(x => x.CardId)
                .ThenBy(x => x.LogicalAxis)
                .ToList();

            NormalizePage();
            RebuildPageItems();
        }

        private List<MotionAxisViewItem> GetCardScopedItems()
        {
            if (!_selectedCardId.HasValue)
                return _allItems.ToList();

            return _allItems
                .Where(x => x.CardId == _selectedCardId.Value)
                .ToList();
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
            var previousLogicalAxis = SelectedItem == null
                ? (short?)null
                : SelectedItem.LogicalAxis;

            var skip = (PageIndex - 1) * PageSize;
            _pageItems = _filteredItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();

            if (previousLogicalAxis.HasValue)
            {
                SelectedItem = _pageItems.FirstOrDefault(x => x.LogicalAxis == previousLogicalAxis.Value)
                    ?? (_pageItems.Count > 0 ? _pageItems[0] : null);
            }
            else
            {
                SelectedItem = _pageItems.Count > 0 ? _pageItems[0] : null;
            }

            RaiseUiChanged();
        }

        private void RestoreSelection(short? logicalAxis)
        {
            if (!logicalAxis.HasValue)
            {
                SelectedItem = _pageItems.Count > 0 ? _pageItems[0] : null;
                return;
            }

            SelectedItem = _pageItems.FirstOrDefault(x => x.LogicalAxis == logicalAxis.Value)
                ?? (_pageItems.Count > 0 ? _pageItems[0] : null);
        }

        private void UpdateScanState()
        {
            var runtime = RuntimeContext.Instance.MotionAxis;
            if (runtime.IsScanServiceRunning)
            {
                var timeText = runtime.LastScanTime.HasValue
                    ? runtime.LastScanTime.Value.ToString("HH:mm:ss")
                    : "--:--:--";

                ScanStateText = "运行中 / " + runtime.ScanIntervalMs + "ms / " + timeText;
            }
            else
            {
                ScanStateText = "已停止";
            }
        }

        private void RaiseUiChanged()
        {
            OnPropertyChanged(nameof(PageItems));
            OnPropertyChanged(nameof(FilteredCount));
            OnPropertyChanged(nameof(PageIndex));
            OnPropertyChanged(nameof(PageSize));
            OnPropertyChanged(nameof(PageCount));
            OnPropertyChanged(nameof(PageSummaryText));
            OnPropertyChanged(nameof(SelectedCardText));
        }

        private static MotionAxisViewItem ToViewItem(MotionAxisDisplayItem item)
        {
            return new MotionAxisViewItem
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
                UpdateTime = item == null ? (DateTime?)null : item.UpdateTime
            };
        }

        /// <summary>
        /// 多轴总览页面显示项。
        /// 供左侧卡片和右侧详情区共同使用。
        /// </summary>
        public sealed class MotionAxisViewItem
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

            public DateTime? UpdateTime { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                        return DisplayName;

                    return string.IsNullOrWhiteSpace(Name) ? "轴-" + LogicalAxis : Name;
                }
            }

            public string AxisCategoryText
            {
                get { return string.IsNullOrWhiteSpace(AxisCategory) ? "Other" : AxisCategory; }
            }

            public string CardText
            {
                get
                {
                    var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                    return "卡#" + CardId + "  " + name;
                }
            }

            public string PhysicalText
            {
                //get { return "核 " + PhysicalCore + " / 轴 " + PhysicalAxis; }
                get { return "轴 " + PhysicalAxis; }
            }

            public string EnableText
            {
                get { return IsEnabled ? "Enable" : "Disable"; }
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

            public bool IsReady
            {
                get { return IsDone && IsEnabled && !IsMoving && !IsAlarm; }
            }

            public string HomeText
            {
                get { return IsAtHome ? "AtHome" : "OffHome"; }
            }

            public string DoneText
            {
                get { return IsDone ? "Done" : "Busy"; }
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

            /// <summary>
            /// 位置误差 指令位置 - 编码器位置。
            /// </summary>
            public double PositionErrorMm
            {
                get { return CommandPositionMm - EncoderPositionMm; }
            }

            public string CommandPositionMmText
            {
                get { return CommandPositionMm.ToString("0.###") + " mm"; }
            }

            public string EncoderPositionMmText
            {
                get { return EncoderPositionMm.ToString("0.###") + " mm"; }
            }

            public string PositionErrorMmText
            {
                get { return PositionErrorMm.ToString("0.###") + " mm"; }
            }

            public string CommandPositionPulseText
            {
                get { return CommandPositionPulse.ToString("0.###"); }
            }

            public string EncoderPositionPulseText
            {
                get { return EncoderPositionPulse.ToString("0.###"); }
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
                    return UpdateTime.HasValue
                        ? UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "—";
                }
            }
        }
    }
}