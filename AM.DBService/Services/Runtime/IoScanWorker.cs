using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Runtime;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Runtime
{
    /// <summary>
    /// Motion IO 后台扫描工作单元。
    /// 重构后不再串行扫描全部控制卡，而是按控制卡维护多个独立 `IoCardScanRunner`。
    /// </summary>
    public class IoScanWorker : ServiceBase, IIoScanService
    {
        #region 常量与字段

        private const int DefaultSupervisorIntervalMs = 500;
        private const int MinScanIntervalMs = 10;
        private const int DefaultScanIntervalMs = 50;

        private readonly object _stateSyncRoot;
        private readonly object _runnerSyncRoot;

        private CancellationTokenSource _supervisorCancellationTokenSource;
        private Task _supervisorTask;

        private IList<MotionCardConfig> _cachedMotionCardsConfig;
        private IDictionary<short, MotionCardConfig> _cachedCardLookup;
        private readonly IDictionary<short, IoCardScanRunner> _cardRunners;

        private const int BackgroundLogThrottleIntervalMs = 30000;
        private const int ScanStallWarnMinMs = 2000;
        private const int ScanStallWarnFactor = 8;

        #endregion

        #region 元数据与构造

        protected override string MessageSourceName
        {
            get { return "IoScanWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public IoScanWorker()
            : this(SystemContext.Instance.Reporter, ConfigContext.Instance.Config.IoScanConfig.ScanIntervalMs)
        {
        }

        public IoScanWorker(IAppReporter reporter, int scanIntervalMs = DefaultScanIntervalMs)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _runnerSyncRoot = new object();
            _cachedCardLookup = new Dictionary<short, MotionCardConfig>();
            _cardRunners = new Dictionary<short, IoCardScanRunner>();
            ScanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            ScanState = IoScanState.Idle;
            LastError = string.Empty;
        }

        #endregion

        #region 属性与启停

        public string Name
        {
            get { return "MotionIoScanWorker"; }
        }

        public IoScanState ScanState { get; private set; }

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

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public int ScanIntervalMs { get; private set; }

        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_supervisorCancellationTokenSource != null && _supervisorTask != null)
                {
                    return Warn((int)MotionErrorCode.Unknown, "IO 扫描工作单元已在运行");
                }

                RefreshScanCacheIfNeeded();
                if (_cachedCardLookup.Count == 0)
                {
                    return Warn((int)MotionErrorCode.IoMapNotFound, "未找到可扫描的 IO 控制卡配置");
                }

                _supervisorCancellationTokenSource = new CancellationTokenSource();

                ReconcileRunners();
                StartAllRunners();
                UpdateRuntimeServiceState();

                _supervisorTask = Task.Run(() => SupervisorLoopAsync(_supervisorCancellationTokenSource.Token));
                ScanState = IoScanState.Running;
            }

            return OkLogOnly("IO 扫描工作单元启动成功");
        }

        public async Task<Result> StopAsync()
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
                ScanState = IoScanState.Idle;
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
                return OkLogOnly("IO 扫描工作单元未启动");
            }

            try
            {
                cts.Cancel();
                await supervisorTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                ScanState = IoScanState.Error;
                return HandleException(ex, (int)MotionErrorCode.Unknown, "停止 IO 扫描工作单元失败");
            }
            finally
            {
                cts.Dispose();
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }

            ScanState = IoScanState.Idle;
            return OkLogOnly("IO 扫描工作单元已停止");
        }

        public Result ScanOnce()
        {
            try
            {
                RefreshScanCacheIfNeeded();
                ReconcileRunners();

                IoCardScanRunner[] runners;
                lock (_runnerSyncRoot)
                {
                    runners = _cardRunners.Values.ToArray();
                }

                if (runners.Length == 0)
                {
                    return WarnSilent((int)MotionErrorCode.IoMapNotFound, "未找到可扫描的 IO 控制卡");
                }

                Task<Result>[] tasks = runners
                    .Where(x => x != null)
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
                    string message = string.Join(
                        " | ",
                        failedResults
                            .Select(x => x.Message)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct()
                            .Take(3));

                    return Result.Fail(
                        failedResults[0].Code,
                        string.IsNullOrWhiteSpace(message) ? "IO 单轮扫描部分失败" : message,
                        ResultSource.Motion);
                }

                return OkSilent("IO 单轮扫描成功");
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, (int)MotionErrorCode.Unknown, "执行 IO 单轮扫描失败");
            }
        }

        #endregion

        #region 协调循环与运行器管理

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
                ScanState = IoScanState.Error;

                FailLogOnlyIfRepeated(
                    "IO-WORKER-LOOP-" + ex.Message,
                    (int)MotionErrorCode.Unknown,
                    "IO 扫描协调循环异常",
                    BackgroundLogThrottleIntervalMs,
                    ex);
            }
            finally
            {
                await StopAllRunnersAsync().ConfigureAwait(false);
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }
        }

        private void RefreshScanCacheIfNeeded()
        {
            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<MotionCardConfig>();
            if (ReferenceEquals(_cachedMotionCardsConfig, motionCards))
            {
                return;
            }

            _cachedMotionCardsConfig = motionCards;
            _cachedCardLookup = BuildCardLookup(motionCards);
        }

        private void ReconcileRunners()
        {
            IDictionary<short, MotionCardConfig> cardLookup = _cachedCardLookup == null
                ? new Dictionary<short, MotionCardConfig>()
                : new Dictionary<short, MotionCardConfig>(_cachedCardLookup);

            List<IoCardScanRunner> removedRunners = new List<IoCardScanRunner>();

            lock (_runnerSyncRoot)
            {
                List<short> removedKeys = _cardRunners.Keys
                    .Where(x => !cardLookup.ContainsKey(x))
                    .ToList();

                foreach (short key in removedKeys)
                {
                    removedRunners.Add(_cardRunners[key]);
                    _cardRunners.Remove(key);
                }

                foreach (var pair in cardLookup)
                {
                    IoCardScanRunner runner;
                    if (_cardRunners.TryGetValue(pair.Key, out runner))
                    {
                        runner.UpdateConfig(pair.Value, ScanIntervalMs);
                        continue;
                    }

                    runner = new IoCardScanRunner(pair.Value, ScanIntervalMs, _reporter);
                    _cardRunners[pair.Key] = runner;
                }
            }

            foreach (var runner in removedRunners)
            {
                try
                {
                    runner.StopAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    WarnLogOnlyIfRepeated(
                        "IO-RUNNER-REMOVE-STOP-" + runner.CardId + "-" + ex.Message,
                        (int)MotionErrorCode.Unknown,
                        "停止已移除的 IO 控制卡扫描运行器失败: CardId=" + runner.CardId,
                        BackgroundLogThrottleIntervalMs);
                }
            }
        }

        private void StartAllRunners()
        {
            IoCardScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _cardRunners.Values.ToArray();
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
                    WarnLogOnlyIfRepeated(
                        "IO-RUNNER-START-" + runner.CardId + "-" + startResult.Message,
                        startResult.Code,
                        "IO 控制卡扫描运行器启动失败: CardId=" + runner.CardId + "，" + startResult.Message,
                        BackgroundLogThrottleIntervalMs);
                }
            }
        }

        private async Task StopAllRunnersAsync()
        {
            IoCardScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _cardRunners.Values.ToArray();
                _cardRunners.Clear();
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
                catch (Exception ex)
                {
                    WarnLogOnlyIfRepeated(
                        "IO-RUNNER-STOP-" + runner.CardId + "-" + ex.Message,
                        (int)MotionErrorCode.Unknown,
                        "停止 IO 控制卡扫描运行器失败: CardId=" + runner.CardId,
                        BackgroundLogThrottleIntervalMs);
                }
            }
        }

        #endregion

        #region 状态汇总与诊断

        private void UpdateRuntimeServiceState()
        {
            IoCardScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _cardRunners.Values.ToArray();
            }

            bool hasRunningRunner = runners.Any(x => x != null && x.IsRunning);
            bool hasFaultRunner = runners.Any(x => x != null && x.HasFault);

            RuntimeContext.Instance.MotionIo.SetScanServiceState(hasRunningRunner, hasRunningRunner ? ScanIntervalMs : 0);

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

            if (!hasRunningRunner)
            {
                ScanState = IoScanState.Idle;
            }
            else if (hasFaultRunner)
            {
                ScanState = IoScanState.Error;
            }
            else
            {
                ScanState = IoScanState.Running;
            }

            ReportScanStallIfNeeded(hasRunningRunner, hasFaultRunner, runners.Length);

            if (LastRunTime.HasValue)
            {
                // 注意：MotionIo 内部不再维护单独的扫描时间戳，而是直接使用各 Runner 的扫描时间来判断数据新旧。
                //RuntimeContext.Instance.MotionIo.MarkScanTime(LastRunTime.Value);
                RuntimeContext.Instance.MotionIo.NotifySnapshotChanged();
            }
        }

        private void ReportScanStallIfNeeded(bool hasRunningRunner, bool hasFaultRunner, int runnerCount)
        {
            if (!hasRunningRunner)
            {
                return;
            }

            if (!LastRunTime.HasValue)
            {
                WarnLogOnlyIfRepeated(
                    "IO-WORKER-NO-SNAPSHOT",
                    (int)MotionErrorCode.IoRuntimeCacheStale,
                    string.Format(
                        "Motion IO 扫描已启动但尚未产生有效快照: RunnerCount={0}, ScanInterval={1}ms, FaultRunner={2}",
                        runnerCount,
                        ScanIntervalMs,
                        hasFaultRunner ? "Y" : "N"),
                    BackgroundLogThrottleIntervalMs);
                return;
            }

            double ageMs = (DateTime.Now - LastRunTime.Value).TotalMilliseconds;
            int thresholdMs = GetScanStallWarnThresholdMs();
            if (ageMs <= thresholdMs)
            {
                return;
            }

            WarnLogOnlyIfRepeated(
                "IO-WORKER-STALL",
                (int)MotionErrorCode.IoRuntimeCacheStale,
                string.Format(
                    "Motion IO 扫描疑似停滞: Age={0:0}ms, Threshold={1}ms, RunnerCount={2}, FaultRunner={3}, LastError={4}",
                    ageMs,
                    thresholdMs,
                    runnerCount,
                    hasFaultRunner ? "Y" : "N",
                    string.IsNullOrWhiteSpace(LastError) ? "-" : LastError),
                BackgroundLogThrottleIntervalMs);
        }

        private int GetScanStallWarnThresholdMs()
        {
            int threshold = ScanIntervalMs * ScanStallWarnFactor;
            return threshold > ScanStallWarnMinMs ? threshold : ScanStallWarnMinMs;
        }

        #endregion

        private static IDictionary<short, MotionCardConfig> BuildCardLookup(IList<MotionCardConfig> motionCards)
        {
            var machineCards = MachineContext.Instance.MotionCards;

            return (motionCards ?? new List<MotionCardConfig>())
                .Where(x =>
                    x != null
                    && machineCards.ContainsKey(x.CardId)
                    && (((x.DIBitMaps == null ? 0 : x.DIBitMaps.Count) > 0)
                        || ((x.DOBitMaps == null ? 0 : x.DOBitMaps.Count) > 0)))
                .OrderBy(x => x.InitOrder)
                .ThenBy(x => x.CardId)
                .ToDictionary(x => x.CardId, x => x);
        }
    }
}