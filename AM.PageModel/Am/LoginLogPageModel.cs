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
        private readonly AuthService _authService;
        private List<LoginLogSummary> _logs;
        private string _searchText;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool? _isSuccessFilter;
        private int _totalCount;
        private int _successCount;
        private int _failedCount;
        private int _pageSize;

        public LoginLogPageModel()
        {
            _authService = new AuthService();
            _logs = new List<LoginLogSummary>();
            _searchText = string.Empty;
            _pageSize = 200;
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

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                int totalCount;
                int successCount;
                int failedCount;

                var result = _authService.GetLoginLogSummaries(
                    SearchText,
                    IsSuccessFilter,
                    StartDate,
                    EndDate,
                    1,
                    _pageSize,
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
                    return Result.Fail(result.Code, result.Message, result.Source);
                }

                _logs = result.Items == null
                    ? new List<LoginLogSummary>()
                    : result.Items.ToList();

                TotalCount = totalCount;
                SuccessCount = successCount;
                FailedCount = failedCount;

                OnPropertyChanged(nameof(Logs));
                return Result.Ok("加载登录日志成功");
            });
        }

        public void SetTodayFilter()
        {
            var today = DateTime.Today;
            StartDate = today;
            EndDate = today;
        }

        public void ClearDateFilter()
        {
            StartDate = null;
            EndDate = null;
        }
    }
}