using AM.DBService.Services.Dev;
using AM.Model.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Alarm
{
    /// <summary>
    /// 运行日志视图模型。
    /// 从 NLog 日志文件读取内容，在内存中完成筛选与分页。
    /// </summary>
    public class RunLogViewModel : ObservableObject
    {
        private readonly RunLogService _logService;

        private RunLogService.LogFileInfo _selectedLogFile;
        private string _selectedLevel;
        private string _searchText;
        private LogModel _selectedEntry;
        private int _pageIndex;
        private int _totalPages;
        private int _totalCount;
        private int _selectedPageSize;
        private string _jumpPageText;
        private bool _isLoading;

        private IReadOnlyList<LogModel> _allEntries = new List<LogModel>();

        public RunLogViewModel()
        {
            _logService = new RunLogService();

            LogFiles = new ObservableCollection<RunLogService.LogFileInfo>();
            Entries = new ObservableCollection<LogModel>();
            Levels = new ObservableCollection<string>();
            PageSizes = new ObservableCollection<int>();

            Levels.Add("全部");
            Levels.Add("INFO");
            Levels.Add("WARN");
            Levels.Add("ERROR");
            Levels.Add("DEBUG");
            Levels.Add("TRACE");
            Levels.Add("FATAL");

            PageSizes.Add(50);
            PageSizes.Add(100);
            PageSizes.Add(200);
            PageSizes.Add(500);

            _selectedLevel = "全部";
            _searchText = string.Empty;
            _pageIndex = 1;
            _totalPages = 1;
            _selectedPageSize = 100;
            _jumpPageText = "1";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            PrevPageCommand = new RelayCommand(PrevPage, CanPrevPage);
            NextPageCommand = new RelayCommand(NextPage, CanNextPage);
            GoPageCommand = new RelayCommand(GoPage);
        }

        public ObservableCollection<RunLogService.LogFileInfo> LogFiles { get; private set; }
        public ObservableCollection<LogModel> Entries { get; private set; }
        public ObservableCollection<string> Levels { get; private set; }
        public ObservableCollection<int> PageSizes { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }
        public IRelayCommand PrevPageCommand { get; private set; }
        public IRelayCommand NextPageCommand { get; private set; }
        public IRelayCommand GoPageCommand { get; private set; }

        public RunLogService.LogFileInfo SelectedLogFile
        {
            get { return _selectedLogFile; }
            set
            {
                if (SetProperty(ref _selectedLogFile, value) && value != null)
                {
                    PageIndex = 1;
                    _ = LoadFileAsyncSafe();
                }
            }
        }

        public string SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                if (SetProperty(ref _selectedLevel, value))
                {
                    PageIndex = 1;
                    ApplyFiltersAndPaginate();
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
                    ApplyFiltersAndPaginate();
                }
            }
        }

        public LogModel SelectedEntry
        {
            get { return _selectedEntry; }
            set { SetProperty(ref _selectedEntry, value); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            private set
            {
                if (SetProperty(ref _pageIndex, value))
                {
                    JumpPageText = value.ToString();
                    UpdatePagingCommandState();
                }
            }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            private set
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
                    ApplyFiltersAndPaginate();
                }
            }
        }

        public string JumpPageText
        {
            get { return _jumpPageText; }
            set { SetProperty(ref _jumpPageText, value); }
        }

        /// <summary>初始化：加载日志文件列表，并默认选中最新文件。</summary>
        public async Task InitAsync()
        {
            var files = await Task.Run(() => _logService.GetAvailableLogFiles());

            LogFiles.Clear();
            foreach (var f in files)
            {
                LogFiles.Add(f);
            }

            if (LogFiles.Count > 0 && SelectedLogFile == null)
            {
                SelectedLogFile = LogFiles[0];
            }
        }

        private async Task LoadFileAsyncSafe()
        {
            try
            {
                await LoadFileAsync();
            }
            catch (Exception ex)
            {
                Trace.TraceError("RunLogViewModel.LoadFileAsync failed: " + ex);
            }
        }

        private async Task LoadFileAsync()
        {
            if (_selectedLogFile == null)
            {
                return;
            }

            _isLoading = true;
            UpdatePagingCommandState();

            try
            {
                var result = await Task.Run(() => _logService.ReadFile(_selectedLogFile.FilePath));
                _allEntries = result.Success && result.Items != null
                    ? result.Items
                    : new List<LogModel>();
            }
            finally
            {
                _isLoading = false;
            }

            ApplyFiltersAndPaginate();
        }

        private void ApplyFiltersAndPaginate()
        {
            IEnumerable<LogModel> filtered = _allEntries;

            if (!string.IsNullOrEmpty(SelectedLevel) && SelectedLevel != "全部")
            {
                var level = SelectedLevel;
                filtered = filtered.Where(e =>
                    string.Equals(e.Level, level, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim();
                filtered = filtered.Where(e =>
                    (e.Message != null && e.Message.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (e.Logger != null && e.Logger.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            var list = filtered.ToList();
            TotalCount = list.Count;
            TotalPages = list.Count <= 0 ? 1 : (int)Math.Ceiling(list.Count / (double)SelectedPageSize);

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            var skip = (PageIndex - 1) * SelectedPageSize;
            var page = list.Skip(skip).Take(SelectedPageSize).ToList();

            Entries.Clear();
            foreach (var item in page)
            {
                Entries.Add(item);
            }

            SelectedEntry = Entries.Count > 0 ? Entries[0] : null;
            UpdatePagingCommandState();
        }

        private async Task RefreshAsync()
        {
            await InitAsync();
        }

        private void PrevPage()
        {
            if (!CanPrevPage()) return;
            PageIndex--;
            ApplyFiltersAndPaginate();
        }

        private bool CanPrevPage()
        {
            return !_isLoading && PageIndex > 1;
        }

        private void NextPage()
        {
            if (!CanNextPage()) return;
            PageIndex++;
            ApplyFiltersAndPaginate();
        }

        private bool CanNextPage()
        {
            return !_isLoading && PageIndex < TotalPages;
        }

        private void GoPage()
        {
            int target;
            if (!int.TryParse(JumpPageText, out target)) return;
            if (target < 1) target = 1;
            if (target > TotalPages) target = TotalPages;
            PageIndex = target;
            ApplyFiltersAndPaginate();
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand?.NotifyCanExecuteChanged();
            NextPageCommand?.NotifyCanExecuteChanged();
        }
    }
}