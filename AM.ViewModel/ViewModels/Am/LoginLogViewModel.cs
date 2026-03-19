using AM.DBService.Services.Auth;
using AM.Model.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Am
{
    /// <summary>
    /// 登录日志视图模型。
    /// </summary>
    public class LoginLogViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        private string _searchText;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private string _selectedFilter;
        private LoginLogSummary _selectedLog;
        private int _totalCount;
        private int _successCount;
        private int _failedCount;
        private int _pageIndex;
        private int _totalPages;
        private bool _isLoading;

        public LoginLogViewModel()
        {
            _authService = new AuthService();

            Logs = new ObservableCollection<LoginLogSummary>();
            Filters = new ObservableCollection<string>();

            Filters.Add("全部日志");
            Filters.Add("登录成功");
            Filters.Add("登录失败");
            Filters.Add("今日日志");

            _selectedFilter = "全部日志";
            _searchText = string.Empty;
            _pageIndex = 1;
            _totalPages = 1;
            PageSize = 100;

            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            QueryCommand = new AsyncRelayCommand(QueryAsync);
            PrevPageCommand = new AsyncRelayCommand(PrevPageAsync, CanPrevPage);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync, CanNextPage);
        }

        public ObservableCollection<LoginLogSummary> Logs { get; private set; }

        public ObservableCollection<string> Filters { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IAsyncRelayCommand QueryCommand { get; private set; }

        public IAsyncRelayCommand PrevPageCommand { get; private set; }

        public IAsyncRelayCommand NextPageCommand { get; private set; }

        public int PageSize { get; private set; }

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

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

        public LoginLogSummary SelectedLog
        {
            get { return _selectedLog; }
            set { SetProperty(ref _selectedLog, value); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { SetProperty(ref _totalCount, value); }
        }

        public int SuccessCount
        {
            get { return _successCount; }
            set { SetProperty(ref _successCount, value); }
        }

        public int FailedCount
        {
            get { return _failedCount; }
            set { SetProperty(ref _failedCount, value); }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            private set
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
            private set
            {
                if (SetProperty(ref _totalPages, value))
                {
                    UpdatePagingCommandState();
                }
            }
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
                bool? isSuccess = null;
                DateTime? startDate = StartDate;
                DateTime? endDate = EndDate;

                switch (SelectedFilter)
                {
                    case "登录成功":
                        isSuccess = true;
                        break;
                    case "登录失败":
                        isSuccess = false;
                        break;
                    case "今日日志":
                        startDate = DateTime.Today;
                        endDate = DateTime.Today;
                        break;
                }

                int totalCount = 0;
                int successCount = 0;
                int failedCount = 0;

                var result = await Task.Run(() => _authService.GetLoginLogSummaries(
                    SearchText,
                    isSuccess,
                    startDate,
                    endDate,
                    PageIndex,
                    PageSize,
                    out totalCount,
                    out successCount,
                    out failedCount));

                if (!result.Success)
                {
                    Logs.Clear();
                    SelectedLog = null;
                    TotalCount = 0;
                    SuccessCount = 0;
                    FailedCount = 0;
                    TotalPages = 1;
                    return;
                }

                TotalCount = totalCount;
                SuccessCount = successCount;
                FailedCount = failedCount;
                TotalPages = totalCount <= 0 ? 1 : (int)Math.Ceiling(totalCount / (double)PageSize);

                if (PageIndex > TotalPages)
                {
                    PageIndex = TotalPages;
                    await LoadAsync();
                    return;
                }

                Logs.Clear();
                foreach (var item in result.Items)
                {
                    Logs.Add(item);
                }

                SelectedLog = Logs.Count > 0 ? Logs[0] : null;
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
            if (!CanPrevPage())
            {
                return;
            }

            PageIndex--;
            await LoadAsync();
        }

        private async Task NextPageAsync()
        {
            if (!CanNextPage())
            {
                return;
            }

            PageIndex++;
            await LoadAsync();
        }

        private bool CanPrevPage()
        {
            return !_isLoading && PageIndex > 1;
        }

        private bool CanNextPage()
        {
            return !_isLoading && PageIndex < TotalPages;
        }

        private void UpdatePagingCommandState()
        {
            PrevPageCommand.NotifyCanExecuteChanged();
            NextPageCommand.NotifyCanExecuteChanged();
        }
    }
}