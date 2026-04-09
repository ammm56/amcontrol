using AM.DBService.Services.Motion.Topology;
using AM.Model.Common;
using AM.Model.Entity.Motion.Topology;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.MotionConfig
{
    /// <summary>
    /// IO 映射配置页面模型。
    /// </summary>
    public class MotionIoMapManagementPageModel : BindableBase
    {
        private readonly MotionIoMapCrudService _ioMapService;
        private readonly MotionCardCrudService _cardService;

        private List<MotionIoMapViewItem> _allItems;
        private List<MotionIoMapViewItem> _items;
        private List<MotionCardEntity> _cards;
        private short? _selectedCardId;
        private string _selectedIoType;
        private int _currentPage;
        private int _pageSize;

        public MotionIoMapManagementPageModel()
        {
            _ioMapService = new MotionIoMapCrudService();
            _cardService = new MotionCardCrudService();

            _allItems = new List<MotionIoMapViewItem>();
            _items = new List<MotionIoMapViewItem>();
            _cards = new List<MotionCardEntity>();
            _selectedIoType = "All";
            _currentPage = 1;
            _pageSize = 9;
        }

        public IList<MotionIoMapViewItem> Items
        {
            get { return _items; }
        }

        public IList<MotionCardEntity> Cards
        {
            get { return _cards; }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
        }

        public int PageSize
        {
            get { return _pageSize; }
        }

        public int TotalCount
        {
            get { return _allItems.Count; }
        }

        public int PageCount
        {
            get
            {
                if (TotalCount <= 0 || _pageSize <= 0)
                    return 1;

                return (int)Math.Ceiling(TotalCount * 1.0 / _pageSize);
            }
        }

        public string PageSummaryText
        {
            get
            {
                if (TotalCount <= 0)
                    return "共 0 项";

                var start = (_currentPage - 1) * _pageSize + 1;
                var end = Math.Min(_currentPage * _pageSize, TotalCount);
                return "第 " + start + " - " + end + " 项，共 " + TotalCount + " 项";
            }
        }

        public short? SelectedCardId
        {
            get { return _selectedCardId; }
            set
            {
                if (_selectedCardId == value)
                    return;

                _selectedCardId = value;
                OnPropertyChanged(nameof(SelectedCardId));
                OnPropertyChanged(nameof(SelectedCardText));
            }
        }

        public string SelectedIoType
        {
            get { return _selectedIoType; }
            set
            {
                var normalized = NormalizeIoTypeFilter(value);
                if (string.Equals(_selectedIoType, normalized, StringComparison.OrdinalIgnoreCase))
                    return;

                _selectedIoType = normalized;
                OnPropertyChanged(nameof(SelectedIoType));
            }
        }

        public string SelectedCardText
        {
            get
            {
                if (!_selectedCardId.HasValue)
                    return "当前：全部控制卡";

                var card = _cards.FirstOrDefault(x => x.CardId == _selectedCardId.Value);
                if (card == null)
                    return "当前：控制卡 #" + _selectedCardId.Value;

                var displayName = string.IsNullOrWhiteSpace(card.DisplayName)
                    ? (string.IsNullOrWhiteSpace(card.Name) ? "控制卡 #" + card.CardId : card.Name)
                    : card.DisplayName;

                return "当前：" + displayName + " (#" + card.CardId + ")";
            }
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                var cardResult = _cardService.QueryAll();
                if (!cardResult.Success)
                {
                    _cards = new List<MotionCardEntity>();
                    _allItems = new List<MotionIoMapViewItem>();
                    _items = new List<MotionIoMapViewItem>();

                    OnPropertyChanged(nameof(Cards));
                    RaisePagingChanged();
                    OnPropertyChanged(nameof(SelectedCardText));

                    return Result.Fail(cardResult.Code, cardResult.Message, cardResult.Source);
                }

                _cards = cardResult.Items == null
                    ? new List<MotionCardEntity>()
                    : cardResult.Items
                        .OrderBy(x => x.InitOrder)
                        .ThenBy(x => x.SortOrder)
                        .ThenBy(x => x.CardId)
                        .ToList();

                var ioMapResult = _ioMapService.QueryAll();
                if (!ioMapResult.Success)
                {
                    _allItems = new List<MotionIoMapViewItem>();
                    _items = new List<MotionIoMapViewItem>();

                    OnPropertyChanged(nameof(Cards));
                    RaisePagingChanged();
                    OnPropertyChanged(nameof(SelectedCardText));

                    return Result.Fail(ioMapResult.Code, ioMapResult.Message, ioMapResult.Source);
                }

                IEnumerable<MotionIoMapEntity> query = ioMapResult.Items ?? new List<MotionIoMapEntity>();

                if (_selectedCardId.HasValue)
                {
                    query = query.Where(x => x.CardId == _selectedCardId.Value);
                }

                if (!string.Equals(_selectedIoType, "All", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => string.Equals(x.IoType, _selectedIoType, StringComparison.OrdinalIgnoreCase));
                }

                _allItems = query
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.IoType)
                    .ThenBy(x => x.LogicalBit)
                    .Select(ToViewItem)
                    .ToList();

                NormalizePage();
                RebuildPageItems();

                OnPropertyChanged(nameof(Cards));
                OnPropertyChanged(nameof(SelectedCardText));

                return Result.Ok("IO 映射列表加载成功");
            });
        }

        public async Task<Result> SaveAsync(MotionIoMapEntity entity)
        {
            return await Task.Run(() => _ioMapService.Save(entity));
        }

        public async Task<Result> DeleteAsync(short logicalBit, string ioType)
        {
            return await Task.Run(() => _ioMapService.DeleteByLogicalBit(logicalBit, ioType));
        }

        public MotionIoMapEntity CreateDefaultEntity()
        {
            var nowIoType = string.Equals(_selectedIoType, "DI", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(_selectedIoType, "DO", StringComparison.OrdinalIgnoreCase)
                ? _selectedIoType
                : "DI";

            var candidates = _allItems
                .Where(x => string.Equals(x.IoType, nowIoType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var nextLogicalBit = candidates.Count == 0
                ? (short)1
                : (short)(candidates.Max(x => x.LogicalBit) + 1);

            var defaultCardId = SelectedCardId
                ?? (_cards.Count > 0 ? _cards[0].CardId : (short)0);

            return new MotionIoMapEntity
            {
                CardId = defaultCardId,
                IoType = nowIoType,
                LogicalBit = nextLogicalBit,
                Name = nowIoType + "-" + nextLogicalBit,
                Core = 1,
                IsExtModule = false,
                HardwareBit = 0,
                IsEnabled = true,
                SortOrder = _allItems.Count + 1,
                Remark = string.Empty
            };
        }

        public void ChangePage(int currentPage, int pageSize)
        {
            if (pageSize <= 0)
                pageSize = 9;

            _pageSize = pageSize;
            _currentPage = currentPage <= 0 ? 1 : currentPage;

            NormalizePage();
            RebuildPageItems();
        }

        private void NormalizePage()
        {
            if (_currentPage <= 0)
                _currentPage = 1;

            var pageCount = PageCount;
            if (_currentPage > pageCount)
                _currentPage = pageCount;
        }

        private void RebuildPageItems()
        {
            var skip = (_currentPage - 1) * _pageSize;
            _items = _allItems
                .Skip(skip)
                .Take(_pageSize)
                .ToList();

            RaisePagingChanged();
        }

        private void RaisePagingChanged()
        {
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(PageSize));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(PageCount));
            OnPropertyChanged(nameof(PageSummaryText));
        }

        private MotionIoMapViewItem ToViewItem(MotionIoMapEntity entity)
        {
            var card = _cards.FirstOrDefault(x => x.CardId == entity.CardId);
            var cardName = card == null
                ? "控制卡 #" + entity.CardId
                : string.IsNullOrWhiteSpace(card.DisplayName)
                    ? (string.IsNullOrWhiteSpace(card.Name) ? "控制卡 #" + card.CardId : card.Name)
                    : card.DisplayName;

            return new MotionIoMapViewItem
            {
                Id = entity.Id,
                CardId = entity.CardId,
                IoType = entity.IoType ?? "DI",
                LogicalBit = entity.LogicalBit,
                Name = entity.Name ?? string.Empty,
                Core = entity.Core,
                IsExtModule = entity.IsExtModule,
                HardwareBit = entity.HardwareBit,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Remark = entity.Remark ?? string.Empty,
                CardDisplayName = cardName
            };
        }

        private static string NormalizeIoTypeFilter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "All";

            if (string.Equals(value, "DI", StringComparison.OrdinalIgnoreCase))
                return "DI";

            if (string.Equals(value, "DO", StringComparison.OrdinalIgnoreCase))
                return "DO";

            return "All";
        }

        public sealed class MotionIoMapViewItem
        {
            public int Id { get; set; }

            public short CardId { get; set; }

            public string IoType { get; set; }

            public short LogicalBit { get; set; }

            public string Name { get; set; }

            public short Core { get; set; }

            public bool IsExtModule { get; set; }

            public short HardwareBit { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Remark { get; set; }

            public string CardDisplayName { get; set; }

            public string IoTypeText
            {
                get { return string.IsNullOrWhiteSpace(IoType) ? "-" : IoType.ToUpperInvariant(); }
            }

            public string LogicalBitText
            {
                get { return LogicalBit.ToString(); }
            }

            public string CardText
            {
                get { return CardDisplayName; }
            }

            public string CoreText
            {
                get { return "Core " + Core; }
            }

            public string HardwareBitText
            {
                get { return HardwareBit.ToString(); }
            }

            public string ExtModuleText
            {
                get { return IsExtModule ? "扩展" : "板载"; }
            }

            public string StatusText
            {
                get { return IsEnabled ? "已启用" : "已禁用"; }
            }

            public string IoTypeDisplayText
            {
                get
                {
                    if (string.Equals(IoType, "DO", StringComparison.OrdinalIgnoreCase))
                        return "数字输出";
                    return "数字输入";
                }
            }

            public MotionIoMapEntity ToEntity()
            {
                return new MotionIoMapEntity
                {
                    Id = Id,
                    CardId = CardId,
                    IoType = IoType,
                    LogicalBit = LogicalBit,
                    Name = Name,
                    Core = Core,
                    IsExtModule = IsExtModule,
                    HardwareBit = HardwareBit,
                    IsEnabled = IsEnabled,
                    SortOrder = SortOrder,
                    Remark = Remark
                };
            }
        }
    }
}