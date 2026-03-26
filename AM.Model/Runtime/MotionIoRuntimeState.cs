using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AM.Model.Runtime
{
    /// <summary>
    /// Motion DI/DO 运行时状态缓存。
    /// 保存后台扫描后的逻辑点位值与更新时间。
    /// </summary>
    public class MotionIoRuntimeState
    {
        private readonly ConcurrentDictionary<short, bool> _diValues;
        private readonly ConcurrentDictionary<short, bool> _doValues;
        private readonly ConcurrentDictionary<short, DateTime> _diUpdateTimes;
        private readonly ConcurrentDictionary<short, DateTime> _doUpdateTimes;

        public MotionIoRuntimeState()
        {
            _diValues = new ConcurrentDictionary<short, bool>();
            _doValues = new ConcurrentDictionary<short, bool>();
            _diUpdateTimes = new ConcurrentDictionary<short, DateTime>();
            _doUpdateTimes = new ConcurrentDictionary<short, DateTime>();
        }

        /// <summary>
        /// 整轮扫描结果更新完成事件。
        /// ViewModel 订阅此事件后执行刷新。
        /// </summary>
        public event Action SnapshotChanged;

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

        public void SetDI(short logicalBit, bool value, DateTime updateTime)
        {
            _diValues[logicalBit] = value;
            _diUpdateTimes[logicalBit] = updateTime;
        }

        public void SetDO(short logicalBit, bool value, DateTime updateTime)
        {
            _doValues[logicalBit] = value;
            _doUpdateTimes[logicalBit] = updateTime;
        }

        public bool TryGetDI(short logicalBit, out bool value)
        {
            return _diValues.TryGetValue(logicalBit, out value);
        }

        public bool TryGetDO(short logicalBit, out bool value)
        {
            return _doValues.TryGetValue(logicalBit, out value);
        }

        public bool TryGetDIUpdateTime(short logicalBit, out DateTime updateTime)
        {
            return _diUpdateTimes.TryGetValue(logicalBit, out updateTime);
        }

        public bool TryGetDOUpdateTime(short logicalBit, out DateTime updateTime)
        {
            return _doUpdateTimes.TryGetValue(logicalBit, out updateTime);
        }

        public IDictionary<short, bool> GetDISnapshot()
        {
            return _diValues.ToDictionary(p => p.Key, p => p.Value);
        }

        public IDictionary<short, bool> GetDOSnapshot()
        {
            return _doValues.ToDictionary(p => p.Key, p => p.Value);
        }

        /// <summary>
        /// 通知订阅方：本轮扫描缓存已整体更新完成。
        /// </summary>
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
            _diValues.Clear();
            _doValues.Clear();
            _diUpdateTimes.Clear();
            _doUpdateTimes.Clear();
            LastScanTime = null;
            IsScanServiceRunning = false;
            ScanIntervalMs = 0;
            NotifySnapshotChanged();
        }
    }
}