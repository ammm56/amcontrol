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
    /// 当前版本采用最简点位扫描模型：
    /// 1. 按站点逐个扫描；
    /// 2. 按点位逐个读取；
    /// 3. Address 直接表达完整协议地址；
    /// 4. 不做 AreaType、BitIndex、块切片与复杂映射；
    /// 5. 扫描过程中复用 MachineContext 中的 PLC 客户端；
    /// 6. 配置采用引用级缓存，避免每轮重复分组装配。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        /// <summary>
        /// 默认主循环周期，单位毫秒。
        /// </summary>
        private const int DefaultLoopIntervalMs = 100;

        /// <summary>
        /// 最小主循环周期，单位毫秒。
        /// </summary>
        private const int MinLoopIntervalMs = 20;

        /// <summary>
        /// 点位质量：成功。
        /// </summary>
        private const string QualityGood = "Good";

        /// <summary>
        /// 点位质量：站点断开。
        /// </summary>
        private const string QualityDisconnected = "Disconnected";

        /// <summary>
        /// 点位质量：读取失败。
        /// </summary>
        private const string QualityError = "Error";

        /// <summary>
        /// 启停同步锁。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// 扫描同步锁。
        /// 确保同一时刻只有一轮扫描在运行。
        /// </summary>
        private readonly object _scanSyncRoot;

        /// <summary>
        /// 各站最后扫描时间缓存。
        /// </summary>
        private readonly Dictionary<string, DateTime> _stationLastScanTimes;

        /// <summary>
        /// 各站下一次允许重连时间缓存。
        /// </summary>
        private readonly Dictionary<string, DateTime> _stationNextReconnectTimes;

        /// <summary>
        /// 扫描循环取消源。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 扫描循环任务。
        /// </summary>
        private Task _scanLoopTask;

        /// <summary>
        /// 当前缓存对应的 PLC 配置对象引用。
        /// 仅当配置对象整体被替换时重建缓存。
        /// </summary>
        private PlcConfig _cachedPlcConfig;

        /// <summary>
        /// 已缓存的启用站点列表。
        /// </summary>
        private IList<PlcStationConfig> _cachedEnabledStations;

        /// <summary>
        /// 按 PLC 名称缓存的点位列表。
        /// </summary>
        private IDictionary<string, IList<PlcPointConfig>> _cachedPointsByPlc;

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
            _cachedEnabledStations = new List<PlcStationConfig>();
            _cachedPointsByPlc = new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase);
            LastError = string.Empty;
        }

        /// <summary>
        /// 后台工作单元名称。
        /// </summary>
        public string Name
        {
            get { return "PlcScanWorker"; }
        }

        /// <summary>
        /// 当前是否正在运行。
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
        /// 最近一次成功运行时间。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次错误信息。
        /// </summary>
        public string LastError { get; private set; }

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

                RefreshScanCacheIfNeeded();
                if (_cachedEnabledStations == null || _cachedEnabledStations.Count == 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到启用的 PLC 站配置，无法启动扫描");
                }

                _cancellationTokenSource = new CancellationTokenSource();
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
                RuntimeContext.Instance.Plc.SetScanServiceState(true, GetLoopIntervalMs(_cachedEnabledStations));
            }

            return OkLogOnly("PLC 扫描工作单元启动成功");
        }

        /// <summary>
        /// 异步停止 PLC 扫描。
        /// </summary>
        public Task<Result> StopAsync()
        {
            return Task.FromResult(Stop());
        }

        /// <summary>
        /// 同步停止 PLC 扫描。
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
                LastError = ex.Message;
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
        /// 手动执行单轮完整扫描。
        /// </summary>
        public Result ScanOnce()
        {
            try
            {
                DateTime now = DateTime.Now;
                Result result = ScanStations(true, now);
                if (result.Success)
                {
                    LastRunTime = now;
                    LastError = string.Empty;
                }
                else
                {
                    LastError = result.Message;
                }

                return result;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, (int)DbErrorCode.Unknown, "执行 PLC 单轮扫描失败");
            }
        }

        /// <summary>
        /// 后台扫描循环。
        /// </summary>
        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    RefreshScanCacheIfNeeded();

                    if (_cachedEnabledStations.Count > 0)
                    {
                        RuntimeContext.Instance.Plc.SetScanServiceState(true, GetLoopIntervalMs(_cachedEnabledStations));

                        DateTime now = DateTime.Now;
                        Result scanResult = ScanStations(false, now);
                        if (!scanResult.Success)
                        {
                            LastError = scanResult.Message;
                            Fail(scanResult.Code, scanResult.Message);
                        }
                        else
                        {
                            LastRunTime = now;
                            LastError = string.Empty;
                        }
                    }

                    await Task.Delay(GetLoopIntervalMs(_cachedEnabledStations), cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Fail((int)DbErrorCode.Unknown, "PLC 扫描循环异常", ex);
            }
            finally
            {
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
            }
        }

        /// <summary>
        /// 执行一轮扫描调度。
        /// </summary>
        private Result ScanStations(bool forceAll, DateTime now)
        {
            lock (_scanSyncRoot)
            {
                RefreshScanCacheIfNeeded();

                if (_cachedEnabledStations.Count == 0)
                {
                    return OkSilent("当前无启用 PLC 站，无需扫描");
                }

                PlcRuntimeState runtime = RuntimeContext.Instance.Plc;
                MachineContext machine = MachineContext.Instance;

                foreach (PlcStationConfig station in _cachedEnabledStations)
                {
                    if (!forceAll && !ShouldScanStation(station, now))
                    {
                        continue;
                    }

                    IList<PlcPointConfig> stationPoints;
                    if (!_cachedPointsByPlc.TryGetValue(station.Name, out stationPoints))
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

        /// <summary>
        /// 刷新扫描缓存。
        /// 仅在配置对象引用变化时重建。
        /// </summary>
        private void RefreshScanCacheIfNeeded()
        {
            PlcConfig plcConfig = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
            if (object.ReferenceEquals(_cachedPlcConfig, plcConfig))
            {
                return;
            }

            _cachedPlcConfig = plcConfig;
            _cachedEnabledStations = BuildEnabledStations(plcConfig);
            _cachedPointsByPlc = BuildPointsByPlc(plcConfig);
        }

        /// <summary>
        /// 扫描单个 PLC 站。
        /// 客户端对象直接复用 MachineContext 中已经创建好的实例。
        /// </summary>
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

            DateTime scanStartedAt = DateTime.Now;
            List<string> stationErrors = new List<string>();
            bool stationHadReadFailure = false;

            foreach (PlcPointConfig point in points)
            {
                UpdatePointSnapshot(runtime, client, station, point, now, ref stationHadReadFailure, stationErrors);
            }

            double averageReadMs = CalculateAverageReadMs(runtime, station.Name, (DateTime.Now - scanStartedAt).TotalMilliseconds);
            PlcStationRuntimeSnapshot stationSnapshot = BuildStationSnapshot(runtime, station, now, true, true, lastConnectTime, stationHadReadFailure, stationErrors, averageReadMs);
            runtime.SetStationSnapshot(stationSnapshot);
            runtime.NotifyStationSnapshotChanged(station.Name);
        }

        /// <summary>
        /// 将整个站点更新为断开状态。
        /// </summary>
        private void UpdateStationDisconnected(
            PlcRuntimeState runtime,
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            DateTime now,
            string errorMessage,
            DateTime? lastConnectTime = null)
        {
            foreach (PlcPointConfig point in points)
            {
                runtime.SetPointSnapshot(new PlcPointRuntimeSnapshot
                {
                    PlcName = station.Name,
                    PointName = point.Name,
                    DisplayName = point.DisplayName,
                    GroupName = point.GroupName,
                    AddressText = point.AddressText,
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

            PlcStationRuntimeSnapshot snapshot = BuildStationSnapshot(
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

        /// <summary>
        /// 更新单个点位运行态快照。
        /// </summary>
        private void UpdatePointSnapshot(
            PlcRuntimeState runtime,
            IPlcClient client,
            PlcStationConfig station,
            PlcPointConfig point,
            DateTime now,
            ref bool stationHadReadFailure,
            IList<string> stationErrors)
        {
            Result<PlcPointReadResult> readResult = client.ReadPoint(new PlcPointReadRequest
            {
                PlcName = point.PlcName,
                Address = point.Address,
                DataType = point.DataType,
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

        /// <summary>
        /// 组装站点运行态快照。
        /// </summary>
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

            long successReadCount = previous == null ? 0L : previous.SuccessReadCount;
            long failedReadCount = previous == null ? 0L : previous.FailedReadCount;

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

        /// <summary>
        /// 确保客户端已连接。
        /// 使用简单重连节流，避免扫描线程高频反复连接。
        /// </summary>
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

            Result<bool> stateResult = client.IsConnected();
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

            Result connectResult = client.Connect();
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

        /// <summary>
        /// 构建点位显示值。
        /// 仅做最小显示转换。
        /// </summary>
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
            }

            double numericValue;
            if (!string.IsNullOrWhiteSpace(point.Unit) &&
                TryConvertToDoubleText(rawValueText, out numericValue))
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1}", rawValueText, point.Unit);
            }

            return rawValueText;
        }

        /// <summary>
        /// 将文本解析为布尔值。
        /// </summary>
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

        /// <summary>
        /// 将文本解析为数值。
        /// </summary>
        private static bool TryConvertToDoubleText(string valueText, out double result)
        {
            return double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// 判断当前站点本轮是否需要参与扫描。
        /// </summary>
        private bool ShouldScanStation(PlcStationConfig station, DateTime now)
        {
            DateTime lastScanTime;
            if (!_stationLastScanTimes.TryGetValue(station.Name, out lastScanTime))
            {
                return true;
            }

            return (now - lastScanTime).TotalMilliseconds >= GetScanIntervalMs(station);
        }

        /// <summary>
        /// 获取主循环周期。
        /// 取所有启用站点扫描周期中的最小值。
        /// </summary>
        private static int GetLoopIntervalMs(IList<PlcStationConfig> stations)
        {
            if (stations == null || stations.Count == 0)
            {
                return DefaultLoopIntervalMs;
            }

            int min = stations
                .Where(p => p != null && p.IsEnabled)
                .Select(GetScanIntervalMs)
                .DefaultIfEmpty(DefaultLoopIntervalMs)
                .Min();

            return min < MinLoopIntervalMs ? MinLoopIntervalMs : min;
        }

        /// <summary>
        /// 获取站点扫描周期。
        /// </summary>
        private static int GetScanIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ScanIntervalMs <= 0)
            {
                return DefaultLoopIntervalMs;
            }

            return station.ScanIntervalMs < MinLoopIntervalMs ? MinLoopIntervalMs : station.ScanIntervalMs;
        }

        /// <summary>
        /// 获取站点重连节流周期。
        /// </summary>
        private static int GetReconnectIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ReconnectIntervalMs <= 0)
            {
                return 2000;
            }

            return station.ReconnectIntervalMs;
        }

        /// <summary>
        /// 构建启用站点缓存。
        /// </summary>
        private static IList<PlcStationConfig> BuildEnabledStations(PlcConfig plcConfig)
        {
            var stations = plcConfig == null ? null : plcConfig.Stations;
            if (stations == null)
            {
                return new List<PlcStationConfig>();
            }

            return stations
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// 构建按 PLC 分组的点位缓存。
        /// 只在配置引用变化时重建，不在每轮扫描中重复分组。
        /// </summary>
        private static IDictionary<string, IList<PlcPointConfig>> BuildPointsByPlc(PlcConfig plcConfig)
        {
            var points = plcConfig == null ? null : plcConfig.Points;
            if (points == null)
            {
                return new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase);
            }

            return points
                .Where(p => p != null && p.IsEnabled)
                .GroupBy(p => p.PlcName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => (IList<PlcPointConfig>)g
                        .OrderBy(p => p.SortOrder)
                        .ThenBy(p => p.Name)
                        .ToList(),
                    StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算平滑平均读取耗时。
        /// </summary>
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