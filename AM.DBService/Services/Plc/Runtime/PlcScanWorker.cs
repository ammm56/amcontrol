using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
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
    /// 重构后职责：
    /// 1. 本类不再串行逐站扫描全部 PLC；
    /// 2. 本类仅作为 PLC 扫描“协调器 / 宿主”存在；
    /// 3. 每个 PLC 站由一个独立的 <see cref="PlcStationScanRunner"/> 执行扫描循环；
    /// 4. 单个 PLC 站连接异常、重连阻塞、读取变慢，不再拖慢其他 PLC 站；
    /// 5. 本类统一负责：
    ///    - 读取当前 PLC 配置；
    ///    - 创建、更新、销毁站级扫描运行器；
    ///    - 启动和停止全部运行器；
    ///    - 汇总扫描服务总状态并写入 <see cref="RuntimeContext"/>。
    /// 
    /// 设计说明：
    /// - 顶层仍由 <see cref="RuntimeTaskManager"/> 管理本 Worker；
    /// - Worker 内部再管理多个 PLC 站子任务；
    /// - 这样既保留统一任务管理入口，也实现设备级并行隔离。
    /// </summary>
    public class PlcScanWorker : ServiceBase, IPlcScanWorker
    {
        /// <summary>
        /// 协调循环默认刷新间隔。
        /// 该间隔仅用于检查配置变化、补齐/删除运行器，不代表站点扫描周期。
        /// </summary>
        private const int DefaultSupervisorIntervalMs = 500;

        /// <summary>
        /// 状态同步锁。
        /// 用于保护顶层 supervisor 任务启停状态。
        /// </summary>
        private readonly object _stateSyncRoot;

        /// <summary>
        /// 运行器集合同步锁。
        /// 用于保护各 PLC 站运行器字典的并发访问。
        /// </summary>
        private readonly object _runnerSyncRoot;

        /// <summary>
        /// 协调循环取消源。
        /// </summary>
        private CancellationTokenSource _supervisorCancellationTokenSource;

        /// <summary>
        /// 协调循环任务。
        /// 负责动态对齐配置与运行器集合，不负责逐站点位扫描。
        /// </summary>
        private Task _supervisorTask;

        /// <summary>
        /// 当前缓存的 PLC 配置对象引用。
        /// 当引用变化时，说明已执行过 ReloadFromDatabase，需要重建运行器集合。
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
        /// 当前已创建的 PLC 站扫描运行器集合。
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
        /// 顶层工作单元是否运行中。
        /// 注意：
        /// - 这里表示 supervisor 是否运行；
        /// - 不是指某个单站 runner 是否运行。
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
        /// 该值为站级运行器的最大 LastRunTime。
        /// </summary>
        public DateTime? LastRunTime { get; private set; }

        /// <summary>
        /// 最近一次顶层错误信息。
        /// 若顶层无错误，则优先显示第一个仍存在错误的站级运行器错误。
        /// </summary>
        public string LastError { get; private set; }

        /// <summary>
        /// 启动 PLC 扫描服务。
        /// 启动后：
        /// 1. 先读取当前 PLC 配置；
        /// 2. 立即对齐并启动各站 runner；
        /// 3. 再启动 supervisor 持续监控配置变化。
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
        /// 异步停止 PLC 扫描服务。
        /// </summary>
        public Task<Result> StopAsync()
        {
            return Task.FromResult(Stop());
        }

        /// <summary>
        /// 同步停止 PLC 扫描服务。
        /// 停止顺序：
        /// 1. 先停止 supervisor；
        /// 2. supervisor 的 finally 中再停止全部站级 runner。
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
        /// 
        /// 实现方式：
        /// - 不再串行调用所有站；
        /// - 而是对每个站 runner 并发执行 `ScanOnceAsync()`；
        /// - 从而保持与常驻后台扫描一致的隔离语义。
        /// </summary>
        public Result ScanOnce()
        {
            try
            {
                RefreshScanCacheIfNeeded();
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

                var tasks = runners
                    .Select(x => x.ScanOnceAsync())
                    .ToArray();

                Task.WaitAll(tasks);

                var failedResults = tasks
                    .Where(x => x.Status == TaskStatus.RanToCompletion && x.Result != null && !x.Result.Success)
                    .Select(x => x.Result)
                    .ToList();

                UpdateRuntimeServiceState();

                if (failedResults.Count > 0)
                {
                    string message = string.Join(" | ", failedResults.Select(x => x.Message).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Take(3));
                    return Result.Fail(failedResults[0].Code, string.IsNullOrWhiteSpace(message) ? "PLC 单轮扫描部分失败" : message, ResultSource.Plc);
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
        /// 当前循环只负责：
        /// 1. 监视配置引用是否变化；
        /// 2. 对齐站级运行器集合；
        /// 3. 汇总全局运行状态。
        /// 
        /// 它不直接参与单站点位读取。
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
        /// 当前系统中 ReloadFromDatabase 会替换整个 PlcConfig 引用，
        /// 因此这里使用引用比较即可识别配置重载。
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
        /// 根据当前配置对齐站级运行器集合。
        /// 包含三类动作：
        /// 1. 停止并删除已不存在或已禁用的 PLC 站 runner；
        /// 2. 创建新增的 PLC 站 runner；
        /// 3. 更新已存在 runner 的站配置与点位列表。
        /// </summary>
        private void ReconcileRunners()
        {
            var stationLookup = _cachedEnabledStationLookup == null
                ? new Dictionary<string, PlcStationConfig>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, PlcStationConfig>(_cachedEnabledStationLookup, StringComparer.OrdinalIgnoreCase);

            var pointsByPlc = _cachedPointsByPlc == null
                ? new Dictionary<string, IList<PlcPointConfig>>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, IList<PlcPointConfig>>(_cachedPointsByPlc, StringComparer.OrdinalIgnoreCase);

            List<PlcStationScanRunner> removedRunners = new List<PlcStationScanRunner>();
            List<PlcStationScanRunner> addedOrUpdatedRunners = new List<PlcStationScanRunner>();

            lock (_runnerSyncRoot)
            {
                var removedKeys = _stationRunners.Keys
                    .Where(x => !stationLookup.ContainsKey(x))
                    .ToList();

                foreach (var key in removedKeys)
                {
                    removedRunners.Add(_stationRunners[key]);
                    _stationRunners.Remove(key);
                }

                foreach (var pair in stationLookup)
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
                        addedOrUpdatedRunners.Add(runner);
                        continue;
                    }

                    runner = new PlcStationScanRunner(
                        pair.Value,
                        points,
                        ResolveClient,
                        SystemContext.Instance.Reporter);

                    _stationRunners[pair.Key] = runner;
                    addedOrUpdatedRunners.Add(runner);
                }
            }

            foreach (var runner in removedRunners)
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
        /// </summary>
        private void StartAllRunners()
        {
            PlcStationScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _stationRunners.Values.ToArray();
            }

            foreach (var runner in runners)
            {
                if (runner == null || runner.IsRunning)
                {
                    continue;
                }

                var startResult = runner.Start();
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

            foreach (var runner in runners)
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
        /// 将全局 PLC 扫描服务状态写入 RuntimeContext。
        /// 
        /// 汇总内容：
        /// 1. 是否有任何站级 runner 正在运行；
        /// 2. 当前最小扫描周期；
        /// 3. 最近一次任意站扫描完成时间；
        /// 4. 第一个非空错误信息。
        /// </summary>
        private void UpdateRuntimeServiceState()
        {
            PlcStationScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _stationRunners.Values.ToArray();
            }

            bool hasRunningRunner = runners.Any(x => x != null && x.IsRunning);
            int scanIntervalMs = GetMinScanIntervalMs(_cachedEnabledStationLookup == null
                ? new List<PlcStationConfig>()
                : _cachedEnabledStationLookup.Values.ToList());

            RuntimeContext.Instance.Plc.SetScanServiceState(hasRunningRunner, hasRunningRunner ? scanIntervalMs : 0);

            var latestRunTime = runners
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
        /// 配置重载后客户端对象会重建，因此不在 runner 内持有长期引用。
        /// </summary>
        private static AM.Model.Interfaces.Plc.IPlcClient ResolveClient(string plcName)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return null;
            }

            AM.Model.Interfaces.Plc.IPlcClient client;
            return MachineContext.Instance.Plcs.TryGetValue(plcName, out client) ? client : null;
        }

        /// <summary>
        /// 构建启用站查找表。
        /// </summary>
        private static IDictionary<string, PlcStationConfig> BuildEnabledStationLookup(PlcConfig plcConfig)
        {
            var stations = plcConfig == null ? null : plcConfig.Stations;
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
            var points = plcConfig == null ? null : plcConfig.Points;
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
        /// 全局扫描服务状态显示使用该值。
        /// </summary>
        private static int GetMinScanIntervalMs(IList<PlcStationConfig> stations)
        {
            if (stations == null || stations.Count == 0)
            {
                return 0;
            }

            return stations
                .Where(x => x != null && x.IsEnabled)
                .Select(x => x.ScanIntervalMs <= 0 ? 100 : x.ScanIntervalMs)
                .DefaultIfEmpty(100)
                .Min();
        }
    }
}