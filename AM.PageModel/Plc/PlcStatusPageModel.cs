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
    /// PLC 运行状态页面模型。
    /// 负责：
    /// 1. 查询 PLC 站级运行态；
    /// 2. 承担站搜索筛选与选中维护；
    /// 3. 提供扫描启动、停止、单轮扫描操作入口；
    /// 4. 输出页面摘要统计信息；
    /// 5. 为虚拟卡片列表与右侧详情区提供统一显示数据。
    /// </summary>
    public class PlcStatusPageModel : BindableBase
    {
        private readonly PlcRuntimeQueryService _runtimeQueryService;

        private List<StationStatusItem> _allStations;
        private List<StationStatusItem> _stations;
        private StationStatusItem _selectedStation;
        private string _searchText;
        private string _runtimeSummaryText;
        private DateTime? _lastRefreshTime;

        public PlcStatusPageModel()
        {
            _runtimeQueryService = new PlcRuntimeQueryService();
            _allStations = new List<StationStatusItem>();
            _stations = new List<StationStatusItem>();
            _searchText = string.Empty;
            _runtimeSummaryText = "状态未知";
        }

        public IList<StationStatusItem> Stations
        {
            get { return _stations; }
        }

        public StationStatusItem SelectedStation
        {
            get { return _selectedStation; }
            private set
            {
                if (SetProperty(ref _selectedStation, value))
                {
                    OnPropertyChanged(nameof(SelectedStationText));
                }
            }
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
            get
            {
                return LastRefreshTime.HasValue
                    ? LastRefreshTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : "-";
            }
        }

        public int TotalStationCount
        {
            get { return _allStations.Count; }
        }

        public int OnlineStationCount
        {
            get { return _allStations.Count(x => x.IsConnected); }
        }

        public int OfflineStationCount
        {
            get { return _allStations.Count(x => !x.IsConnected); }
        }

        public int ErrorStationCount
        {
            get { return _allStations.Count(x => x.HasError); }
        }

        public string SelectedStationText
        {
            get
            {
                return SelectedStation == null
                    ? "未选择 PLC"
                    : SelectedStation.DisplayTitle;
            }
        }

        /// <summary>
        /// 是否允许执行扫描控制动作。
        /// 运行状态页对操作员开放监视，但扫描控制仅允许工程师与管理员。
        /// </summary>
        public bool CanControlScanOperations
        {
            get
            {
                var roles = UserContext.Instance.CurrentRoles;
                if (roles == null)
                {
                    return false;
                }

                return roles.Any(x =>
                    string.Equals(x.RoleCode, "Engineer", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(x.RoleCode, "Am", StringComparison.OrdinalIgnoreCase));
            }
        }

        public async Task<Result> LoadAsync()
        {
            return await RefreshAsync();
        }

        public async Task<Result> RefreshAsync()
        {
            return await Task.Run(() =>
            {
                var runtimeResult = _runtimeQueryService.QueryAllStations();
                if (!runtimeResult.Success)
                {
                    ClearAll();
                    RuntimeSummaryText = "状态未知";
                    LastRefreshTime = DateTime.Now;
                    return Result.Fail(runtimeResult.Code, runtimeResult.Message, runtimeResult.Source);
                }

                var configLookup = BuildStationConfigLookup();

                _allStations = runtimeResult.Items == null
                    ? new List<StationStatusItem>()
                    : runtimeResult.Items
                        .Where(x => x != null)
                        .OrderBy(x => x.PlcName)
                        .Select(x => ToStationStatusItem(x, configLookup))
                        .ToList();

                ApplyRuntimeSummary();
                RebuildStations(SelectedStation == null ? null : SelectedStation.Name);
                LastRefreshTime = DateTime.Now;
                RaiseSummaryChanged();

                return Result.Ok("PLC 运行状态加载成功", ResultSource.UI);
            });
        }

        public void SetSearchText(string searchText)
        {
            SearchText = searchText ?? string.Empty;
            RebuildStations(SelectedStation == null ? null : SelectedStation.Name);
            RaiseSummaryChanged();
        }

        public void SelectStationByName(string plcName)
        {
            string normalized = NormalizeText(plcName);

            SelectedStation = _stations.FirstOrDefault(x =>
                string.Equals(x.Name, normalized, StringComparison.OrdinalIgnoreCase));

            RaiseSummaryChanged();
        }

        public Result StartScan()
        {
            if (SystemContext.Instance.RuntimeTaskManager == null)
            {
                return Result.Fail(-4301, "运行时任务管理器未初始化", ResultSource.Plc);
            }

            return SystemContext.Instance.RuntimeTaskManager.Start("PlcScanWorker");
        }

        public Result StopScan()
        {
            if (SystemContext.Instance.RuntimeTaskManager == null)
            {
                return Result.Fail(-4302, "运行时任务管理器未初始化", ResultSource.Plc);
            }

            try
            {
                return Task.Run(async () =>
                    await SystemContext.Instance.RuntimeTaskManager.StopAsync("PlcScanWorker"))
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                return Result.Fail(-4303, "停止 PLC 扫描失败: " + ex.Message, ResultSource.Plc);
            }
        }

        public Result ScanOnce()
        {
            return new PlcScanWorker().ScanOnce();
        }

        /// <summary>
        /// 页面顶部摘要显示：
        /// 当前是否运行 + 最近一轮扫描完成时间。
        /// </summary>
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
            _allStations = new List<StationStatusItem>();
            _stations = new List<StationStatusItem>();
            SelectedStation = null;
            OnPropertyChanged(nameof(Stations));
            RaiseSummaryChanged();
        }

        private void RebuildStations(string preferredPlcName)
        {
            IEnumerable<StationStatusItem> query = _allStations;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    ContainsText(x.Name, keyword) ||
                    ContainsText(x.DisplayName, keyword) ||
                    ContainsText(x.ProtocolType, keyword) ||
                    ContainsText(x.ConnectionType, keyword) ||
                    ContainsText(x.EndpointText, keyword) ||
                    ContainsText(x.LastError, keyword));
            }

            _stations = query
                .OrderBy(x => x.Name)
                .ToList();

            OnPropertyChanged(nameof(Stations));

            StationStatusItem selected = null;

            if (!string.IsNullOrWhiteSpace(preferredPlcName))
            {
                selected = _stations.FirstOrDefault(x =>
                    string.Equals(x.Name, preferredPlcName, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && SelectedStation != null)
            {
                selected = _stations.FirstOrDefault(x =>
                    string.Equals(x.Name, SelectedStation.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _stations.Count > 0)
            {
                selected = _stations[0];
            }

            SelectedStation = selected;
        }

        private void RaiseSummaryChanged()
        {
            OnPropertyChanged(nameof(TotalStationCount));
            OnPropertyChanged(nameof(OnlineStationCount));
            OnPropertyChanged(nameof(OfflineStationCount));
            OnPropertyChanged(nameof(ErrorStationCount));
            OnPropertyChanged(nameof(SelectedStationText));
            OnPropertyChanged(nameof(CanControlScanOperations));
        }

        private static IDictionary<string, PlcStationConfig> BuildStationConfigLookup()
        {
            var plcConfig = ConfigContext.Instance.Config.PlcConfig;
            var stations = plcConfig == null || plcConfig.Stations == null
                ? new List<PlcStationConfig>()
                : plcConfig.Stations.Where(x => x != null).ToList();

            return stations.ToDictionary(x => x.Name ?? string.Empty, x => x, StringComparer.OrdinalIgnoreCase);
        }

        private static StationStatusItem ToStationStatusItem(
            PlcStationRuntimeSnapshot snapshot,
            IDictionary<string, PlcStationConfig> configLookup)
        {
            PlcStationConfig config = null;
            if (snapshot != null
                && configLookup != null
                && !string.IsNullOrWhiteSpace(snapshot.PlcName))
            {
                configLookup.TryGetValue(snapshot.PlcName, out config);
            }

            return new StationStatusItem
            {
                Name = snapshot == null ? string.Empty : (snapshot.PlcName ?? string.Empty),
                DisplayName = snapshot == null ? string.Empty : (snapshot.DisplayName ?? string.Empty),
                ProtocolType = snapshot == null ? string.Empty : (snapshot.CurrentProtocol ?? string.Empty),
                ConnectionType = snapshot == null ? string.Empty : (snapshot.CurrentConnectionType ?? string.Empty),
                EndpointText = BuildEndpointText(config, snapshot),
                Description = config == null ? string.Empty : (config.Description ?? string.Empty),
                Remark = config == null ? string.Empty : (config.Remark ?? string.Empty),
                IsEnabled = snapshot != null && snapshot.IsEnabled,
                IsConnected = snapshot != null && snapshot.IsConnected,
                IsScanRunning = snapshot != null && snapshot.IsScanRunning,
                LastConnectTime = snapshot == null ? null : snapshot.LastConnectTime,
                LastScanTime = snapshot == null ? null : snapshot.LastScanTime,
                LastError = snapshot == null ? string.Empty : (snapshot.LastError ?? string.Empty),
                AverageReadMs = snapshot == null ? 0D : snapshot.AverageReadMs,
                AverageWriteMs = snapshot == null ? 0D : snapshot.AverageWriteMs,
                SuccessReadCount = snapshot == null ? 0L : snapshot.SuccessReadCount,
                FailedReadCount = snapshot == null ? 0L : snapshot.FailedReadCount
            };
        }

        private static string BuildEndpointText(PlcStationConfig config, PlcStationRuntimeSnapshot snapshot)
        {
            var connectionType = snapshot == null
                ? (config == null ? string.Empty : config.ConnectionType)
                : snapshot.CurrentConnectionType;

            if (string.Equals(connectionType, "Tcp", StringComparison.OrdinalIgnoreCase))
            {
                var ip = config == null ? string.Empty : (config.IpAddress ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    return ip + (config != null && config.Port.HasValue ? ":" + config.Port.Value : string.Empty);
                }
            }

            if (string.Equals(connectionType, "Serial", StringComparison.OrdinalIgnoreCase))
            {
                var comPort = config == null ? string.Empty : (config.ComPort ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(comPort))
                {
                    return comPort + (config != null && config.BaudRate.HasValue ? " / " + config.BaudRate.Value : string.Empty);
                }
            }

            return "-";
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
            return time.HasValue
                ? time.Value.ToString("yyyy-MM-dd HH:mm:ss")
                : "-";
        }

        public sealed class StationStatusItem
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string ProtocolType { get; set; }

            public string ConnectionType { get; set; }

            public string EndpointText { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public bool IsEnabled { get; set; }

            public bool IsConnected { get; set; }

            public bool IsScanRunning { get; set; }

            public DateTime? LastConnectTime { get; set; }

            /// <summary>
            /// 最近一轮扫描完成时间。
            /// </summary>
            public DateTime? LastScanTime { get; set; }

            public string LastError { get; set; }

            /// <summary>
            /// 整轮读取耗时平滑均值，不是单点平均耗时。
            /// </summary>
            public double AverageReadMs { get; set; }

            public double AverageWriteMs { get; set; }

            /// <summary>
            /// 成功扫描轮次。
            /// </summary>
            public long SuccessReadCount { get; set; }

            /// <summary>
            /// 失败扫描轮次。
            /// </summary>
            public long FailedReadCount { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                    {
                        return DisplayName;
                    }

                    return string.IsNullOrWhiteSpace(Name) ? "未命名 PLC" : Name;
                }
            }

            public bool HasError
            {
                get
                {
                    return !string.IsNullOrWhiteSpace(LastError)
                        || (IsEnabled && !IsConnected);
                }
            }

            public string ConfigStatusText
            {
                get { return IsEnabled ? "启用" : "禁用"; }
            }

            public string ConnectionStatusText
            {
                get
                {
                    if (!IsEnabled)
                    {
                        return "未参与";
                    }

                    return IsConnected ? "在线" : "离线";
                }
            }

            public string ScanStatusText
            {
                get
                {
                    if (!IsEnabled)
                    {
                        return "未参与";
                    }

                    return IsScanRunning ? "扫描中" : "已停止";
                }
            }

            public string LastConnectTimeText
            {
                get
                {
                    return LastConnectTime.HasValue
                        ? LastConnectTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "-";
                }
            }

            /// <summary>
            /// 最近一轮扫描完成时间文本。
            /// </summary>
            public string LastScanTimeText
            {
                get
                {
                    return LastScanTime.HasValue
                        ? LastScanTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "-";
                }
            }

            public string AverageReadMsText
            {
                get { return AverageReadMs <= 0D ? "-" : AverageReadMs.ToString("0.##") + " ms"; }
            }

            public string AverageWriteMsText
            {
                get { return AverageWriteMs <= 0D ? "-" : AverageWriteMs.ToString("0.##") + " ms"; }
            }

            public string SuccessReadCountText
            {
                get { return SuccessReadCount <= 0 ? "0" : SuccessReadCount.ToString(); }
            }

            public string FailedReadCountText
            {
                get { return FailedReadCount <= 0 ? "0" : FailedReadCount.ToString(); }
            }

            public string ErrorBriefText
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(LastError))
                    {
                        return IsEnabled && !IsConnected ? "站离线" : "-";
                    }

                    return LastError.Length <= 16
                        ? LastError
                        : LastError.Substring(0, 15) + "…";
                }
            }

            public string LastErrorText
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(LastError))
                    {
                        return IsEnabled && !IsConnected ? "站离线" : "-";
                    }

                    return LastError;
                }
            }
        }
    }
}