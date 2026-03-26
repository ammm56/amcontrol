using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AM.Model.Runtime
{
    /// <summary>
    /// Motion 轴运行时状态缓存。
    /// 仅供 UI / 监视 / 诊断使用，不参与运动控制安全判断。
    /// </summary>
    public class MotionAxisRuntimeState
    {
        private readonly ConcurrentDictionary<short, MotionAxisRuntimeSnapshot> _axisSnapshots;

        public MotionAxisRuntimeState()
        {
            _axisSnapshots = new ConcurrentDictionary<short, MotionAxisRuntimeSnapshot>();
        }

        /// <summary>
        /// 整轮采样完成事件。
        /// UI 层可订阅此事件刷新显示。
        /// </summary>
        public event Action SnapshotChanged;

        /// <summary>
        /// 单轴采样更新事件。
        /// 后续若某些页面只关注单轴，可订阅该事件。
        /// </summary>
        public event Action<short> AxisSnapshotChanged;

        /// <summary>
        /// 采样服务是否运行中。
        /// </summary>
        public bool IsScanServiceRunning { get; private set; }

        /// <summary>
        /// 最近一次采样时间。
        /// </summary>
        public DateTime? LastScanTime { get; private set; }

        /// <summary>
        /// 当前采样周期，单位 ms。
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

        public void SetAxisSnapshot(MotionAxisRuntimeSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            _axisSnapshots[snapshot.LogicalAxis] = snapshot.Clone();
        }

        public bool TryGetAxisSnapshot(short logicalAxis, out MotionAxisRuntimeSnapshot snapshot)
        {
            MotionAxisRuntimeSnapshot cached;
            if (_axisSnapshots.TryGetValue(logicalAxis, out cached))
            {
                snapshot = cached.Clone();
                return true;
            }

            snapshot = null;
            return false;
        }

        public IDictionary<short, MotionAxisRuntimeSnapshot> GetAxisSnapshots()
        {
            return _axisSnapshots.ToDictionary(p => p.Key, p => p.Value.Clone());
        }

        public void NotifyAxisSnapshotChanged(short logicalAxis)
        {
            var handler = AxisSnapshotChanged;
            if (handler != null)
            {
                handler(logicalAxis);
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
            _axisSnapshots.Clear();
            LastScanTime = null;
            IsScanServiceRunning = false;
            ScanIntervalMs = 0;
            NotifySnapshotChanged();
        }
    }

    /// <summary>
    /// 单轴运行态快照。
    /// 该对象是 UI 侧快照，不作为控制安全依据。
    /// </summary>
    public class MotionAxisRuntimeSnapshot
    {
        public short LogicalAxis { get; set; }

        public short CardId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string CardDisplayName { get; set; }

        public double CommandPositionPulse { get; set; }

        public double EncoderPositionPulse { get; set; }

        public double CommandPositionMm { get; set; }

        public double EncoderPositionMm { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsAlarm { get; set; }

        public bool IsAtHome { get; set; }

        public bool PositiveLimit { get; set; }

        public bool NegativeLimit { get; set; }

        public bool IsDone { get; set; }

        public bool IsMoving { get; set; }

        public DateTime UpdateTime { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(Name) ? "L#" + LogicalAxis : Name;
            }
        }

        public double PositionErrorMm
        {
            get { return CommandPositionMm - EncoderPositionMm; }
        }

        public string StateText
        {
            get
            {
                if (IsAlarm)
                {
                    return "Alarm";
                }

                if (IsMoving)
                {
                    return "Moving";
                }

                if (!IsEnabled)
                {
                    return "Disabled";
                }

                if (IsDone)
                {
                    return "Ready";
                }

                return "Idle";
            }
        }

        public string LimitStateText
        {
            get
            {
                if (PositiveLimit && NegativeLimit)
                {
                    return "正负限位";
                }

                if (PositiveLimit)
                {
                    return "正限位";
                }

                if (NegativeLimit)
                {
                    return "负限位";
                }

                return "无限位";
            }
        }

        public string SignalSummaryText
        {
            get
            {
                return "使能:" + (IsEnabled ? "Y" : "N")
                    + " / 原点:" + (IsAtHome ? "Y" : "N")
                    + " / 到位:" + (IsDone ? "Y" : "N");
            }
        }

        public MotionAxisRuntimeSnapshot Clone()
        {
            return new MotionAxisRuntimeSnapshot
            {
                LogicalAxis = LogicalAxis,
                CardId = CardId,
                Name = Name,
                DisplayName = DisplayName,
                CardDisplayName = CardDisplayName,
                CommandPositionPulse = CommandPositionPulse,
                EncoderPositionPulse = EncoderPositionPulse,
                CommandPositionMm = CommandPositionMm,
                EncoderPositionMm = EncoderPositionMm,
                IsEnabled = IsEnabled,
                IsAlarm = IsAlarm,
                IsAtHome = IsAtHome,
                PositiveLimit = PositiveLimit,
                NegativeLimit = NegativeLimit,
                IsDone = IsDone,
                IsMoving = IsMoving,
                UpdateTime = UpdateTime
            };
        }
    }
}