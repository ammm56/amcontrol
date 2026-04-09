using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Motion;
using AM.Model.MotionCard;
using AM.PageModel.Common;
using AM.PageModel.Motion.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Motion
{
    /// <summary>
    /// WinForms DI 监视页页面模型。
    /// 负责：
    /// 1. 查询 DI 运行态快照；
    /// 2. 按控制卡与关键字筛选；
    /// 3. 计算分页、统计信息；
    /// 4. 维护当前选中项。
    /// </summary>
    public class DIMotionPageModel : BindableBase
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;

        private List<DIMotionIoViewItem> _allItems;
        private List<DIMotionIoViewItem> _filteredItems;
        private List<DIMotionIoViewItem> _pageItems;
        private List<MotionCardFilterItem> _cardFilters;

        private short? _selectedCardId;
        private string _searchText;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;
        private int _activeCount;
        private string _scanStateText;
        private DIMotionIoViewItem _selectedItem;

        public DIMotionPageModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();

            _allItems = new List<DIMotionIoViewItem>();
            _filteredItems = new List<DIMotionIoViewItem>();
            _pageItems = new List<DIMotionIoViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();

            _searchText = string.Empty;
            _pageIndex = 1;
            _pageSize = 48;
            _scanStateText = "未启动";
        }

        /// <summary>
        /// 当前页数据。
        /// </summary>
        public IList<DIMotionIoViewItem> PageItems
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
        /// 选中的控制卡。
        /// null 表示全部控制卡。
        /// </summary>
        public short? SelectedCardId
        {
            get { return _selectedCardId; }
        }

        /// <summary>
        /// 搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
        }

        /// <summary>
        /// 当前选中的 DI 点。
        /// </summary>
        public DIMotionIoViewItem SelectedItem
        {
            get { return _selectedItem; }
            private set { SetProperty(ref _selectedItem, value); }
        }

        /// <summary>
        /// 当前控制卡范围内的输入总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        /// <summary>
        /// 当前控制卡范围内激活数。
        /// </summary>
        public int ActiveCount
        {
            get { return _activeCount; }
            private set { SetProperty(ref _activeCount, value); }
        }

        /// <summary>
        /// 当前筛选后的总数。
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
            private set { SetProperty(ref _pageSize, value <= 0 ? 48 : value); }
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
        /// 选中指定逻辑位。
        /// </summary>
        public void SelectItem(short logicalBit)
        {
            var selected = _pageItems.FirstOrDefault(x => x.LogicalBit == logicalBit);
            if (selected == null)
                return;

            SelectedItem = selected;
        }

        private async Task<Result> ReloadAsync()
        {
            return await Task.Run(() =>
            {
                var previousSelectedLogicalBit = SelectedItem == null
                    ? (short?)null
                    : SelectedItem.LogicalBit;

                var result = _runtimeQueryService.QueryIoSnapshot("DI");
                if (!result.Success)
                {
                    ClearAll();
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allItems = result.Items == null
                    ? new List<DIMotionIoViewItem>()
                    : result.Items
                        .Select(ToViewItem)
                        .OrderBy(x => x.CardId)
                        .ThenBy(x => x.LogicalBit)
                        .ToList();

                RefreshCardFilters();
                EnsureSelectedCardStillValid();
                ApplyFilterAndPaging();
                RestoreSelection(previousSelectedLogicalBit);

                return Result.Ok("DI 运行态加载成功");
            });
        }

        private void ClearAll()
        {
            _allItems = new List<DIMotionIoViewItem>();
            _filteredItems = new List<DIMotionIoViewItem>();
            _pageItems = new List<DIMotionIoViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();
            SelectedItem = null;
            TotalCount = 0;
            ActiveCount = 0;
            ScanStateText = "未启动";

            RaiseUiChanged();
        }

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

            TotalCount = scopedItems.Count;
            ActiveCount = scopedItems.Count(x => x.CurrentValue);

            UpdateScanState();

            IEnumerable<DIMotionIoViewItem> query = scopedItems;

            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                var keyword = _searchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (x.DisplayTitle ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.Name ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.TypeText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.ModuleText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.CardText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.LinkObjectDisplayText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (x.ValueText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    x.LogicalBit.ToString().Contains(keyword) ||
                    x.HardwareBit.ToString().Contains(keyword));
            }

            _filteredItems = query
                .OrderBy(x => x.CardId)
                .ThenBy(x => x.LogicalBit)
                .ToList();

            NormalizePage();
            RebuildPageItems();
        }

        private List<DIMotionIoViewItem> GetCardScopedItems()
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
            var previousLogicalBit = SelectedItem == null
                ? (short?)null
                : SelectedItem.LogicalBit;

            var skip = (PageIndex - 1) * PageSize;
            _pageItems = _filteredItems
                .Skip(skip)
                .Take(PageSize)
                .ToList();

            if (previousLogicalBit.HasValue)
            {
                SelectedItem = _pageItems.FirstOrDefault(x => x.LogicalBit == previousLogicalBit.Value)
                    ?? (_pageItems.Count > 0 ? _pageItems[0] : null);
            }
            else
            {
                SelectedItem = _pageItems.Count > 0 ? _pageItems[0] : null;
            }

            RaiseUiChanged();
        }

        private void RestoreSelection(short? logicalBit)
        {
            if (!logicalBit.HasValue)
            {
                SelectedItem = _pageItems.Count > 0 ? _pageItems[0] : null;
                return;
            }

            SelectedItem = _pageItems.FirstOrDefault(x => x.LogicalBit == logicalBit.Value)
                ?? (_pageItems.Count > 0 ? _pageItems[0] : null);
        }

        private void UpdateScanState()
        {
            var runtime = RuntimeContext.Instance.MotionIo;
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

        private static DIMotionIoViewItem ToViewItem(MotionIoDisplayItem item)
        {
            return new DIMotionIoViewItem
            {
                IoType = item == null ? "DI" : item.IoType,
                LogicalBit = item == null ? (short)0 : item.LogicalBit,
                Name = item == null ? string.Empty : item.Name,
                DisplayName = item == null ? string.Empty : item.DisplayName,
                SignalCategoryDisplayName = item == null ? string.Empty : item.SignalCategoryDisplayName,
                CardId = item == null ? (short)0 : item.CardId,
                CardDisplayName = item == null ? string.Empty : item.CardDisplayName,
                Core = item == null ? (short)0 : item.Core,
                HardwareBit = item == null ? (short)0 : item.HardwareBit,
                IsExtModule = item != null && item.IsExtModule,
                CurrentValue = item != null && item.CurrentValue,
                LastUpdateTime = item == null ? (DateTime?)null : item.LastUpdateTime,
                Description = item == null ? string.Empty : item.Description,
                Remark = item == null ? string.Empty : item.Remark,
                LinkObjectName = item == null ? string.Empty : item.LinkObjectName
            };
        }

        
    }
}