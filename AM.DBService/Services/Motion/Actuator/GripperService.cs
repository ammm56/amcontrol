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
    /// 夹爪对象运行时服务。
    /// 通过 MachineContext 中已注册的夹爪对象配置与 MotionHub 执行 IO 控制。
    /// 输出动作直接走 MotionHub；
    /// 反馈等待与工件检测统一走 RuntimeContext.MotionIo 缓存。
    /// </summary>
    public class GripperService : ServiceBase, IGripperService
    {
        private const int DefaultPollIntervalMs = 50;
        private const int DefaultActionTimeoutMs = 3000;
        private const int DefaultCacheStaleToleranceMs = 500;

        protected override string MessageSourceName
        {
            get { return "GripperService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public GripperService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public GripperService(IAppReporter reporter)
            : base(reporter)
        {
        }

        public Result<GripperConfig> QueryAll()
        {
            var machine = MachineContext.Instance;

            if (machine.Grippers == null || machine.Grippers.Count == 0)
            {
                return Warn<GripperConfig>((int)MotionErrorCode.IoMapNotFound, "当前未注册任何夹爪对象");
            }

            var list = machine.Grippers.Values
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();

            return OkListLogOnly(list, "夹爪对象查询成功");
        }

        public Result<GripperConfig> QueryByName(string name)
        {
            GripperConfig gripper;
            var resolveResult = ResolveGripper(name, out gripper);
            if (!resolveResult.Success)
            {
                return Fail<GripperConfig>(resolveResult.Code, resolveResult.Message);
            }

            return OkLogOnly(gripper, "夹爪对象查询成功");
        }

        public Result Close(string name, bool waitFeedback = true, bool waitWorkpiece = false)
        {
            return CloseAsync(name, waitFeedback, waitWorkpiece, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public Result Open(string name, bool waitFeedback = true)
        {
            return OpenAsync(name, waitFeedback, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<Result> CloseAsync(
            string name,
            bool waitFeedback = true,
            bool waitWorkpiece = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                GripperConfig gripper;
                var resolveResult = ResolveGripper(name, out gripper);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = SetOutputsForClose(gripper);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (NeedWaitFeedback(gripper, waitFeedback))
                {
                    if (!gripper.CloseFeedbackBit.HasValue)
                    {
                        return Fail((int)MotionErrorCode.IoMapNotFound, "夹爪未配置夹紧反馈位");
                    }

                    var waitCloseResult = await WaitForDiStateAsync(
                        gripper.CloseFeedbackBit.Value,
                        true,
                        GetCloseTimeout(gripper.CloseTimeoutMs),
                        "夹爪夹紧成功",
                        "夹爪夹紧等待到位超时: " + gripper.Name,
                        cancellationToken);

                    if (!waitCloseResult.Success && waitCloseResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseCloseTimeoutAlarm(gripper);
                    }

                    if (!waitCloseResult.Success)
                    {
                        return waitCloseResult;
                    }
                }

                if (NeedWaitWorkpiece(gripper, waitWorkpiece))
                {
                    if (!gripper.WorkpiecePresentBit.HasValue)
                    {
                        return Fail((int)MotionErrorCode.IoMapNotFound, "夹爪未配置工件检测位");
                    }

                    var waitWorkpieceResult = await WaitForDiStateAsync(
                        gripper.WorkpiecePresentBit.Value,
                        true,
                        GetCloseTimeout(gripper.CloseTimeoutMs),
                        "夹爪工件检测成功",
                        "夹爪等待工件检测超时: " + gripper.Name,
                        cancellationToken);

                    if (!waitWorkpieceResult.Success && waitWorkpieceResult.Code == (int)MotionErrorCode.HomeTimeout)
                    {
                        RaiseWorkpieceLostAlarm(gripper, "夹爪夹紧后未检测到工件: " + gripper.Name);
                    }

                    if (!waitWorkpieceResult.Success)
                    {
                        return waitWorkpieceResult;
                    }
                }

                return Ok("夹爪夹紧命令执行成功");
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "夹爪夹紧操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "夹爪夹紧执行失败");
            }
        }

        public async Task<Result> OpenAsync(
            string name,
            bool waitFeedback = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                GripperConfig gripper;
                var resolveResult = ResolveGripper(name, out gripper);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = SetOutputsForOpen(gripper);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (!NeedWaitFeedback(gripper, waitFeedback))
                {
                    return Ok("夹爪打开命令执行成功");
                }

                if (!gripper.OpenFeedbackBit.HasValue)
                {
                    return Fail((int)MotionErrorCode.IoMapNotFound, "夹爪未配置打开反馈位");
                }

                var waitOpenResult = await WaitForDiStateAsync(
                    gripper.OpenFeedbackBit.Value,
                    true,
                    GetOpenTimeout(gripper.OpenTimeoutMs),
                    "夹爪打开成功",
                    "夹爪打开等待到位超时: " + gripper.Name,
                    cancellationToken);

                if (!waitOpenResult.Success && waitOpenResult.Code == (int)MotionErrorCode.HomeTimeout)
                {
                    RaiseOpenTimeoutAlarm(gripper);
                }

                return waitOpenResult;
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "夹爪打开操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "夹爪打开执行失败");
            }
        }

        public Result<bool> IsClosed(string name)
        {
            GripperConfig gripper;
            var resolveResult = ResolveGripper(name, out gripper);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!gripper.CloseFeedbackBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "夹爪未配置夹紧反馈位");
            }

            bool value;
            var readResult = TryReadCachedDI(gripper.CloseFeedbackBit.Value, out value);
            if (!readResult.Success)
            {
                return Fail<bool>(readResult.Code, readResult.Message);
            }

            return OkSilent(value, "夹爪夹紧状态读取成功");
        }

        public Result<bool> IsOpened(string name)
        {
            GripperConfig gripper;
            var resolveResult = ResolveGripper(name, out gripper);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!gripper.OpenFeedbackBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "夹爪未配置打开反馈位");
            }

            bool value;
            var readResult = TryReadCachedDI(gripper.OpenFeedbackBit.Value, out value);
            if (!readResult.Success)
            {
                return Fail<bool>(readResult.Code, readResult.Message);
            }

            return OkSilent(value, "夹爪打开状态读取成功");
        }

        public Result<bool> HasWorkpiece(string name)
        {
            GripperConfig gripper;
            var resolveResult = ResolveGripper(name, out gripper);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!gripper.WorkpiecePresentBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "夹爪未配置工件检测位");
            }

            bool value;
            var readResult = TryReadCachedDI(gripper.WorkpiecePresentBit.Value, out value);
            if (!readResult.Success)
            {
                return Fail<bool>(readResult.Code, readResult.Message);
            }

            return OkSilent(value, "夹爪工件检测状态读取成功");
        }

        private Result ResolveGripper(string name, out GripperConfig gripper)
        {
            gripper = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "夹爪对象名称不能为空");
            }

            var machine = MachineContext.Instance;
            if (machine.Grippers == null || machine.Grippers.Count == 0)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "当前未注册任何夹爪对象");
            }

            if (!machine.Grippers.TryGetValue(name.Trim(), out gripper) || gripper == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "未找到夹爪对象: " + name);
            }

            if (!gripper.IsEnabled)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "夹爪对象未启用: " + name);
            }

            return OkSilent("夹爪对象解析成功");
        }

        private IMotionCardService GetMotionHub()
        {
            return MachineContext.Instance.MotionHub;
        }

        private bool NeedWaitFeedback(GripperConfig gripper, bool waitFeedback)
        {
            return waitFeedback && gripper != null && gripper.UseFeedbackCheck;
        }

        private bool NeedWaitWorkpiece(GripperConfig gripper, bool waitWorkpiece)
        {
            return waitWorkpiece && gripper != null && gripper.UseWorkpieceCheck;
        }

        private int GetCloseTimeout(int timeoutMs)
        {
            return timeoutMs > 0 ? timeoutMs : DefaultActionTimeoutMs;
        }

        private int GetOpenTimeout(int timeoutMs)
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

        private Result SetOutputsForClose(GripperConfig gripper)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            if (string.Equals(gripper.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                if (gripper.OpenOutputBit.HasValue && !gripper.AllowBothOn)
                {
                    var offOpenResult = motionHub.SetDO(gripper.OpenOutputBit.Value, false);
                    if (!offOpenResult.Success)
                    {
                        return Fail(offOpenResult.Code, "关闭夹爪打开输出失败: " + gripper.Name);
                    }
                }

                var closeResult = motionHub.SetDO(gripper.CloseOutputBit, true);
                if (!closeResult.Success)
                {
                    return Fail(closeResult.Code, "夹爪夹紧输出失败: " + gripper.Name);
                }

                return OkSilent("夹爪夹紧输出成功");
            }

            var singleCloseResult = motionHub.SetDO(gripper.CloseOutputBit, true);
            if (!singleCloseResult.Success)
            {
                return Fail(singleCloseResult.Code, "单线圈夹爪夹紧输出失败: " + gripper.Name);
            }

            return OkSilent("夹爪夹紧输出成功");
        }

        private Result SetOutputsForOpen(GripperConfig gripper)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            if (string.Equals(gripper.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                if (!gripper.AllowBothOn)
                {
                    var offCloseResult = motionHub.SetDO(gripper.CloseOutputBit, false);
                    if (!offCloseResult.Success)
                    {
                        return Fail(offCloseResult.Code, "关闭夹爪夹紧输出失败: " + gripper.Name);
                    }
                }

                if (gripper.OpenOutputBit.HasValue)
                {
                    var openResult = motionHub.SetDO(gripper.OpenOutputBit.Value, true);
                    if (!openResult.Success)
                    {
                        return Fail(openResult.Code, "夹爪打开输出失败: " + gripper.Name);
                    }
                }
                else if (!gripper.AllowBothOff)
                {
                    return Fail((int)MotionErrorCode.InvalidIoBit, "双线圈夹爪未配置打开输出位: " + gripper.Name);
                }

                return OkSilent("夹爪打开输出成功");
            }

            var singleOpenResult = motionHub.SetDO(gripper.CloseOutputBit, false);
            if (!singleOpenResult.Success)
            {
                return Fail(singleOpenResult.Code, "单线圈夹爪打开输出失败: " + gripper.Name);
            }

            return OkSilent("夹爪打开输出成功");
        }

        private void RaiseCloseTimeoutAlarm(GripperConfig gripper)
        {
            if (gripper == null)
            {
                return;
            }

            var message = "夹爪夹紧超时: " + gripper.Name;
            if (gripper.AlarmCodeOnCloseTimeout.HasValue)
            {
                RaiseAlarm((AlarmCode)gripper.AlarmCodeOnCloseTimeout.Value, AlarmLevel.Warning, message);
                return;
            }

            _reporter?.Warn(MessageSourceName, message, (int)MotionErrorCode.HomeTimeout);
        }

        private void RaiseOpenTimeoutAlarm(GripperConfig gripper)
        {
            if (gripper == null)
            {
                return;
            }

            var message = "夹爪打开超时: " + gripper.Name;
            if (gripper.AlarmCodeOnOpenTimeout.HasValue)
            {
                RaiseAlarm((AlarmCode)gripper.AlarmCodeOnOpenTimeout.Value, AlarmLevel.Warning, message);
                return;
            }

            _reporter?.Warn(MessageSourceName, message, (int)MotionErrorCode.HomeTimeout);
        }

        private void RaiseWorkpieceLostAlarm(GripperConfig gripper, string message)
        {
            if (gripper == null)
            {
                return;
            }

            if (gripper.AlarmCodeOnWorkpieceLost.HasValue)
            {
                RaiseAlarm((AlarmCode)gripper.AlarmCodeOnWorkpieceLost.Value, AlarmLevel.Warning, message);
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