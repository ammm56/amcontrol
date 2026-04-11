using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using AM.Model.Runtime;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 站独立扫描运行器。
    /// 每个 PLC 站对应一个独立的后台循环任务，
    /// 用于实现站点级扫描隔离，避免单个离线站拖慢其他站点。
    /// </summary>
    internal sealed class PlcStationScanRunner : ServiceBase
    {
        private const string QualityGood = "Good";
        private const string QualityDisconnected = "Disconnected";
        private const string QualityError = "Error";
        private const int DefaultScanIntervalMs = 100;
        private const int MinScanIntervalMs = 20;
        private const int DefaultReconnectIntervalMs = 2000;
        private const int DisconnectSteadyLogIntervalMs = 30000;

        /// <summary>
        /// 运行器状态同步锁。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// 配置更新同步锁。
        /// </summary>
        private readonly object _configSyncRoot;

        /// <summary>
        /// PLC 客户端解析委托。
        /// 每轮扫描时动态取当前 MachineContext 中的客户端，避免配置重载后持有旧对象。
        /// </summary>
        private readonly Func<string, IPlcClient> _clientResolver;

        /// <summary>
        /// 当前站配置。
        /// </summary>
        private PlcStationConfig _station;

        /// <summary>
        /// 当前站点位列表快照。
        /// </summary>
        private IList<PlcPointConfig> _points;

        /// <summary>
        /// 本站扫描循环取消源。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 本站扫描循环任务。
        /// </summary>
        private Task _scanLoopTask;

        /// <summary>
        /// 下一次允许重连时间。
        /// 仅对本站生效，不影响其他 PLC 站。
        /// </summary>
        private DateTime? _nextReconnectTime;

        /// <summary>
        /// 最近一次真正连接成功时间。
        /// 只在 Connect 成功时更新，不在 IsConnected 成功时刷新。
        /// </summary>
        private DateTime? _lastConnectTime;

        /// <summary>
        /// 上一轮是否处于离线状态。
        /// 用于做“在线 → 离线”“离线 → 在线”的边沿通知。
        /// </summary>
        private bool _wasDisconnected;

        /// <summary>
        /// 最近一次已上报的离线错误文本。
        /// 若错误文本变化，允许立即再次上报一次边沿日志/消息。
        /// </summary>
        private string _lastDisconnectMessage = string.Empty;

        protected override string MessageSourceName
        {
            get { return "PlcStationScanRunner"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Plc; }
        }

        public PlcStationScanRunner(
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            Func<string, IPlcClient> clientResolver,
            IAppReporter reporter)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _configSyncRoot = new object();
            _station = station ?? new PlcStationConfig();
            _points = points == null ? new List<PlcPointConfig>() : points.ToList();
            _clientResolver = clientResolver;
            LastError = string.Empty;
        }

        /// <summary>
        /// 站名称。
        /// </summary>
        public string PlcName
        {
            get
            {
                lock (_configSyncRoot)
                {
                    return _station == null ? string.Empty : (_station.Name ?? string.Empty);
                }
            }
        }

        /// <summary>
        /// 当前运行器是否已启动。
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
        /// 最近一次扫描完成时间。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次错误信息。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 更新当前站配置与点位列表。
        /// </summary>
        public void UpdateConfig(PlcStationConfig station, IList<PlcPointConfig> points)
        {
            lock (_configSyncRoot)
            {
                _station = station ?? new PlcStationConfig();
                _points = points == null ? new List<PlcPointConfig>() : points.ToList();
            }
        }

        /// <summary>
        /// 启动本站扫描循环。
        /// </summary>
        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_cancellationTokenSource != null && _scanLoopTask != null)
                {
                    return Warn((int)DbErrorCode.InvalidArgument, "PLC 站扫描运行器已在运行: " + PlcName);
                }

                ResetDisconnectReportState();
                _cancellationTokenSource = new CancellationTokenSource();
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
            }

            return OkLogOnly("PLC 站扫描运行器启动成功: " + PlcName);
        }

        /// <summary>
        /// 停止本站扫描循环。
        /// </summary>
        public async Task<Result> StopAsync()
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
                return OkLogOnly("PLC 站扫描运行器未启动: " + PlcName);
            }

            try
            {
                cts.Cancel();
                await loopTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, (int)DbErrorCode.Unknown, "停止 PLC 站扫描运行器失败: " + PlcName);
            }
            finally
            {
                cts.Dispose();
                ResetDisconnectReportState();
            }

            return OkLogOnly("PLC 站扫描运行器已停止: " + PlcName);
        }

        /// <summary>
        /// 手动执行本站单轮扫描。
        /// </summary>
        public Task<Result> ScanOnceAsync()
        {
            return Task.Run(() => ScanOnceInternal(DateTime.Now));
        }

        /// <summary>
        /// 本站扫描循环。
        /// </summary>
        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    DateTime startedAt = DateTime.Now;
                    Result scanResult = ScanOnceInternal(startedAt);
                    if (!scanResult.Success)
                    {
                        LastError = scanResult.Message;
                    }
                    else
                    {
                        LastError = string.Empty;
                    }

                    await Task.Delay(GetScanIntervalMs(), cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Fail((int)DbErrorCode.Unknown, "PLC 站扫描循环异常: " + PlcName, ex);
            }
        }

        /// <summary>
        /// 执行本站单轮扫描核心逻辑。
        /// </summary>
        private Result ScanOnceInternal(DateTime startedAt)
        {
            PlcStationConfig station;
            IList<PlcPointConfig> points;

            lock (_configSyncRoot)
            {
                station = _station;
                points = _points == null ? new List<PlcPointConfig>() : _points.ToList();
            }

            if (station == null || string.IsNullOrWhiteSpace(station.Name))
            {
                return FailSilent((int)DbErrorCode.InvalidArgument, "PLC 站配置无效");
            }

            IPlcClient client = ResolveClient(station.Name);
            if (client == null)
            {
                const string message = "PLC 客户端未注册到 MachineContext";
                UpdateStationDisconnected(station, points, startedAt, message);
                LastError = message;
                ReportDisconnectIfNeeded(station, (int)DbErrorCode.NotFound, message);
                return WarnSilent((int)DbErrorCode.NotFound, message);
            }

            bool isConnected;
            string connectError;
            EnsureClientConnected(client, station, startedAt, out isConnected, out connectError);

            if (!isConnected)
            {
                string message = string.IsNullOrWhiteSpace(connectError) ? "PLC 未连接" : connectError;
                UpdateStationDisconnected(station, points, startedAt, message);
                LastError = message;
                ReportDisconnectIfNeeded(station, (int)DbErrorCode.Unknown, message);
                return WarnSilent((int)DbErrorCode.Unknown, message);
            }

            ReportReconnectIfNeeded(station);

            bool stationHadReadFailure = false;
            List<string> stationErrors = new List<string>();
            DateTime readStartedAt = DateTime.Now;

            foreach (PlcPointConfig point in points)
            {
                UpdatePointSnapshot(
                    client,
                    station,
                    point,
                    startedAt,
                    ref stationHadReadFailure,
                    stationErrors);
            }

            DateTime finishedAt = DateTime.Now;
            double averageReadMs = CalculateAverageReadMs(
                RuntimeContext.Instance.Plc,
                station.Name,
                (finishedAt - readStartedAt).TotalMilliseconds);

            PlcStationRuntimeSnapshot snapshot = BuildStationSnapshot(
                station,
                finishedAt,
                true,
                true,
                stationHadReadFailure,
                stationErrors,
                averageReadMs);

            RuntimeContext.Instance.Plc.SetStationSnapshot(snapshot);
            RuntimeContext.Instance.Plc.NotifyStationSnapshotChanged(station.Name);
            RuntimeContext.Instance.Plc.MarkScanTime(finishedAt);
            RuntimeContext.Instance.Plc.NotifySnapshotChanged();

            LastRunTime = finishedAt;
            LastError = string.Empty;
            return OkSilent("PLC 单站扫描成功: " + station.Name);
        }

        /// <summary>
        /// 解析当前 PLC 客户端。
        /// </summary>
        private IPlcClient ResolveClient(string plcName)
        {
            if (_clientResolver == null || string.IsNullOrWhiteSpace(plcName))
            {
                return null;
            }

            return _clientResolver(plcName);
        }

        /// <summary>
        /// 确保客户端已连接。
        /// 仅在本站重连窗口允许时尝试 Connect。
        /// </summary>
        private void EnsureClientConnected(
            IPlcClient client,
            PlcStationConfig station,
            DateTime now,
            out bool isConnected,
            out string errorMessage)
        {
            isConnected = false;
            errorMessage = null;

            Result<bool> stateResult = client.IsConnected();
            if (stateResult.Success && stateResult.Item)
            {
                isConnected = true;
                return;
            }

            if (_nextReconnectTime.HasValue && now < _nextReconnectTime.Value)
            {
                errorMessage = stateResult.Success
                    ? "PLC 离线，等待下一次重连"
                    : stateResult.Message;
                return;
            }

            Result connectResult = client.Connect();
            if (connectResult.Success)
            {
                isConnected = true;
                _lastConnectTime = now;
                _nextReconnectTime = null;
                return;
            }

            errorMessage = "PLC 离线，等待下一次重连";
            _nextReconnectTime = now.AddMilliseconds(GetReconnectIntervalMs(station));
        }

        /// <summary>
        /// 更新点位快照。
        /// </summary>
        private void UpdatePointSnapshot(
            IPlcClient client,
            PlcStationConfig station,
            PlcPointConfig point,
            DateTime now,
            ref bool stationHadReadFailure,
            IList<string> stationErrors)
        {
            Result<M_PointData> readResult = client.ReadPoint(new M_PointReadRequest
            {
                address = point.Address,
                dataType = NormalizeDataType(point.DataType),
                length = ResolvePointLength(point)
            });

            if (!readResult.Success || readResult.Item == null)
            {
                stationHadReadFailure = true;

                if (!string.IsNullOrWhiteSpace(readResult.Message))
                {
                    stationErrors.Add(string.Format("点位读取失败 [{0}]: {1}", point.Name, readResult.Message));
                }

                RuntimeContext.Instance.Plc.SetPointSnapshot(new PlcPointRuntimeSnapshot
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

                RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
                return;
            }

            string rawValueText = readResult.Item.value ?? string.Empty;
            string displayValueText = BuildReadDisplay(point, rawValueText);

            RuntimeContext.Instance.Plc.SetPointSnapshot(new PlcPointRuntimeSnapshot
            {
                PlcName = station.Name,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = string.IsNullOrWhiteSpace(readResult.Item.quality) ? QualityGood : readResult.Item.quality,
                UpdateTime = now,
                IsConnected = true,
                HasError = false,
                ErrorMessage = null
            });

            RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
        }

        /// <summary>
        /// 更新本站离线状态。
        /// </summary>
        private void UpdateStationDisconnected(
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            DateTime now,
            string errorMessage)
        {
            foreach (PlcPointConfig point in points)
            {
                RuntimeContext.Instance.Plc.SetPointSnapshot(new PlcPointRuntimeSnapshot
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

                RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
            }

            PlcStationRuntimeSnapshot snapshot = BuildStationSnapshot(
                station,
                now,
                false,
                IsRunning,
                true,
                string.IsNullOrWhiteSpace(errorMessage) ? new List<string>() : new List<string> { errorMessage },
                0D);

            RuntimeContext.Instance.Plc.SetStationSnapshot(snapshot);
            RuntimeContext.Instance.Plc.NotifyStationSnapshotChanged(station.Name);
            RuntimeContext.Instance.Plc.MarkScanTime(now);
            RuntimeContext.Instance.Plc.NotifySnapshotChanged();

            LastRunTime = now;
            LastError = errorMessage ?? string.Empty;
        }

        /// <summary>
        /// 构建站快照。
        /// </summary>
        private PlcStationRuntimeSnapshot BuildStationSnapshot(
            PlcStationConfig station,
            DateTime scanFinishedTime,
            bool isConnected,
            bool isScanRunning,
            bool hasError,
            IList<string> errors,
            double averageReadMs)
        {
            PlcStationRuntimeSnapshot previous;
            RuntimeContext.Instance.Plc.TryGetStationSnapshot(station.Name, out previous);

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
                LastConnectTime = _lastConnectTime ?? (previous == null ? (DateTime?)null : previous.LastConnectTime),
                LastScanTime = scanFinishedTime,
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
        /// 获取本站扫描周期。
        /// </summary>
        private int GetScanIntervalMs()
        {
            PlcStationConfig station;
            lock (_configSyncRoot)
            {
                station = _station;
            }

            if (station == null || station.ScanIntervalMs <= 0)
            {
                return DefaultScanIntervalMs;
            }

            return station.ScanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : station.ScanIntervalMs;
        }

        /// <summary>
        /// 获取本站重连周期。
        /// </summary>
        private static int GetReconnectIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ReconnectIntervalMs <= 0)
            {
                return DefaultReconnectIntervalMs;
            }

            return station.ReconnectIntervalMs;
        }

        /// <summary>
        /// 计算整轮读取平滑均值。
        /// </summary>
        private static double CalculateAverageReadMs(PlcRuntimeState runtime, string plcName, double currentElapsedMs)
        {
            PlcStationRuntimeSnapshot snapshot;
            if (!runtime.TryGetStationSnapshot(plcName, out snapshot) || snapshot == null || snapshot.AverageReadMs <= 0D)
            {
                return currentElapsedMs;
            }

            return Math.Round((snapshot.AverageReadMs * 0.7D) + (currentElapsedMs * 0.3D), 3);
        }

        /// <summary>
        /// 构建读取显示值。
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

            if (string.Equals(NormalizeDataType(point.DataType), "bool", StringComparison.OrdinalIgnoreCase))
            {
                bool boolValue;
                if (TryConvertToBooleanText(rawValueText, out boolValue))
                {
                    return boolValue ? "ON" : "OFF";
                }
            }

            return rawValueText;
        }

        /// <summary>
        /// 将文本转换为布尔值。
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
        /// 解析点位长度。
        /// </summary>
        private static int ResolvePointLength(PlcPointConfig point)
        {
            if (point == null || point.Length <= 0)
            {
                return 1;
            }

            return point.Length;
        }

        /// <summary>
        /// 规范化数据类型。
        /// </summary>
        private static string NormalizeDataType(string dataType)
        {
            return string.IsNullOrWhiteSpace(dataType)
                ? string.Empty
                : dataType.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// 按“边沿通知 + 稳态节流”上报离线信息。
        /// 规则：
        /// 1. 首次离线：日志 + 消息；
        /// 2. 错误文本变化：日志 + 消息；
        /// 3. 持续离线：仅按固定周期记录一条日志，不再弹消息。
        /// </summary>
        private void ReportDisconnectIfNeeded(PlcStationConfig station, int code, string message)
        {
            string finalMessage = string.IsNullOrWhiteSpace(message) ? "PLC 未连接" : message;
            string displayName = station == null ? PlcName : station.DisplayTitle;

            bool isEdgeDisconnect = !_wasDisconnected;
            bool errorChanged = !string.Equals(_lastDisconnectMessage, finalMessage, StringComparison.Ordinal);

            if (isEdgeDisconnect || errorChanged)
            {
                Warn(code, "PLC 站离线: " + displayName + "，" + finalMessage, ReportChannels.Log | ReportChannels.Message);
            }
            else if (ShouldReportRepeated("PLC-DISCONNECT-" + PlcName + "-" + finalMessage, DisconnectSteadyLogIntervalMs))
            {
                WarnLogOnly(code, "PLC 站持续离线: " + displayName + "，" + finalMessage);
            }

            _wasDisconnected = true;
            _lastDisconnectMessage = finalMessage;
        }

        /// <summary>
        /// 从离线恢复到在线时记录一条恢复日志。
        /// 恢复通常只写日志，不弹消息，避免干扰操作员。
        /// </summary>
        private void ReportReconnectIfNeeded(PlcStationConfig station)
        {
            if (!_wasDisconnected)
            {
                return;
            }

            string displayName = station == null ? PlcName : station.DisplayTitle;
            OkLogOnly("PLC 站恢复在线: " + displayName);

            _wasDisconnected = false;
            _lastDisconnectMessage = string.Empty;
        }

        /// <summary>
        /// 停止时重置离线状态记忆。
        /// 避免下次重新启动时沿用旧的边沿状态。
        /// </summary>
        private void ResetDisconnectReportState()
        {
            _wasDisconnected = false;
            _lastDisconnectMessage = string.Empty;
        }
    }
}