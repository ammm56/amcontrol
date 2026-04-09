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
    /// 轴拓扑配置页面模型。
    /// </summary>
    public class MotionAxisManagementPageModel : BindableBase
    {
        private readonly MotionAxisCrudService _axisService;
        private readonly MotionCardCrudService _cardService;

        private List<MotionAxisViewItem> _allItems;
        private List<MotionAxisViewItem> _items;
        private List<MotionCardEntity> _cards;
        private short? _selectedCardId;
        private int _currentPage;
        private int _pageSize;

        public MotionAxisManagementPageModel()
        {
            _axisService = new MotionAxisCrudService();
            _cardService = new MotionCardCrudService();

            _allItems = new List<MotionAxisViewItem>();
            _items = new List<MotionAxisViewItem>();
            _cards = new List<MotionCardEntity>();
            _currentPage = 1;
            _pageSize = 9;
        }

        public IList<MotionAxisViewItem> Items
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
                    _allItems = new List<MotionAxisViewItem>();
                    _items = new List<MotionAxisViewItem>();

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

                var axisResult = _axisService.QueryAll();
                if (!axisResult.Success)
                {
                    _allItems = new List<MotionAxisViewItem>();
                    _items = new List<MotionAxisViewItem>();

                    OnPropertyChanged(nameof(Cards));
                    RaisePagingChanged();
                    OnPropertyChanged(nameof(SelectedCardText));

                    return Result.Fail(axisResult.Code, axisResult.Message, axisResult.Source);
                }

                IEnumerable<MotionAxisEntity> query = axisResult.Items ?? new List<MotionAxisEntity>();
                if (_selectedCardId.HasValue)
                {
                    query = query.Where(x => x.CardId == _selectedCardId.Value);
                }

                _allItems = query
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.LogicalAxis)
                    .Select(ToViewItem)
                    .ToList();

                NormalizePage();
                RebuildPageItems();

                OnPropertyChanged(nameof(Cards));
                OnPropertyChanged(nameof(SelectedCardText));

                return Result.Ok("轴拓扑列表加载成功");
            });
        }

        public async Task<Result> SaveAsync(MotionAxisEntity entity)
        {
            return await Task.Run(() => _axisService.Save(entity));
        }

        public async Task<Result> DeleteAsync(short logicalAxis)
        {
            return await Task.Run(() => _axisService.DeleteByLogicalAxis(logicalAxis));
        }

        public MotionAxisEntity CreateDefaultEntity()
        {
            var nextLogicalAxis = _allItems.Count == 0
                ? (short)101
                : (short)(_allItems.Max(x => x.LogicalAxis) + 1);

            var defaultCardId = SelectedCardId
                ?? (_cards.Count > 0 ? _cards[0].CardId : (short)0);

            var now = DateTime.Now;

            return new MotionAxisEntity
            {
                CardId = defaultCardId,
                AxisId = 0,
                LogicalAxis = nextLogicalAxis,
                Name = "Axis-" + nextLogicalAxis,
                DisplayName = "轴-" + nextLogicalAxis,
                AxisCategory = "Linear",
                PhysicalCore = 1,
                PhysicalAxis = 0,
                IsEnabled = true,
                SortOrder = _allItems.Count + 1,
                Description = "新建轴拓扑配置",
                Remark = string.Empty,
                CreateTime = now,
                UpdateTime = now
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

        private MotionAxisViewItem ToViewItem(MotionAxisEntity entity)
        {
            var card = _cards.FirstOrDefault(x => x.CardId == entity.CardId);
            var cardName = card == null
                ? "控制卡 #" + entity.CardId
                : string.IsNullOrWhiteSpace(card.DisplayName)
                    ? (string.IsNullOrWhiteSpace(card.Name) ? "控制卡 #" + card.CardId : card.Name)
                    : card.DisplayName;

            return new MotionAxisViewItem
            {
                Id = entity.Id,
                CardId = entity.CardId,
                AxisId = entity.AxisId,
                LogicalAxis = entity.LogicalAxis,
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                AxisCategory = string.IsNullOrWhiteSpace(entity.AxisCategory) ? "Other" : entity.AxisCategory,
                PhysicalCore = entity.PhysicalCore,
                PhysicalAxis = entity.PhysicalAxis,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                UpdateTime = entity.UpdateTime,
                CardDisplayName = cardName
            };
        }

        public sealed class MotionAxisViewItem
        {
            public int Id { get; set; }

            public short CardId { get; set; }

            public short AxisId { get; set; }

            public short LogicalAxis { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string AxisCategory { get; set; }

            public short PhysicalCore { get; set; }

            public short PhysicalAxis { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public DateTime UpdateTime { get; set; }

            public string CardDisplayName { get; set; }

            public string LogicalAxisText
            {
                get { return LogicalAxis.ToString(); }
            }

            public string CardText
            {
                get { return CardDisplayName; }
            }

            public string StatusText
            {
                get { return IsEnabled ? "已启用" : "已禁用"; }
            }

            public string UpdateTimeText
            {
                get { return UpdateTime == default(DateTime) ? "-" : UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            }

            public string DescriptionText
            {
                get { return string.IsNullOrWhiteSpace(Description) ? "-" : Description; }
            }

            public string RemarkText
            {
                get { return string.IsNullOrWhiteSpace(Remark) ? "-" : Remark; }
            }

            public string AxisCategoryText
            {
                get
                {
                    switch ((AxisCategory ?? string.Empty).Trim())
                    {
                        case "Linear": return "直线轴";
                        case "Rotary": return "旋转轴";
                        case "GantryMaster": return "龙门主轴";
                        case "GantrySlave": return "龙门从轴";
                        case "Virtual": return "虚拟轴";
                        default: return "其他";
                    }
                }
            }

            public string AxisCategoryIconSvg
            {
                get
                {
                    switch ((AxisCategory ?? string.Empty).Trim())
                    {
                        case "Linear": return "DragOutlined";
                        case "Rotary": return "RedoOutlined";
                        case "GantryMaster": return "GatewayOutlined";
                        case "GantrySlave": return "ApartmentOutlined";
                        case "Virtual": return "ClusterOutlined";
                        default: return "ControlOutlined";
                    }
                }
            }

            public MotionAxisEntity ToEntity()
            {
                return new MotionAxisEntity
                {
                    Id = Id,
                    CardId = CardId,
                    AxisId = AxisId,
                    LogicalAxis = LogicalAxis,
                    Name = Name,
                    DisplayName = DisplayName,
                    AxisCategory = AxisCategory,
                    PhysicalCore = PhysicalCore,
                    PhysicalAxis = PhysicalAxis,
                    IsEnabled = IsEnabled,
                    SortOrder = SortOrder,
                    Description = Description,
                    Remark = Remark,
                    UpdateTime = UpdateTime
                };
            }
        }
    }
}