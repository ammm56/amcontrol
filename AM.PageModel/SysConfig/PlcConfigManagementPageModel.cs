using AM.Core.Context;
using AM.DBService.Services.Plc.App;
using AM.DBService.Services.Plc.Config;
using AM.DBService.Services.Plc.Runtime;
using AM.Model.Common;
using AM.Model.Entity.Plc;
using AM.Model.Runtime;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.SysConfig
{
    /// <summary>
    /// PLC 配置管理页面模型。
    /// 负责：
    /// 1. 查询 PLC 站/点位配置；
    /// 2. 承担列表筛选、选中维护与统计摘要；
    /// 3. 提供配置保存、删除、重载与扫描控制入口；
    /// 4. 合并 PLC 站运行态摘要，供配置页展示当前运行状态。
    /// </summary>
    public class PlcConfigManagementPageModel : BindableBase
    {
        private readonly PlcStationCrudService _stationCrudService;
        private readonly PlcPointCrudService _pointCrudService;
        private readonly PlcConfigAppService _plcConfigAppService;

        private List<StationViewItem> _allStations;
        private List<StationViewItem> _stations;
        private List<PointViewItem> _allPoints;
        private List<PointViewItem> _points;

        private StationViewItem _selectedStation;
        private PointViewItem _selectedPoint;

        private string _stationSearchText;
        private string _pointSearchText;
        private string _runtimeSummaryText;

        public PlcConfigManagementPageModel()
        {
            _stationCrudService = new PlcStationCrudService();
            _pointCrudService = new PlcPointCrudService();
            _plcConfigAppService = new PlcConfigAppService();

            _allStations = new List<StationViewItem>();
            _stations = new List<StationViewItem>();
            _allPoints = new List<PointViewItem>();
            _points = new List<PointViewItem>();

            _stationSearchText = string.Empty;
            _pointSearchText = string.Empty;
            _runtimeSummaryText = "未加载";
        }

        public IList<StationViewItem> Stations
        {
            get { return _stations; }
        }

        public IList<PointViewItem> Points
        {
            get { return _points; }
        }

        public StationViewItem SelectedStation
        {
            get { return _selectedStation; }
            private set
            {
                if (SetProperty(ref _selectedStation, value))
                {
                    OnPropertyChanged(nameof(SelectedStationText));
                    OnPropertyChanged(nameof(CurrentStationPointCount));
                }
            }
        }

        public PointViewItem SelectedPoint
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

        public string StationSearchText
        {
            get { return _stationSearchText; }
            private set { SetProperty(ref _stationSearchText, value ?? string.Empty); }
        }

        public string PointSearchText
        {
            get { return _pointSearchText; }
            private set { SetProperty(ref _pointSearchText, value ?? string.Empty); }
        }

        public string RuntimeSummaryText
        {
            get { return _runtimeSummaryText; }
            private set { SetProperty(ref _runtimeSummaryText, value ?? string.Empty); }
        }

        public int TotalStationCount
        {
            get { return _allStations.Count; }
        }

        public int OnlineStationCount
        {
            get { return _allStations.Count(x => x.IsConnected); }
        }

        public int TotalPointCount
        {
            get { return _allPoints.Count; }
        }

        public int CurrentStationPointCount
        {
            get
            {
                if (SelectedStation == null)
                {
                    return 0;
                }

                return _allPoints.Count(x => string.Equals(x.PlcName, SelectedStation.Name, StringComparison.OrdinalIgnoreCase));
            }
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

        public string SelectedPointText
        {
            get
            {
                return SelectedPoint == null
                    ? "未选择"
                    : SelectedPoint.DisplayTitle;
            }
        }

        public string SelectedStationName
        {
            get { return SelectedStation == null ? null : SelectedStation.Name; }
        }

        public string SelectedPointName
        {
            get { return SelectedPoint == null ? null : SelectedPoint.Name; }
        }

        /// <summary>
        /// 加载全部 PLC 配置并同步运行态摘要。
        /// </summary>
        public async Task<Result> LoadAsync(string preferredStationName = null, string preferredPointName = null)
        {
            return await Task.Run(() =>
            {
                var ensureResult = _plcConfigAppService.EnsureTables();
                if (!ensureResult.Success)
                {
                    ClearAll();
                    return Result.Fail(ensureResult.Code, ensureResult.Message, ensureResult.Source);
                }

                var stationResult = _stationCrudService.QueryAll();
                if (!stationResult.Success && stationResult.Code != 0)
                {
                    ClearAll();
                    return Result.Fail(stationResult.Code, stationResult.Message, stationResult.Source);
                }

                var pointResult = _pointCrudService.QueryAll();
                if (!pointResult.Success && pointResult.Code != 0)
                {
                    ClearAll();
                    return Result.Fail(pointResult.Code, pointResult.Message, pointResult.Source);
                }

                var stationEntities = stationResult.Success && stationResult.Items != null
                    ? stationResult.Items.Where(x => x != null).OrderBy(x => x.SortOrder).ThenBy(x => x.Name).ToList()
                    : new List<PlcStationConfigEntity>();

                var pointEntities = pointResult.Success && pointResult.Items != null
                    ? pointResult.Items.Where(x => x != null).OrderBy(x => x.PlcName).ThenBy(x => x.SortOrder).ThenBy(x => x.Name).ToList()
                    : new List<PlcPointConfigEntity>();

                var pointCountLookup = pointEntities
                    .GroupBy(x => x.PlcName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

                _allStations = stationEntities
                    .Select(x => ToStationViewItem(x, pointCountLookup))
                    .ToList();

                _allPoints = pointEntities
                    .Select(ToPointViewItem)
                    .ToList();

                MergeRuntimeSnapshots();

                RebuildStations(preferredStationName);
                RebuildPoints(preferredPointName);
                RaiseSummaryChanged();

                return Result.Ok("PLC 配置列表加载成功", ResultSource.UI);
            });
        }

        public void SetStationSearchText(string searchText)
        {
            StationSearchText = searchText ?? string.Empty;
            RebuildStations(SelectedStationName);
            RebuildPoints(SelectedPointName);
            RaiseSummaryChanged();
        }

        public void SetPointSearchText(string searchText)
        {
            PointSearchText = searchText ?? string.Empty;
            RebuildPoints(SelectedPointName);
            RaiseSummaryChanged();
        }

        public void SelectStationByName(string stationName)
        {
            string normalized = NormalizeText(stationName);
            var selected = _stations.FirstOrDefault(x => string.Equals(x.Name, normalized, StringComparison.OrdinalIgnoreCase));
            SelectedStation = selected;
            RebuildPoints(null);
            RaiseSummaryChanged();
        }

        public void SelectPointByName(string pointName)
        {
            string normalized = NormalizeText(pointName);
            SelectedPoint = _points.FirstOrDefault(x => string.Equals(x.Name, normalized, StringComparison.OrdinalIgnoreCase));
            RaiseSummaryChanged();
        }

        public Result SaveStation(PlcStationConfigEntity entity)
        {
            return _stationCrudService.Save(entity);
        }

        public Result DeleteStation(string stationName)
        {
            return _stationCrudService.DeleteByName(stationName);
        }

        public Result SavePoint(PlcPointConfigEntity entity)
        {
            return _pointCrudService.Save(entity);
        }

        public Result DeletePoint(string plcName, string pointName)
        {
            return _pointCrudService.DeleteByName(plcName, pointName);
        }

        public Result ReloadConfig()
        {
            return _plcConfigAppService.ReloadFromDatabase();
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

        private void MergeRuntimeSnapshots()
        {
            try
            {
                var runtimeResult = new PlcRuntimeQueryService().QueryAllStations();
                if (!runtimeResult.Success || runtimeResult.Items == null)
                {
                    RuntimeSummaryText = "未获取到运行时快照";
                    return;
                }

                var runtimeLookup = runtimeResult.Items
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.PlcName))
                    .ToDictionary(x => x.PlcName, x => x, StringComparer.OrdinalIgnoreCase);

                bool scanRunning = runtimeResult.Items.Any(x => x != null && x.IsScanRunning);

                foreach (var station in _allStations)
                {
                    PlcStationRuntimeSnapshot snapshot;
                    if (!runtimeLookup.TryGetValue(station.Name ?? string.Empty, out snapshot) || snapshot == null)
                    {
                        continue;
                    }

                    station.IsConnected = snapshot.IsConnected;
                    station.LastError = snapshot.LastError ?? string.Empty;
                    station.AverageReadMs = snapshot.AverageReadMs;
                    station.LastScanTime = snapshot.LastScanTime;
                    station.IsScanRunning = snapshot.IsScanRunning;
                }

                RuntimeSummaryText = scanRunning
                    ? "扫描中"
                    : "扫描已停止";
            }
            catch
            {
                RuntimeSummaryText = "查询失败";
            }
        }

        private void ClearAll()
        {
            _allStations = new List<StationViewItem>();
            _stations = new List<StationViewItem>();
            _allPoints = new List<PointViewItem>();
            _points = new List<PointViewItem>();
            SelectedStation = null;
            SelectedPoint = null;
            RuntimeSummaryText = "未加载";
            RaiseSummaryChanged();
            OnPropertyChanged(nameof(Stations));
            OnPropertyChanged(nameof(Points));
        }

        private void RebuildStations(string preferredStationName)
        {
            IEnumerable<StationViewItem> query = _allStations;

            if (!string.IsNullOrWhiteSpace(StationSearchText))
            {
                string keyword = StationSearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    ContainsText(x.Name, keyword) ||
                    ContainsText(x.DisplayName, keyword) ||
                    ContainsText(x.ProtocolType, keyword) ||
                    ContainsText(x.ConnectionType, keyword) ||
                    ContainsText(x.EndpointText, keyword));
            }

            _stations = query
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();

            OnPropertyChanged(nameof(Stations));

            StationViewItem selected = null;

            if (!string.IsNullOrWhiteSpace(preferredStationName))
            {
                selected = _stations.FirstOrDefault(x => string.Equals(x.Name, preferredStationName, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && SelectedStation != null)
            {
                selected = _stations.FirstOrDefault(x => string.Equals(x.Name, SelectedStation.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _stations.Count > 0)
            {
                selected = _stations[0];
            }

            SelectedStation = selected;
        }

        private void RebuildPoints(string preferredPointName)
        {
            IEnumerable<PointViewItem> query = _allPoints;

            if (SelectedStation != null)
            {
                query = query.Where(x => string.Equals(x.PlcName, SelectedStation.Name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                query = Enumerable.Empty<PointViewItem>();
            }

            if (!string.IsNullOrWhiteSpace(PointSearchText))
            {
                string keyword = PointSearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    ContainsText(x.Name, keyword) ||
                    ContainsText(x.DisplayName, keyword) ||
                    ContainsText(x.GroupName, keyword) ||
                    ContainsText(x.Address, keyword) ||
                    ContainsText(x.DataType, keyword));
            }

            _points = query
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();

            OnPropertyChanged(nameof(Points));

            PointViewItem selected = null;

            if (!string.IsNullOrWhiteSpace(preferredPointName))
            {
                selected = _points.FirstOrDefault(x => string.Equals(x.Name, preferredPointName, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && SelectedPoint != null)
            {
                selected = _points.FirstOrDefault(x => string.Equals(x.Name, SelectedPoint.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (selected == null && _points.Count > 0)
            {
                selected = _points[0];
            }

            SelectedPoint = selected;
        }

        private void RaiseSummaryChanged()
        {
            OnPropertyChanged(nameof(TotalStationCount));
            OnPropertyChanged(nameof(OnlineStationCount));
            OnPropertyChanged(nameof(TotalPointCount));
            OnPropertyChanged(nameof(CurrentStationPointCount));
            OnPropertyChanged(nameof(SelectedStationText));
            OnPropertyChanged(nameof(SelectedPointText));
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

        private static StationViewItem ToStationViewItem(
            PlcStationConfigEntity entity,
            IDictionary<string, int> pointCountLookup)
        {
            int pointCount = 0;
            pointCountLookup.TryGetValue(entity.Name ?? string.Empty, out pointCount);

            return new StationViewItem
            {
                Id = entity.Id,
                Name = entity.Name ?? string.Empty,
                DisplayName = entity.DisplayName ?? string.Empty,
                Vendor = entity.Vendor ?? string.Empty,
                Model = entity.Model ?? string.Empty,
                ConnectionType = entity.ConnectionType ?? string.Empty,
                ProtocolType = entity.ProtocolType ?? string.Empty,
                IpAddress = entity.IpAddress ?? string.Empty,
                Port = entity.Port,
                ComPort = entity.ComPort ?? string.Empty,
                BaudRate = entity.BaudRate,
                DataBits = entity.DataBits,
                Parity = entity.Parity ?? string.Empty,
                StopBits = entity.StopBits ?? string.Empty,
                StationNo = entity.StationNo,
                NetworkNo = entity.NetworkNo,
                PcNo = entity.PcNo,
                Rack = entity.Rack,
                Slot = entity.Slot,
                TimeoutMs = entity.TimeoutMs,
                ReconnectIntervalMs = entity.ReconnectIntervalMs,
                ScanIntervalMs = entity.ScanIntervalMs,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty,
                CreateTime = entity.CreateTime,
                UpdateTime = entity.UpdateTime,
                PointCount = pointCount,
                IsConnected = false,
                LastError = string.Empty,
                AverageReadMs = 0D,
                LastScanTime = null,
                IsScanRunning = false
            };
        }

        private static PointViewItem ToPointViewItem(PlcPointConfigEntity entity)
        {
            return new PointViewItem
            {
                Id = entity.Id,
                PlcName = entity.PlcName ?? string.Empty,
                Name = entity.Name ?? string.Empty,
                DisplayName = entity.DisplayName ?? string.Empty,
                GroupName = entity.GroupName ?? string.Empty,
                Address = entity.Address ?? string.Empty,
                DataType = entity.DataType ?? string.Empty,
                Length = entity.Length,
                AccessMode = entity.AccessMode ?? string.Empty,
                IsEnabled = entity.IsEnabled,
                SortOrder = entity.SortOrder,
                Description = entity.Description ?? string.Empty,
                Remark = entity.Remark ?? string.Empty
            };
        }

        public sealed class StationViewItem
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string Vendor { get; set; }

            public string Model { get; set; }

            public string ConnectionType { get; set; }

            public string ProtocolType { get; set; }

            public string IpAddress { get; set; }

            public int? Port { get; set; }

            public string ComPort { get; set; }

            public int? BaudRate { get; set; }

            public int? DataBits { get; set; }

            public string Parity { get; set; }

            public string StopBits { get; set; }

            public short? StationNo { get; set; }

            public short? NetworkNo { get; set; }

            public short? PcNo { get; set; }

            public short? Rack { get; set; }

            public short? Slot { get; set; }

            public int TimeoutMs { get; set; }

            public int ReconnectIntervalMs { get; set; }

            public int ScanIntervalMs { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public DateTime CreateTime { get; set; }

            public DateTime UpdateTime { get; set; }

            public int PointCount { get; set; }

            public bool IsConnected { get; set; }

            public string LastError { get; set; }

            public double AverageReadMs { get; set; }

            public DateTime? LastScanTime { get; set; }

            public bool IsScanRunning { get; set; }

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

            public string EndpointText
            {
                get
                {
                    if (string.Equals(ConnectionType, "Tcp", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrWhiteSpace(IpAddress))
                        {
                            return IpAddress + (Port.HasValue ? ":" + Port.Value : string.Empty);
                        }
                    }

                    if (string.Equals(ConnectionType, "Serial", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrWhiteSpace(ComPort))
                        {
                            return ComPort + (BaudRate.HasValue ? " / " + BaudRate.Value : string.Empty);
                        }
                    }

                    return "-";
                }
            }

            public string LastScanTimeText
            {
                get
                {
                    return LastScanTime.HasValue
                        ? LastScanTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                        : "-";
                }
            }

            public PlcStationConfigEntity ToEntity()
            {
                return new PlcStationConfigEntity
                {
                    Id = Id,
                    Name = Name,
                    DisplayName = DisplayName,
                    Vendor = Vendor,
                    Model = Model,
                    ConnectionType = ConnectionType,
                    ProtocolType = ProtocolType,
                    IpAddress = IpAddress,
                    Port = Port,
                    ComPort = ComPort,
                    BaudRate = BaudRate,
                    DataBits = DataBits,
                    Parity = Parity,
                    StopBits = StopBits,
                    StationNo = StationNo,
                    NetworkNo = NetworkNo,
                    PcNo = PcNo,
                    Rack = Rack,
                    Slot = Slot,
                    TimeoutMs = TimeoutMs,
                    ReconnectIntervalMs = ReconnectIntervalMs,
                    ScanIntervalMs = ScanIntervalMs,
                    IsEnabled = IsEnabled,
                    SortOrder = SortOrder,
                    Description = Description,
                    Remark = Remark,
                    CreateTime = CreateTime == default(DateTime) ? DateTime.Now : CreateTime,
                    UpdateTime = DateTime.Now
                };
            }
        }

        public sealed class PointViewItem
        {
            public int Id { get; set; }

            public string PlcName { get; set; }

            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string GroupName { get; set; }

            public string Address { get; set; }

            public string DataType { get; set; }

            public int Length { get; set; }

            public string AccessMode { get; set; }

            public bool IsEnabled { get; set; }

            public int SortOrder { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                    {
                        return DisplayName;
                    }

                    return string.IsNullOrWhiteSpace(Name) ? "未命名点位" : Name;
                }
            }

            public PlcPointConfigEntity ToEntity()
            {
                return new PlcPointConfigEntity
                {
                    Id = Id,
                    PlcName = PlcName,
                    Name = Name,
                    DisplayName = DisplayName,
                    GroupName = GroupName,
                    Address = Address,
                    DataType = DataType,
                    Length = Length,
                    AccessMode = AccessMode,
                    IsEnabled = IsEnabled,
                    SortOrder = SortOrder,
                    Description = Description,
                    Remark = Remark
                };
            }
        }
    }
}