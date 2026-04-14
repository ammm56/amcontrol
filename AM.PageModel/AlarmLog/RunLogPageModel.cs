using AM.DBService.Services.Dev;
using AM.Model.Common;
using AM.Model.Model;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.AlarmLog
{
    /// <summary>
    /// 运行日志页面模型。
    /// 负责：
    /// 1. 读取日志文件列表；
    /// 2. 加载选中日志文件内容；
    /// 3. 按级别与关键字筛选；
    /// 4. 提供分页与统计数据。
    /// </summary>
    public class RunLogPageModel : BindableBase
    {
        private const int DefaultPageSize = 100;

        private readonly RunLogService _logService;

        private List<RunLogService.LogFileInfo> _logFiles;
        private List<LogModel> _entries;
        private IReadOnlyList<LogModel> _allEntries;

        private string _selectedLogFilePath;
        private string _searchText;
        private string _selectedLevel;
        private int _pageIndex;
        private int _pageSize;
        private int _totalCount;
        private int _totalLogCount;
        private int _errorCount;
        private int _warnCount;
        private int _infoCount;

        public RunLogPageModel()
        {
            _logService = new RunLogService();

            _logFiles = new List<RunLogService.LogFileInfo>();
            _entries = new List<LogModel>();
            _allEntries = new List<LogModel>();

            _selectedLogFilePath = string.Empty;
            _searchText = string.Empty;
            _selectedLevel = "All";
            _pageIndex = 1;
            _pageSize = DefaultPageSize;
        }

        /// <summary>
        /// 日志文件列表。
        /// </summary>
        public IList<RunLogService.LogFileInfo> LogFiles
        {
            get { return _logFiles; }
        }

        /// <summary>
        /// 当前页日志条目。
        /// </summary>
        public IList<LogModel> Entries
        {
            get { return _entries; }
        }

        /// <summary>
        /// 当前选中文件路径。
        /// </summary>
        public string SelectedLogFilePath
        {
            get { return _selectedLogFilePath; }
            private set { SetProperty(ref _selectedLogFilePath, value ?? string.Empty); }
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
        /// 当前级别筛选。
        /// 可选值：
        /// All / ERROR / WARN / INFO / DEBUG
        /// </summary>
        public string SelectedLevel
        {
            get { return _selectedLevel; }
            private set { SetProperty(ref _selectedLevel, string.IsNullOrWhiteSpace(value) ? "All" : value); }
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
        /// 当前筛选结果总数。
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：日志总数。
        /// 基于搜索结果，不受级别按钮二次筛选影响。
        /// </summary>
        public int TotalLogCount
        {
            get { return _totalLogCount; }
            private set { SetProperty(ref _totalLogCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：ERROR 数量。
        /// </summary>
        public int ErrorCount
        {
            get { return _errorCount; }
            private set { SetProperty(ref _errorCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：WARN 数量。
        /// </summary>
        public int WarnCount
        {
            get { return _warnCount; }
            private set { SetProperty(ref _warnCount, value < 0 ? 0 : value); }
        }

        /// <summary>
        /// 统计卡片：INFO 数量。
        /// </summary>
        public int InfoCount
        {
            get { return _infoCount; }
            private set { SetProperty(ref _infoCount, value < 0 ? 0 : value); }
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
        /// 刷新日志文件列表，并尽量保持当前选中文件。
        /// </summary>
        public async Task<Result> RefreshFilesAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<RunLogService.LogFileInfo> files = _logService.GetAvailableLogFiles();
                    _logFiles = files ?? new List<RunLogService.LogFileInfo>();
                    OnPropertyChanged(nameof(LogFiles));

                    string preferredPath = SelectedLogFilePath;
                    RunLogService.LogFileInfo selected = _logFiles.FirstOrDefault(x =>
                        string.Equals(x.FilePath, preferredPath, StringComparison.OrdinalIgnoreCase))
                        ?? _logFiles.FirstOrDefault();

                    SelectedLogFilePath = selected == null ? string.Empty : selected.FilePath;
                    ResetToFirstPage();

                    return Result.Ok("运行日志文件列表刷新成功", ResultSource.UI);
                }
                catch (Exception ex)
                {
                    _logFiles = new List<RunLogService.LogFileInfo>();
                    SelectedLogFilePath = string.Empty;
                    OnPropertyChanged(nameof(LogFiles));

                    return Result.Fail(-1, "运行日志文件列表刷新失败: " + ex.Message, ResultSource.UI);
                }
            });
        }

        /// <summary>
        /// 设置当前选中文件。
        /// </summary>
        public void SetSelectedLogFile(string filePath)
        {
            SelectedLogFilePath = filePath ?? string.Empty;
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置全部级别筛选。
        /// </summary>
        public void SetAllFilter()
        {
            SelectedLevel = "All";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置 ERROR 级别筛选。
        /// </summary>
        public void SetErrorFilter()
        {
            SelectedLevel = "ERROR";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置 WARN 级别筛选。
        /// </summary>
        public void SetWarnFilter()
        {
            SelectedLevel = "WARN";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置 INFO 级别筛选。
        /// </summary>
        public void SetInfoFilter()
        {
            SelectedLevel = "INFO";
            ResetToFirstPage();
        }

        /// <summary>
        /// 设置 DEBUG 级别筛选。
        /// </summary>
        public void SetDebugFilter()
        {
            SelectedLevel = "DEBUG";
            ResetToFirstPage();
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
        /// 加载当前选中文件内容并应用筛选分页。
        /// </summary>
        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SelectedLogFilePath))
                    {
                        _allEntries = new List<LogModel>();
                        _entries = new List<LogModel>();

                        TotalCount = 0;
                        TotalLogCount = 0;
                        ErrorCount = 0;
                        WarnCount = 0;
                        InfoCount = 0;

                        OnPropertyChanged(nameof(Entries));
                        OnPropertyChanged(nameof(TotalPages));

                        return Result.Ok("当前没有可加载的日志文件", ResultSource.UI);
                    }

                    Result<LogModel> readResult = _logService.ReadFile(SelectedLogFilePath);
                    _allEntries = readResult.Success && readResult.Items != null
                        ? readResult.Items.ToList()
                        : new List<LogModel>();

                    ApplyFiltersAndPaginate();

                    if (!readResult.Success)
                    {
                        return Result.Fail(readResult.Code, readResult.Message, ResultSource.UI);
                    }

                    return Result.Ok("运行日志加载成功", ResultSource.UI);
                }
                catch (Exception ex)
                {
                    _allEntries = new List<LogModel>();
                    _entries = new List<LogModel>();

                    TotalCount = 0;
                    TotalLogCount = 0;
                    ErrorCount = 0;
                    WarnCount = 0;
                    InfoCount = 0;

                    OnPropertyChanged(nameof(Entries));
                    OnPropertyChanged(nameof(TotalPages));

                    return Result.Fail(-1, "运行日志加载失败: " + ex.Message, ResultSource.UI);
                }
            });
        }

        private void ApplyFiltersAndPaginate()
        {
            IEnumerable<LogModel> searched = _allEntries ?? new List<LogModel>();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string keyword = SearchText.Trim();
                searched = searched.Where(x => MatchSearch(x, keyword));
            }

            List<LogModel> searchedList = searched.ToList();

            TotalLogCount = searchedList.Count;
            ErrorCount = searchedList.Count(x => IsLevel(x, "ERROR"));
            WarnCount = searchedList.Count(x => IsLevel(x, "WARN"));
            InfoCount = searchedList.Count(x => IsLevel(x, "INFO"));

            IEnumerable<LogModel> filtered = searchedList;
            if (!string.Equals(SelectedLevel, "All", StringComparison.OrdinalIgnoreCase))
            {
                string selectedLevel = SelectedLevel;
                filtered = filtered.Where(x => IsLevel(x, selectedLevel));
            }

            List<LogModel> filteredList = filtered.ToList();
            TotalCount = filteredList.Count;

            EnsureValidPageIndex();

            _entries = filteredList
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            OnPropertyChanged(nameof(Entries));
            OnPropertyChanged(nameof(TotalPages));
        }

        private static bool MatchSearch(LogModel item, string keyword)
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
                item.LineNumber.ToString(),
                item.Time.HasValue ? item.Time.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : string.Empty,
                item.Level ?? string.Empty,
                item.Logger ?? string.Empty,
                item.Message ?? string.Empty,
                item.RawLine ?? string.Empty
            });

            return text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsLevel(LogModel item, string level)
        {
            if (item == null)
            {
                return false;
            }

            return string.Equals(item.Level, level, StringComparison.OrdinalIgnoreCase);
        }

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