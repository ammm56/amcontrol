using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Runtime;
using AM.Model.MotionCard;
using AM.Model.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Runtime
{
    /// <summary>
    /// Motion 轴运行态后台采样服务。
    /// 该服务仅用于 UI / 监视 / 诊断缓存，不参与运动控制安全判断。
    /// 重构后按控制卡维护多个独立 `MotionAxisCardScanRunner`。
    /// </summary>
    public class MotionAxisScanWorker : ServiceBase, IRuntimeWorker
    {
        private const int DefaultSupervisorIntervalMs = 500;
        private const int MinScanIntervalMs = 10;
        private const int DefaultScanIntervalMs = 100;

        private readonly object _stateSyncRoot;
        private readonly object _runnerSyncRoot;

        private CancellationTokenSource _supervisorCancellationTokenSource;
        private Task _supervisorTask;

        private IList<MotionCardConfig> _cachedMotionCardsConfig;
        private IDictionary<short, MotionCardConfig> _cachedCardLookup;
        private readonly IDictionary<short, MotionAxisCardScanRunner> _cardRunners;

        protected override string MessageSourceName
        {
            get { return "MotionAxisScanWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public MotionAxisScanWorker()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public MotionAxisScanWorker(IAppReporter reporter, int scanIntervalMs = DefaultScanIntervalMs)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _runnerSyncRoot = new object();
            _cachedCardLookup = new Dictionary<short, MotionCardConfig>();
            _cardRunners = new Dictionary<short, MotionAxisCardScanRunner>();
            ScanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            LastError = string.Empty;
        }

        public string Name
        {
            get { return "MotionAxisScanWorker"; }
        }

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

        public int ScanIntervalMs { get; private set; }

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_supervisorCancellationTokenSource != null && _supervisorTask != null)
                {
                    return Warn((int)MotionErrorCode.Unknown, "轴运行态采样服务已在运行");
                }

                RefreshScanCacheIfNeeded();
                if (_cachedCardLookup.Count == 0)
                {
                    return Warn((int)MotionErrorCode.Unknown, "未找到可采样的控制卡配置");
                }

                _supervisorCancellationTokenSource = new CancellationTokenSource();

                ReconcileRunners();
                StartAllRunners();
                UpdateRuntimeServiceState();

                _supervisorTask = Task.Run(() => SupervisorLoopAsync(_supervisorCancellationTokenSource.Token));
            }

            return OkLogOnly("轴运行态采样服务启动成功");
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
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
                return OkLogOnly("轴运行态采样服务未启动");
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
                return HandleException(ex, (int)MotionErrorCode.Unknown, "停止轴运行态采样服务失败");
            }
            finally
            {
                cts.Dispose();
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
            }

            return OkLogOnly("轴运行态采样服务已停止");
        }

        /// <summary>
        /// 手动执行全部控制卡单轮采样。
        /// </summary>
        public Result ScanOnce()
        {
            try
            {
                RefreshScanCacheIfNeeded();
                ReconcileRunners();

                MotionAxisCardScanRunner[] runners;
                lock (_runnerSyncRoot)
                {
                    runners = _cardRunners.Values.ToArray();
                }

                if (runners.Length == 0)
                {
                    return WarnSilent((int)MotionErrorCode.Unknown, "未找到可采样的控制卡");
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
                        string.IsNullOrWhiteSpace(message) ? "轴运行态单轮采样部分失败" : message,
                        ResultSource.Motion);
                }

                return OkSilent("轴运行态单轮采样成功");
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, (int)MotionErrorCode.Unknown, "执行轴运行态单轮采样失败");
            }
        }

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
                FailLogOnly((int)MotionErrorCode.Unknown, "轴运行态采样协调循环异常", ex);
            }
            finally
            {
                await StopAllRunnersAsync().ConfigureAwait(false);
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
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

            List<MotionAxisCardScanRunner> removedRunners = new List<MotionAxisCardScanRunner>();

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
                    MotionAxisCardScanRunner runner;
                    if (_cardRunners.TryGetValue(pair.Key, out runner))
                    {
                        runner.UpdateConfig(pair.Value, ScanIntervalMs);
                        continue;
                    }

                    runner = new MotionAxisCardScanRunner(pair.Value, ScanIntervalMs, _reporter);
                    _cardRunners[pair.Key] = runner;
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

        private void StartAllRunners()
        {
            MotionAxisCardScanRunner[] runners;
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
                    WarnLogOnly(startResult.Code, "轴采样运行器启动失败: CardId=" + runner.CardId + "，" + startResult.Message);
                }
            }
        }

        private async Task StopAllRunnersAsync()
        {
            MotionAxisCardScanRunner[] runners;
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
                catch
                {
                }
            }
        }

        private void UpdateRuntimeServiceState()
        {
            MotionAxisCardScanRunner[] runners;
            lock (_runnerSyncRoot)
            {
                runners = _cardRunners.Values.ToArray();
            }

            bool hasRunningRunner = runners.Any(x => x != null && x.IsRunning);

            RuntimeContext.Instance.MotionAxis.SetScanServiceState(hasRunningRunner, hasRunningRunner ? ScanIntervalMs : 0);

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

            if (LastRunTime.HasValue)
            {
                // Runner中已经记录了单轴的扫描时间。
                //RuntimeContext.Instance.MotionAxis.MarkScanTime(LastRunTime.Value);
                RuntimeContext.Instance.MotionAxis.NotifySnapshotChanged();
            }
        }

        private static IDictionary<short, MotionCardConfig> BuildCardLookup(IList<MotionCardConfig> motionCards)
        {
            var machineCards = MachineContext.Instance.MotionCards;

            return (motionCards ?? new List<MotionCardConfig>())
                .Where(x =>
                    x != null
                    && machineCards.ContainsKey(x.CardId)
                    && x.AxisConfigs != null
                    && x.AxisConfigs.Count > 0)
                .OrderBy(x => x.InitOrder)
                .ThenBy(x => x.CardId)
                .ToDictionary(x => x.CardId, x => x);
        }
    }
}