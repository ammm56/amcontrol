using AM.DBService.Services.Dev;
using AM.Model.Entity.Dev;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Alarm
{
    /// <summary>
    /// 报警历史视图模型。
    /// 分页读取 dev_alarm_record 表，支持级别筛选和时间范围筛选。
    /// </summary>
    public class AlarmHistoryViewModel : ObservableObject
    {
        private readonly DevAlarmRecordService _alarmRecordService;

        private string _selectedFilter;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private DevAlarmRecordEntity _selectedAlarm;
        private int _filteredCount;
        private int _totalAllCount;
        private int _unclearedCount;
        private int _pageIndex;
        private int _totalPages;
        private bool _isLoading;
        private int _selectedPageSize;
        private string _jumpPageText;

        public AlarmHistoryViewModel()
        {
            _alarmRecordService = new DevAlarmRecordService();

            Alarms = new ObservableCollection<DevAlarmRecordEntity>();
            Filters = new ObservableCollection<string>();
            PageSizes = new ObservableCollection<int>();

            Filters.Add("全部");
            Filters.Add("Critical");
            Filters.Add("Error");
            Filters.Add("Warning");
            Filters.Add("Info");
            Filters.Add("未清除");

            PageSizes.Add(20);
            PageSizes.Add(50);
            PageSizes.Add(100);
            PageSizes.Add(200);

            _selectedFilter = "全部";
            _pageIndex = 1;
            _totalPages = 1;
            _selectedPageSize = 50;
            _jumpPageText = "1";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            QueryCommand = new AsyncRelayCommand(QueryAsync);
            PrevPageCommand = new AsyncRelayCommand(PrevPageAsync, CanPrevPage);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync, CanNextPage);
            GoPageCommand = new AsyncRelayCommand(GoPageAsync);
        }

        public ObservableCollection<DevAlarmRecordEntity> Alarms { get; private set; }
        public ObservableCollection<string> Filters { get; private set; }
        public ObservableCollection<int> PageSizes { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }
        public IAsyncRelayCommand QueryCommand { get; private set; }
        public IAsyncRelayCommand PrevPageCommand { get; private set; }
        public IAsyncRelayCommand NextPageCommand { get; private set; }
        public IAsyncRelayCommand GoPageCommand { get; private set; }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    PageIndex = 1;
                    _ = LoadAsync();
                }
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    PageIndex = 1;
                    _ = LoadAsync();
                }
            }
        }

        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (SetProperty(ref _selectedFilter, value))
                {
                    PageIndex = 1;
                    _ = LoadAsync();
                }
            }
        }

        public DevAlarmRecordEntity SelectedAlarm
        {
            get { return _selectedAlarm; }
            set { SetProperty(ref _selectedAlarm, value); }
        }

        public int FilteredCount
        {
            get { return _filteredCount; }
            private set { SetProperty(ref _filteredCount, value); }
        }

        public int TotalAllCount
        {
            get { return _totalAllCount; }
            private set { SetProperty(ref _totalAllCount, value); }
        }

        public int UnclearedCount
        {
            get { return _unclearedCount; }
            private set { SetProperty(ref _unclearedCount, value); }
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
                    _ = LoadAsync();
                }
            }
        }

        public string JumpPageText
        {
            get { return _jumpPageText; }
            set { SetProperty(ref _jumpPageText, value); }
        }

        public async Task LoadAsync()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            UpdatePagingCommandState();

            try
            {
                string levelFilter = null;
                bool? isCleared = null;

                switch (SelectedFilter)
                {
                    case "Critical":
                    case "Error":
                    case "Warning":
                    case "Info":
                        levelFilter = SelectedFilter;
                        break;
                    case "未清除":
                        isCleared = false;
                        break;
                }

                var requestedPage = PageIndex <= 0 ? 1 : PageIndex;
                int filteredCount = 0;
                int totalAllCount = 0;
                int unclearedCount = 0;

                var result = await Task.Run(() =>
                {
                    filteredCount = _alarmRecordService.QueryTotalCount(levelFilter, isCleared, StartDate, EndDate);
                    totalAllCount = _alarmRecordService.QueryTotalCount();
                    unclearedCount = _alarmRecordService.QueryTotalCount(null, false);
                    return _alarmRecordService.QueryPage(requestedPage, SelectedPageSize, levelFilter, isCleared, StartDate, EndDate);
                });

                FilteredCount = filteredCount;
                TotalAllCount = totalAllCount;
                UnclearedCount = unclearedCount;
                TotalPages = filteredCount <= 0 ? 1 : (int)Math.Ceiling(filteredCount / (double)SelectedPageSize);

                if (requestedPage > TotalPages)
                {
                    requestedPage = TotalPages;
                    PageIndex = requestedPage;

                    result = await Task.Run(() =>
                        _alarmRecordService.QueryPage(requestedPage, SelectedPageSize, levelFilter, isCleared, StartDate, EndDate));
                }
                else if (requestedPage != PageIndex)
                {
                    PageIndex = requestedPage;
                }

                Alarms.Clear();
                SelectedAlarm = null;

                if (!result.Success)
                {
                    return;
                }

                foreach (var item in result.Items)
                {
                    Alarms.Add(item);
                }

                SelectedAlarm = Alarms.Count > 0 ? Alarms[0] : null;
            }
            finally
            {
                _isLoading = false;
                UpdatePagingCommandState();
            }
        }

        private async Task QueryAsync()
        {
            PageIndex = 1;
            await LoadAsync();
        }

        private async Task RefreshAsync()
        {
            PageIndex = 1;
            await LoadAsync();
        }

        private async Task PrevPageAsync()
        {
            if (!CanPrevPage()) return;
            PageIndex--;
            await LoadAsync();
        }

        private bool CanPrevPage()
        {
            return !_isLoading && PageIndex > 1;
        }

        private async Task NextPageAsync()
        {
            if (!CanNextPage()) return;
            PageIndex++;
            await LoadAsync();
        }

        private bool CanNextPage()
        {
            return !_isLoading && PageIndex < TotalPages;
        }

        private async Task GoPageAsync()
        {
            int target;
            if (!int.TryParse(JumpPageText, out target)) return;
            if (target < 1) target = 1;
            if (target > TotalPages) target = TotalPages;
            PageIndex = target;
            await LoadAsync();
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand?.NotifyCanExecuteChanged();
            NextPageCommand?.NotifyCanExecuteChanged();
        }
    }
}