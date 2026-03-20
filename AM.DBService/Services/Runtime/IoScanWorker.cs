using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Runtime;
using AM.Model.MotionCard;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Runtime
{
    /// <summary>
    /// Motion IO 后台扫描工作单元。
    /// 周期性扫描已注册逻辑 DI/DO，并写入 RuntimeContext。
    /// </summary>
    public class IoScanWorker : ServiceBase, IIoScanService
    {
        private const int MinScanIntervalMs = 10;
        private const int DefaultScanIntervalMs = 50;

        private readonly object _syncRoot;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _scanLoopTask;

        protected override string MessageSourceName
        {
            get { return "IoScanWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public IoScanWorker()
            : this(SystemContext.Instance.Reporter, DefaultScanIntervalMs)
        {
        }

        public IoScanWorker(IAppReporter reporter, int scanIntervalMs = DefaultScanIntervalMs)
            : base(reporter)
        {
            _syncRoot = new object();
            ScanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            LastError = string.Empty;
        }

        public string Name
        {
            get { return "MotionIoScanWorker"; }
        }

        public bool IsRunning
        {
            get
            {
                lock (_syncRoot)
                {
                    return _scanLoopTask != null && !_scanLoopTask.IsCompleted;
                }
            }
        }

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public int ScanIntervalMs { get; private set; }

        public Result Start()
        {
            lock (_syncRoot)
            {
                if (_scanLoopTask != null && !_scanLoopTask.IsCompleted)
                {
                    return Warn((int)MotionErrorCode.Unknown, "IO 扫描工作单元已启动");
                }

                _cancellationTokenSource = new CancellationTokenSource();
                RuntimeContext.Instance.MotionIo.SetScanServiceState(true, ScanIntervalMs);
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));

                return Ok("IO 扫描工作单元启动成功");
            }
        }

        public async Task<Result> StopAsync()
        {
            CancellationTokenSource cts;
            Task loopTask;

            lock (_syncRoot)
            {
                cts = _cancellationTokenSource;
                loopTask = _scanLoopTask;

                if (cts == null || loopTask == null)
                {
                    RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
                    return Ok("IO 扫描工作单元未启动");
                }

                _cancellationTokenSource = null;
                _scanLoopTask = null;
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
                return HandleException(ex, (int)MotionErrorCode.Unknown, "停止 IO 扫描工作单元失败");
            }
            finally
            {
                cts.Dispose();
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }

            return Ok("IO 扫描工作单元已停止");
        }

        public Result ScanOnce()
        {
            var machine = MachineContext.Instance;
            var motionHub = machine.MotionHub;
            if (motionHub == null)
            {
                LastError = "MotionHub 未初始化，无法执行 IO 扫描";
                return Fail((int)MotionErrorCode.IoMapNotFound, LastError);
            }

            var now = DateTime.Now;
            var diBits = machine.DICards.Keys.ToList();
            var doBits = machine.DOCards.Keys.ToList();

            foreach (var bit in diBits)
            {
                var diResult = motionHub.GetDI(bit);
                if (!diResult.Success)
                {
                    LastError = "扫描 DI 失败: " + bit + "，" + diResult.Message;
                    return Fail(diResult.Code, LastError);
                }

                var logicalValue = ApplyLogicalTransform(bit, true, diResult.Item);
                RuntimeContext.Instance.MotionIo.SetDI(bit, logicalValue, now);
            }

            foreach (var bit in doBits)
            {
                var doResult = motionHub.GetDO(bit);
                if (!doResult.Success)
                {
                    LastError = "扫描 DO 失败: " + bit + "，" + doResult.Message;
                    return Fail(doResult.Code, LastError);
                }

                var logicalValue = ApplyLogicalTransform(bit, false, doResult.Item);
                RuntimeContext.Instance.MotionIo.SetDO(bit, logicalValue, now);
            }

            LastRunTime = now;
            LastError = string.Empty;
            RuntimeContext.Instance.MotionIo.MarkScanTime(now);
            return Ok("IO 扫描成功",false);
        }

        private async Task ScanLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var scanResult = ScanOnce();
                    if (!scanResult.Success)
                    {
                        _reporter?.Warn(MessageSourceName, scanResult.Message, scanResult.Code);
                    }

                    await Task.Delay(ScanIntervalMs, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }
        }

        private static bool ApplyLogicalTransform(short logicalBit, bool isDI, bool rawValue)
        {
            MotionIoBitMap bitMap;
            if (!TryGetIoBitMap(logicalBit, isDI, out bitMap) || bitMap == null)
            {
                return rawValue;
            }

            return bitMap.Invert ? !rawValue : rawValue;
        }

        private static bool TryGetIoBitMap(short logicalBit, bool isDI, out MotionIoBitMap bitMap)
        {
            bitMap = null;

            var motionCards = ConfigContext.Instance.Config.MotionCardsConfig;
            if (motionCards == null)
            {
                return false;
            }

            foreach (var card in motionCards)
            {
                if (card == null)
                {
                    continue;
                }

                var list = isDI ? card.DIBitMaps : card.DOBitMaps;
                if (list == null)
                {
                    continue;
                }

                bitMap = list.FirstOrDefault(p => p != null && p.LogicalBit == logicalBit);
                if (bitMap != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}