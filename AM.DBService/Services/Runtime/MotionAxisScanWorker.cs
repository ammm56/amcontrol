using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Runtime;
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
    /// </summary>
    public class MotionAxisScanWorker : ServiceBase, IRuntimeWorker
    {
        private const int MinScanIntervalMs = 10;
        private const int DefaultScanIntervalMs = 100;

        private readonly object _syncRoot;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _scanLoopTask;
        private bool _isRunning;

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
            _syncRoot = new object();
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
                lock (_syncRoot)
                {
                    return _isRunning;
                }
            }
        }

        public int ScanIntervalMs { get; private set; }

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public Result Start()
        {
            lock (_syncRoot)
            {
                if (_isRunning)
                {
                    return Warn(-1200, "轴运行态采样服务已在运行");
                }

                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _isRunning = true;
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(true, ScanIntervalMs);
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
            }

            return Ok("轴运行态采样服务启动成功");
        }

        public async Task<Result> StopAsync()
        {
            CancellationTokenSource cts;
            Task loopTask;

            lock (_syncRoot)
            {
                cts = _cancellationTokenSource;
                loopTask = _scanLoopTask;
                _cancellationTokenSource = null;
                _scanLoopTask = null;
                _isRunning = false;
            }

            if (cts == null || loopTask == null)
            {
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
                return Ok("轴运行态采样服务未启动");
            }

            try
            {
                cts.Cancel();
                await loopTask;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return HandleException(ex, -1201, "停止轴运行态采样服务失败");
            }
            finally
            {
                cts.Dispose();
                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
            }

            return Ok("轴运行态采样服务已停止");
        }

        /// <summary>
        /// 对所有已配置轴执行一次运行态采样。
        /// 注意：该采样结果只供 UI 使用，不作为控制安全逻辑依据。
        /// </summary>
        public Result ScanOnce()
        {
            var motionHub = MachineContext.Instance.MotionHub;
            if (motionHub == null)
            {
                LastError = "MotionHub 未初始化，无法执行轴运行态采样";
                return Fail(-1202, LastError);
            }

            var cards = ConfigContext.Instance.Config.MotionCardsConfig ?? new List<AM.Model.MotionCard.MotionCardConfig>();
            var now = DateTime.Now;
            var runtime = RuntimeContext.Instance.MotionAxis;

            foreach (var card in cards.Where(x => x != null))
            {
                if (card.AxisConfigs == null)
                {
                    continue;
                }

                foreach (var axis in card.AxisConfigs.Where(x => x != null))
                {
                    MotionAxisRuntimeSnapshot snapshot;
                    if (!runtime.TryGetAxisSnapshot(axis.LogicalAxis, out snapshot) || snapshot == null)
                    {
                        snapshot = new MotionAxisRuntimeSnapshot();
                    }

                    snapshot.LogicalAxis = axis.LogicalAxis;
                    snapshot.CardId = card.CardId;
                    snapshot.Name = axis.Name;
                    snapshot.DisplayName = axis.DisplayName;
                    snapshot.CardDisplayName = string.IsNullOrWhiteSpace(card.DisplayName) ? card.Name : card.DisplayName;
                    snapshot.UpdateTime = now;

                    var statusResult = motionHub.GetAxisStatus(axis.LogicalAxis);
                    if (statusResult.Success)
                    {
                        snapshot.IsEnabled = statusResult.Item.IsEnabled;
                        snapshot.IsAlarm = statusResult.Item.IsAlarm;
                        snapshot.IsAtHome = statusResult.Item.IsAtHome;
                        snapshot.PositiveLimit = statusResult.Item.PositiveLimit;
                        snapshot.NegativeLimit = statusResult.Item.NegativeLimit;
                        snapshot.IsDone = statusResult.Item.IsDone;
                    }

                    var movingResult = motionHub.IsMoving(axis.LogicalAxis);
                    if (movingResult.Success)
                    {
                        snapshot.IsMoving = movingResult.Item;
                    }

                    var cmdPulseResult = motionHub.GetCommandPosition(axis.LogicalAxis);
                    if (cmdPulseResult.Success)
                    {
                        snapshot.CommandPositionPulse = cmdPulseResult.Item;
                    }

                    var encPulseResult = motionHub.GetEncoderPosition(axis.LogicalAxis);
                    if (encPulseResult.Success)
                    {
                        snapshot.EncoderPositionPulse = encPulseResult.Item;
                    }

                    var cmdMmResult = motionHub.GetCommandPositionMm(axis.LogicalAxis);
                    if (cmdMmResult.Success)
                    {
                        snapshot.CommandPositionMm = cmdMmResult.Item;
                    }

                    var encMmResult = motionHub.GetEncoderPositionMm(axis.LogicalAxis);
                    if (encMmResult.Success)
                    {
                        snapshot.EncoderPositionMm = encMmResult.Item;
                    }

                    runtime.SetAxisSnapshot(snapshot);
                }
            }

            LastRunTime = now;
            LastError = string.Empty;
            runtime.MarkScanTime(now);
            runtime.NotifySnapshotChanged();
            return OkSilent("轴运行态采样成功");
        }

        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    ScanOnce();
                    await Task.Delay(ScanIntervalMs, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                HandleException(ex, -1203, "轴运行态采样循环异常");
            }
            finally
            {
                lock (_syncRoot)
                {
                    _isRunning = false;
                }

                RuntimeContext.Instance.MotionAxis.SetScanServiceState(false, 0);
            }
        }
    }
}