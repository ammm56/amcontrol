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
    /// WinForms DO 监视页页面模型。
    /// 负责：
    /// 1. 查询 DO 运行态快照；
    /// 2. 按控制卡与关键字筛选；
    /// 3. 计算分页、统计信息；
    /// 4. 维护当前选中项。
    /// </summary>
    public class DOMotionPageModel : BindableBase
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;

        private List<DOMotionIoViewItem> _allItems;
        private List<DOMotionIoViewItem> _filteredItems;
        private List<DOMotionIoViewItem> _pageItems;
        private List<MotionCardFilterItem> _cardFilters;

        private short? _selectedCardId;
        private string _searchText;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;
        private int _activeCount;
        private string _scanStateText;
        private DOMotionIoViewItem _selectedItem;

        public DOMotionPageModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();

            _allItems = new List<DOMotionIoViewItem>();
            _filteredItems = new List<DOMotionIoViewItem>();
            _pageItems = new List<DOMotionIoViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();

            _searchText = string.Empty;
            _pageIndex = 1;
            _pageSize = 48;
            _scanStateText = "未启动";
        }

        public IList<DOMotionIoViewItem> PageItems
        {
            get { return _pageItems; }
        }

        public IList<MotionCardFilterItem> CardFilters
        {
            get { return _cardFilters; }
        }

        public short? SelectedCardId
        {
            get { return _selectedCardId; }
        }

        public string SearchText
        {
            get { return _searchText; }
        }

        public DOMotionIoViewItem SelectedItem
        {
            get { return _selectedItem; }
            private set { SetProperty(ref _selectedItem, value); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        public int ActiveCount
        {
            get { return _activeCount; }
            private set { SetProperty(ref _activeCount, value); }
        }

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

        public async Task<Result> LoadAsync()
        {
            return await ReloadAsync();
        }

        public async Task<Result> RefreshAsync()
        {
            return await ReloadAsync();
        }

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

        public void ChangePage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? PageSize : pageSize;

            NormalizePage();
            RebuildPageItems();
        }

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

                var result = _runtimeQueryService.QueryIoSnapshot("DO");
                if (!result.Success)
                {
                    ClearAll();
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allItems = result.Items == null
                    ? new List<DOMotionIoViewItem>()
                    : result.Items
                        .Select(ToViewItem)
                        .OrderBy(x => x.CardId)
                        .ThenBy(x => x.LogicalBit)
                        .ToList();

                RefreshCardFilters();
                EnsureSelectedCardStillValid();
                ApplyFilterAndPaging();
                RestoreSelection(previousSelectedLogicalBit);

                return Result.Ok("DO 运行态加载成功");
            });
        }

        private void ClearAll()
        {
            _allItems = new List<DOMotionIoViewItem>();
            _filteredItems = new List<DOMotionIoViewItem>();
            _pageItems = new List<DOMotionIoViewItem>();
            _cardFilters = new List<MotionCardFilterItem>();
            SelectedItem = null;
            TotalCount = 0;
            ActiveCount = 0;
            ScanStateText = "未启动";

            RaiseUiChanged();
        }

        /// <summary>
        /// 控制卡筛选项基于系统全部控制卡，而不是仅基于当前有 DO 点位的控制卡。
        /// 这样新增控制卡后，即使暂时没有 DO 点位，也仍然可以被选择。
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

            TotalCount = scopedItems.Count;
            ActiveCount = scopedItems.Count(x => x.CurrentValue);

            UpdateScanState();

            IEnumerable<DOMotionIoViewItem> query = scopedItems;

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
                    (x.OutputModeText ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
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

        private List<DOMotionIoViewItem> GetCardScopedItems()
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

        private static DOMotionIoViewItem ToViewItem(MotionIoDisplayItem item)
        {
            return new DOMotionIoViewItem
            {
                IoType = item == null ? "DO" : item.IoType,
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
                LinkObjectName = item == null ? string.Empty : item.LinkObjectName,
                OutputMode = item == null ? string.Empty : item.OutputMode
            };
        }

        /// <summary>
        /// DO 监视页显示项。
        /// </summary>
        public sealed class DOMotionIoViewItem
        {
            public string IoType { get; set; }
            public short LogicalBit { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string SignalCategoryDisplayName { get; set; }
            public short CardId { get; set; }
            public string CardDisplayName { get; set; }
            public short Core { get; set; }
            public short HardwareBit { get; set; }
            public bool IsExtModule { get; set; }
            public bool CurrentValue { get; set; }
            public DateTime? LastUpdateTime { get; set; }
            public string Description { get; set; }
            public string Remark { get; set; }
            public string LinkObjectName { get; set; }
            public string OutputMode { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                        return DisplayName;

                    return string.IsNullOrWhiteSpace(Name) ? "未命名输出点" : Name;
                }
            }

            public string ValueText
            {
                get { return CurrentValue ? "ON" : "OFF"; }
            }

            public string TypeText
            {
                get
                {
                    return string.IsNullOrWhiteSpace(SignalCategoryDisplayName)
                        ? "数字输出"
                        : SignalCategoryDisplayName;
                }
            }

            public string ModuleText
            {
                get { return IsExtModule ? "扩展" : "板载"; }
            }

            public string CardText
            {
                get
                {
                    var name = string.IsNullOrWhiteSpace(CardDisplayName) ? "未命名控制卡" : CardDisplayName;
                    return "卡#" + CardId + "  " + name;
                }
            }

            public string CoreText
            {
                get { return "Core " + Core; }
            }

            public string HardwareBitText
            {
                get { return HardwareBit.ToString(); }
            }

            public string HardwareAddressText
            {
                get { return CoreText + " / Bit " + HardwareBit + " / " + ModuleText; }
            }

            public string LastUpdateTimeText
            {
                get
                {
                    return LastUpdateTime.HasValue
                        ? LastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "—";
                }
            }

            public string DescriptionText
            {
                get { return string.IsNullOrWhiteSpace(Description) ? "—" : Description; }
            }

            public string RemarkText
            {
                get { return string.IsNullOrWhiteSpace(Remark) ? "—" : Remark; }
            }

            public string LinkObjectDisplayText
            {
                get { return string.IsNullOrWhiteSpace(LinkObjectName) ? "—" : LinkObjectName; }
            }

            public string OutputModeText
            {
                get { return string.IsNullOrWhiteSpace(OutputMode) ? "Keep" : OutputMode; }
            }
        }
    }
}