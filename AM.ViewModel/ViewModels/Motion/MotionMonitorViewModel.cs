using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Model.Motion;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 多轴总览页视图模型。
    /// 轴状态数据来自 RuntimeContext.MotionAxis 快照，订阅 SnapshotChanged 驱动 UI 刷新。
    /// 页面由 MainWindow 缓存复用，不在 Unloaded 中释放订阅。
    /// </summary>
    public class MotionMonitorViewModel : ObservableObject
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly List<MotionAxisDisplayItem> _allItems;
        private readonly List<MotionAxisDisplayItem> _filteredItems;
        private readonly SynchronizationContext _uiContext;

        private string _searchText;
        private MotionCardFilterItem _selectedCardFilter;
        private MotionAxisDisplayItem _selectedItem;
        private string _statusText;
        private bool _isBusy;
        private bool _isRefreshingFilters;
        private int _totalCount;
        private int _alarmCount;
        private int _movingCount;
        private int _pageIndex;
        private int _totalPages;
        private int _selectedPageSize;
        private string _scanStateText;

        public MotionMonitorViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _allItems = new List<MotionAxisDisplayItem>();
            _filteredItems = new List<MotionAxisDisplayItem>();
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();

            CardFilters = new ObservableCollection<MotionCardFilterItem>();
            Items = new ObservableCollection<MotionAxisDisplayItem>();
            PageSizes = new ObservableCollection<int>();

            PageSizes.Add(6);
            PageSizes.Add(12);
            PageSizes.Add(24);
            PageSizes.Add(48);

            _searchText = string.Empty;
            _statusText = "请等待轴运行态加载";
            _scanStateText = "未启动";
            _selectedPageSize = 24;
            _pageIndex = 1;
            _totalPages = 1;

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            PrevPageCommand = new RelayCommand(PrevPage, CanPrevPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);

            // 订阅轴状态快照变更事件，数据驱动 UI 刷新
            RuntimeContext.Instance.MotionAxis.SnapshotChanged += MotionAxis_SnapshotChanged;
        }

        public ObservableCollection<MotionCardFilterItem> CardFilters { get; private set; }

        public ObservableCollection<MotionAxisDisplayItem> Items { get; private set; }

        public ObservableCollection<int> PageSizes { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand PrevPageCommand { get; private set; }

        public IRelayCommand NextPageCommand { get; private set; }

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
                    if (!_isRefreshingFilters)
                    {
                        PageIndex = 1;
                        ApplyFilter();
                    }
                }
            }
        }

        public MotionAxisDisplayItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
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

        public string ScanStateText
        {
            get { return _scanStateText; }
            set { SetProperty(ref _scanStateText, value); }
        }

        public string PageSummaryText
        {
            get
            {
                return "页码 " + PageIndex + " / " + TotalPages;
            }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            try
            {
                var previousAxis = SelectedItem == null ? (short?)null : SelectedItem.LogicalAxis;
                var previousCardId = SelectedCardFilter == null ? (short?)null : SelectedCardFilter.CardId;
                var previousPageIndex = PageIndex;

                // 从运行时服务获取轴定义 + 当前运行态快照
                var result = await Task.Run(() => _runtimeQueryService.QueryAxisSnapshot());
                if (!result.Success)
                {
                    StatusText = result.Message;
                    return;
                }

                _allItems.Clear();
                _allItems.AddRange(result.Items);

                RefreshCardFilters(previousCardId);

                if (previousPageIndex > 0)
                {
                    PageIndex = previousPageIndex;
                }

                ApplyFilter();

                if (previousAxis.HasValue)
                {
                    SelectedItem = Items.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                        ?? (Items.Count > 0 ? Items[0] : null);
                }
                else
                {
                    SelectedItem = Items.Count > 0 ? Items[0] : null;
                }

                StatusText = string.Format(
                    "多轴已刷新，当前控制卡共 {0} 轴，匹配 {1} 轴，当前页显示 {2} 轴",
                    TotalCount,
                    _filteredItems.Count,
                    Items.Count);
            }
            finally
            {
                _isBusy = false;
            }
        }

        /// <summary>
        /// 轴运行态快照变更事件处理。
        /// 将运行态覆盖到内存中的轴列表，然后更新当前页 Items（事件驱动，不重新查询数据库）。
        /// </summary>
        private void MotionAxis_SnapshotChanged()
        {
            _uiContext.Post(_ => RefreshRuntimeDisplay(), null);
        }

        /// <summary>
        /// 将最新运行态覆盖到 _allItems，再重建当前页显示项。
        /// </summary>
        private void RefreshRuntimeDisplay()
        {
            if (_allItems.Count == 0)
            {
                return;
            }

            // 将最新快照覆盖到内存中的轴对象
            _runtimeQueryService.ApplyAxisRuntime(_allItems);

            // 更新摘要统计
            var scopedItems = GetCardScopedItems();
            UpdateSummary(scopedItems);

            // 保留当前选中项，重建当前页
            var previousAxis = SelectedItem == null ? (short?)null : SelectedItem.LogicalAxis;

            var pageItems = _filteredItems
                .Skip((PageIndex - 1) * SelectedPageSize)
                .Take(SelectedPageSize)
                .ToList();

            Items.Clear();
            foreach (var item in pageItems)
            {
                Items.Add(item);
            }

            if (previousAxis.HasValue)
            {
                SelectedItem = Items.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                    ?? (Items.Count > 0 ? Items[0] : null);
            }
            else
            {
                SelectedItem = Items.Count > 0 ? Items[0] : null;
            }

            OnPropertyChanged(nameof(PageSummaryText));
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

            var runtime = RuntimeContext.Instance.MotionAxis;
            ScanStateText = runtime.IsScanServiceRunning
                ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                : "已停止";
        }

        private void ApplyFilter()
        {
            var previousAxis = SelectedItem == null ? (short?)null : SelectedItem.LogicalAxis;
            var scopedItems = GetCardScopedItems();

            IEnumerable<MotionAxisDisplayItem> query = scopedItems;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.DisplayName) && x.DisplayName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.AxisCategory) && x.AxisCategory.ToLowerInvariant().Contains(keyword)) ||
                    x.LogicalAxis.ToString().Contains(keyword));
            }

            var filteredList = query.OrderBy(x => x.LogicalAxis).ToList();

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
                SelectedItem = Items.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                    ?? (Items.Count > 0 ? Items[0] : null);
            }
            else
            {
                SelectedItem = Items.Count > 0 ? Items[0] : null;
            }

            StatusText = string.Format(
                "当前控制卡共 {0} 轴，筛选命中 {1} 轴，当前页显示 {2} 轴",
                TotalCount,
                _filteredItems.Count,
                Items.Count);
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
