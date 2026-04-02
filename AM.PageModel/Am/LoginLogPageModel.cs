using AM.DBService.Services.Auth;
using AM.Model.Auth;
using AM.Model.Common;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Am
{
    /// <summary>
    /// 登录日志页面模型。
    /// </summary>
    public class LoginLogPageModel : BindableBase
    {
        private static readonly DateTime DefaultStartDate = new DateTime(1970, 1, 1);

        private readonly AuthService _authService;
        private List<LoginLogSummary> _logs;
        private string _searchText;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool? _isSuccessFilter;
        private int _totalCount;
        private int _successCount;
        private int _failedCount;
        private int _pageIndex;
        private int _pageSize;

        public LoginLogPageModel()
        {
            _authService = new AuthService();
            _logs = new List<LoginLogSummary>();
            _searchText = string.Empty;
            _pageIndex = 1;
            _pageSize = 50;
        }

        public IList<LoginLogSummary> Logs
        {
            get { return _logs; }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value ?? string.Empty); }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        public bool? IsSuccessFilter
        {
            get { return _isSuccessFilter; }
            set { SetProperty(ref _isSuccessFilter, value); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        public int SuccessCount
        {
            get { return _successCount; }
            private set { SetProperty(ref _successCount, value); }
        }

        public int FailedCount
        {
            get { return _failedCount; }
            private set { SetProperty(ref _failedCount, value); }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            private set { SetProperty(ref _pageIndex, value <= 0 ? 1 : value); }
        }

        public int PageSize
        {
            get { return _pageSize; }
            private set { SetProperty(ref _pageSize, value <= 0 ? 50 : value); }
        }

        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                    return 1;

                var pages = (int)Math.Ceiling(TotalCount / (double)PageSize);
                return pages <= 0 ? 1 : pages;
            }
        }

        public void ResetToFirstPage()
        {
            PageIndex = 1;
        }

        public void SetPage(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex <= 0 ? 1 : pageIndex;
            PageSize = pageSize <= 0 ? PageSize : pageSize;
            OnPropertyChanged(nameof(TotalPages));
        }

        public void SetAllFilter()
        {
            IsSuccessFilter = null;
            StartDate = null;
            EndDate = null;
            ResetToFirstPage();
        }

        public void SetSuccessFilter()
        {
            IsSuccessFilter = true;
            ResetToFirstPage();
        }

        public void SetFailedFilter()
        {
            IsSuccessFilter = false;
            ResetToFirstPage();
        }

        public void SetTodayFilter()
        {
            var today = DateTime.Today;
            StartDate = today;
            EndDate = today;
            IsSuccessFilter = null;
            ResetToFirstPage();
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                int totalCount;
                int successCount;
                int failedCount;

                var startDate = StartDate ?? DefaultStartDate;
                var endDate = EndDate ?? DateTime.Now;

                if (endDate < startDate)
                {
                    var temp = startDate;
                    startDate = endDate;
                    endDate = temp;
                }

                var result = _authService.GetLoginLogSummaries(
                    SearchText,
                    IsSuccessFilter,
                    startDate,
                    endDate,
                    PageIndex,
                    PageSize,
                    out totalCount,
                    out successCount,
                    out failedCount);

                if (!result.Success)
                {
                    _logs = new List<LoginLogSummary>();
                    TotalCount = 0;
                    SuccessCount = 0;
                    FailedCount = 0;
                    OnPropertyChanged(nameof(Logs));
                    OnPropertyChanged(nameof(TotalPages));
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _logs = result.Items == null
                    ? new List<LoginLogSummary>()
                    : result.Items.ToList();

                TotalCount = totalCount;
                SuccessCount = successCount;
                FailedCount = failedCount;

                if (TotalPages > 0 && PageIndex > TotalPages)
                {
                    PageIndex = TotalPages;
                }

                OnPropertyChanged(nameof(Logs));
                OnPropertyChanged(nameof(TotalPages));

                return Result.Ok("加载登录日志成功");
            });
        }
    }
}