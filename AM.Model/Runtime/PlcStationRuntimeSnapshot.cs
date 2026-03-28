using System;

namespace AM.Model.Runtime
{
    /// <summary>
    /// PLC 站运行时快照。
    /// </summary>
    public class PlcStationRuntimeSnapshot
    {
        public string PlcName { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsConnected { get; set; }

        public bool IsScanRunning { get; set; }

        public DateTime? LastConnectTime { get; set; }

        public DateTime? LastScanTime { get; set; }

        public string LastError { get; set; }

        public long SuccessReadCount { get; set; }

        public long FailedReadCount { get; set; }

        public double AverageReadMs { get; set; }

        public double AverageWriteMs { get; set; }

        public string CurrentProtocol { get; set; }

        public string CurrentConnectionType { get; set; }

        public PlcStationRuntimeSnapshot Clone()
        {
            return new PlcStationRuntimeSnapshot
            {
                PlcName = PlcName,
                DisplayName = DisplayName,
                IsEnabled = IsEnabled,
                IsConnected = IsConnected,
                IsScanRunning = IsScanRunning,
                LastConnectTime = LastConnectTime,
                LastScanTime = LastScanTime,
                LastError = LastError,
                SuccessReadCount = SuccessReadCount,
                FailedReadCount = FailedReadCount,
                AverageReadMs = AverageReadMs,
                AverageWriteMs = AverageWriteMs,
                CurrentProtocol = CurrentProtocol,
                CurrentConnectionType = CurrentConnectionType
            };
        }
    }
}