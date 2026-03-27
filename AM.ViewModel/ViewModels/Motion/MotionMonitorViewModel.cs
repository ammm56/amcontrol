using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Model.Motion;
using AM.Model.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 多轴总览页视图模型。
    /// 以控制卡为筛选单位，显示当前页轴卡片和右侧详情。
    /// 静态结构通过查询服务加载，运行态通过 RuntimeContext 快照增量刷新。
    /// </summary>
    public class MotionMonitorViewModel : ObservableObject
    {
        private static readonly TimeSpan RuntimeUiRefreshMinInterval = TimeSpan.FromMilliseconds(300);

        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly List<MotionAxisDisplayItem> _allItems;
        private readonly List<MotionAxisDisplayItem> _filteredItems;
        private readonly SynchronizationContext _uiContext;

        private MotionCardFilterItem _selectedCardFilter;
        private MotionAxisDisplayItem _selectedAxisItem;
        private string _searchText;
        private string _statusText;
        private bool _isBusy;
        private bool _isRefreshingFilters;
        private bool _isDisposed;
        private int _totalCount;
        private int _alarmCount;
        private int _movingCount;
        private int _readyCount;
        private int _pageIndex;
        private int _totalPages;
        private int _selectedPageSize;
        private DateTime _lastRuntimeUiRefreshTimeUtc;

        public MotionMonitorViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _allItems = new List<MotionAxisDisplayItem>();
            _filteredItems = new List<MotionAxisDisplayItem>();
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();

            CardFilters = new ObservableCollection<MotionCardFilterItem>();
            Items = new ObservableCollection<MotionAxisDisplayItem>();
            PageSizes = new ObservableCollection<int>();

            PageSizes.Add(8);
            PageSizes.Add(12);
            PageSizes.Add(18);
            PageSizes.Add(24);
            PageSizes.Add(36);
            PageSizes.Add(48);

            _searchText = string.Empty;
            _statusText = "请等待多轴总览加载";
            _selectedPageSize = 12;
            _pageIndex = 1;
            _totalPages = 1;
            _lastRuntimeUiRefreshTimeUtc = DateTime.MinValue;

            RefreshCommand = new AsyncRelayCommand(RefreshAsync, CanRefresh);
            PrevPageCommand = new RelayCommand(PrevPage, CanPrevPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);

            RuntimeContext.Instance.MotionAxis.SnapshotChanged += MotionAxis_SnapshotChanged;
        }

        public ObservableCollection<MotionCardFilterItem> CardFilters { get; private set; }

        public ObservableCollection<MotionAxisDisplayItem> Items { get; private set; }

        public ObservableCollection<int> PageSizes { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand PrevPageCommand { get; private set; }

        public IRelayCommand NextPageCommand { get; private set; }

        public MotionCardFilterItem SelectedCardFilter
        {
            get { return _selectedCardFilter; }
            set
            {
                if (IsSameCardFilter(_selectedCardFilter, value))
                {
                    return;
                }

                if (SetProperty(ref _selectedCardFilter, value))
                {
                    OnPropertyChanged(nameof(SelectedCardHeader));

                    if (!_isRefreshingFilters)
                    {
                        PageIndex = 1;
                        ApplyFilter();
                    }
                }
            }
        }

        public MotionAxisDisplayItem SelectedAxisItem
        {
            get { return _selectedAxisItem; }
            set
            {
                if (SetProperty(ref _selectedAxisItem, value))
                {
                    RaiseSelectedAxisDisplayChanged();
                }
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    PageIndex = 1;
                    ApplyFilter();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RefreshCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { SetProperty(ref _totalCount, value); }
        }

        public int AlarmCount
        {
            get { return _alarmCount; }
            set { SetProperty(ref _alarmCount, value); }
        }

        public int MovingCount
        {
            get { return _movingCount; }
            set { SetProperty(ref _movingCount, value); }
        }

        public int ReadyCount
        {
            get { return _readyCount; }
            set { SetProperty(ref _readyCount, value); }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (SetProperty(ref _pageIndex, value))
                {
                    UpdatePagingCommandState();
                }
            }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                if (SetProperty(ref _totalPages, value))
                {
                    UpdatePagingCommandState();
                }
            }
        }

        public int SelectedPageSize
        {
            get { return _selectedPageSize; }
            set
            {
                if (SetProperty(ref _selectedPageSize, value))
                {
                    PageIndex = 1;
                    ApplyFilter();
                }
            }
        }

        public string PageSummaryText
        {
            get
            {
                return "页码 " + PageIndex + " / " + TotalPages;
            }
        }

        public string ScanStateText
        {
            get
            {
                var runtime = RuntimeContext.Instance.MotionAxis;
                return runtime.IsScanServiceRunning
                    ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                    : "已停止";
            }
        }

        public string SelectedCardHeader
        {
            get
            {
                if (SelectedCardFilter == null)
                {
                    return "当前控制卡：未选择";
                }

                return "当前控制卡：" + SelectedCardFilter.DisplayText;
            }
        }

        public string SelectedAxisHeader
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "当前轴：未选择";
                }

                return "当前轴：L#" + SelectedAxisItem.LogicalAxis + "  " + SelectedAxisItem.DisplayTitle;
            }
        }

        public string SelectedAxisCardText
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "—";
                }

                return "卡#" + SelectedAxisItem.CardId + "  "
                    + (string.IsNullOrWhiteSpace(SelectedAxisItem.CardDisplayName)
                        ? "未命名控制卡"
                        : SelectedAxisItem.CardDisplayName);
            }
        }

        public string SelectedAxisCategoryText
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "—";
                }

                switch (SelectedAxisItem.AxisCategory)
                {
                    case "Linear": return "直线轴";
                    case "Rotary": return "旋转轴";
                    case "GantryMaster": return "龙门主轴";
                    case "GantrySlave": return "龙门从轴";
                    case "Virtual": return "虚拟轴";
                    default:
                        return string.IsNullOrWhiteSpace(SelectedAxisItem.AxisCategory)
                            ? "—"
                            : SelectedAxisItem.AxisCategory;
                }
            }
        }

        public string SelectedAxisPhysicalText
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "—";
                }

                return "核 " + SelectedAxisItem.PhysicalCore + " / 轴 " + SelectedAxisItem.PhysicalAxis;
            }
        }

        public string SelectedAxisStateText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.StateText;
                }

                return SelectedAxisItem == null ? "—" : SelectedAxisItem.StateText;
            }
        }

        public string SelectedAxisEnabledText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.IsEnabled ? "已使能" : "未使能";
                }

                return SelectedAxisItem == null ? "—" : (SelectedAxisItem.IsEnabled ? "已使能" : "未使能");
            }
        }

        public string SelectedAxisHomeText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.IsAtHome ? "在原点" : "未回原点";
                }

                return SelectedAxisItem == null ? "—" : (SelectedAxisItem.IsAtHome ? "在原点" : "未回原点");
            }
        }

        public string SelectedAxisDoneText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.IsDone ? "已到位" : "未到位";
                }

                return SelectedAxisItem == null ? "—" : (SelectedAxisItem.IsDone ? "已到位" : "未到位");
            }
        }

        public string SelectedAxisLimitText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.LimitStateText;
                }

                return SelectedAxisItem == null ? "—" : SelectedAxisItem.LimitStateText;
            }
        }

        public string SelectedAxisSignalSummaryText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.SignalSummaryText;
                }

                return SelectedAxisItem == null ? "—" : SelectedAxisItem.SignalSummaryText;
            }
        }

        public string SelectedAxisCommandPositionText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.CommandPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
                }

                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.CommandPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisEncoderPositionText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.EncoderPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
                }

                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.EncoderPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisCommandPulseText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.CommandPositionPulse.ToString("0.###", CultureInfo.InvariantCulture);
                }

                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.CommandPositionPulse.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisEncoderPulseText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot != null)
                {
                    return snapshot.EncoderPositionPulse.ToString("0.###", CultureInfo.InvariantCulture);
                }

                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.EncoderPositionPulse.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisPositionErrorText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot == null)
                {
                    return "—";
                }

                return snapshot.PositionErrorMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisDefaultVelocityText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisJogVelocityText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.JogVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisRuntimeUpdateTimeText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null
                    ? "—"
                    : snapshot.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// 手动刷新：重新加载轴静态结构，并叠加当前运行态快照。
        /// </summary>
        public async Task RefreshAsync()
        {
            if (IsBusy || _isDisposed)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var previousCardId = SelectedCardFilter == null ? (short?)null : SelectedCardFilter.CardId;
                var previousAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;
                var previousPageIndex = PageIndex;

                var result = await Task.Run(() => _runtimeQueryService.QueryAxisSnapshot());
                if (!result.Success)
                {
                    StatusText = result.Message;
                    return;
                }

                _allItems.Clear();
                _allItems.AddRange(result.Items
                    .OrderBy(x => x.CardId)
                    .ThenBy(x => x.LogicalAxis));

                RefreshCardFilters(previousCardId);

                if (previousPageIndex > 0)
                {
                    PageIndex = previousPageIndex;
                }

                ApplyFilter(previousAxis, false);
                _lastRuntimeUiRefreshTimeUtc = DateTime.UtcNow;

                StatusText = string.Format(
                    "多轴总览已刷新，当前卡共 {0} 轴，筛选命中 {1} 轴，当前页显示 {2} 轴",
                    TotalCount,
                    _filteredItems.Count,
                    Items.Count);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 注意：当前页面由 MainWindow 缓存复用。
        /// 正常导航切换时不要调用本方法，否则会断开运行态订阅。
        /// 本方法仅适用于页面实例真正销毁时。
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            RuntimeContext.Instance.MotionAxis.SnapshotChanged -= MotionAxis_SnapshotChanged;
            _isDisposed = true;
        }

        private void MotionAxis_SnapshotChanged()
        {
            if (_isDisposed || IsBusy)
            {
                return;
            }

            var nowUtc = DateTime.UtcNow;
            if (nowUtc - _lastRuntimeUiRefreshTimeUtc < RuntimeUiRefreshMinInterval)
            {
                return;
            }

            _lastRuntimeUiRefreshTimeUtc = nowUtc;

            _uiContext.Post(_ =>
            {
                if (_isDisposed || IsBusy)
                {
                    return;
                }

                RefreshRuntimeSnapshot();
            }, null);
        }

        private void RefreshRuntimeSnapshot()
        {
            var previousAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;

            _runtimeQueryService.ApplyAxisRuntime(_allItems);
            ApplyFilter(previousAxis, false);

            StatusText = string.Format(
                "多轴总览实时更新，当前卡共 {0} 轴，筛选命中 {1} 轴，当前页显示 {2} 轴",
                TotalCount,
                _filteredItems.Count,
                Items.Count);

            OnPropertyChanged(nameof(ScanStateText));
        }

        private MotionAxisRuntimeSnapshot GetSelectedAxisRuntime()
        {
            if (SelectedAxisItem == null)
            {
                return null;
            }

            MotionAxisRuntimeSnapshot snapshot;
            return RuntimeContext.Instance.MotionAxis.TryGetAxisSnapshot(SelectedAxisItem.LogicalAxis, out snapshot)
                ? snapshot
                : null;
        }

        private void RefreshCardFilters(short? previousCardId)
        {
            CardFilters.Clear();

            foreach (var item in _allItems
                .GroupBy(x => x.CardId)
                .Select(g => new MotionCardFilterItem
                {
                    CardId = g.Key,
                    DisplayName = g.Select(x => x.CardDisplayName).FirstOrDefault()
                })
                .OrderBy(x => x.CardId))
            {
                CardFilters.Add(item);
            }

            MotionCardFilterItem selected = null;

            if (previousCardId.HasValue)
            {
                selected = CardFilters.FirstOrDefault(x => x.CardId == previousCardId.Value);
            }

            if (selected == null && CardFilters.Count > 0)
            {
                selected = CardFilters[0];
            }

            _isRefreshingFilters = true;
            try
            {
                SelectedCardFilter = selected;
            }
            finally
            {
                _isRefreshingFilters = false;
            }

            OnPropertyChanged(nameof(SelectedCardHeader));
        }

        private List<MotionAxisDisplayItem> GetCardScopedItems()
        {
            if (SelectedCardFilter == null)
            {
                return _allItems.ToList();
            }

            return _allItems
                .Where(x => x.CardId == SelectedCardFilter.CardId)
                .OrderBy(x => x.LogicalAxis)
                .ToList();
        }

        private void UpdateSummary(IList<MotionAxisDisplayItem> scopedItems)
        {
            var list = scopedItems ?? new List<MotionAxisDisplayItem>();

            TotalCount = list.Count;
            AlarmCount = list.Count(x => x.IsAlarm);
            MovingCount = list.Count(x => x.IsMoving);
            ReadyCount = list.Count(x => x.IsEnabled && x.IsDone && !x.IsAlarm && !x.IsMoving);

            OnPropertyChanged(nameof(ScanStateText));
        }

        private void ApplyFilter()
        {
            var previousAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;
            ApplyFilter(previousAxis, true);
        }

        private void ApplyFilter(short? previousAxis, bool updateStatusText)
        {
            var scopedItems = GetCardScopedItems();
            IEnumerable<MotionAxisDisplayItem> query = scopedItems;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.DisplayName) && x.DisplayName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.AxisCategory) && x.AxisCategory.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.CardDisplayName) && x.CardDisplayName.ToLowerInvariant().Contains(keyword)) ||
                    x.LogicalAxis.ToString().Contains(keyword));
            }

            var filteredList = query
                .OrderBy(x => x.LogicalAxis)
                .ToList();

            _filteredItems.Clear();
            _filteredItems.AddRange(filteredList);

            TotalPages = _filteredItems.Count == 0
                ? 1
                : (_filteredItems.Count + SelectedPageSize - 1) / SelectedPageSize;

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            if (PageIndex <= 0)
            {
                PageIndex = 1;
            }

            var pageItems = _filteredItems
                .Skip((PageIndex - 1) * SelectedPageSize)
                .Take(SelectedPageSize)
                .ToList();

            Items.Clear();
            foreach (var item in pageItems)
            {
                Items.Add(item);
            }

            UpdateSummary(scopedItems);
            OnPropertyChanged(nameof(PageSummaryText));

            if (previousAxis.HasValue)
            {
                SelectedAxisItem = Items.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                    ?? (Items.Count > 0 ? Items[0] : null);
            }
            else
            {
                SelectedAxisItem = Items.Count > 0 ? Items[0] : null;
            }

            RaiseSelectedAxisDisplayChanged();

            if (updateStatusText)
            {
                StatusText = string.Format(
                    "当前控制卡共 {0} 轴，筛选命中 {1} 轴，当前页显示 {2} 轴",
                    TotalCount,
                    _filteredItems.Count,
                    Items.Count);
            }
        }

        private void RaiseSelectedAxisDisplayChanged()
        {
            OnPropertyChanged(nameof(SelectedAxisHeader));
            OnPropertyChanged(nameof(SelectedAxisCardText));
            OnPropertyChanged(nameof(SelectedAxisCategoryText));
            OnPropertyChanged(nameof(SelectedAxisPhysicalText));
            OnPropertyChanged(nameof(SelectedAxisStateText));
            OnPropertyChanged(nameof(SelectedAxisEnabledText));
            OnPropertyChanged(nameof(SelectedAxisHomeText));
            OnPropertyChanged(nameof(SelectedAxisDoneText));
            OnPropertyChanged(nameof(SelectedAxisLimitText));
            OnPropertyChanged(nameof(SelectedAxisSignalSummaryText));
            OnPropertyChanged(nameof(SelectedAxisCommandPositionText));
            OnPropertyChanged(nameof(SelectedAxisEncoderPositionText));
            OnPropertyChanged(nameof(SelectedAxisCommandPulseText));
            OnPropertyChanged(nameof(SelectedAxisEncoderPulseText));
            OnPropertyChanged(nameof(SelectedAxisPositionErrorText));
            OnPropertyChanged(nameof(SelectedAxisDefaultVelocityText));
            OnPropertyChanged(nameof(SelectedAxisJogVelocityText));
            OnPropertyChanged(nameof(SelectedAxisRuntimeUpdateTimeText));
            OnPropertyChanged(nameof(ScanStateText));
        }

        private void PrevPage()
        {
            if (PageIndex <= 1)
            {
                return;
            }

            PageIndex--;
            ApplyFilter();
        }

        private bool CanPrevPage()
        {
            return PageIndex > 1;
        }

        private void NextPage()
        {
            if (PageIndex >= TotalPages)
            {
                return;
            }

            PageIndex++;
            ApplyFilter();
        }

        private bool CanNextPage()
        {
            return PageIndex < TotalPages;
        }

        private bool CanRefresh()
        {
            return !IsBusy && !_isDisposed;
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(PageSummaryText));
        }

        private static bool IsSameCardFilter(MotionCardFilterItem left, MotionCardFilterItem right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            return left.CardId == right.CardId;
        }
    }
}