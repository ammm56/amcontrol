using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc.Runtime;
using AM.Model.Plc;
using AM.Model.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 运行时统一服务。
    /// 对上层提供两类能力：
    /// 1. 扫描控制：启动、停止、单轮扫描；
    /// 2. 运行态查询：站状态、点位状态、全量快照。
    ///
    /// 设计说明：
    /// - 页面层不直接访问 `PlcScanWorker`，统一通过本服务获取运行态；
    /// - 查询优先返回 `RuntimeContext` 中的最新快照；
    /// - 若尚未扫描，也会基于 `ConfigContext` 补齐静态定义，避免状态页/监视页初次进入时列表为空。
    /// </summary>
    public class PlcRuntimeQueryService : ServiceBase, IPlcRuntimeService
    {
        private readonly IPlcScanWorker _plcScanWorker;

        protected override string MessageSourceName
        {
            get { return "PlcRuntimeQuery"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Plc; }
        }

        public PlcRuntimeQueryService()
            : this(new PlcScanWorker(), SystemContext.Instance.Reporter)
        {
        }

        public PlcRuntimeQueryService(IPlcScanWorker plcScanWorker, IAppReporter reporter)
            : base(reporter)
        {
            if (plcScanWorker == null)
            {
                throw new ArgumentNullException("plcScanWorker");
            }

            _plcScanWorker = plcScanWorker;
        }

        /// <summary>
        /// 启动 PLC 扫描服务。
        /// </summary>
        public Result Start()
        {
            return _plcScanWorker.Start();
        }

        /// <summary>
        /// 停止 PLC 扫描服务。
        /// </summary>
        public Result Stop()
        {
            return _plcScanWorker.Stop();
        }

        /// <summary>
        /// 手动执行一轮扫描。
        /// </summary>
        public Result ScanOnce()
        {
            return _plcScanWorker.ScanOnce();
        }

        /// <summary>
        /// 查询指定 PLC 站运行态。
        /// </summary>
        public Result<PlcStationRuntimeSnapshot> QueryStation(string plcName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName))
                {
                    return Fail<PlcStationRuntimeSnapshot>((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
                }

                var normalizedPlcName = plcName.Trim();
                var config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                var runtime = RuntimeContext.Instance.Plc;

                PlcStationRuntimeSnapshot runtimeSnapshot;
                var hasRuntimeSnapshot = runtime.TryGetStationSnapshot(normalizedPlcName, out runtimeSnapshot) && runtimeSnapshot != null;

                var stationConfig = config.Stations
                    .FirstOrDefault(p => p != null && string.Equals(p.Name, normalizedPlcName, StringComparison.OrdinalIgnoreCase));

                if (!hasRuntimeSnapshot && stationConfig == null)
                {
                    return Warn<PlcStationRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 站运行态");
                }

                var snapshot = hasRuntimeSnapshot
                    ? MergeStationSnapshot(stationConfig, runtimeSnapshot)
                    : CreateDefaultStationSnapshot(stationConfig, runtime);

                return OkSilent(snapshot, "PLC站运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC站运行态查询失败");
            }
        }

        /// <summary>
        /// 查询指定 PLC 点位运行态。
        /// </summary>
        public Result<PlcPointRuntimeSnapshot> QueryPoint(string pointName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pointName))
                {
                    return Fail<PlcPointRuntimeSnapshot>((int)DbErrorCode.InvalidArgument, "点位名称不能为空");
                }

                var normalizedPointName = pointName.Trim();
                var config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                var runtime = RuntimeContext.Instance.Plc;

                PlcPointRuntimeSnapshot runtimeSnapshot;
                var hasRuntimeSnapshot = runtime.TryGetPointSnapshot(normalizedPointName, out runtimeSnapshot) && runtimeSnapshot != null;

                var pointConfig = config.Points
                    .FirstOrDefault(p => p != null && string.Equals(p.Name, normalizedPointName, StringComparison.OrdinalIgnoreCase));

                if (!hasRuntimeSnapshot && pointConfig == null)
                {
                    return Warn<PlcPointRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 点位运行态");
                }

                var snapshot = hasRuntimeSnapshot
                    ? MergePointSnapshot(pointConfig, runtimeSnapshot)
                    : CreateDefaultPointSnapshot(pointConfig, runtime);

                return OkSilent(snapshot, "PLC点位运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC点位运行态查询失败");
            }
        }

        /// <summary>
        /// 查询全部 PLC 站运行态。
        /// </summary>
        public Result<PlcStationRuntimeSnapshot> QueryAllStations()
        {
            try
            {
                var config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                var runtime = RuntimeContext.Instance.Plc;
                var runtimeSnapshots = runtime.GetStationSnapshots();
                var runtimeLookup = runtimeSnapshots == null
                    ? new Dictionary<string, PlcStationRuntimeSnapshot>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, PlcStationRuntimeSnapshot>(runtimeSnapshots, StringComparer.OrdinalIgnoreCase);

                var items = new List<PlcStationRuntimeSnapshot>();
                var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var station in config.Stations.Where(p => p != null).OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
                {
                    PlcStationRuntimeSnapshot runtimeSnapshot;
                    runtimeLookup.TryGetValue(station.Name, out runtimeSnapshot);

                    items.Add(runtimeSnapshot == null
                        ? CreateDefaultStationSnapshot(station, runtime)
                        : MergeStationSnapshot(station, runtimeSnapshot));

                    processed.Add(station.Name);
                }

                foreach (var pair in runtimeLookup.OrderBy(p => p.Key))
                {
                    if (processed.Contains(pair.Key))
                    {
                        continue;
                    }

                    items.Add(pair.Value == null ? null : pair.Value.Clone());
                }

                return OkListSilent(items.Where(p => p != null).ToList(), "PLC站运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC站运行态查询失败");
            }
        }

        /// <summary>
        /// 查询全部 PLC 点位运行态。
        /// </summary>
        public Result<PlcPointRuntimeSnapshot> QueryAllPoints()
        {
            try
            {
                var config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                var runtime = RuntimeContext.Instance.Plc;
                var runtimeSnapshots = runtime.GetPointSnapshots();
                var runtimeLookup = runtimeSnapshots == null
                    ? new Dictionary<string, PlcPointRuntimeSnapshot>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, PlcPointRuntimeSnapshot>(runtimeSnapshots, StringComparer.OrdinalIgnoreCase);

                var items = new List<PlcPointRuntimeSnapshot>();
                var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var point in config.Points.Where(p => p != null).OrderBy(p => p.PlcName).ThenBy(p => p.SortOrder).ThenBy(p => p.Name))
                {
                    PlcPointRuntimeSnapshot runtimeSnapshot;
                    runtimeLookup.TryGetValue(point.Name, out runtimeSnapshot);

                    items.Add(runtimeSnapshot == null
                        ? CreateDefaultPointSnapshot(point, runtime)
                        : MergePointSnapshot(point, runtimeSnapshot));

                    processed.Add(point.Name);
                }

                foreach (var pair in runtimeLookup.OrderBy(p => p.Key))
                {
                    if (processed.Contains(pair.Key))
                    {
                        continue;
                    }

                    items.Add(pair.Value == null ? null : pair.Value.Clone());
                }

                return OkListSilent(items.Where(p => p != null).ToList(), "PLC点位运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC点位运行态查询失败");
            }
        }

        private static PlcStationRuntimeSnapshot CreateDefaultStationSnapshot(PlcStationConfig station, PlcRuntimeState runtime)
        {
            return new PlcStationRuntimeSnapshot
            {
                PlcName = station == null ? null : station.Name,
                DisplayName = station == null ? null : station.DisplayTitle,
                IsEnabled = station != null && station.IsEnabled,
                IsConnected = false,
                IsScanRunning = runtime.IsScanServiceRunning,
                LastConnectTime = null,
                LastScanTime = runtime.LastScanTime,
                LastError = runtime.IsScanServiceRunning ? null : "尚未建立运行态快照",
                SuccessReadCount = 0,
                FailedReadCount = 0,
                AverageReadMs = 0,
                AverageWriteMs = 0,
                CurrentProtocol = station == null ? null : station.ProtocolType,
                CurrentConnectionType = station == null ? null : station.ConnectionType
            };
        }

        private static PlcStationRuntimeSnapshot MergeStationSnapshot(PlcStationConfig station, PlcStationRuntimeSnapshot runtimeSnapshot)
        {
            var snapshot = runtimeSnapshot == null ? new PlcStationRuntimeSnapshot() : runtimeSnapshot.Clone();
            if (station == null)
            {
                return snapshot;
            }

            snapshot.PlcName = string.IsNullOrWhiteSpace(snapshot.PlcName) ? station.Name : snapshot.PlcName;
            snapshot.DisplayName = string.IsNullOrWhiteSpace(snapshot.DisplayName) ? station.DisplayTitle : snapshot.DisplayName;
            snapshot.IsEnabled = station.IsEnabled;
            snapshot.CurrentProtocol = string.IsNullOrWhiteSpace(snapshot.CurrentProtocol) ? station.ProtocolType : snapshot.CurrentProtocol;
            snapshot.CurrentConnectionType = string.IsNullOrWhiteSpace(snapshot.CurrentConnectionType) ? station.ConnectionType : snapshot.CurrentConnectionType;
            return snapshot;
        }

        private static PlcPointRuntimeSnapshot CreateDefaultPointSnapshot(PlcPointConfig point, PlcRuntimeState runtime)
        {
            return new PlcPointRuntimeSnapshot
            {
                PlcName = point == null ? null : point.PlcName,
                PointName = point == null ? null : point.Name,
                DisplayName = point == null ? null : point.DisplayName,
                GroupName = point == null ? null : point.GroupName,
                AddressText = point == null ? null : point.AddressText,
                AreaType = point == null ? null : point.AreaType,
                DataType = point == null ? null : point.DataType,
                ValueText = "--",
                RawValue = null,
                Quality = runtime.IsScanServiceRunning ? "Stale" : "Unknown",
                UpdateTime = runtime.LastScanTime ?? DateTime.MinValue,
                IsConnected = false,
                HasError = false,
                ErrorMessage = null
            };
        }

        private static PlcPointRuntimeSnapshot MergePointSnapshot(PlcPointConfig point, PlcPointRuntimeSnapshot runtimeSnapshot)
        {
            var snapshot = runtimeSnapshot == null ? new PlcPointRuntimeSnapshot() : runtimeSnapshot.Clone();
            if (point == null)
            {
                return snapshot;
            }

            snapshot.PlcName = string.IsNullOrWhiteSpace(snapshot.PlcName) ? point.PlcName : snapshot.PlcName;
            snapshot.PointName = string.IsNullOrWhiteSpace(snapshot.PointName) ? point.Name : snapshot.PointName;
            snapshot.DisplayName = string.IsNullOrWhiteSpace(snapshot.DisplayName) ? point.DisplayName : snapshot.DisplayName;
            snapshot.GroupName = string.IsNullOrWhiteSpace(snapshot.GroupName) ? point.GroupName : snapshot.GroupName;
            snapshot.AddressText = string.IsNullOrWhiteSpace(snapshot.AddressText) ? point.AddressText : snapshot.AddressText;
            snapshot.AreaType = string.IsNullOrWhiteSpace(snapshot.AreaType) ? point.AreaType : snapshot.AreaType;
            snapshot.DataType = string.IsNullOrWhiteSpace(snapshot.DataType) ? point.DataType : snapshot.DataType;
            return snapshot;
        }
    }
}
