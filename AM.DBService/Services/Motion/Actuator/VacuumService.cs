using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Interfaces.Motion.Actuator;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
using AM.Model.Runtime;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Motion.Actuator
{
    /// <summary>
    /// 真空对象运行时服务。
    /// 通过 MachineContext 中已注册的真空对象配置与 MotionHub 执行 IO 控制。
    /// 输出动作直接走 MotionHub；
    /// 反馈等待与工件检测统一走 RuntimeContext.MotionIo 缓存。
    /// </summary>
    public class VacuumService : ServiceBase, IVacuumService
    {
        private const int DefaultPollIntervalMs = 50;
        private const int DefaultActionTimeoutMs = 3000;
        private const int DefaultCacheStaleToleranceMs = 500;
        private const int DefaultBlowOffPulseMs = 100;

        protected override string MessageSourceName
        {
            get { return "VacuumService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public VacuumService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public VacuumService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 查询全部已注册真空对象。
        /// </summary>
        public Result<VacuumConfig> QueryAll()
        {
            var machine = MachineContext.Instance;

            if (machine.Vacuums == null || machine.Vacuums.Count == 0)
            {
                return Warn<VacuumConfig>((int)MotionErrorCode.IoMapNotFound, "当前未注册任何真空对象");
            }

            var list = machine.Vacuums.Values
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();

            return OkList(list, "真空对象查询成功");
        }

        /// <summary>
        /// 按名称查询真空对象。
        /// </summary>
        public Result<VacuumConfig> QueryByName(string name)
        {
            VacuumConfig vacuum;
            var resolveResult = ResolveVacuum(name, out vacuum);
            if (!resolveResult.Success)
            {
                return Fail<VacuumConfig>(resolveResult.Code, resolveResult.Message);
            }

            return Ok(vacuum, "真空对象查询成功");
        }

        /// <summary>
        /// 打开真空。
        /// </summary>
        public Result VacuumOn(string name, bool waitFeedback = true, bool waitWorkpiece = false)
        {
            return VacuumOnAsync(name, waitFeedback, waitWorkpiece, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// 关闭真空。
        /// </summary>
        public Result VacuumOff(string name, bool waitFeedback = true)
        {
            return VacuumOffAsync(name, waitFeedback, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// 异步打开真空。
        /// </summary>
        public async Task<Result> VacuumOnAsync(
            string name,
            bool waitFeedback = true,
            bool waitWorkpiece = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                VacuumConfig vacuum;
                var resolveResult = ResolveVacuum(name, out vacuum);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = SetOutputsForVacuumOn(vacuum);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (NeedWaitFeedback(vacuum, waitFeedback))
                {
                    if (!vacuum.VacuumFeedbackBit.HasValue)
                    {
                        return Fail((int)MotionErrorCode.IoMapNotFound, "真空对象未配置建立反馈位");
                    }

                    var waitBuildResult = await WaitForDiStateAsync(
                        vacuum.VacuumFeedbackBit.Value,
                        true,
                        GetBuildTimeout(vacuum.VacuumBuildTimeoutMs),
                        "真空建立成功",
                        "等待真空建立超时: " + vacuum.Name,
                        cancellationToken);

                    if (!waitBuildResult.Success && waitBuildResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseBuildTimeoutAlarm(vacuum);
                    }

                    if (!waitBuildResult.Success)
                    {
                        return waitBuildResult;
                    }
                }

                if (NeedWaitWorkpiece(vacuum, waitWorkpiece))
                {
                    if (!vacuum.WorkpiecePresentBit.HasValue)
                    {
                        return Fail((int)MotionErrorCode.IoMapNotFound, "真空对象未配置工件检测位");
                    }

                    var waitWorkpieceResult = await WaitForDiStateAsync(
                        vacuum.WorkpiecePresentBit.Value,
                        true,
                        GetBuildTimeout(vacuum.VacuumBuildTimeoutMs),
                        "工件检测成功",
                        "等待工件吸附检测超时: " + vacuum.Name,
                        cancellationToken);

                    if (!waitWorkpieceResult.Success && waitWorkpieceResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseWorkpieceLostAlarm(vacuum, "真空吸附超时未检测到工件: " + vacuum.Name);
                    }

                    if (!waitWorkpieceResult.Success)
                    {
                        return waitWorkpieceResult;
                    }
                }

                return Ok("真空打开命令执行成功");
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "真空打开操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "真空打开执行失败");
            }
        }

        /// <summary>
        /// 异步关闭真空。
        /// </summary>
        public async Task<Result> VacuumOffAsync(
            string name,
            bool waitFeedback = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                VacuumConfig vacuum;
                var resolveResult = ResolveVacuum(name, out vacuum);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = await SetOutputsForVacuumOffAsync(vacuum, cancellationToken);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (!NeedWaitFeedback(vacuum, waitFeedback))
                {
                    return Ok("真空关闭命令执行成功");
                }

                if (vacuum.ReleaseFeedbackBit.HasValue)
                {
                    var waitReleaseResult = await WaitForDiStateAsync(
                        vacuum.ReleaseFeedbackBit.Value,
                        true,
                        GetReleaseTimeout(vacuum.ReleaseTimeoutMs),
                        "真空释放成功",
                        "等待真空释放超时: " + vacuum.Name,
                        cancellationToken);

                    if (!waitReleaseResult.Success && waitReleaseResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseReleaseTimeoutAlarm(vacuum);
                    }

                    return waitReleaseResult;
                }

                if (vacuum.VacuumFeedbackBit.HasValue)
                {
                    var waitReleaseResult = await WaitForDiStateAsync(
                        vacuum.VacuumFeedbackBit.Value,
                        false,
                        GetReleaseTimeout(vacuum.ReleaseTimeoutMs),
                        "真空释放成功",
                        "等待真空释放超时: " + vacuum.Name,
                        cancellationToken);

                    if (!waitReleaseResult.Success && waitReleaseResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseReleaseTimeoutAlarm(vacuum);
                    }

                    return waitReleaseResult;
                }

                return Fail((int)MotionErrorCode.IoMapNotFound, "真空对象未配置释放反馈位或建立反馈位");
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "真空关闭操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "真空关闭执行失败");
            }
        }

        /// <summary>
        /// 判断真空是否建立。
        /// </summary>
        public Result<bool> IsVacuumBuilt(string name)
        {
            VacuumConfig vacuum;
            var resolveResult = ResolveVacuum(name, out vacuum);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!vacuum.VacuumFeedbackBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "真空对象未配置建立反馈位");
            }

            bool value;
            var readResult = TryReadCachedDI(vacuum.VacuumFeedbackBit.Value, out value);
            if (!readResult.Success)
            {
                return Fail<bool>(readResult.Code, readResult.Message);
            }

            return Ok(value, "真空建立状态读取成功");
        }

        /// <summary>
        /// 判断真空是否已释放。
        /// </summary>
        public Result<bool> IsReleased(string name)
        {
            VacuumConfig vacuum;
            var resolveResult = ResolveVacuum(name, out vacuum);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            bool value;
            Result readResult;

            if (vacuum.ReleaseFeedbackBit.HasValue)
            {
                readResult = TryReadCachedDI(vacuum.ReleaseFeedbackBit.Value, out value);
                if (!readResult.Success)
                {
                    return Fail<bool>(readResult.Code, readResult.Message);
                }

                return Ok(value, "真空释放状态读取成功");
            }

            if (vacuum.VacuumFeedbackBit.HasValue)
            {
                readResult = TryReadCachedDI(vacuum.VacuumFeedbackBit.Value, out value);
                if (!readResult.Success)
                {
                    return Fail<bool>(readResult.Code, readResult.Message);
                }

                return Ok(!value, "真空释放状态读取成功");
            }

            return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "真空对象未配置释放反馈位或建立反馈位");
        }

        /// <summary>
        /// 判断是否检测到工件。
        /// </summary>
        public Result<bool> HasWorkpiece(string name)
        {
            VacuumConfig vacuum;
            var resolveResult = ResolveVacuum(name, out vacuum);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!vacuum.WorkpiecePresentBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "真空对象未配置工件检测位");
            }

            bool value;
            var readResult = TryReadCachedDI(vacuum.WorkpiecePresentBit.Value, out value);
            if (!readResult.Success)
            {
                return Fail<bool>(readResult.Code, readResult.Message);
            }

            return Ok(value, "工件检测状态读取成功");
        }

        private Result ResolveVacuum(string name, out VacuumConfig vacuum)
        {
            vacuum = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "真空对象名称不能为空");
            }

            var machine = MachineContext.Instance;
            if (machine.Vacuums == null || machine.Vacuums.Count == 0)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "当前未注册任何真空对象");
            }

            if (!machine.Vacuums.TryGetValue(name.Trim(), out vacuum) || vacuum == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "未找到真空对象: " + name);
            }

            if (!vacuum.IsEnabled)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "真空对象未启用: " + name);
            }

            return OkSilent("真空对象解析成功");
        }

        private IMotionCardService GetMotionHub()
        {
            return MachineContext.Instance.MotionHub;
        }

        private bool NeedWaitFeedback(VacuumConfig vacuum, bool waitFeedback)
        {
            return waitFeedback && vacuum != null && vacuum.UseFeedbackCheck;
        }

        private bool NeedWaitWorkpiece(VacuumConfig vacuum, bool waitWorkpiece)
        {
            return waitWorkpiece && vacuum != null && vacuum.UseWorkpieceCheck;
        }

        private int GetBuildTimeout(int timeoutMs)
        {
            return timeoutMs > 0 ? timeoutMs : DefaultActionTimeoutMs;
        }

        private int GetReleaseTimeout(int timeoutMs)
        {
            return timeoutMs > 0 ? timeoutMs : DefaultActionTimeoutMs;
        }

        private int GetCacheStaleToleranceMs(MotionIoRuntimeState runtimeState)
        {
            var scanInterval = runtimeState.ScanIntervalMs > 0
                ? runtimeState.ScanIntervalMs
                : DefaultPollIntervalMs;

            var calculated = scanInterval * 4;
            return calculated > DefaultCacheStaleToleranceMs
                ? calculated
                : DefaultCacheStaleToleranceMs;
        }

        private Result ValidateRuntimeCache(MotionIoRuntimeState runtimeState)
        {
            if (runtimeState == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "Motion IO 运行时缓存未初始化");
            }

            if (!runtimeState.IsScanServiceRunning)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "Motion IO 扫描工作单元未运行");
            }

            if (!runtimeState.LastScanTime.HasValue)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "Motion IO 运行时缓存尚无扫描数据");
            }

            var ageMs = (DateTime.Now - runtimeState.LastScanTime.Value).TotalMilliseconds;
            if (ageMs > GetCacheStaleToleranceMs(runtimeState))
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "Motion IO 运行时缓存已过期");
            }

            return OkSilent("Motion IO 运行时缓存可用");
        }

        private Result TryReadCachedDI(short logicalBit, out bool value)
        {
            value = false;

            var runtimeState = RuntimeContext.Instance.MotionIo;
            var validateResult = ValidateRuntimeCache(runtimeState);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            if (!runtimeState.TryGetDI(logicalBit, out value))
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "逻辑DI缓存不存在: " + logicalBit);
            }

            DateTime updateTime;
            if (runtimeState.TryGetDIUpdateTime(logicalBit, out updateTime))
            {
                var ageMs = (DateTime.Now - updateTime).TotalMilliseconds;
                if (ageMs > GetCacheStaleToleranceMs(runtimeState))
                {
                    return Fail((int)MotionErrorCode.IoMapNotFound, "逻辑DI缓存值已过期: " + logicalBit);
                }
            }

            return OkSilent("逻辑DI缓存读取成功");
        }

        private Result SetOutputsForVacuumOn(VacuumConfig vacuum)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            if (vacuum.BlowOffOutputBit.HasValue)
            {
                var closeBlowOffResult = motionHub.SetDO(vacuum.BlowOffOutputBit.Value, false);
                if (!closeBlowOffResult.Success)
                {
                    return Fail(closeBlowOffResult.Code, "关闭破真空输出失败: " + vacuum.Name);
                }
            }

            var vacuumOnResult = motionHub.SetDO(vacuum.VacuumOnOutputBit, true);
            if (!vacuumOnResult.Success)
            {
                return Fail(vacuumOnResult.Code, "打开真空输出失败: " + vacuum.Name);
            }

            return OkSilent("真空输出打开成功");
        }

        private async Task<Result> SetOutputsForVacuumOffAsync(VacuumConfig vacuum, CancellationToken cancellationToken)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            var vacuumOffResult = motionHub.SetDO(vacuum.VacuumOnOutputBit, false);
            if (!vacuumOffResult.Success)
            {
                return Fail(vacuumOffResult.Code, "关闭真空输出失败: " + vacuum.Name);
            }

            if (vacuum.BlowOffOutputBit.HasValue)
            {
                var blowOffOnResult = motionHub.SetDO(vacuum.BlowOffOutputBit.Value, true);
                if (!blowOffOnResult.Success)
                {
                    return Fail(blowOffOnResult.Code, "打开破真空输出失败: " + vacuum.Name);
                }

                await Task.Delay(DefaultBlowOffPulseMs, cancellationToken);

                var blowOffOffResult = motionHub.SetDO(vacuum.BlowOffOutputBit.Value, false);
                if (!blowOffOffResult.Success)
                {
                    return Fail(blowOffOffResult.Code, "关闭破真空输出失败: " + vacuum.Name);
                }
            }

            return OkSilent("真空输出关闭成功");
        }

        private void RaiseBuildTimeoutAlarm(VacuumConfig vacuum)
        {
            if (vacuum == null)
            {
                return;
            }

            var message = "真空建立超时: " + vacuum.Name;
            if (vacuum.AlarmCodeOnBuildTimeout.HasValue)
            {
                RaiseAlarm((AlarmCode)vacuum.AlarmCodeOnBuildTimeout.Value, AlarmLevel.Warning, message);
                return;
            }

            _reporter?.Warn(MessageSourceName, message, (int)MotionErrorCode.HomeTimeout);
        }

        private void RaiseReleaseTimeoutAlarm(VacuumConfig vacuum)
        {
            if (vacuum == null)
            {
                return;
            }

            var message = "真空释放超时: " + vacuum.Name;
            if (vacuum.AlarmCodeOnReleaseTimeout.HasValue)
            {
                RaiseAlarm((AlarmCode)vacuum.AlarmCodeOnReleaseTimeout.Value, AlarmLevel.Warning, message);
                return;
            }

            _reporter?.Warn(MessageSourceName, message, (int)MotionErrorCode.HomeTimeout);
        }

        private void RaiseWorkpieceLostAlarm(VacuumConfig vacuum, string message)
        {
            if (vacuum == null)
            {
                return;
            }

            if (vacuum.AlarmCodeOnWorkpieceLost.HasValue)
            {
                RaiseAlarm((AlarmCode)vacuum.AlarmCodeOnWorkpieceLost.Value, AlarmLevel.Warning, message);
                return;
            }

            _reporter?.Warn(MessageSourceName, message, (int)MotionErrorCode.HomeTimeout);
        }

        private async Task<Result> WaitForDiStateAsync(
            short logicalBit,
            bool expectedState,
            int timeoutMs,
            string successMessage,
            string timeoutMessage,
            CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bool currentValue;
                var readResult = TryReadCachedDI(logicalBit, out currentValue);
                if (readResult.Success)
                {
                    if (currentValue == expectedState)
                    {
                        return Ok(successMessage);
                    }
                }
                else
                {
                    return readResult;
                }

                if ((DateTime.Now - startTime).TotalMilliseconds >= timeoutMs)
                {
                    return Fail((int)MotionErrorCode.HomeTimeout, timeoutMessage);
                }

                await Task.Delay(DefaultPollIntervalMs, cancellationToken);
            }
        }
    }
}
