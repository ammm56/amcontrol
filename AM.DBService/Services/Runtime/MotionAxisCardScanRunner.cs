using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
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
    /// 单张控制卡轴运行态独立采样运行器。
    /// 仅负责当前控制卡下所有轴的 UI 监视快照更新。
    /// </summary>
    internal sealed class MotionAxisCardScanRunner : ServiceBase
    {
        private const int MinScanIntervalMs = 10;
        private const int ErrorLogThrottleIntervalMs = 30000;

        private readonly object _stateSyncRoot;
        private readonly object _configSyncRoot;

        private MotionCardConfig _cardConfig;
        private int _scanIntervalMs;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _scanLoopTask;

        private bool _wasFaulted;
        private string _lastFaultMessage = string.Empty;

        protected override string MessageSourceName
        {
            get { return "MotionAxisCardScanRunner"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        protected override short? MessageCardId
        {
            get
            {
                lock (_configSyncRoot)
                {
                    return _cardConfig == null ? (short?)null : _cardConfig.CardId;
                }
            }
        }

        public MotionAxisCardScanRunner(MotionCardConfig cardConfig, int scanIntervalMs, IAppReporter reporter)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _configSyncRoot = new object();
            _cardConfig = cardConfig ?? new MotionCardConfig();
            _scanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            LastError = string.Empty;
        }

        public short CardId
        {
            get
            {
                lock (_configSyncRoot)
                {
                    return _cardConfig == null ? (short)0 : _cardConfig.CardId;
                }
            }
        }

        public string CardDisplayTitle
        {
            get
            {
                lock (_configSyncRoot)
                {
                    if (_cardConfig == null)
                    {
                        return "Card-0";
                    }

                    if (!string.IsNullOrWhiteSpace(_cardConfig.DisplayName))
                    {
                        return _cardConfig.DisplayName;
                    }

                    if (!string.IsNullOrWhiteSpace(_cardConfig.Name))
                    {
                        return _cardConfig.Name;
                    }

                    return "Card-" + _cardConfig.CardId;
                }
            }
        }

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

        public bool HasFault
        {
            get { return _wasFaulted; }
        }

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public void UpdateConfig(MotionCardConfig cardConfig, int scanIntervalMs)
        {
            lock (_configSyncRoot)
            {
                _cardConfig = cardConfig ?? new MotionCardConfig();
                _scanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            }
        }

        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_cancellationTokenSource != null && _scanLoopTask != null)
                {
                    return Warn((int)MotionErrorCode.Unknown, "轴采样运行器已在运行: " + CardDisplayTitle);
                }

                ResetFaultState();
                _cancellationTokenSource = new CancellationTokenSource();
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
            }

            return OkLogOnly("轴采样运行器启动成功: " + CardDisplayTitle);
        }

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
                return OkLogOnly("轴采样运行器未启动: " + CardDisplayTitle);
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
                return HandleException(ex, (int)MotionErrorCode.Unknown, "停止轴采样运行器失败: " + CardDisplayTitle);
            }
            finally
            {
                cts.Dispose();
                ResetFaultState();
            }

            return OkLogOnly("轴采样运行器已停止: " + CardDisplayTitle);
        }

        public Task<Result> ScanOnceAsync()
        {
            return Task.Run(() => ScanOnceInternal(DateTime.Now));
        }

        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    DateTime now = DateTime.Now;
                    Result scanResult = ScanOnceInternal(now);
                    if (!scanResult.Success)
                    {
                        LastError = scanResult.Message;
                        ReportFaultIfNeeded(scanResult.Code, scanResult.Message);
                    }
                    else
                    {
                        ReportRecoveredIfNeeded();
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

                if (ShouldReportRepeated("AXIS-CARD-LOOP-" + CardId + "-" + ex.Message, ErrorLogThrottleIntervalMs))
                {
                    FailLogOnly((int)MotionErrorCode.Unknown, "轴采样循环异常: " + CardDisplayTitle, ex);
                }
            }
        }

        /// <summary>
        /// 轴采样与 PLC/IO 不同：
        /// 1. 该任务只用于 UI 监视；
        /// 2. 单项状态读取失败不作为整卡失败；
        /// 3. 仅 MotionHub 不可用或运行器异常时才视为失败。
        /// </summary>
        private Result ScanOnceInternal(DateTime now)
        {
            MotionCardConfig cardConfig;
            lock (_configSyncRoot)
            {
                cardConfig = _cardConfig;
            }

            if (cardConfig == null)
            {
                return FailSilent((int)MotionErrorCode.Unknown, "控制卡配置不能为空");
            }

            var motionHub = MachineContext.Instance.MotionHub;
            if (motionHub == null)
            {
                return FailSilent((int)MotionErrorCode.Unknown, "MotionHub 未初始化，无法执行轴运行态采样");
            }

            var runtime = RuntimeContext.Instance.MotionAxis;
            var axes = cardConfig.AxisConfigs ?? new List<AxisConfig>();

            foreach (var axis in axes.Where(x => x != null))
            {
                MotionAxisRuntimeSnapshot snapshot;
                if (!runtime.TryGetAxisSnapshot(axis.LogicalAxis, out snapshot) || snapshot == null)
                {
                    snapshot = new MotionAxisRuntimeSnapshot();
                }

                snapshot.LogicalAxis = axis.LogicalAxis;
                snapshot.CardId = cardConfig.CardId;
                snapshot.Name = axis.Name;
                snapshot.DisplayName = axis.DisplayName;
                snapshot.CardDisplayName = string.IsNullOrWhiteSpace(cardConfig.DisplayName) ? cardConfig.Name : cardConfig.DisplayName;
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

            // 采样成功立即更新时间，防止Worker和Runner之间的缓存过期导致的UI监视异常
            RuntimeContext.Instance.MotionAxis.MarkScanTime(now);
            LastRunTime = now;
            return OkSilent("轴采样成功: " + CardDisplayTitle);
        }

        private void ReportFaultIfNeeded(int code, string message)
        {
            string finalMessage = string.IsNullOrWhiteSpace(message) ? "轴采样失败" : message;
            bool isEdgeFault = !_wasFaulted;
            bool faultChanged = !string.Equals(_lastFaultMessage, finalMessage, StringComparison.Ordinal);

            if (isEdgeFault || faultChanged)
            {
                Warn(code, "控制卡轴采样异常: " + CardDisplayTitle + "，" + finalMessage, ReportChannels.Log | ReportChannels.Message);
            }
            else if (ShouldReportRepeated("AXIS-CARD-FAULT-" + CardId + "-" + finalMessage, ErrorLogThrottleIntervalMs))
            {
                WarnLogOnly(code, "控制卡轴采样持续异常: " + CardDisplayTitle + "，" + finalMessage);
            }

            _wasFaulted = true;
            _lastFaultMessage = finalMessage;
        }

        private void ReportRecoveredIfNeeded()
        {
            if (!_wasFaulted)
            {
                return;
            }

            OkLogOnly("控制卡轴采样恢复正常: " + CardDisplayTitle);
            ResetFaultState();
        }

        private int GetScanIntervalMs()
        {
            lock (_configSyncRoot)
            {
                return _scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : _scanIntervalMs;
            }
        }

        private void ResetFaultState()
        {
            _wasFaulted = false;
            _lastFaultMessage = string.Empty;
        }
    }
}