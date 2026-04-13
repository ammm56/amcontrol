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
    /// </summary>
    public class PlcRuntimeQueryService : ServiceBase, IPlcRuntimeService
    {
        /// <summary>
        /// PLC 扫描工作单元。
        /// </summary>
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

        public Result Start()
        {
            return _plcScanWorker.Start();
        }

        public Result Stop()
        {
            return _plcScanWorker.Stop();
        }

        public Result ScanOnce()
        {
            return _plcScanWorker.ScanOnce();
        }

        public Result<PlcStationRuntimeSnapshot> QueryStation(string plcName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plcName))
                {
                    return Fail<PlcStationRuntimeSnapshot>((int)DbErrorCode.InvalidArgument, "PLC 名称不能为空");
                }

                string normalizedPlcName = plcName.Trim();
                PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                PlcRuntimeState runtime = RuntimeContext.Instance.Plc;

                PlcStationRuntimeSnapshot runtimeSnapshot;
                bool hasRuntimeSnapshot = runtime.TryGetStationSnapshot(normalizedPlcName, out runtimeSnapshot) && runtimeSnapshot != null;

                PlcStationConfig stationConfig = (config.Stations ?? new List<PlcStationConfig>())
                    .FirstOrDefault(p => p != null && string.Equals(p.Name, normalizedPlcName, StringComparison.OrdinalIgnoreCase));

                if (!hasRuntimeSnapshot && stationConfig == null)
                {
                    return Warn<PlcStationRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 站运行态");
                }

                PlcStationRuntimeSnapshot snapshot = hasRuntimeSnapshot
                    ? MergeStationSnapshot(stationConfig, runtimeSnapshot, runtime)
                    : CreateDefaultStationSnapshot(stationConfig, runtime);

                return OkSilent(snapshot, "PLC 站运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC 站运行态查询失败");
            }
        }

        public Result<PlcPointRuntimeSnapshot> QueryPoint(string pointName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pointName))
                {
                    return Fail<PlcPointRuntimeSnapshot>((int)DbErrorCode.InvalidArgument, "点位名称不能为空");
                }

                string normalizedPointName = pointName.Trim();
                PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                PlcRuntimeState runtime = RuntimeContext.Instance.Plc;

                PlcPointRuntimeSnapshot runtimeSnapshot;
                bool hasRuntimeSnapshot = runtime.TryGetPointSnapshot(normalizedPointName, out runtimeSnapshot) && runtimeSnapshot != null;

                PlcPointConfig pointConfig = (config.Points ?? new List<PlcPointConfig>())
                    .FirstOrDefault(p => p != null && string.Equals(p.Name, normalizedPointName, StringComparison.OrdinalIgnoreCase));

                if (!hasRuntimeSnapshot && pointConfig == null)
                {
                    return Warn<PlcPointRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 点位运行态");
                }

                PlcPointRuntimeSnapshot snapshot = hasRuntimeSnapshot
                    ? MergePointSnapshot(pointConfig, runtimeSnapshot)
                    : CreateDefaultPointSnapshot(pointConfig, runtime);

                return OkSilent(snapshot, "PLC 点位运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC 点位运行态查询失败");
            }
        }

        public Result<PlcStationRuntimeSnapshot> QueryAllStations()
        {
            try
            {
                PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                PlcRuntimeState runtime = RuntimeContext.Instance.Plc;

                var runtimeSnapshots = runtime.GetStationSnapshots();
                var runtimeLookup = runtimeSnapshots == null
                    ? new Dictionary<string, PlcStationRuntimeSnapshot>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, PlcStationRuntimeSnapshot>(runtimeSnapshots, StringComparer.OrdinalIgnoreCase);

                var items = new List<PlcStationRuntimeSnapshot>();
                var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (PlcStationConfig station in (config.Stations ?? new List<PlcStationConfig>())
                    .Where(p => p != null)
                    .OrderBy(p => p.SortOrder)
                    .ThenBy(p => p.Name))
                {
                    PlcStationRuntimeSnapshot runtimeSnapshot;
                    runtimeLookup.TryGetValue(station.Name, out runtimeSnapshot);

                    items.Add(runtimeSnapshot == null
                        ? CreateDefaultStationSnapshot(station, runtime)
                        : MergeStationSnapshot(station, runtimeSnapshot, runtime));

                    processed.Add(station.Name);
                }

                foreach (var pair in runtimeLookup.OrderBy(p => p.Key))
                {
                    if (processed.Contains(pair.Key))
                    {
                        continue;
                    }

                    var snapshot = pair.Value == null ? null : pair.Value.Clone();
                    items.Add(ApplyScanServiceState(snapshot, runtime));
                }

                return OkListSilent(items.Where(p => p != null).ToList(), "PLC 站运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcStationRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC 站运行态查询失败");
            }
        }

        public Result<PlcPointRuntimeSnapshot> QueryAllPoints()
        {
            try
            {
                PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                PlcRuntimeState runtime = RuntimeContext.Instance.Plc;

                var runtimeSnapshots = runtime.GetPointSnapshots();
                var runtimeLookup = runtimeSnapshots == null
                    ? new Dictionary<string, PlcPointRuntimeSnapshot>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, PlcPointRuntimeSnapshot>(runtimeSnapshots, StringComparer.OrdinalIgnoreCase);

                var items = new List<PlcPointRuntimeSnapshot>();
                var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (PlcPointConfig point in (config.Points ?? new List<PlcPointConfig>())
                    .Where(p => p != null)
                    .OrderBy(p => p.PlcName)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.Name))
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

                return OkListSilent(items.Where(p => p != null).ToList(), "PLC 点位运行态查询成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.QueryFailed, "PLC 点位运行态查询失败");
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
                IsScanRunning = runtime != null && runtime.IsScanServiceRunning,
                LastConnectTime = null,
                LastScanTime = runtime == null ? null : runtime.LastScanTime,
                LastError = runtime.IsScanServiceRunning ? null : "尚未建立运行态快照",
                SuccessReadCount = 0,
                FailedReadCount = 0,
                AverageReadMs = 0,
                AverageWriteMs = 0,
                CurrentProtocol = station == null ? null : station.ProtocolType,
                CurrentConnectionType = station == null ? null : station.ConnectionType
            };
        }

        private static PlcStationRuntimeSnapshot MergeStationSnapshot(
            PlcStationConfig station,
            PlcStationRuntimeSnapshot runtimeSnapshot,
            PlcRuntimeState runtime)
        {
            var snapshot = runtimeSnapshot == null ? new PlcStationRuntimeSnapshot() : runtimeSnapshot.Clone();

            if (station != null)
            {
                snapshot.PlcName = string.IsNullOrWhiteSpace(snapshot.PlcName) ? station.Name : snapshot.PlcName;
                snapshot.DisplayName = string.IsNullOrWhiteSpace(snapshot.DisplayName) ? station.DisplayTitle : snapshot.DisplayName;
                snapshot.IsEnabled = station.IsEnabled;
                snapshot.CurrentProtocol = string.IsNullOrWhiteSpace(snapshot.CurrentProtocol) ? station.ProtocolType : snapshot.CurrentProtocol;
                snapshot.CurrentConnectionType = string.IsNullOrWhiteSpace(snapshot.CurrentConnectionType) ? station.ConnectionType : snapshot.CurrentConnectionType;
            }

            return ApplyScanServiceState(snapshot, runtime);
        }

        private static PlcStationRuntimeSnapshot ApplyScanServiceState(
            PlcStationRuntimeSnapshot snapshot,
            PlcRuntimeState runtime)
        {
            if (snapshot == null)
            {
                return null;
            }

            snapshot.IsScanRunning = runtime != null && runtime.IsScanServiceRunning;

            if (!snapshot.LastScanTime.HasValue && runtime != null)
            {
                snapshot.LastScanTime = runtime.LastScanTime;
            }

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
                DataType = point == null ? null : point.DataType,
                AccessMode = point == null ? null : point.AccessMode,
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
            snapshot.DataType = string.IsNullOrWhiteSpace(snapshot.DataType) ? point.DataType : snapshot.DataType;
            snapshot.AccessMode = string.IsNullOrWhiteSpace(snapshot.AccessMode) ? point.AccessMode : snapshot.AccessMode;
            return snapshot;
        }
    }
}