using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Runtime
{
    /// <summary>
    /// 单张控制卡 IO 独立扫描运行器。
    /// 每张控制卡一个独立 Task，负责扫描该卡下的 DI/DO。
    /// </summary>
    internal sealed class IoCardScanRunner : ServiceBase
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
            get { return "IoCardScanRunner"; }
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

        public IoCardScanRunner(MotionCardConfig cardConfig, int scanIntervalMs, IAppReporter reporter)
            : base(reporter)
        {
            _stateSyncRoot = new object();
            _configSyncRoot = new object();
            _cardConfig = cardConfig ?? new MotionCardConfig();
            _scanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            LastError = string.Empty;
        }

        /// <summary>
        /// 控制卡编号。
        /// </summary>
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

        /// <summary>
        /// 控制卡显示标题。
        /// </summary>
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
        /// 当前卡是否处于异常状态。
        /// </summary>
        public bool HasFault
        {
            get { return _wasFaulted; }
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
        /// 更新卡配置与扫描周期。
        /// </summary>
        public void UpdateConfig(MotionCardConfig cardConfig, int scanIntervalMs)
        {
            lock (_configSyncRoot)
            {
                _cardConfig = cardConfig ?? new MotionCardConfig();
                _scanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            }
        }

        /// <summary>
        /// 启动当前卡扫描循环。
        /// </summary>
        public Result Start()
        {
            lock (_stateSyncRoot)
            {
                if (_cancellationTokenSource != null && _scanLoopTask != null)
                {
                    return Warn((int)MotionErrorCode.Unknown, "IO 控制卡扫描运行器已在运行: " + CardDisplayTitle);
                }

                ResetFaultState();
                _cancellationTokenSource = new CancellationTokenSource();
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
            }

            return OkLogOnly("IO 控制卡扫描运行器启动成功: " + CardDisplayTitle);
        }

        /// <summary>
        /// 停止当前卡扫描循环。
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
                return OkLogOnly("IO 控制卡扫描运行器未启动: " + CardDisplayTitle);
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
                return HandleException(ex, (int)MotionErrorCode.Unknown, "停止 IO 控制卡扫描运行器失败: " + CardDisplayTitle);
            }
            finally
            {
                cts.Dispose();
                ResetFaultState();
            }

            return OkLogOnly("IO 控制卡扫描运行器已停止: " + CardDisplayTitle);
        }

        /// <summary>
        /// 手动执行当前卡单轮扫描。
        /// </summary>
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
                ReportFaultIfNeeded((int)MotionErrorCode.Unknown, ex.Message);

                if (ShouldReportRepeated("IO-CARD-LOOP-" + CardId + "-" + ex.Message, ErrorLogThrottleIntervalMs))
                {
                    FailLogOnly((int)MotionErrorCode.Unknown, "IO 控制卡扫描循环异常: " + CardDisplayTitle, ex);
                }
            }
        }

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
                return FailSilent((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            var diBitMaps = cardConfig.DIBitMaps ?? new List<MotionIoBitMap>();
            var doBitMaps = cardConfig.DOBitMaps ?? new List<MotionIoBitMap>();

            foreach (var bitMap in diBitMaps.Where(x => x != null))
            {
                var diResult = motionHub.GetDI(bitMap.LogicalBit);
                if (!diResult.Success)
                {
                    return FailSilent(
                        diResult.Code,
                        string.Format("IO 控制卡扫描失败: CardId={0}，DI Bit={1}，{2}", cardConfig.CardId, bitMap.LogicalBit, diResult.Message));
                }

                bool logicalValue = bitMap.Invert ? !diResult.Item : diResult.Item;
                RuntimeContext.Instance.MotionIo.SetDI(bitMap.LogicalBit, logicalValue, now);
            }

            foreach (var bitMap in doBitMaps.Where(x => x != null))
            {
                var doResult = motionHub.GetDO(bitMap.LogicalBit);
                if (!doResult.Success)
                {
                    return FailSilent(
                        doResult.Code,
                        string.Format("IO 控制卡扫描失败: CardId={0}，DO Bit={1}，{2}", cardConfig.CardId, bitMap.LogicalBit, doResult.Message));
                }

                bool logicalValue = bitMap.Invert ? !doResult.Item : doResult.Item;
                RuntimeContext.Instance.MotionIo.SetDO(bitMap.LogicalBit, logicalValue, now);
            }

            LastRunTime = now;
            return OkSilent("IO 控制卡扫描成功: " + CardDisplayTitle);
        }

        /// <summary>
        /// 首次故障发一次通知；持续故障只做节流日志。
        /// </summary>
        private void ReportFaultIfNeeded(int code, string message)
        {
            string finalMessage = string.IsNullOrWhiteSpace(message) ? "IO 控制卡扫描失败" : message;
            bool isEdgeFault = !_wasFaulted;
            bool faultChanged = !string.Equals(_lastFaultMessage, finalMessage, StringComparison.Ordinal);

            if (isEdgeFault || faultChanged)
            {
                Warn(code, finalMessage, ReportChannels.Log | ReportChannels.Message);
                RaiseAlarm(AlarmCode.IoScanFailed, AlarmLevel.Critical, finalMessage);
            }
            else if (ShouldReportRepeated("IO-CARD-FAULT-" + CardId + "-" + finalMessage, ErrorLogThrottleIntervalMs))
            {
                WarnLogOnly(code, "IO 控制卡持续异常: " + finalMessage);
            }

            _wasFaulted = true;
            _lastFaultMessage = finalMessage;
        }

        /// <summary>
        /// 故障恢复时记录一条恢复日志。
        /// </summary>
        private void ReportRecoveredIfNeeded()
        {
            if (!_wasFaulted)
            {
                return;
            }

            OkLogOnly("IO 控制卡恢复正常: " + CardDisplayTitle);
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