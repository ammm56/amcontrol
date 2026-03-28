using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AM.Model.Runtime
{
    /// <summary>
    /// PLC 运行时状态缓存。
    /// 保存 PLC 站状态、点位值、扫描状态与更新时间。
    /// </summary>
    public class PlcRuntimeState
    {
        private readonly ConcurrentDictionary<string, PlcStationRuntimeSnapshot> _stationSnapshots;
        private readonly ConcurrentDictionary<string, PlcPointRuntimeSnapshot> _pointSnapshots;

        public PlcRuntimeState()
        {
            _stationSnapshots = new ConcurrentDictionary<string, PlcStationRuntimeSnapshot>();
            _pointSnapshots = new ConcurrentDictionary<string, PlcPointRuntimeSnapshot>();
        }

        /// <summary>
        /// 整轮扫描结果更新完成事件。
        /// </summary>
        public event Action SnapshotChanged;

        /// <summary>
        /// 单个 PLC 站状态更新事件。
        /// 参数为 PlcName。
        /// </summary>
        public event Action<string> StationSnapshotChanged;

        /// <summary>
        /// 单个点位状态更新事件。
        /// 参数为 PointName。
        /// </summary>
        public event Action<string> PointSnapshotChanged;

        /// <summary>
        /// 扫描服务是否运行中。
        /// </summary>
        public bool IsScanServiceRunning { get; private set; }

        /// <summary>
        /// 最近一次扫描时间。
        /// </summary>
        public DateTime? LastScanTime { get; private set; }

        /// <summary>
        /// 当前扫描周期，单位 ms。
        /// </summary>
        public int ScanIntervalMs { get; private set; }

        public void SetScanServiceState(bool isRunning, int scanIntervalMs)
        {
            IsScanServiceRunning = isRunning;
            ScanIntervalMs = scanIntervalMs;
        }

        public void MarkScanTime(DateTime time)
        {
            LastScanTime = time;
        }

        public void SetStationSnapshot(PlcStationRuntimeSnapshot snapshot)
        {
            if (snapshot == null || string.IsNullOrWhiteSpace(snapshot.PlcName))
            {
                return;
            }

            _stationSnapshots[snapshot.PlcName] = snapshot.Clone();
        }

        public void SetPointSnapshot(PlcPointRuntimeSnapshot snapshot)
        {
            if (snapshot == null || string.IsNullOrWhiteSpace(snapshot.PointName))
            {
                return;
            }

            _pointSnapshots[snapshot.PointName] = snapshot.Clone();
        }

        public bool TryGetStationSnapshot(string plcName, out PlcStationRuntimeSnapshot snapshot)
        {
            snapshot = null;

            if (string.IsNullOrWhiteSpace(plcName))
            {
                return false;
            }

            PlcStationRuntimeSnapshot cached;
            if (_stationSnapshots.TryGetValue(plcName, out cached))
            {
                snapshot = cached.Clone();
                return true;
            }

            return false;
        }

        public bool TryGetPointSnapshot(string pointName, out PlcPointRuntimeSnapshot snapshot)
        {
            snapshot = null;

            if (string.IsNullOrWhiteSpace(pointName))
            {
                return false;
            }

            PlcPointRuntimeSnapshot cached;
            if (_pointSnapshots.TryGetValue(pointName, out cached))
            {
                snapshot = cached.Clone();
                return true;
            }

            return false;
        }

        public IDictionary<string, PlcStationRuntimeSnapshot> GetStationSnapshots()
        {
            return _stationSnapshots.ToDictionary(p => p.Key, p => p.Value.Clone());
        }

        public IDictionary<string, PlcPointRuntimeSnapshot> GetPointSnapshots()
        {
            return _pointSnapshots.ToDictionary(p => p.Key, p => p.Value.Clone());
        }

        public IList<PlcPointRuntimeSnapshot> GetPointsByPlc(string plcName)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return new List<PlcPointRuntimeSnapshot>();
            }

            return _pointSnapshots.Values
                .Where(x => x != null && string.Equals(x.PlcName, plcName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Clone())
                .ToList();
        }

        public IList<PlcPointRuntimeSnapshot> GetPointsByGroup(string plcName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(plcName) || string.IsNullOrWhiteSpace(groupName))
            {
                return new List<PlcPointRuntimeSnapshot>();
            }

            return _pointSnapshots.Values
                .Where(x =>
                    x != null &&
                    string.Equals(x.PlcName, plcName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(x.GroupName, groupName, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Clone())
                .ToList();
        }

        public void NotifyStationSnapshotChanged(string plcName)
        {
            var handler = StationSnapshotChanged;
            if (handler != null)
            {
                handler(plcName);
            }
        }

        public void NotifyPointSnapshotChanged(string pointName)
        {
            var handler = PointSnapshotChanged;
            if (handler != null)
            {
                handler(pointName);
            }
        }

        public void NotifySnapshotChanged()
        {
            var handler = SnapshotChanged;
            if (handler != null)
            {
                handler();
            }
        }

        public void Clear()
        {
            _stationSnapshots.Clear();
            _pointSnapshots.Clear();
            LastScanTime = null;
            IsScanServiceRunning = false;
            ScanIntervalMs = 0;
            NotifySnapshotChanged();
        }
    }
}