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
    /// 控制卡配置页面模型。
    /// </summary>
    public class MotionCardManagementPageModel : BindableBase
    {
        private readonly MotionCardCrudService _cardService;
        private List<MotionCardViewItem> _allCards;
        private List<MotionCardViewItem> _cards;
        private int _currentPage;
        private int _pageSize;

        public MotionCardManagementPageModel()
        {
            _cardService = new MotionCardCrudService();
            _allCards = new List<MotionCardViewItem>();
            _cards = new List<MotionCardViewItem>();
            _currentPage = 1;
            _pageSize = 9;
        }

        public IList<MotionCardViewItem> Cards
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
            get { return _allCards.Count; }
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

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                var result = _cardService.QueryAll();
                if (!result.Success)
                {
                    _allCards = new List<MotionCardViewItem>();
                    _cards = new List<MotionCardViewItem>();
                    RaisePagingChanged();
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _allCards = result.Items == null
                    ? new List<MotionCardViewItem>()
                    : result.Items
                        .OrderBy(x => x.InitOrder)
                        .ThenBy(x => x.SortOrder)
                        .ThenBy(x => x.CardId)
                        .Select(ToViewItem)
                        .ToList();

                NormalizePage();
                RebuildPageItems();

                return Result.Ok("控制卡列表加载成功");
            });
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

        public short GetDefaultCardId()
        {
            return _allCards.Count == 0
                ? (short)1
                : (short)(_allCards.Max(x => x.CardId) + 1);
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
            _cards = _allCards
                .Skip(skip)
                .Take(_pageSize)
                .ToList();

            RaisePagingChanged();
        }

        private void RaisePagingChanged()
        {
            OnPropertyChanged(nameof(Cards));
            OnPropertyChanged(nameof(CurrentPage));
            OnPropertyChanged(nameof(PageSize));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(PageCount));
            OnPropertyChanged(nameof(PageSummaryText));
        }

        private static MotionCardViewItem ToViewItem(MotionCardEntity entity)
        {
            return new MotionCardViewItem
            {
                Id = entity.Id,
                CardId = entity.CardId,
                CardType = entity.CardType,
                Name = entity.Name ?? string.Empty,
                DisplayName = string.IsNullOrWhiteSpace(entity.DisplayName) ? (entity.Name ?? string.Empty) : entity.DisplayName,
                DriverKey = entity.DriverKey ?? string.Empty,
                ModeParam = entity.ModeParam,
                OpenConfig = entity.OpenConfig ?? string.Empty,
                CoreNumber = entity.CoreNumber,
                AxisCountNumber = entity.AxisCountNumber,
                UseExtModule = entity.UseExtModule,
                InitOrder = entity.InitOrder,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                UpdateTime = entity.UpdateTime
            };
        }

        public sealed class MotionCardViewItem
        {
            public int Id { get; set; }

            public short CardId { get; set; }

            public int CardType { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string DriverKey { get; set; }

            public short ModeParam { get; set; }

            public string OpenConfig { get; set; }

            public int CoreNumber { get; set; }

            public short AxisCountNumber { get; set; }

            public bool UseExtModule { get; set; }

            public int InitOrder { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public DateTime UpdateTime { get; set; }

            public string CardTypeText
            {
                get
                {
                    switch (CardType)
                    {
                        case 10: return "GOOGO";
                        case 20: return "LEISAI";
                        case 90: return "VIRTUAL";
                        default: return "OTHER";
                    }
                }
            }

            public string StatusText
            {
                get { return IsEnabled ? "已启用" : "已禁用"; }
            }

            public string ModeParamText
            {
                get { return ModeParam.ToString(); }
            }

            public string CoreNumberText
            {
                get { return CoreNumber.ToString(); }
            }

            public string AxisCountText
            {
                get { return AxisCountNumber.ToString(); }
            }

            public string ExtModuleText
            {
                get { return UseExtModule ? "是" : "否"; }
            }

            public string UpdateTimeText
            {
                get { return UpdateTime == default(DateTime) ? "-" : UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            }

            public string OpenConfigText
            {
                get { return string.IsNullOrWhiteSpace(OpenConfig) ? "-" : OpenConfig; }
            }

            public string DriverKeyText
            {
                get { return string.IsNullOrWhiteSpace(DriverKey) ? "-" : DriverKey; }
            }

            public string DescriptionText
            {
                get { return string.IsNullOrWhiteSpace(Description) ? "-" : Description; }
            }

            public string RemarkText
            {
                get { return string.IsNullOrWhiteSpace(Remark) ? "-" : Remark; }
            }
        }
    }
}