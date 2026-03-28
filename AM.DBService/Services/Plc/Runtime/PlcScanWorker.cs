using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Interfaces.Plc.Runtime;
using AM.Model.Plc;
using AM.Model.Plc.Enums;
using AM.Model.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 后台扫描工作单元。
    /// 设计目标：
    /// 1. 适配工业现场高频扫描场景；
    /// 2. 成功扫描默认静默，不制造日志污染；
    /// 3. 单个 PLC 站异常不拖垮整轮扫描；
    /// 4. 优先使用显式读块配置，未覆盖点位再回退到单点读取；
    /// 5. 统一将运行态写入 RuntimeContext.Instance.Plc。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        private const int DefaultLoopIntervalMs = 100;
        private const int MinLoopIntervalMs = 20;
        private const string QualityGood = "Good";
        private const string QualityStale = "Stale";
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
                return ScanStations(forceAll: true, now: DateTime.Now);
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
                        var scanResult = ScanStations(forceAll: false, now: DateTime.Now);
                        if (!scanResult.Success)
                        {
                            // 这里只对工作单元级异常记失败。
                            // 单站连接中断或个别点位异常会写入运行态快照，不中断整体扫描循环。
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

                var pointsByPlc = plcConfig.Points
                    .Where(p => p != null && p.IsEnabled)
                    .GroupBy(p => p.PlcName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

                var readBlocksByPlc = plcConfig.ReadBlocks
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

                    List<PlcReadBlockConfig> stationBlocks;
                    if (!readBlocksByPlc.TryGetValue(station.Name, out stationBlocks))
                    {
                        stationBlocks = new List<PlcReadBlockConfig>();
                    }

                    ScanStation(machine, runtime, station, stationPoints, stationBlocks, now);
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
            IList<PlcReadBlockConfig> readBlocks,
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
            var successfulBlocks = new List<BlockScanResult>();
            var stationErrors = new List<string>();
            var stationHadReadFailure = false;

            var effectiveBlocks = BuildEffectiveReadBlocks(station, points, readBlocks);
            foreach (var block in effectiveBlocks)
            {
                var readResult = client.ReadBlock(block.AreaType, block.StartAddress, block.Length, block.DataType);
                if (!readResult.Success || readResult.Item == null)
                {
                    stationHadReadFailure = true;
                    stationErrors.Add(string.Format("读块失败 [{0}] {1} {2}: {3}",
                        block.BlockName,
                        block.AreaType,
                        block.StartAddress,
                        readResult.Message));
                    continue;
                }

                successfulBlocks.Add(new BlockScanResult(block, readResult.Item));
            }

            foreach (var point in points.OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
            {
                UpdatePointSnapshot(runtime, client, station, point, successfulBlocks, now, ref stationHadReadFailure, stationErrors);
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

            var snapshot = BuildStationSnapshot(runtime, station, now, false, IsRunning, lastConnectTime, true,
                string.IsNullOrWhiteSpace(errorMessage) ? new List<string>() : new List<string> { errorMessage }, 0D);

            runtime.SetStationSnapshot(snapshot);
            runtime.NotifyStationSnapshotChanged(station.Name);
        }

        private void UpdatePointSnapshot(
            PlcRuntimeState runtime,
            IPlcClient client,
            PlcStationConfig station,
            PlcPointConfig point,
            IList<BlockScanResult> successfulBlocks,
            DateTime now,
            ref bool stationHadReadFailure,
            IList<string> stationErrors)
        {
            object rawValue;
            string rawValueText;
            string displayValueText;
            string errorMessage;

            if (TryReadPointFromBlocks(point, successfulBlocks, out rawValue, out rawValueText, out displayValueText, out errorMessage))
            {
                runtime.SetPointSnapshot(CreatePointSnapshot(point, station.Name, now, true, false, null, rawValueText, displayValueText));
                return;
            }

            if (TryReadPointDirectly(client, point, out rawValue, out rawValueText, out displayValueText, out errorMessage))
            {
                runtime.SetPointSnapshot(CreatePointSnapshot(point, station.Name, now, true, false, null, rawValueText, displayValueText));
                return;
            }

            stationHadReadFailure = true;
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                stationErrors.Add(string.Format("点位读取失败 [{0}]: {1}", point.Name, errorMessage));
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
                ErrorMessage = string.IsNullOrWhiteSpace(errorMessage) ? "读取失败" : errorMessage
            });
        }

        private PlcPointRuntimeSnapshot CreatePointSnapshot(
            PlcPointConfig point,
            string plcName,
            DateTime now,
            bool isConnected,
            bool hasError,
            string errorMessage,
            string rawValueText,
            string displayValueText)
        {
            return new PlcPointRuntimeSnapshot
            {
                PlcName = plcName,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                AreaType = point.AreaType,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = hasError ? QualityError : QualityGood,
                UpdateTime = now,
                IsConnected = isConnected,
                HasError = hasError,
                ErrorMessage = errorMessage
            };
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

        private IList<PlcReadBlockConfig> BuildEffectiveReadBlocks(
            PlcStationConfig station,
            IList<PlcPointConfig> points,
            IList<PlcReadBlockConfig> configuredBlocks)
        {
            if (configuredBlocks != null && configuredBlocks.Count > 0)
            {
                return configuredBlocks
                    .Where(p => p != null && p.IsEnabled)
                    .OrderBy(p => p.Priority)
                    .ThenBy(p => p.SortOrder)
                    .ThenBy(p => p.BlockName)
                    .ToList();
            }

            var fallbackBlocks = new List<PlcReadBlockConfig>();
            if (points == null || points.Count == 0)
            {
                return fallbackBlocks;
            }

            foreach (var point in points.OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
            {
                fallbackBlocks.Add(new PlcReadBlockConfig
                {
                    PlcName = station.Name,
                    BlockName = "Point_" + point.Name,
                    AreaType = point.AreaType,
                    StartAddress = point.Address,
                    Length = GetPointReadLength(point),
                    ReadUnit = UsesWordUnit(point) ? "Word" : "Byte",
                    DataType = point.DataType,
                    ReadMode = point.ReadMode,
                    Priority = 0,
                    IsEnabled = true,
                    SortOrder = point.SortOrder,
                    Description = point.Description,
                    Remark = point.Remark
                });
            }

            return fallbackBlocks;
        }

        private bool TryReadPointFromBlocks(
            PlcPointConfig point,
            IList<BlockScanResult> successfulBlocks,
            out object rawValue,
            out string rawValueText,
            out string displayValueText,
            out string errorMessage)
        {
            rawValue = null;
            rawValueText = null;
            displayValueText = null;
            errorMessage = null;

            if (successfulBlocks == null || successfulBlocks.Count == 0)
            {
                return false;
            }

            foreach (var block in successfulBlocks)
            {
                if (!string.Equals(block.Config.AreaType, point.AreaType, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!TryResolvePointBytesFromBlock(point, block, out var pointBytes, out errorMessage))
                {
                    continue;
                }

                if (TryParsePointValue(point, pointBytes, out rawValue, out rawValueText, out displayValueText, out errorMessage))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryReadPointDirectly(
            IPlcClient client,
            PlcPointConfig point,
            out object rawValue,
            out string rawValueText,
            out string displayValueText,
            out string errorMessage)
        {
            rawValue = null;
            rawValueText = null;
            displayValueText = null;
            errorMessage = null;

            var readResult = client.ReadBlock(
                point.AreaType,
                point.Address,
                GetPointReadLength(point),
                point.DataType);

            if (!readResult.Success || readResult.Item == null || readResult.Item.Buffer == null || readResult.Item.Buffer.Length == 0)
            {
                errorMessage = readResult.Message;
                return false;
            }

            return TryParsePointValue(point, readResult.Item.Buffer, out rawValue, out rawValueText, out displayValueText, out errorMessage);
        }

        private bool TryResolvePointBytesFromBlock(
            PlcPointConfig point,
            BlockScanResult block,
            out byte[] pointBytes,
            out string errorMessage)
        {
            pointBytes = null;
            errorMessage = null;

            if (block == null || block.RawBlock == null || block.RawBlock.Buffer == null)
            {
                errorMessage = "读块为空";
                return false;
            }

            var blockStartAddress = ParseNumericAddress(block.Config.StartAddress);
            var pointAddress = ParseNumericAddress(point.Address);
            if (!blockStartAddress.HasValue || !pointAddress.HasValue)
            {
                errorMessage = "地址无法解析为数值偏移";
                return false;
            }

            var unitBytes = GetReadUnitByteSize(block.Config.ReadUnit);
            var byteOffset = (pointAddress.Value - blockStartAddress.Value) * unitBytes;
            if (byteOffset < 0)
            {
                errorMessage = "点位地址不在读块范围内";
                return false;
            }

            var requiredBytes = GetPointByteLength(point);
            if (requiredBytes <= 0)
            {
                errorMessage = "点位字节长度无效";
                return false;
            }

            if (block.RawBlock.Buffer.Length < byteOffset + requiredBytes)
            {
                errorMessage = "读块数据长度不足以解析点位";
                return false;
            }

            pointBytes = new byte[requiredBytes];
            Array.Copy(block.RawBlock.Buffer, byteOffset, pointBytes, 0, requiredBytes);
            return true;
        }

        private bool TryParsePointValue(
            PlcPointConfig point,
            byte[] bytes,
            out object rawValue,
            out string rawValueText,
            out string displayValueText,
            out string errorMessage)
        {
            rawValue = null;
            rawValueText = null;
            displayValueText = null;
            errorMessage = null;

            if (bytes == null || bytes.Length == 0)
            {
                errorMessage = "原始字节为空";
                return false;
            }

            try
            {
                switch ((point.DataType ?? string.Empty).Trim())
                {
                    case "Bit":
                    case "Bool":
                        rawValue = ReadBooleanValue(point, bytes);
                        rawValueText = ((bool)rawValue) ? "True" : "False";
                        displayValueText = ((bool)rawValue) ? "ON" : "OFF";
                        return true;

                    case "Byte":
                        rawValue = bytes[0];
                        rawValueText = ((byte)rawValue).ToString(CultureInfo.InvariantCulture);
                        displayValueText = rawValueText;
                        return true;

                    case "Short":
                        rawValue = BitConverter.ToInt16(ApplyEndian(bytes, point, 2), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "UShort":
                        rawValue = BitConverter.ToUInt16(ApplyEndian(bytes, point, 2), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "Int":
                        rawValue = BitConverter.ToInt32(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "UInt":
                        rawValue = BitConverter.ToUInt32(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "Float":
                        rawValue = BitConverter.ToSingle(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "Double":
                        rawValue = BitConverter.ToDouble(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "Long":
                        rawValue = BitConverter.ToInt64(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "ULong":
                        rawValue = BitConverter.ToUInt64(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);

                    case "String":
                        rawValue = ReadStringValue(point, bytes);
                        rawValueText = rawValue == null ? string.Empty : rawValue.ToString();
                        displayValueText = rawValueText;
                        return true;

                    case "ByteArray":
                        rawValue = bytes.ToArray();
                        rawValueText = ToHexString(bytes);
                        displayValueText = rawValueText;
                        return true;

                    default:
                        rawValue = bytes.ToArray();
                        rawValueText = ToHexString(bytes);
                        displayValueText = rawValueText;
                        return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static bool ReadBooleanValue(PlcPointConfig point, byte[] bytes)
        {
            if (!point.BitIndex.HasValue)
            {
                return bytes[0] != 0;
            }

            var bitIndex = point.BitIndex.Value;
            if (bitIndex < 0)
            {
                bitIndex = 0;
            }

            if (bytes.Length >= 2 && bitIndex < 16)
            {
                var word = BitConverter.ToUInt16(bytes.Length >= 2 ? bytes : new[] { bytes[0], (byte)0 }, 0);
                return (word & (1 << bitIndex)) != 0;
            }

            if (bitIndex > 7)
            {
                bitIndex = 7;
            }

            return (bytes[0] & (1 << bitIndex)) != 0;
        }

        private static string ReadStringValue(PlcPointConfig point, byte[] bytes)
        {
            var buffer = bytes;
            if (point.StringLength > 0 && point.StringLength < buffer.Length)
            {
                buffer = buffer.Take(point.StringLength).ToArray();
            }

            var encodingName = string.IsNullOrWhiteSpace(point.StringEncoding) ? "ASCII" : point.StringEncoding.Trim();
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(encodingName);
            }
            catch
            {
                encoding = Encoding.ASCII;
            }

            return encoding.GetString(buffer).TrimEnd('\0', ' ');
        }

        private static bool BuildNumericDisplay(
            PlcPointConfig point,
            double rawNumeric,
            out string rawValueText,
            out string displayValueText)
        {
            rawValueText = rawNumeric.ToString("0.########", CultureInfo.InvariantCulture);
            var scaled = (rawNumeric * (point.Scale == 0D ? 1D : point.Scale)) + point.Offset;
            displayValueText = string.IsNullOrWhiteSpace(point.Unit)
                ? scaled.ToString("0.########", CultureInfo.InvariantCulture)
                : string.Format(CultureInfo.InvariantCulture, "{0:0.########} {1}", scaled, point.Unit);
            return true;
        }

        private static byte[] ApplyEndian(byte[] bytes, PlcPointConfig point, int expectedLength)
        {
            var buffer = bytes.Take(expectedLength).ToArray();

            var wordOrder = point.WordOrder ?? string.Empty;
            if (buffer.Length == 4 && string.Equals(wordOrder, "HighLow", StringComparison.OrdinalIgnoreCase))
            {
                buffer = new[] { buffer[2], buffer[3], buffer[0], buffer[1] };
            }
            else if (buffer.Length == 8 && string.Equals(wordOrder, "HighLow", StringComparison.OrdinalIgnoreCase))
            {
                buffer = new[] { buffer[6], buffer[7], buffer[4], buffer[5], buffer[2], buffer[3], buffer[0], buffer[1] };
            }

            var byteOrder = point.ByteOrder ?? string.Empty;
            if (string.Equals(byteOrder, "BigEndian", StringComparison.OrdinalIgnoreCase))
            {
                Array.Reverse(buffer);
            }

            return buffer;
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
            return plcConfig.Stations
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private static int GetPointReadLength(PlcPointConfig point)
        {
            if (point.ReadLength > 0)
            {
                return point.ReadLength;
            }

            if (UsesWordUnit(point))
            {
                var byteLength = GetPointByteLength(point);
                return byteLength <= 0 ? 1 : (int)Math.Ceiling(byteLength / 2D);
            }

            return GetPointByteLength(point);
        }

        private static int GetPointByteLength(PlcPointConfig point)
        {
            switch ((point.DataType ?? string.Empty).Trim())
            {
                case "Bit":
                case "Bool":
                case "Byte":
                    return 1;
                case "Short":
                case "UShort":
                    return 2;
                case "Int":
                case "UInt":
                case "Float":
                    return 4;
                case "Double":
                case "Long":
                case "ULong":
                    return 8;
                case "String":
                    return point.StringLength > 0 ? point.StringLength : 1;
                case "ByteArray":
                    return point.ArrayLength > 0 ? point.ArrayLength : 1;
                default:
                    return 2;
            }
        }

        private static bool UsesWordUnit(PlcPointConfig point)
        {
            switch ((point.DataType ?? string.Empty).Trim())
            {
                case "Short":
                case "UShort":
                case "Int":
                case "UInt":
                case "Float":
                case "Double":
                case "Long":
                case "ULong":
                    return true;
                default:
                    return false;
            }
        }

        private static int GetReadUnitByteSize(string readUnit)
        {
            if (string.Equals(readUnit, "Word", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }

            return 1;
        }

        private static int? ParseNumericAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            var digits = new StringBuilder();
            var started = false;
            foreach (var ch in address)
            {
                if (char.IsDigit(ch))
                {
                    digits.Append(ch);
                    started = true;
                    continue;
                }

                if (started)
                {
                    break;
                }
            }

            if (digits.Length == 0)
            {
                return null;
            }

            int value;
            return int.TryParse(digits.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
                ? (int?)value
                : null;
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

        private static string ToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        private sealed class BlockScanResult
        {
            public BlockScanResult(PlcReadBlockConfig config, PlcRawDataBlock rawBlock)
            {
                Config = config;
                RawBlock = rawBlock;
            }

            public PlcReadBlockConfig Config { get; private set; }

            public PlcRawDataBlock RawBlock { get; private set; }
        }
    }
}
