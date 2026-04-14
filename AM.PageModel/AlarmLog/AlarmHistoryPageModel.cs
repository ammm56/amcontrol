using AM.DBService.Services.Dev;
using AM.Model.Common;
using AM.Model.Entity.Dev;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.AlarmLog
{
    /// <summary>
    /// 报警历史页面模型。
    /// 负责：
    /// 1. 报警历史分页查询；
    /// 2. 报警级别与未清除状态筛选；
    /// 3. 关键字搜索；
    /// 4. 统计卡片数据汇总；
    /// 5. 为 WinForms 报警历史页提供统一数据入口。
    /// </summary>
    public class AlarmHistoryPageModel : BindableBase
    {
        private const int DefaultPageSize = 50;

        private readonly DevAlarmRecordService _alarmRecordService;

        private List<DevAlarmRecordEntity> _records;
        private string _searchText;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _selectedFilter;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;
        private int _totalAlarmCount;
        private int _criticalCount;
        private int _unclearedCount;
        private int _clearedCount;

        public AlarmHistoryPageModel()
        {
            _alarmRecordService = new DevAlarmRecordService();
            _records = new List<DevAlarmRecordEntity>();
            _searchText = string.Empty;
            _selectedFilter = "All";
            _pageIndex = 1;
            _pageSize = DefaultPageSize;
        }

        /// <summary>
        /// 当前页报警历史记录。
        /// </summary>
        public IList<DevAlarmRecordEntity> Records
        {
            get { return _records; }
        }

        /// <summary>
        /// 搜索关键字。
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value ?? string.Empty); }
        }

        /// <summary>
        /// 起始日期。
        /// </summary>
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        /// <summary>
        /// 结束日期。
        /// </summary>
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        /// <summary>
        /// 当前筛选类型。
        /// 可选值：
        /// All / Critical / Alarm / Warning / Info / Uncleared
        /// </summary>
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set { SetProperty(ref _selectedFilter, string.IsNullOrWhiteSpace(value) ? "All" : value); }
        }

        /// <summary>
        /// 当前页码。
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            private set { SetProperty(ref _pageIndex, value <= 0 ? 1 : value); }
        }

        /// <summary>
        /// 当前分页大小。
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            private set { SetProperty(ref _pageSize, value <= 0 ? DefaultPageSize : value); }
        }

        /// <summary>
        /// 当前筛选条件下总数。
        /// 用于分页。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：报警总数。
        /// </summary>
        public int TotalAlarmCount
        {
            get { return _totalAlarmCount; }
            private set { SetProperty(ref _totalAlarmCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：严重报警数。
        /// </summary>
        public int CriticalCount
        {
            get { return _criticalCount; }
            private set { SetProperty(ref _criticalCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：未清除报警数。
        /// </summary>
        public int UnclearedCount
        {
            get { return _unclearedCount; }
            private set { SetProperty(ref _unclearedCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：已清除报警数。
        /// </summary>
        public int ClearedCount
        {
            get { return _clearedCount; }
            private set { SetProperty(ref _clearedCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 总页数。
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                {
                    return 1;
                }

                int pages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                return pages <= 0 ? 1 : pages;
            }
        }

        /// <summary>
        /// 重置到第一页。
        /// </summary>
        public void ResetToFirstPage()
        {
            PageIndex = 1;
        }

        /// <summary>
        /// 设置分页。
        /// </summary>
        public void SetPage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? PageSize : pageSize;
            EnsureValidPageIndex();
            OnPropertyChanged(nameof(TotalPages));
        }

        /// <summary>
        /// 设置全部报警筛选。
        /// </summary>
        public void SetAllFilter()
        {
            SelectedFilter = "All";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置严重报警筛选。
        /// </summary>
        public void SetCriticalFilter()
        {
            SelectedFilter = "Critical";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置报警筛选。
        /// </summary>
        public void SetAlarmFilter()
        {
            SelectedFilter = "Alarm";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置警告筛选。
        /// </summary>
        public void SetWarningFilter()
        {
            SelectedFilter = "Warning";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置信息筛选。
        /// </summary>
        public void SetInfoFilter()
        {
            SelectedFilter = "Info";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置未清除报警筛选。
        /// </summary>
        public void SetUnclearedFilter()
        {
            SelectedFilter = "Uncleared";
            ResetToFirstPage();
        }

        /// <summary>
        /// 加载报警历史。
        /// </summary>
        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    DateTime? startDate = StartDate;
                    DateTime? endDate = EndDate;

                    if (startDate.HasValue && endDate.HasValue && endDate.Value < startDate.Value)
                    {
                        DateTime temp = startDate.Value;
                        startDate = endDate;
                        endDate = temp;
                    }

                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        return LoadPagedFromDatabase(startDate, endDate);
                    }

                    return LoadWithLocalSearch(startDate, endDate);
                }
                catch (Exception ex)
                {
                    _records = new List<DevAlarmRecordEntity>();
                    TotalCount = 0;
                    TotalAlarmCount = 0;
                    CriticalCount = 0;
                    UnclearedCount = 0;
                    ClearedCount = 0;

                    OnPropertyChanged(nameof(Records));
                    OnPropertyChanged(nameof(TotalPages));

                    return Result.Fail(-1, "报警历史加载失败: " + ex.Message, ResultSource.UI);
                }
            });
        }

        /// <summary>
        /// 无搜索条件时，直接走数据库分页。
        /// </summary>
        private Result LoadPagedFromDatabase(DateTime? startDate, DateTime? endDate)
        {
            string levelFilter;
            bool? isCleared;
            ResolveFilter(out levelFilter, out isCleared);

            int requestedPage = PageIndex <= 0 ? 1 : PageIndex;

            int totalCount = _alarmRecordService.QueryTotalCount(levelFilter, isCleared, startDate, endDate);
            int totalAlarmCount = _alarmRecordService.QueryTotalCount(null, null, startDate, endDate);
            int criticalCount = _alarmRecordService.QueryTotalCount("Critical", null, startDate, endDate);
            int unclearedCount = _alarmRecordService.QueryTotalCount(null, false, startDate, endDate);
            int clearedCount = _alarmRecordService.QueryTotalCount(null, true, startDate, endDate);

            TotalCount = totalCount;
            TotalAlarmCount = totalAlarmCount;
            CriticalCount = criticalCount;
            UnclearedCount = unclearedCount;
            ClearedCount = clearedCount;

            EnsureValidPageIndex();
            requestedPage = PageIndex;

            Result<DevAlarmRecordEntity> result = _alarmRecordService.QueryPage(
                requestedPage,
                PageSize,
                levelFilter,
                isCleared,
                startDate,
                endDate);

            _records = result.Success && result.Items != null
                ? result.Items.ToList()
                : new List<DevAlarmRecordEntity>();

            OnPropertyChanged(nameof(Records));
            OnPropertyChanged(nameof(TotalPages));

            if (!result.Success)
            {
                return Result.Fail(result.Code, result.Message, ResultSource.UI);
            }

            return Result.Ok("报警历史加载成功", ResultSource.UI);
        }

        /// <summary>
        /// 有搜索条件时，先读取当前日期范围内全部记录，再本地过滤分页。
        /// </summary>
        private Result LoadWithLocalSearch(DateTime? startDate, DateTime? endDate)
        {
            List<DevAlarmRecordEntity> allRecords = QueryAllByDateRange(startDate, endDate);
            string keyword = (SearchText ?? string.Empty).Trim();

            List<DevAlarmRecordEntity> searchedRecords = allRecords
                .Where(x => MatchSearch(x, keyword))
                .ToList();

            TotalAlarmCount = searchedRecords.Count;
            CriticalCount = searchedRecords.Count(x => IsLevel(x, "Critical"));
            UnclearedCount = searchedRecords.Count(x => !x.IsCleared);
            ClearedCount = searchedRecords.Count(x => x.IsCleared);

            List<DevAlarmRecordEntity> filteredRecords = ApplySelectedFilter(searchedRecords);
            TotalCount = filteredRecords.Count;

            EnsureValidPageIndex();

            _records = filteredRecords
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            OnPropertyChanged(nameof(Records));
            OnPropertyChanged(nameof(TotalPages));

            return Result.Ok("报警历史加载成功", ResultSource.UI);
        }

        /// <summary>
        /// 查询指定日期范围内全部报警记录。
        /// </summary>
        private List<DevAlarmRecordEntity> QueryAllByDateRange(DateTime? startDate, DateTime? endDate)
        {
            int total = _alarmRecordService.QueryTotalCount(null, null, startDate, endDate);
            if (total <= 0)
            {
                return new List<DevAlarmRecordEntity>();
            }

            Result<DevAlarmRecordEntity> result = _alarmRecordService.QueryPage(
                1,
                total,
                null,
                null,
                startDate,
                endDate);

            if (!result.Success || result.Items == null)
            {
                return new List<DevAlarmRecordEntity>();
            }

            return result.Items.ToList();
        }

        /// <summary>
        /// 根据当前筛选类型解析级别与清除状态过滤条件。
        /// </summary>
        private void ResolveFilter(out string levelFilter, out bool? isCleared)
        {
            levelFilter = null;
            isCleared = null;

            switch (SelectedFilter)
            {
                case "Critical":
                case "Alarm":
                case "Warning":
                case "Info":
                    levelFilter = SelectedFilter;
                    break;
                case "Uncleared":
                    isCleared = false;
                    break;
            }
        }

        /// <summary>
        /// 对搜索结果应用当前筛选。
        /// </summary>
        private List<DevAlarmRecordEntity> ApplySelectedFilter(List<DevAlarmRecordEntity> source)
        {
            if (source == null || source.Count <= 0)
            {
                return new List<DevAlarmRecordEntity>();
            }

            switch (SelectedFilter)
            {
                case "Critical":
                case "Alarm":
                case "Warning":
                case "Info":
                    return source.Where(x => IsLevel(x, SelectedFilter)).ToList();
                case "Uncleared":
                    return source.Where(x => !x.IsCleared).ToList();
                default:
                    return source.ToList();
            }
        }

        /// <summary>
        /// 判断记录是否命中关键字。
        /// </summary>
        private static bool MatchSearch(DevAlarmRecordEntity item, string keyword)
        {
            if (item == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return true;
            }

            string text = string.Join("|", new[]
            {
                item.AlarmCode.ToString(),
                item.AlarmLevel ?? string.Empty,
                item.Message ?? string.Empty,
                item.Source ?? string.Empty,
                item.Description ?? string.Empty,
                item.Suggestion ?? string.Empty,
                item.CardId.HasValue ? item.CardId.Value.ToString() : string.Empty,
                item.RaisedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                item.ClearedTime.HasValue ? item.ClearedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty
            });

            return text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 判断报警级别文本是否匹配。
        /// </summary>
        private static bool IsLevel(DevAlarmRecordEntity item, string level)
        {
            if (item == null)
            {
                return false;
            }

            return string.Equals(item.AlarmLevel, level, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 确保当前页码合法。
        /// </summary>
        private void EnsureValidPageIndex()
        {
            int totalPages = TotalPages;

            if (totalPages <= 0)
            {
                PageIndex = 1;
                return;
            }

            if (PageIndex < 1)
            {
                PageIndex = 1;
            }

            if (PageIndex > totalPages)
            {
                PageIndex = totalPages;
            }
        }
    }
}