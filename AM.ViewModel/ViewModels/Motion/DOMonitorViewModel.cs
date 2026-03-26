using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Model.Motion;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// DO 监视页视图模型。
    /// </summary>
    public class DOMonitorViewModel : ObservableObject
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly List<MotionIoDisplayItem> _allItems;
        private readonly List<MotionIoDisplayItem> _filteredItems;

        private string _searchText;
        private MotionCardFilterItem _selectedCardFilter;
        private MotionIoDisplayItem _selectedItem;
        private string _statusText;
        private bool _isBusy;
        private bool _isRefreshingFilters;
        private int _totalCount;
        private int _activeCount;
        private int _pageIndex;
        private int _totalPages;
        private int _selectedPageSize;
        private string _scanStateText;

        public DOMonitorViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _allItems = new List<MotionIoDisplayItem>();
            _filteredItems = new List<MotionIoDisplayItem>();

            CardFilters = new ObservableCollection<MotionCardFilterItem>();
            Items = new ObservableCollection<MotionIoDisplayItem>();
            PageSizes = new ObservableCollection<int>();

            PageSizes.Add(6);
            PageSizes.Add(12);
            PageSizes.Add(24);
            PageSizes.Add(48);
            PageSizes.Add(72);
            PageSizes.Add(96);
            PageSizes.Add(192);

            _searchText = string.Empty;
            _statusText = "请等待 DO 运行态加载";
            _scanStateText = "未启动";
            _selectedPageSize = 48;
            _pageIndex = 1;
            _totalPages = 1;

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            PrevPageCommand = new RelayCommand(PrevPage, CanPrevPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);
        }

        public ObservableCollection<MotionCardFilterItem> CardFilters { get; private set; }

        public ObservableCollection<MotionIoDisplayItem> Items { get; private set; }

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

        public MotionIoDisplayItem SelectedItem
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

        public int ActiveCount
        {
            get { return _activeCount; }
            set { SetProperty(ref _activeCount, value); }
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
            get { return "第 " + PageIndex + " / " + TotalPages + " 页"; }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;

            try
            {
                var previousBit = SelectedItem == null ? (short?)null : SelectedItem.LogicalBit;
                var previousCardId = SelectedCardFilter == null ? (short?)null : SelectedCardFilter.CardId;
                var previousPageIndex = PageIndex;

                var result = await Task.Run(() => _runtimeQueryService.QueryIoSnapshot("DO"));
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

                if (previousBit.HasValue)
                {
                    SelectedItem = Items.FirstOrDefault(x => x.LogicalBit == previousBit.Value)
                        ?? (Items.Count > 0 ? Items[0] : null);
                }
                else
                {
                    SelectedItem = Items.Count > 0 ? Items[0] : null;
                }

                StatusText = string.Format(
                    "DO 已刷新，当前卡共 {0} 条，匹配 {1} 条，当前页 {2} 条",
                    TotalCount,
                    _filteredItems.Count,
                    Items.Count);
            }
            finally
            {
                _isBusy = false;
            }
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

        private List<MotionIoDisplayItem> GetCardScopedItems()
        {
            if (SelectedCardFilter == null)
            {
                return _allItems.ToList();
            }

            return _allItems
                .Where(x => x.CardId == SelectedCardFilter.CardId)
                .OrderBy(x => x.LogicalBit)
                .ToList();
        }

        private void UpdateSummary(IList<MotionIoDisplayItem> scopedItems)
        {
            var list = scopedItems ?? new List<MotionIoDisplayItem>();

            TotalCount = list.Count;
            ActiveCount = list.Count(x => x.CurrentValue);

            var runtime = RuntimeContext.Instance.MotionIo;
            ScanStateText = runtime.IsScanServiceRunning
                ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                : "已停止";
        }

        private void ApplyFilter()
        {
            var previousBit = SelectedItem == null ? (short?)null : SelectedItem.LogicalBit;
            var scopedItems = GetCardScopedItems();

            IEnumerable<MotionIoDisplayItem> query = scopedItems;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.DisplayName) && x.DisplayName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.SignalCategoryDisplayName) && x.SignalCategoryDisplayName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.LinkObjectName) && x.LinkObjectName.ToLowerInvariant().Contains(keyword)) ||
                    x.LogicalBit.ToString().Contains(keyword));
            }

            var filteredList = query.OrderBy(x => x.LogicalBit).ToList();

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

            if (previousBit.HasValue)
            {
                SelectedItem = Items.FirstOrDefault(x => x.LogicalBit == previousBit.Value)
                    ?? (Items.Count > 0 ? Items[0] : null);
            }
            else
            {
                SelectedItem = Items.Count > 0 ? Items[0] : null;
            }

            StatusText = string.Format(
                "当前卡共 {0} 条，匹配 {1} 条，当前页 {2} 条",
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