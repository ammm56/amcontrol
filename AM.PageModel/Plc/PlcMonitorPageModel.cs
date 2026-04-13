using AM.Core.Context;
using AM.DBService.Services.Plc.Runtime;
using AM.Model.Common;
using AM.Model.Plc;
using AM.Model.Runtime;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Plc
{
    /// <summary>
    /// PLC 点位监视页面模型。
    /// </summary>
    public class PlcMonitorPageModel : BindableBase
    {
        private readonly PlcRuntimeQueryService _runtimeQueryService;

        private List<PointMonitorItem> _allPoints;
        private List<PointMonitorItem> _points;
        private PointMonitorItem _selectedPoint;
        private string _selectedPlcName;
        private string _selectedGroupName;
        private string _searchText;
        private string _runtimeSummaryText;
        private DateTime? _lastRefreshTime;
        private List<string> _plcOptions;
        private List<string> _groupOptions;

        public PlcMonitorPageModel()
        {
            _runtimeQueryService = new PlcRuntimeQueryService();
            _allPoints = new List<PointMonitorItem>();
            _points = new List<PointMonitorItem>();
            _plcOptions = new List<string>();
            _groupOptions = new List<string>();
            _selectedPlcName = string.Empty;
            _selectedGroupName = string.Empty;
            _searchText = string.Empty;
            _runtimeSummaryText = "状态未知";
        }

        public IList<PointMonitorItem> Points
        {
            get { return _points; }
        }

        public PointMonitorItem SelectedPoint
        {
            get { return _selectedPoint; }
            private set
            {
                if (SetProperty(ref _selectedPoint, value))
                {
                    OnPropertyChanged(nameof(SelectedPointText));
                }
            }
        }

        public IList<string> PlcOptions
        {
            get { return _plcOptions; }
        }

        public IList<string> GroupOptions
        {
            get { return _groupOptions; }
        }

        public string SelectedPlcName
        {
            get { return _selectedPlcName; }
            private set { SetProperty(ref _selectedPlcName, value ?? string.Empty); }
        }

        public string SelectedGroupName
        {
            get { return _selectedGroupName; }
            private set { SetProperty(ref _selectedGroupName, value ?? string.Empty); }
        }

        public string SearchText
        {
            get { return _searchText; }
            private set { SetProperty(ref _searchText, value ?? string.Empty); }
        }

        public string RuntimeSummaryText
        {
            get { return _runtimeSummaryText; }
            private set { SetProperty(ref _runtimeSummaryText, value ?? string.Empty); }
        }

        public DateTime? LastRefreshTime
        {
            get { return _lastRefreshTime; }
            private set
            {
                if (SetProperty(ref _lastRefreshTime, value))
                {
                    OnPropertyChanged(nameof(LastRefreshTimeText));
                }
            }
        }

        public string LastRefreshTimeText
        {
            get { return LastRefreshTime.HasValue ? LastRefreshTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-"; }
        }

        public int TotalPointCount
        {
            get { return _allPoints.Count; }
        }

        public int OnlinePointCount
        {
            get { return _allPoints.Count(x => x.IsConnected); }
        }

        public int OfflinePointCount
        {
            get { return _allPoints.Count(x => !x.IsConnected); }
        }

        public int ErrorPointCount
        {
            get { return _allPoints.Count(x => x.HasError); }
        }

        public string SelectedPointText
        {
            get { return SelectedPoint == null ? "未选择点位" : SelectedPoint.DisplayTitle; }
        }

        public async Task<Result> LoadAsync()
        {
            return await RefreshAsync();
        }

        public async Task<Result> RefreshAsync()
        {
            return await Task.Run(() =>
            {
                var runtimeResult = _runtimeQueryService.QueryAllPoints();
                if (!runtimeResult.Success)
                {
                    ClearAll();
                    RuntimeSummaryText = "状态未知";
                    LastRefreshTime = DateTime.Now;
                    return Result.Fail(runtimeResult.Code, runtimeResult.Message, runtimeResult.Source);
                }

                _allPoints = runtimeResult.Items == null
                    ? new List<PointMonitorItem>()
                    : runtimeResult.Items
                        .Where(x => x != null)
                        .OrderBy(x => x.PlcName)
                        .ThenBy(x => x.GroupName)
                        .ThenBy(x => x.PointName)
                        .Select(ToPointMonitorItem)
                        .ToList();

                RebuildPlcOptions();
                RebuildGroupOptions();
                ApplyRuntimeSummary();
                RebuildPoints(SelectedPoint == null ? null : SelectedPoint.PointName);
                LastRefreshTime = DateTime.Now;
                RaiseSummaryChanged();

                return Result.Ok("PLC 点位监视加载成功", ResultSource.UI);
            });
        }

        public void SetSelectedPlc(string plcName)
        {
            SelectedPlcName = NormalizeText(plcName);
            SelectedGroupName = string.Empty;
            RebuildGroupOptions();
            RebuildPoints(null);
            RaiseSummaryChanged();
        }

        public void SetSelectedGroup(string groupName)
        {
            SelectedGroupName = NormalizeText(groupName);
            RebuildPoints(null);
            RaiseSummaryChanged();
        }

        public void SetSearchText(string searchText)
        {
            SearchText = searchText ?? string.Empty;
            RebuildPoints(SelectedPoint == null ? null : SelectedPoint.PointName);
            RaiseSummaryChanged();
        }

        public void SelectPointByName(string pointName)
        {
            string normalized = NormalizeText(pointName);
            SelectedPoint = _points.FirstOrDefault(x =>
                string.Equals(x.PointName, normalized, StringComparison.OrdinalIgnoreCase));
            RaiseSummaryChanged();
        }

        private void RebuildPlcOptions()
        {
            _plcOptions = _allPoints
                .Select(x => x.PlcName ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            OnPropertyChanged(nameof(PlcOptions));

            if (!string.IsNullOrWhiteSpace(SelectedPlcName) &&
                !_plcOptions.Any(x => string.Equals(x, SelectedPlcName, StringComparison.OrdinalIgnoreCase)))
            {
                SelectedPlcName = string.Empty;
            }
        }

        private void RebuildGroupOptions()
        {
            IEnumerable<PointMonitorItem> query = _allPoints;

            if (!string.IsNullOrWhiteSpace(SelectedPlcName))
            {
                query = query.Where(x => string.Equals(x.PlcName, SelectedPlcName, StringComparison.OrdinalIgnoreCase));
            }

            _groupOptions = query
                .Select(x => x.GroupName ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            OnPropertyChanged(nameof(GroupOptions));

            if (!string.IsNullOrWhiteSpace(SelectedGroupName) &&
                !_groupOptions.Any(x => string.Equals(x, SelectedGroupName, StringComparison.OrdinalIgnoreCase)))
            {
                SelectedGroupName = string.Empty;
            }
        }

        private void RebuildPoints(string preferredPointName)
        {
            IEnumerable<PointMonitorItem> query = _allPoints;

            if (!string.IsNullOrWhiteSpace(SelectedPlcName))
            {
                query = query.Where(x => string.Equals(x.PlcName, SelectedPlcName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedGroupName))
            {
                query = query.Where(x => string.Equals(x.GroupName, SelectedGroupName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    ContainsText(x.PointName, keyword) ||
                    ContainsText(x.DisplayName, keyword) ||
                    ContainsText(x.GroupName, keyword) ||
                    ContainsText(x.AddressText, keyword) ||
                    ContainsText(x.ValueText, keyword) ||
                    ContainsText(x.ErrorMessage, keyword));
            }

            _points = query
                .OrderBy(x => x.PlcName)
                .ThenBy(x => x.GroupName)
                .ThenBy(x => x.PointName)
                .ToList();

            OnPropertyChanged(nameof(Points));

            PointMonitorItem selected = null;

            if (!string.IsNullOrWhiteSpace(preferredPointName))
            {
                selected = _points.FirstOrDefault(x =>
                    string.Equals(x.PointName, preferredPointName, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && SelectedPoint != null)
            {
                selected = _points.FirstOrDefault(x =>
                    string.Equals(x.PointName, SelectedPoint.PointName, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _points.Count > 0)
            {
                selected = _points[0];
            }

            SelectedPoint = selected;
        }

        private void ApplyRuntimeSummary()
        {
            PlcRuntimeState runtimeState = RuntimeContext.Instance.Plc;
            bool scanRunning = runtimeState != null && runtimeState.IsScanServiceRunning;
            DateTime? lastScanTime = runtimeState == null ? null : runtimeState.LastScanTime;

            RuntimeSummaryText = scanRunning
                ? "运行中 " + FormatTime(lastScanTime)
                : "已停止 " + FormatTime(lastScanTime);
        }

        private void ClearAll()
        {
            _allPoints = new List<PointMonitorItem>();
            _points = new List<PointMonitorItem>();
            _plcOptions = new List<string>();
            _groupOptions = new List<string>();
            SelectedPoint = null;
            SelectedPlcName = string.Empty;
            SelectedGroupName = string.Empty;
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(PlcOptions));
            OnPropertyChanged(nameof(GroupOptions));
            RaiseSummaryChanged();
        }

        private void RaiseSummaryChanged()
        {
            OnPropertyChanged(nameof(TotalPointCount));
            OnPropertyChanged(nameof(OnlinePointCount));
            OnPropertyChanged(nameof(OfflinePointCount));
            OnPropertyChanged(nameof(ErrorPointCount));
            OnPropertyChanged(nameof(SelectedPointText));
        }

        private static PointMonitorItem ToPointMonitorItem(PlcPointRuntimeSnapshot snapshot)
        {
            return new PointMonitorItem
            {
                PlcName = snapshot == null ? string.Empty : (snapshot.PlcName ?? string.Empty),
                PointName = snapshot == null ? string.Empty : (snapshot.PointName ?? string.Empty),
                DisplayName = snapshot == null ? string.Empty : (snapshot.DisplayName ?? string.Empty),
                GroupName = snapshot == null ? string.Empty : (snapshot.GroupName ?? string.Empty),
                AddressText = snapshot == null ? string.Empty : (snapshot.AddressText ?? string.Empty),
                DataType = snapshot == null ? string.Empty : (snapshot.DataType ?? string.Empty),
                ValueText = snapshot == null ? string.Empty : (snapshot.ValueText ?? string.Empty),
                RawValue = snapshot == null ? string.Empty : (snapshot.RawValue ?? string.Empty),
                Quality = snapshot == null ? string.Empty : (snapshot.Quality ?? string.Empty),
                IsConnected = snapshot != null && snapshot.IsConnected,
                HasError = snapshot != null && snapshot.HasError,
                ErrorMessage = snapshot == null ? string.Empty : (snapshot.ErrorMessage ?? string.Empty),
                UpdateTime = snapshot == null ? default(DateTime) : snapshot.UpdateTime
            };
        }

        private static bool ContainsText(string source, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return true;
            }

            return !string.IsNullOrWhiteSpace(source)
                && source.ToLowerInvariant().Contains(keyword);
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string FormatTime(DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
        }

        public sealed class PointMonitorItem
        {
            public string PlcName { get; set; }

            public string PointName { get; set; }

            public string DisplayName { get; set; }

            public string GroupName { get; set; }

            public string AddressText { get; set; }

            public string DataType { get; set; }

            public string ValueText { get; set; }

            public string RawValue { get; set; }

            public string Quality { get; set; }

            public bool IsConnected { get; set; }

            public bool HasError { get; set; }

            public string ErrorMessage { get; set; }

            public DateTime UpdateTime { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                    {
                        return DisplayName;
                    }

                    return string.IsNullOrWhiteSpace(PointName) ? "未命名点位" : PointName;
                }
            }

            public string UpdateTimeText
            {
                get
                {
                    return UpdateTime == default(DateTime)
                        ? "-"
                        : UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            public string QualityText
            {
                get
                {
                    return string.IsNullOrWhiteSpace(Quality) ? "-" : Quality;
                }
            }

            public string ValueBriefText
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(ValueText))
                    {
                        return "值：-";
                    }

                    return "值：" + ValueText;
                }
            }

            public string ErrorBriefText
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(ErrorMessage))
                    {
                        return "-";
                    }

                    return ErrorMessage.Length <= 24
                        ? ErrorMessage
                        : ErrorMessage.Substring(0, 24) + "...";
                }
            }
        }
    }
}