using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
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
    ///
    /// 工业设备安全规范：
    ///   - 任意一次扫描失败 → 立即停止扫描循环 → 触发 Critical 级别报警
    ///   - 停止后 ScanState 置为 Error，需操作员确认后手动重启
    ///   - 默认启动时处于 Idle 状态，不自动开始扫描
    ///   - 是否自动启动由 config.json 中 IoScanConfig.AutoStart 控制
    /// </summary>
    public class IoScanWorker : ServiceBase, IIoScanService
    {
        private const int MinScanIntervalMs = 10;
        private const int DefaultScanIntervalMs = 50;

        private readonly object _syncRoot;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _scanLoopTask;
        private IoScanState _scanState;

        protected override string MessageSourceName
        {
            get { return "IoScanWorker"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public IoScanWorker()
            : this(SystemContext.Instance.Reporter,
                   ConfigContext.Instance.Config.IoScanConfig.ScanIntervalMs)
        {
        }

        public IoScanWorker(IAppReporter reporter, int scanIntervalMs = DefaultScanIntervalMs)
            : base(reporter)
        {
            _syncRoot = new object();
            _scanState = IoScanState.Idle;
            ScanIntervalMs = scanIntervalMs < MinScanIntervalMs ? MinScanIntervalMs : scanIntervalMs;
            LastError = string.Empty;
        }

        public string Name
        {
            get { return "MotionIoScanWorker"; }
        }

        /// <summary>
        /// 当前扫描状态（线程安全）。
        /// </summary>
        public IoScanState ScanState
        {
            get { lock (_syncRoot) { return _scanState; } }
        }

        /// <summary>
        /// 是否扫描运行中，派生自 ScanState。
        /// </summary>
        public bool IsRunning
        {
            get { lock (_syncRoot) { return _scanState == IoScanState.Running; } }
        }

        public DateTime? LastRunTime { get; private set; }

        public string LastError { get; private set; }

        public int ScanIntervalMs { get; private set; }

        /// <summary>
        /// 启动 IO 扫描。
        /// 允许从 Idle 或 Error 状态启动（Error 时为手动重启）。
        /// </summary>
        public Result Start()
        {
            lock (_syncRoot)
            {
                if (_scanState == IoScanState.Running)
                {
                    return Warn((int)MotionErrorCode.Unknown, "IO 扫描工作单元已在运行");
                }

                // Idle 或 Error 均允许启动/重启
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _scanState = IoScanState.Running;
                RuntimeContext.Instance.MotionIo.SetScanServiceState(true, ScanIntervalMs);
                _scanLoopTask = Task.Run(() => ScanLoopAsync(_cancellationTokenSource.Token));
            }

            return Ok("IO 扫描工作单元启动成功");
        }

        /// <summary>
        /// 手动停止 IO 扫描。
        /// Error 状态下调用此方法可将状态重置为 Idle，便于后续重启。
        /// </summary>
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
            }

            if (cts == null || loopTask == null)
            {
                SetScanState(IoScanState.Idle);
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
                return Ok("IO 扫描工作单元未启动");
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
                // 手动停止后统一重置为 Idle（含 Error 状态重置）
                SetScanState(IoScanState.Idle);
                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }

            return Ok("IO 扫描工作单元已停止");
        }

        /// <summary>
        /// 立即执行一次完整 DI/DO 扫描。
        /// 任意点位读取失败即返回 Fail，不跳过、不容错。
        /// </summary>
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
                    LastError = string.Format("扫描 DI 失败: Bit={0}，{1}", bit, diResult.Message);
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
                    LastError = string.Format("扫描 DO 失败: Bit={0}，{1}", bit, doResult.Message);
                    return Fail(doResult.Code, LastError);
                }

                var logicalValue = ApplyLogicalTransform(bit, false, doResult.Item);
                RuntimeContext.Instance.MotionIo.SetDO(bit, logicalValue, now);
            }

            LastRunTime = now;
            LastError = string.Empty;
            RuntimeContext.Instance.MotionIo.MarkScanTime(now);
            return OkSilent("IO 扫描成功");
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
                        // 工业设备安全规范：扫描失败不容忍，立即停止并触发 Critical 报警
                        // ScanOnce 内的 Fail() 已记录 Error 日志，此处只触发报警链路
                        SetScanState(IoScanState.Error);
                        RaiseAlarm(AlarmCode.IoScanFailed, AlarmLevel.Critical,
                            string.Format("IO 扫描失败，服务已停止，需手动重启。原因：{0}", scanResult.Message));
                        break;
                    }

                    await Task.Delay(ScanIntervalMs, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // 外部调用 StopAsync() 正常取消，不作为错误处理
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                SetScanState(IoScanState.Error);
                RaiseAlarm(AlarmCode.IoScanFailed, AlarmLevel.Critical,
                    string.Format("IO 扫描循环异常，服务已停止，需手动重启。原因：{0}", ex.Message));
            }
            finally
            {
                // 正常取消（外部 Stop）时状态为 Running → 置回 Idle
                // 错误停止时状态已为 Error → 保持 Error，由手动 StopAsync 重置
                lock (_syncRoot)
                {
                    if (_scanState == IoScanState.Running)
                    {
                        _scanState = IoScanState.Idle;
                    }
                }

                RuntimeContext.Instance.MotionIo.SetScanServiceState(false, 0);
            }
        }

        private void SetScanState(IoScanState state)
        {
            lock (_syncRoot)
            {
                _scanState = state;
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