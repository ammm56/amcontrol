using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Interfaces.Plc.Runtime;
using AM.Model.Plc;
using AM.Model.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 后台扫描工作单元。
    /// 当前版本采用最简单实现：
    /// 1. 按 PLC 站逐个扫描；
    /// 2. 按点位配置逐点读取；
    /// 3. 不做块聚合、块切片与地址偏移解析；
    /// 4. 单站异常不拖垮整轮扫描；
    /// 5. 统一将运行态写入 RuntimeContext.Instance.Plc。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        private const int DefaultLoopIntervalMs = 100;
        private const int MinLoopIntervalMs = 20;
        private const string QualityGood = "Good";
        private const string QualityDisconnected = "Disconnected";
        private const string QualityError = "Error";

        private readonly object _stateSyncRoot;
        private readonly object _scanSyncRoot;
        private readonly Dictionary<string, DateTime> _stationLastScanTimes;
        private readonly Dictionary<string, DateTime> _stationNextReconnectTimes;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _scanLoopTask;

        protected override string MessageSourceName
        {
            get { return "PlcScanWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Plc; }
        }

        public PlcScanWorker()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcScanWorker(IAppReporter reporter)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _scanSyncRoot = new object();
            _stationLastScanTimes = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
            _stationNextReconnectTimes = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 是否正在运行。
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_stateSyncRoot)
                {
                    return _cancellationTokenSource != null && _scanLoopTask != null;
                }
            }
        }

        /// <summary>
        /// 启动 PLC 扫描。
        /// </summary>
        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_cancellationTokenSource != null && _scanLoopTask != null)
                {
                    return Warn((int)DbErrorCode.InvalidArgument, "PLC 扫描工作单元已在运行");
                }

                var enabledStations = GetEnabledStations();
                if (enabledStations.Count == 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到启用的 PLC 站配置，无法启动扫描");
                }

                _cancellationTokenSource = new CancellationTokenSource();
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
                RuntimeContext.Instance.Plc.SetScanServiceState(true, GetLoopIntervalMs(enabledStations));
            }

            return OkLogOnly("PLC 扫描工作单元启动成功");
        }

        /// <summary>
        /// 停止 PLC 扫描。
        /// </summary>
        public Result Stop()
        {
            CancellationTokenSource cts;
            Task loopTask;

            lock (_stateSyncRoot)
            {
                cts = _cancellationTokenSource;
                loopTask = _scanLoopTask;
                _cancellationTokenSource = null;
                _scanLoopTask = null;
            }

            if (cts == null || loopTask == null)
            {
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
                return OkLogOnly("PLC 扫描工作单元未启动");
            }

            try
            {
                cts.Cancel();
                loopTask.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "停止 PLC 扫描工作单元失败");
            }
            finally
            {
                cts.Dispose();
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
            }

            return OkLogOnly("PLC 扫描工作单元已停止");
        }

        /// <summary>
        /// 执行单轮完整扫描。
        /// </summary>
        public Result ScanOnce()
        {
            try
            {
                return ScanStations(true, DateTime.Now);
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "执行 PLC 单轮扫描失败");
            }
        }

        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var enabledStations = GetEnabledStations();
                    RuntimeContext.Instance.Plc.SetScanServiceState(true, GetLoopIntervalMs(enabledStations));

                    if (enabledStations.Count > 0)
                    {
                        var scanResult = ScanStations(false, DateTime.Now);
                        if (!scanResult.Success)
                        {
                            Fail(scanResult.Code, scanResult.Message);
                        }
                    }

                    await Task.Delay(GetLoopIntervalMs(enabledStations), cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Fail((int)DbErrorCode.Unknown, "PLC 扫描循环异常", ex);
            }
            finally
            {
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
            }
        }

        private Result ScanStations(bool forceAll, DateTime now)
        {
            lock (_scanSyncRoot)
            {
                var runtime = RuntimeContext.Instance.Plc;
                var machine = MachineContext.Instance;
                var plcConfig = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
                var enabledStations = GetEnabledStations(plcConfig);

                if (enabledStations.Count == 0)
                {
                    return OkSilent("当前无启用 PLC 站，无需扫描");
                }

                var pointsByPlc = (plcConfig.Points ?? new List<PlcPointConfig>())
                    .Where(p => p != null && p.IsEnabled)
                    .GroupBy(p => p.PlcName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

                foreach (var station in enabledStations)
                {
                    if (!forceAll && !ShouldScanStation(station, now))
                    {
                        continue;
                    }

                    List<PlcPointConfig> stationPoints;
                    if (!pointsByPlc.TryGetValue(station.Name, out stationPoints))
                    {
                        stationPoints = new List<PlcPointConfig>();
                    }

                    ScanStation(machine, runtime, station, stationPoints, now);
                    _stationLastScanTimes[station.Name] = now;
                }

                runtime.MarkScanTime(now);
                runtime.NotifySnapshotChanged();
                return OkSilent("PLC 扫描成功");
            }
        }

        private void ScanStation(
            MachineContext machine,
            PlcRuntimeState runtime,
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            DateTime now)
        {
            IPlcClient client;
            if (!machine.Plcs.TryGetValue(station.Name, out client) || client == null)
            {
                UpdateStationDisconnected(runtime, station, points, now, "PLC 客户端未注册到 MachineContext");
                return;
            }

            bool isConnected;
            string connectError;
            DateTime? lastConnectTime;
            TryEnsureClientConnected(client, station, now, out isConnected, out connectError, out lastConnectTime);

            if (!isConnected)
            {
                UpdateStationDisconnected(runtime, station, points, now, connectError, lastConnectTime);
                return;
            }

            var scanStartedAt = DateTime.Now;
            var stationErrors = new List<string>();
            var stationHadReadFailure = false;

            foreach (var point in points.OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
            {
                UpdatePointSnapshot(runtime, client, station, point, now, ref stationHadReadFailure, stationErrors);
            }

            var averageReadMs = CalculateAverageReadMs(runtime, station.Name, (DateTime.Now - scanStartedAt).TotalMilliseconds);
            var stationSnapshot = BuildStationSnapshot(runtime, station, now, true, true, lastConnectTime, stationHadReadFailure, stationErrors, averageReadMs);
            runtime.SetStationSnapshot(stationSnapshot);
            runtime.NotifyStationSnapshotChanged(station.Name);
        }

        private void UpdateStationDisconnected(
            PlcRuntimeState runtime,
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            DateTime now,
            string errorMessage,
            DateTime? lastConnectTime = null)
        {
            foreach (var point in points.OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
            {
                runtime.SetPointSnapshot(new PlcPointRuntimeSnapshot
                {
                    PlcName = station.Name,
                    PointName = point.Name,
                    DisplayName = point.DisplayName,
                    GroupName = point.GroupName,
                    AddressText = point.AddressText,
                    AreaType = point.AreaType,
                    DataType = point.DataType,
                    ValueText = "--",
                    RawValue = null,
                    Quality = QualityDisconnected,
                    UpdateTime = now,
                    IsConnected = false,
                    HasError = true,
                    ErrorMessage = errorMessage
                });
            }

            var snapshot = BuildStationSnapshot(
                runtime,
                station,
                now,
                false,
                IsRunning,
                lastConnectTime,
                true,
                string.IsNullOrWhiteSpace(errorMessage) ? new List<string>() : new List<string> { errorMessage },
                0D);

            runtime.SetStationSnapshot(snapshot);
            runtime.NotifyStationSnapshotChanged(station.Name);
        }

        private void UpdatePointSnapshot(
            PlcRuntimeState runtime,
            IPlcClient client,
            PlcStationConfig station,
            PlcPointConfig point,
            DateTime now,
            ref bool stationHadReadFailure,
            IList<string> stationErrors)
        {
            var readResult = client.ReadPoint(new PlcPointReadRequest
            {
                PlcName = point.PlcName,
                AreaType = point.AreaType,
                Address = point.Address,
                DataType = point.DataType,
                BitIndex = point.BitIndex,
                StringLength = point.StringLength,
                ArrayLength = point.ArrayLength
            });

            if (!readResult.Success || readResult.Item == null)
            {
                stationHadReadFailure = true;
                if (!string.IsNullOrWhiteSpace(readResult.Message))
                {
                    stationErrors.Add(string.Format("点位读取失败 [{0}]: {1}", point.Name, readResult.Message));
                }

                runtime.SetPointSnapshot(new PlcPointRuntimeSnapshot
                {
                    PlcName = station.Name,
                    PointName = point.Name,
                    DisplayName = point.DisplayName,
                    GroupName = point.GroupName,
                    AddressText = point.AddressText,
                    AreaType = point.AreaType,
                    DataType = point.DataType,
                    ValueText = "ERR",
                    RawValue = null,
                    Quality = QualityError,
                    UpdateTime = now,
                    IsConnected = true,
                    HasError = true,
                    ErrorMessage = string.IsNullOrWhiteSpace(readResult.Message) ? "读取失败" : readResult.Message
                });
                return;
            }

            string rawValueText = readResult.Item.ValueText ?? string.Empty;
            string displayValueText = BuildReadDisplay(point, rawValueText);

            runtime.SetPointSnapshot(new PlcPointRuntimeSnapshot
            {
                PlcName = station.Name,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                AreaType = point.AreaType,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = QualityGood,
                UpdateTime = now,
                IsConnected = true,
                HasError = false,
                ErrorMessage = null
            });
        }

        private PlcStationRuntimeSnapshot BuildStationSnapshot(
            PlcRuntimeState runtime,
            PlcStationConfig station,
            DateTime now,
            bool isConnected,
            bool isScanRunning,
            DateTime? lastConnectTime,
            bool hasError,
            IList<string> errors,
            double averageReadMs)
        {
            PlcStationRuntimeSnapshot previous;
            runtime.TryGetStationSnapshot(station.Name, out previous);

            var successReadCount = previous == null ? 0L : previous.SuccessReadCount;
            var failedReadCount = previous == null ? 0L : previous.FailedReadCount;
            if (hasError)
            {
                failedReadCount++;
            }
            else
            {
                successReadCount++;
            }

            return new PlcStationRuntimeSnapshot
            {
                PlcName = station.Name,
                DisplayName = station.DisplayTitle,
                IsEnabled = station.IsEnabled,
                IsConnected = isConnected,
                IsScanRunning = isScanRunning,
                LastConnectTime = lastConnectTime ?? (previous == null ? (DateTime?)null : previous.LastConnectTime),
                LastScanTime = now,
                LastError = errors == null || errors.Count == 0 ? null : string.Join(" | ", errors.Distinct().Take(3)),
                SuccessReadCount = successReadCount,
                FailedReadCount = failedReadCount,
                AverageReadMs = averageReadMs,
                AverageWriteMs = previous == null ? 0D : previous.AverageWriteMs,
                CurrentProtocol = station.ProtocolType,
                CurrentConnectionType = station.ConnectionType
            };
        }

        private void TryEnsureClientConnected(
            IPlcClient client,
            PlcStationConfig station,
            DateTime now,
            out bool isConnected,
            out string errorMessage,
            out DateTime? lastConnectTime)
        {
            isConnected = false;
            errorMessage = null;
            lastConnectTime = null;

            var stateResult = client.IsConnected();
            if (stateResult.Success && stateResult.Item)
            {
                isConnected = true;
                lastConnectTime = now;
                _stationNextReconnectTimes.Remove(station.Name);
                return;
            }

            DateTime nextReconnectTime;
            if (_stationNextReconnectTimes.TryGetValue(station.Name, out nextReconnectTime) && now < nextReconnectTime)
            {
                errorMessage = stateResult.Success
                    ? "PLC 尚未连接，等待下一次重连窗口"
                    : stateResult.Message;
                return;
            }

            var connectResult = client.Connect();
            if (connectResult.Success)
            {
                isConnected = true;
                lastConnectTime = now;
                _stationNextReconnectTimes.Remove(station.Name);
                return;
            }

            errorMessage = connectResult.Message;
            _stationNextReconnectTimes[station.Name] = now.AddMilliseconds(GetReconnectIntervalMs(station));
        }

        private static string BuildReadDisplay(PlcPointConfig point, string rawValueText)
        {
            if (point == null)
            {
                return rawValueText ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(rawValueText))
            {
                return string.Empty;
            }

            if (string.Equals(point.DataType, "Bit", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "Bool", StringComparison.OrdinalIgnoreCase))
            {
                bool boolValue;
                if (TryConvertToBooleanText(rawValueText, out boolValue))
                {
                    return boolValue ? "ON" : "OFF";
                }

                return rawValueText;
            }

            if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase))
            {
                return rawValueText;
            }

            double numericValue;
            if (TryConvertToDoubleText(rawValueText, out numericValue))
            {
                string rawText;
                string displayText;
                BuildNumericDisplay(point, numericValue, out rawText, out displayText);
                return displayText;
            }

            return rawValueText;
        }

        private static bool BuildNumericDisplay(
            PlcPointConfig point,
            double rawNumeric,
            out string rawValueText,
            out string displayValueText)
        {
            rawValueText = rawNumeric.ToString("0.########", CultureInfo.InvariantCulture);
            var scaled = (rawNumeric * (point == null || point.Scale == 0D ? 1D : point.Scale)) + (point == null ? 0D : point.Offset);
            displayValueText = point == null || string.IsNullOrWhiteSpace(point.Unit)
                ? scaled.ToString("0.########", CultureInfo.InvariantCulture)
                : string.Format(CultureInfo.InvariantCulture, "{0:0.########} {1}", scaled, point.Unit);
            return true;
        }

        private static bool TryConvertToBooleanText(string valueText, out bool result)
        {
            result = false;
            if (string.IsNullOrWhiteSpace(valueText))
            {
                return false;
            }

            if (string.Equals(valueText, "1", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "on", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }

            if (string.Equals(valueText, "0", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "false", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "off", StringComparison.OrdinalIgnoreCase))
            {
                result = false;
                return true;
            }

            return bool.TryParse(valueText, out result);
        }

        private static bool TryConvertToDoubleText(string valueText, out double result)
        {
            return double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        private bool ShouldScanStation(PlcStationConfig station, DateTime now)
        {
            DateTime lastScanTime;
            if (!_stationLastScanTimes.TryGetValue(station.Name, out lastScanTime))
            {
                return true;
            }

            return (now - lastScanTime).TotalMilliseconds >= GetScanIntervalMs(station);
        }

        private static int GetLoopIntervalMs(IList<PlcStationConfig> stations)
        {
            if (stations == null || stations.Count == 0)
            {
                return DefaultLoopIntervalMs;
            }

            var min = stations
                .Where(p => p != null && p.IsEnabled)
                .Select(GetScanIntervalMs)
                .DefaultIfEmpty(DefaultLoopIntervalMs)
                .Min();

            return min < MinLoopIntervalMs ? MinLoopIntervalMs : min;
        }

        private static int GetScanIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ScanIntervalMs <= 0)
            {
                return DefaultLoopIntervalMs;
            }

            return station.ScanIntervalMs < MinLoopIntervalMs ? MinLoopIntervalMs : station.ScanIntervalMs;
        }

        private static int GetReconnectIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ReconnectIntervalMs <= 0)
            {
                return 2000;
            }

            return station.ReconnectIntervalMs;
        }

        private static IList<PlcStationConfig> GetEnabledStations()
        {
            return GetEnabledStations(ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig());
        }

        private static IList<PlcStationConfig> GetEnabledStations(PlcConfig plcConfig)
        {
            var stations = plcConfig.Stations ?? new List<PlcStationConfig>();
            return stations
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private double CalculateAverageReadMs(PlcRuntimeState runtime, string plcName, double currentElapsedMs)
        {
            PlcStationRuntimeSnapshot snapshot;
            if (!runtime.TryGetStationSnapshot(plcName, out snapshot) || snapshot == null || snapshot.AverageReadMs <= 0D)
            {
                return currentElapsedMs;
            }

            return Math.Round((snapshot.AverageReadMs * 0.7D) + (currentElapsedMs * 0.3D), 3);
        }
    }
}