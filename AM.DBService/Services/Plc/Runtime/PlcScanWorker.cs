using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Interfaces.Plc.Runtime;
using AM.Model.Plc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 后台扫描工作单元。
    ///
    /// 当前版本重构为“顶层协调器 + 站级独立运行器”模型：
    /// 1. 本类不再在单个循环中串行扫描全部 PLC 站；
    /// 2. 每个 PLC 站由一个独立的 `PlcStationScanRunner` 负责循环扫描；
    /// 3. 本类只负责：
    ///    - 读取当前 PLC 配置；
    ///    - 创建、更新、删除站级运行器；
    ///    - 启动与停止全部站级运行器；
    ///    - 汇总整体扫描服务状态；
    ///    - 提供“全部站单轮扫描”入口；
    /// 4. 单个离线 PLC 站的连接阻塞、重连等待、点位读取失败，不再拖慢其他 PLC 站。
    ///
    /// 分层关系：
    /// - RuntimeTaskManager 只管理本顶层 Worker；
    /// - 本 Worker 内部再管理多个 `PlcStationScanRunner`；
    /// - 每个 Runner 独立 Task / 独立节流 / 独立异常隔离。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        /// <summary>
        /// 顶层协调循环默认刷新间隔。
        /// 用于检查配置变化、增删 runner，不表示 PLC 点位扫描周期。
        /// </summary>
        private const int DefaultSupervisorIntervalMs = 500;

        /// <summary>
        /// 最小扫描周期。
        /// 仅用于汇总状态时的防御性下限。
        /// </summary>
        private const int MinScanIntervalMs = 20;

        /// <summary>
        /// 状态同步锁。
        /// 用于保护 supervisor 启停状态。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// Runner 集合同步锁。
        /// 用于保护 PLC 站运行器字典并发访问。
        /// </summary>
        private readonly object _runnerSyncRoot;

        /// <summary>
        /// 顶层协调循环取消源。
        /// </summary>
        private CancellationTokenSource _supervisorCancellationTokenSource;

        /// <summary>
        /// 顶层协调循环任务。
        /// 负责配置对齐与运行器集合维护。
        /// </summary>
        private Task _supervisorTask;

        /// <summary>
        /// 当前缓存的 PLC 配置对象引用。
        /// 通过引用变化识别 ReloadFromDatabase 后的新配置。
        /// </summary>
        private PlcConfig _cachedPlcConfig;

        /// <summary>
        /// 当前启用 PLC 站查找表缓存。
        /// Key 为 PlcName。
        /// </summary>
        private IDictionary<string, PlcStationConfig> _cachedEnabledStationLookup;

        /// <summary>
        /// 当前按 PLC 名称分组的点位缓存。
        /// </summary>
        private IDictionary<string, IList<PlcPointConfig>> _cachedPointsByPlc;

        /// <summary>
        /// 当前已创建的站级运行器集合。
        /// 一个 PLC 站对应一个独立运行器。
        /// </summary>
        private readonly IDictionary<string, PlcStationScanRunner> _stationRunners;

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
            _runnerSyncRoot = new object();
            _cachedEnabledStationLookup = new Dictionary<string, PlcStationConfig>(StringComparer.OrdinalIgnoreCase);
            _cachedPointsByPlc = new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase);
            _stationRunners = new Dictionary<string, PlcStationScanRunner>(StringComparer.OrdinalIgnoreCase);
            LastError = string.Empty;
        }

        /// <summary>
        /// 顶层工作单元名称。
        /// 供 RuntimeTaskManager 注册与启停。
        /// </summary>
        public string Name
        {
            get { return "PlcScanWorker"; }
        }

        /// <summary>
        /// 当前顶层工作单元是否运行中。
        /// 这里表示 supervisor 是否运行，而不是某个单站 runner 是否运行。
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_stateSyncRoot)
                {
                    return _supervisorCancellationTokenSource != null && _supervisorTask != null;
                }
            }
        }

        /// <summary>
        /// 最近一次任意 PLC 站完成扫描的时间。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次顶层错误信息。
        /// 若顶层无异常，则取当前第一个存在错误的站级运行器错误。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 启动 PLC 扫描工作单元。
        /// 启动顺序：
        /// 1. 刷新配置缓存；
        /// 2. 对齐站级 runner 集合；
        /// 3. 启动全部 runner；
        /// 4. 更新 RuntimeContext 中的扫描服务状态；
        /// 5. 最后启动 supervisor 持续维护配置变化。
        /// </summary>
        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_supervisorCancellationTokenSource != null && _supervisorTask != null)
                {
                    return Warn((int)DbErrorCode.InvalidArgument, "PLC 扫描工作单元已在运行");
                }

                RefreshScanCacheIfNeeded();
                if (_cachedEnabledStationLookup == null || _cachedEnabledStationLookup.Count == 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到启用的 PLC 站配置，无法启动扫描");
                }

                _supervisorCancellationTokenSource = new CancellationTokenSource();

                ReconcileRunners();
                StartAllRunners();
                UpdateRuntimeServiceState();

                _supervisorTask = Task.Run(() => SupervisorLoopAsync(_supervisorCancellationTokenSource.Token));
            }

            return OkLogOnly("PLC 扫描工作单元启动成功");
        }

        /// <summary>
        /// 异步停止 PLC 扫描工作单元。
        /// </summary>
        public Task<Result> StopAsync()
        {
            return Task.FromResult(Stop());
        }

        /// <summary>
        /// 同步停止 PLC 扫描工作单元。
        /// 停止顺序：
        /// 1. 先停止 supervisor；
        /// 2. supervisor 的 finally 中统一停止全部站级 runner；
        /// 3. 最后将 RuntimeContext 中扫描服务状态置为 false。
        /// </summary>
        public Result Stop()
        {
            CancellationTokenSource cts;
            Task supervisorTask;

            lock (_stateSyncRoot)
            {
                cts = _supervisorCancellationTokenSource;
                supervisorTask = _supervisorTask;
                _supervisorCancellationTokenSource = null;
                _supervisorTask = null;
            }

            if (cts == null || supervisorTask == null)
            {
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
                return OkLogOnly("PLC 扫描工作单元未启动");
            }

            try
            {
                cts.Cancel();
                supervisorTask.GetAwaiter().GetResult();
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
        /// 手动执行全部启用 PLC 站的单轮扫描。
        /// 实现方式：
        /// 1. 先确保 runner 集合与当前配置对齐；
        /// 2. 对所有 runner 并发执行 `ScanOnceAsync()`；
        /// 3. 汇总结果并更新整体扫描服务状态。
        /// </summary>
        public Result ScanOnce()
        {
            try
            {
                RefreshScanCacheIfNeeded();

                if (_cachedEnabledStationLookup == null || _cachedEnabledStationLookup.Count == 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到启用的 PLC 站配置，无法执行单轮扫描");
                }

                ReconcileRunners();

                PlcStationScanRunner[] runners;
                lock (_runnerSyncRoot)
                {
                    runners = _stationRunners.Values.ToArray();
                }

                if (runners.Length == 0)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到可执行单轮扫描的 PLC 站");
                }

                Task<Result>[] tasks = runners
                    .Where(x => x != null)
                    .Select(x => x.ScanOnceAsync())
                    .ToArray();

                Task.WaitAll(tasks);

                List<Result> failedResults = tasks
                    .Where(x => x.Status == TaskStatus.RanToCompletion && x.Result != null && !x.Result.Success)
                    .Select(x => x.Result)
                    .ToList();

                UpdateRuntimeServiceState();

                if (failedResults.Count > 0)
                {
                    string message = string.Join(
                        " | ",
                        failedResults
                            .Select(x => x.Message)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .Take(3));

                    return Result.Fail(
                        failedResults[0].Code,
                        string.IsNullOrWhiteSpace(message) ? "PLC 单轮扫描部分失败" : message,
                        ResultSource.Plc);
                }

                return OkSilent("PLC 单轮扫描成功");
            }
            catch (AggregateException ex)
            {
                string message = ex.Flatten().InnerExceptions.FirstOrDefault() == null
                    ? ex.Message
                    : ex.Flatten().InnerExceptions.First().Message;

                LastError = message;
                return Result.Fail((int)DbErrorCode.Unknown, "执行 PLC 单轮扫描失败: " + message, ResultSource.Plc);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, (int)DbErrorCode.Unknown, "执行 PLC 单轮扫描失败");
            }
        }

        /// <summary>
        /// 顶层协调循环。
        /// 当前循环不直接读 PLC 点位，只负责：
        /// 1. 监视配置引用是否变化；
        /// 2. 对齐站级 runner 集合；
        /// 3. 启动新增但尚未运行的 runner；
        /// 4. 汇总扫描服务整体状态。
        /// </summary>
        private async Task SupervisorLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    RefreshScanCacheIfNeeded();
                    ReconcileRunners();
                    StartAllRunners();
                    UpdateRuntimeServiceState();

                    await Task.Delay(DefaultSupervisorIntervalMs, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Fail((int)DbErrorCode.Unknown, "PLC 扫描协调循环异常", ex);
            }
            finally
            {
                StopAllRunnersAsync().GetAwaiter().GetResult();
                RuntimeContext.Instance.Plc.SetScanServiceState(false, 0);
            }
        }

        /// <summary>
        /// 如 PLC 配置对象引用变化，则刷新缓存。
        /// 当前系统中 `ReloadFromDatabase()` 会替换整个 `PlcConfig` 引用，
        /// 因此这里通过引用比较识别是否需要重新构建缓存。
        /// </summary>
        private void RefreshScanCacheIfNeeded()
        {
            PlcConfig plcConfig = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
            if (object.ReferenceEquals(_cachedPlcConfig, plcConfig))
            {
                return;
            }

            _cachedPlcConfig = plcConfig;
            _cachedEnabledStationLookup = BuildEnabledStationLookup(plcConfig);
            _cachedPointsByPlc = BuildPointsByPlc(plcConfig);
        }

        /// <summary>
        /// 根据当前缓存配置对齐站级 runner 集合。
        /// 包含三类动作：
        /// 1. 删除并停止已不存在或已禁用的站 runner；
        /// 2. 创建新增站 runner；
        /// 3. 更新已存在 runner 的站配置与点位列表。
        /// </summary>
        private void ReconcileRunners()
        {
            IDictionary<string, PlcStationConfig> stationLookup = _cachedEnabledStationLookup == null
                ? new Dictionary<string, PlcStationConfig>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, PlcStationConfig>(_cachedEnabledStationLookup, StringComparer.OrdinalIgnoreCase);

            IDictionary<string, IList<PlcPointConfig>> pointsByPlc = _cachedPointsByPlc == null
                ? new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, IList<PlcPointConfig>>(_cachedPointsByPlc, StringComparer.OrdinalIgnoreCase);

            List<PlcStationScanRunner> removedRunners = new List<PlcStationScanRunner>();

            lock (_runnerSyncRoot)
            {
                List<string> removedKeys = _stationRunners.Keys
                    .Where(x => !stationLookup.ContainsKey(x))
                    .ToList();

                foreach (string key in removedKeys)
                {
                    removedRunners.Add(_stationRunners[key]);
                    _stationRunners.Remove(key);
                }

                foreach (KeyValuePair<string, PlcStationConfig> pair in stationLookup)
                {
                    IList<PlcPointConfig> points;
                    if (!pointsByPlc.TryGetValue(pair.Key, out points))
                    {
                        points = new List<PlcPointConfig>();
                    }

                    PlcStationScanRunner runner;
                    if (_stationRunners.TryGetValue(pair.Key, out runner))
                    {
                        runner.UpdateConfig(pair.Value, points);
                        continue;
                    }

                    runner = new PlcStationScanRunner(
                        pair.Value,
                        points,
                        ResolveClient,
                        SystemContext.Instance.Reporter);

                    _stationRunners[pair.Key] = runner;
                }
            }

            foreach (PlcStationScanRunner runner in removedRunners)
            {
                try
                {
                    runner.StopAsync().GetAwaiter().GetResult();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 启动全部尚未运行的站级 runner。
        /// 已运行的 runner 跳过。
        /// </summary>
        private void StartAllRunners()
        {
            PlcStationScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _stationRunners.Values.ToArray();
            }

            foreach (PlcStationScanRunner runner in runners)
            {
                if (runner == null || runner.IsRunning)
                {
                    continue;
                }

                Result startResult = runner.Start();
                if (!startResult.Success)
                {
                    _reporter.Warn(
                        MessageSourceName,
                        "PLC 站扫描运行器启动失败: " + runner.PlcName + "，" + startResult.Message,
                        startResult.Code);
                }
            }
        }

        /// <summary>
        /// 停止全部站级 runner。
        /// </summary>
        private async Task StopAllRunnersAsync()
        {
            PlcStationScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _stationRunners.Values.ToArray();
                _stationRunners.Clear();
            }

            foreach (PlcStationScanRunner runner in runners)
            {
                if (runner == null)
                {
                    continue;
                }

                try
                {
                    await runner.StopAsync().ConfigureAwait(false);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 汇总整体扫描服务状态，并写回 RuntimeContext。
        /// 汇总项包括：
        /// 1. 是否有任意站级 runner 正在运行；
        /// 2. 当前最小扫描周期；
        /// 3. 最近一次任意站扫描完成时间；
        /// 4. 当前第一个非空错误信息。
        /// </summary>
        private void UpdateRuntimeServiceState()
        {
            PlcStationScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _stationRunners.Values.ToArray();
            }

            bool hasRunningRunner = runners.Any(x => x != null && x.IsRunning);
            int scanIntervalMs = GetMinScanIntervalMs(
                _cachedEnabledStationLookup == null
                    ? new List<PlcStationConfig>()
                    : _cachedEnabledStationLookup.Values.ToList());

            RuntimeContext.Instance.Plc.SetScanServiceState(
                hasRunningRunner,
                hasRunningRunner ? scanIntervalMs : 0);

            DateTime latestRunTime = runners
                .Where(x => x != null && x.LastRunTime.HasValue)
                .Select(x => x.LastRunTime.Value)
                .DefaultIfEmpty()
                .Max();

            LastRunTime = latestRunTime == default(DateTime) ? (DateTime?)null : latestRunTime;

            string runnerError = runners
                .Where(x => x != null && !string.IsNullOrWhiteSpace(x.LastError))
                .Select(x => x.LastError)
                .FirstOrDefault();

            LastError = string.IsNullOrWhiteSpace(runnerError) ? string.Empty : runnerError;
        }

        /// <summary>
        /// 从 MachineContext 中动态解析当前 PLC 客户端。
        /// 配置重载后客户端对象会重建，因此不在 runner 内长期持有客户端实例。
        /// </summary>
        private static IPlcClient ResolveClient(string plcName)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return null;
            }

            IPlcClient client;
            return MachineContext.Instance.Plcs.TryGetValue(plcName, out client) ? client : null;
        }

        /// <summary>
        /// 构建启用站查找表。
        /// </summary>
        private static IDictionary<string, PlcStationConfig> BuildEnabledStationLookup(PlcConfig plcConfig)
        {
            IList<PlcStationConfig> stations = plcConfig == null ? null : plcConfig.Stations;
            if (stations == null)
            {
                return new Dictionary<string, PlcStationConfig>(StringComparer.OrdinalIgnoreCase);
            }

            return stations
                .Where(x => x != null && x.IsEnabled && !string.IsNullOrWhiteSpace(x.Name))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 构建按 PLC 名称分组的点位表。
        /// </summary>
        private static IDictionary<string, IList<PlcPointConfig>> BuildPointsByPlc(PlcConfig plcConfig)
        {
            IList<PlcPointConfig> points = plcConfig == null ? null : plcConfig.Points;
            if (points == null)
            {
                return new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase);
            }

            return points
                .Where(x => x != null && x.IsEnabled)
                .GroupBy(x => x.PlcName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => (IList<PlcPointConfig>)g
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Name)
                        .ToList(),
                    StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取当前启用站中的最小扫描周期。
        /// 该值仅用于整体扫描服务状态显示。
        /// </summary>
        private static int GetMinScanIntervalMs(IList<PlcStationConfig> stations)
        {
            if (stations == null || stations.Count == 0)
            {
                return 0;
            }

            int min = stations
                .Where(x => x != null && x.IsEnabled)
                .Select(x => x.ScanIntervalMs <= 0 ? 100 : x.ScanIntervalMs)
                .DefaultIfEmpty(100)
                .Min();

            return min < MinScanIntervalMs ? MinScanIntervalMs : min;
        }
    }
}