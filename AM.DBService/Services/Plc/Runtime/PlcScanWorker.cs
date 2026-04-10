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
    ///
    /// 当前实现原则：
    /// 1. 按 PLC 站逐个扫描；
    /// 2. 按点位配置逐点读取；
    /// 3. 不做块聚合、块切片与地址偏移解析；
    /// 4. 单站异常不拖垮整轮扫描；
    /// 5. 扫描结果统一写入 <see cref="RuntimeContext"/> 的 PLC 运行态缓存。
    ///
    /// 说明：
    /// - 本类只实现 <see cref="IPlcScanWorker"/>；
    /// - 由于 <see cref="IPlcScanWorker"/> 已继承 <see cref="AM.Model.Interfaces.Runtime.IRuntimeWorker"/>，
    ///   因此无需在类声明中重复实现 <c>IRuntimeWorker</c>；
    /// - 后续若要进一步优化性能，应优先从“初始化阶段预组装扫描计划与缓存引用”入手，
    ///   而不是在扫描循环中引入更多临时映射与复杂封装。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        /// <summary>
        /// 默认扫描主循环周期，单位毫秒。
        /// 当站点未显式配置扫描周期时，使用该值作为基础循环间隔。
        /// </summary>
        private const int DefaultLoopIntervalMs = 100;

        /// <summary>
        /// 允许的最小扫描主循环周期，单位毫秒。
        /// 用于避免配置过小导致后台线程高频空转。
        /// </summary>
        private const int MinLoopIntervalMs = 20;

        /// <summary>
        /// 点位质量标识：读取成功。
        /// </summary>
        private const string QualityGood = "Good";

        /// <summary>
        /// 点位质量标识：站点未连接或客户端不可用。
        /// </summary>
        private const string QualityDisconnected = "Disconnected";

        /// <summary>
        /// 点位质量标识：本次读取失败。
        /// </summary>
        private const string QualityError = "Error";

        /// <summary>
        /// 运行状态同步锁。
        /// 用于保护后台任务启动/停止相关状态，避免并发启动或并发停止。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// 扫描逻辑同步锁。
        /// 用于保证同一时刻只存在一轮扫描执行，避免扫描重入。
        /// </summary>
        private readonly object _scanSyncRoot;

        /// <summary>
        /// 各 PLC 站最近一次扫描时间缓存。
        /// 用于按站点扫描周期判断当前站点本轮是否需要参与扫描。
        /// 键为 PLC 站名称，值为最近一次成功调度扫描的时间。
        /// </summary>
        private readonly Dictionary<string, DateTime> _stationLastScanTimes;

        /// <summary>
        /// 各 PLC 站下一次允许重连时间缓存。
        /// 用于连接失败后做简单节流，避免每轮扫描都立即发起重连。
        /// 键为 PLC 站名称，值为下一次允许尝试连接的时间点。
        /// </summary>
        private readonly Dictionary<string, DateTime> _stationNextReconnectTimes;

        /// <summary>
        /// 当前扫描循环的取消令牌源。
        /// 启动扫描时创建，停止扫描时取消并释放。
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 当前后台扫描循环任务。
        /// 启动后保存任务引用，停止时等待其退出。
        /// </summary>
        private Task _scanLoopTask;

        /// <summary>
        /// 当前服务的消息源名称。
        /// 用于统一日志、消息与结果来源标识。
        /// </summary>
        protected override string MessageSourceName
        {
            get { return "PlcScanWorker"; }
        }

        /// <summary>
        /// 当前服务默认结果来源。
        /// PLC 扫描相关结果统一标记为 PLC 领域来源。
        /// </summary>
        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Plc; }
        }

        /// <summary>
        /// 使用全局 Reporter 创建 PLC 扫描工作单元。
        /// 适用于系统标准启动流程。
        /// </summary>
        public PlcScanWorker()
            : this(SystemContext.Instance.Reporter)
        {
        }

        /// <summary>
        /// 使用指定 Reporter 创建 PLC 扫描工作单元。
        /// 适用于测试、调试或外部显式注入场景。
        /// </summary>
        /// <param name="reporter">统一应用报告器。</param>
        public PlcScanWorker(IAppReporter reporter)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _scanSyncRoot = new object();
            _stationLastScanTimes = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
            _stationNextReconnectTimes = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
            LastError = string.Empty;
        }

        /// <summary>
        /// 后台工作单元名称。
        /// 供 <c>RuntimeTaskManager</c> 注册、查询与日志输出使用。
        /// </summary>
        public string Name
        {
            get { return "PlcScanWorker"; }
        }

        /// <summary>
        /// 是否正在运行。
        /// 当取消令牌源和循环任务均已创建时，视为扫描工作单元处于运行状态。
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
        /// 最近一次成功执行扫描的时间。
        /// 该值可用于状态页显示或后台任务运行状态诊断。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次扫描或扫描循环异常信息。
        /// 成功扫描后会清空，失败时记录最后一次错误描述。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 启动 PLC 扫描。
        /// 若当前已经运行，则返回告警结果而不重复启动。
        /// </summary>
        /// <returns>启动结果。</returns>
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
        /// 异步停止 PLC 扫描。
        /// 该方法用于满足统一后台工作单元接口要求，
        /// 内部仍复用同步停止逻辑。
        /// </summary>
        /// <returns>停止结果任务。</returns>
        public Task<Result> StopAsync()
        {
            return Task.FromResult(Stop());
        }

        /// <summary>
        /// 同步停止 PLC 扫描。
        /// 会取消后台循环并等待任务退出。
        /// </summary>
        /// <returns>停止结果。</returns>
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
        /// 适用于调试、诊断或测试场景。
        /// </summary>
        /// <returns>单轮扫描结果。</returns>
        public Result ScanOnce()
        {
            try
            {
                var now = DateTime.Now;
                var result = ScanStations(true, now);
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
        /// PLC 扫描后台循环。
        /// 循环按主周期执行调度，并在每轮内部根据各站点扫描周期决定是否实际扫描。
        /// </summary>
        /// <param name="cancellationToken">取消令牌。</param>
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
                        var now = DateTime.Now;
                        var scanResult = ScanStations(false, now);
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

                    await Task.Delay(GetLoopIntervalMs(enabledStations), cancellationToken).ConfigureAwait(false);
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
        /// 执行一轮 PLC 扫描调度。
        /// 该方法负责按站点扫描周期筛选当前应参与扫描的站点，并逐站调用扫描逻辑。
        /// </summary>
        /// <param name="forceAll">是否强制全站扫描。</param>
        /// <param name="now">本轮扫描时间。</param>
        /// <returns>扫描结果。</returns>
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

        /// <summary>
        /// 扫描单个 PLC 站。
        /// 该方法负责：
        /// 1. 获取站点客户端；
        /// 2. 检查/建立连接；
        /// 3. 按点位逐个读取；
        /// 4. 汇总站点扫描结果并写入运行态快照。
        /// </summary>
        /// <param name="machine">设备上下文。</param>
        /// <param name="runtime">PLC 运行态缓存。</param>
        /// <param name="station">当前 PLC 站配置。</param>
        /// <param name="points">当前站的点位列表。</param>
        /// <param name="now">当前扫描时间。</param>
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

        /// <summary>
        /// 将单个 PLC 站更新为“断开连接”状态。
        /// 当客户端不存在、连接失败或当前不允许重连时调用该方法。
        /// </summary>
        /// <param name="runtime">PLC 运行态缓存。</param>
        /// <param name="station">PLC 站配置。</param>
        /// <param name="points">该站点位列表。</param>
        /// <param name="now">当前时间。</param>
        /// <param name="errorMessage">错误描述。</param>
        /// <param name="lastConnectTime">最近一次连接成功时间。</param>
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

        /// <summary>
        /// 更新单个点位的运行态快照。
        /// 当前实现直接通过 <see cref="IPlcClient.ReadPoint"/> 按点位配置逐点读取。
        /// </summary>
        /// <param name="runtime">PLC 运行态缓存。</param>
        /// <param name="client">当前 PLC 客户端。</param>
        /// <param name="station">站配置。</param>
        /// <param name="point">点位配置。</param>
        /// <param name="now">当前时间。</param>
        /// <param name="stationHadReadFailure">站点本轮是否已有点位读取失败。</param>
        /// <param name="stationErrors">站点错误集合。</param>
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

        /// <summary>
        /// 组装站点运行态快照。
        /// 用于统一汇总站点连接状态、扫描状态、错误信息与统计信息。
        /// </summary>
        /// <param name="runtime">PLC 运行态缓存。</param>
        /// <param name="station">站配置。</param>
        /// <param name="now">当前时间。</param>
        /// <param name="isConnected">当前是否连接。</param>
        /// <param name="isScanRunning">当前扫描是否处于运行状态。</param>
        /// <param name="lastConnectTime">最近一次连接时间。</param>
        /// <param name="hasError">本轮是否有错误。</param>
        /// <param name="errors">错误列表。</param>
        /// <param name="averageReadMs">平均读取耗时。</param>
        /// <returns>站点运行态快照。</returns>
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

        /// <summary>
        /// 确保指定 PLC 客户端处于连接状态。
        /// 若当前未连接，则按重连节流策略尝试执行连接。
        /// </summary>
        /// <param name="client">PLC 客户端。</param>
        /// <param name="station">站配置。</param>
        /// <param name="now">当前时间。</param>
        /// <param name="isConnected">输出：当前是否已连接。</param>
        /// <param name="errorMessage">输出：错误信息。</param>
        /// <param name="lastConnectTime">输出：最近一次连接时间。</param>
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

        /// <summary>
        /// 构建点位显示值文本。
        /// 原始文本优先由协议层返回，本方法只做轻量显示转换：
        /// - 布尔值转为 ON/OFF；
        /// - 数值型应用 Scale / Offset / Unit；
        /// - 字符串与字节数组原样显示。
        /// </summary>
        /// <param name="point">点位配置。</param>
        /// <param name="rawValueText">协议层返回的原始值文本。</param>
        /// <returns>用于界面展示的文本。</returns>
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

        /// <summary>
        /// 根据点位缩放与单位配置构建数值显示文本。
        /// </summary>
        /// <param name="point">点位配置。</param>
        /// <param name="rawNumeric">原始数值。</param>
        /// <param name="rawValueText">输出：原始数值文本。</param>
        /// <param name="displayValueText">输出：显示数值文本。</param>
        /// <returns>始终返回 true，便于与其他解析流程统一形式。</returns>
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

        /// <summary>
        /// 将文本解析为布尔值。
        /// 支持：
        /// - 1 / 0
        /// - true / false
        /// - on / off
        /// </summary>
        /// <param name="valueText">待解析文本。</param>
        /// <param name="result">输出布尔值。</param>
        /// <returns>解析是否成功。</returns>
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
        /// 将文本解析为浮点数。
        /// 使用不依赖区域性的固定格式解析，避免现场环境语言差异影响数值读取。
        /// </summary>
        /// <param name="valueText">待解析文本。</param>
        /// <param name="result">输出数值。</param>
        /// <returns>解析是否成功。</returns>
        private static bool TryConvertToDoubleText(string valueText, out double result)
        {
            return double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// 判断当前站点在当前时刻是否需要参与扫描。
        /// 根据最近一次扫描时间与站点扫描周期配置进行判断。
        /// </summary>
        /// <param name="station">站配置。</param>
        /// <param name="now">当前时间。</param>
        /// <returns>是否应扫描。</returns>
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
        /// 计算后台主循环周期。
        /// 取全部启用站点扫描周期中的最小值，并受最小周期限制。
        /// </summary>
        /// <param name="stations">启用站点列表。</param>
        /// <returns>主循环周期，单位毫秒。</returns>
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

        /// <summary>
        /// 获取单个站点的扫描周期。
        /// 若未配置或配置非法，则回退到默认周期。
        /// </summary>
        /// <param name="station">站配置。</param>
        /// <returns>扫描周期，单位毫秒。</returns>
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
        /// 若未配置或配置非法，则使用默认 2000ms。
        /// </summary>
        /// <param name="station">站配置。</param>
        /// <returns>重连节流周期，单位毫秒。</returns>
        private static int GetReconnectIntervalMs(PlcStationConfig station)
        {
            if (station == null || station.ReconnectIntervalMs <= 0)
            {
                return 2000;
            }

            return station.ReconnectIntervalMs;
        }

        /// <summary>
        /// 从当前全局 PLC 配置中获取全部启用站点。
        /// </summary>
        /// <returns>启用站点列表。</returns>
        private static IList<PlcStationConfig> GetEnabledStations()
        {
            return GetEnabledStations(ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig());
        }

        /// <summary>
        /// 从指定 PLC 配置对象中获取全部启用站点。
        /// 按排序号和名称排序，保证扫描顺序稳定。
        /// </summary>
        /// <param name="plcConfig">PLC 配置对象。</param>
        /// <returns>启用站点列表。</returns>
        private static IList<PlcStationConfig> GetEnabledStations(PlcConfig plcConfig)
        {
            var stations = plcConfig.Stations ?? new List<PlcStationConfig>();
            return stations
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// 计算站点平均读取耗时。
        /// 若已有历史值，则采用简单平滑算法更新平均值；
        /// 否则直接使用当前轮耗时。
        /// </summary>
        /// <param name="runtime">PLC 运行态缓存。</param>
        /// <param name="plcName">站名称。</param>
        /// <param name="currentElapsedMs">当前轮读取耗时。</param>
        /// <returns>平滑后的平均读取耗时。</returns>
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