using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Alarm;
using AM.Model.Common;
using AM.Model.Interfaces.Motion.Actuator;
using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard;
using AM.Model.MotionCard.Actuator;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.DBService.Services.Motion.Actuator
{
    /// <summary>
    /// 气缸对象运行时服务。
    /// 通过 MachineContext 中已注册的气缸对象配置与 MotionHub 执行 IO 控制。
    /// 输出动作直接走 MotionHub
    /// 引入全局 IO 扫描缓存层，反馈等待走 RuntimeContext.MotionIo 缓存。
    /// </summary>
    public class CylinderService : ServiceBase, ICylinderService
    {
        #region 常量

        private const int DefaultPollIntervalMs = 50;
        private const int DefaultActionTimeoutMs = 3000;
        private const int DefaultCacheStaleToleranceMs = 500;
        private const int RuntimeCacheStaleLogThrottleIntervalMs = 30000;

        #endregion

        #region 元数据与构造

        protected override string MessageSourceName
        {
            get { return "CylinderService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public CylinderService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public CylinderService(IAppReporter reporter)
            : base(reporter)
        {
        }

        #endregion

        #region 查询与动作

        public Result<CylinderConfig> QueryAll()
        {
            var machine = MachineContext.Instance;

            if (machine.Cylinders == null || machine.Cylinders.Count == 0)
            {
                return Warn<CylinderConfig>((int)MotionErrorCode.IoMapNotFound, "当前未注册任何气缸对象");
            }

            var list = machine.Cylinders.Values
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();

            return OkListLogOnly(list, "气缸对象查询成功");
        }

        public Result<CylinderConfig> QueryByName(string name)
        {
            CylinderConfig cylinder;
            var resolveResult = ResolveCylinder(name, out cylinder);
            if (!resolveResult.Success)
            {
                return Fail<CylinderConfig>(resolveResult.Code, resolveResult.Message);
            }

            return OkLogOnly(cylinder, "气缸对象查询成功");
        }

        public Result Extend(string name, bool waitFeedback = true)
        {
            return ExtendAsync(name, waitFeedback, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public Result Retract(string name, bool waitFeedback = true)
        {
            return RetractAsync(name, waitFeedback, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<Result> ExtendAsync(string name, bool waitFeedback = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                CylinderConfig cylinder;
                var resolveResult = ResolveCylinder(name, out cylinder);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = SetOutputsForExtend(cylinder);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (!NeedWaitFeedback(cylinder, waitFeedback))
                {
                    return Ok("气缸伸出命令执行成功");
                }

                if (!cylinder.ExtendFeedbackBit.HasValue)
                {
                    return Fail((int)MotionErrorCode.IoMapNotFound, "气缸未配置伸出反馈位");
                }

                var waitResult = await WaitForDiStateAsync(
                    cylinder.ExtendFeedbackBit.Value,
                    true,
                    GetTimeout(cylinder.ExtendTimeoutMs),
                    "气缸伸出到位",
                    "气缸伸出等待到位超时: " + cylinder.Name,
                    cancellationToken);

                if (!waitResult.Success && waitResult.Code == (int)MotionErrorCode.HomeTimeout)
                {
                    RaiseTimeoutAlarm(cylinder, true);
                }

                return waitResult;
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "气缸伸出操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "气缸伸出执行失败");
            }
        }

        public async Task<Result> RetractAsync(string name, bool waitFeedback = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                CylinderConfig cylinder;
                var resolveResult = ResolveCylinder(name, out cylinder);
                if (!resolveResult.Success)
                {
                    return resolveResult;
                }

                var outputResult = SetOutputsForRetract(cylinder);
                if (!outputResult.Success)
                {
                    return outputResult;
                }

                if (!NeedWaitFeedback(cylinder, waitFeedback))
                {
                    return Ok("气缸缩回命令执行成功");
                }

                if (!cylinder.RetractFeedbackBit.HasValue)
                {
                    return Fail((int)MotionErrorCode.IoMapNotFound, "气缸未配置缩回反馈位");
                }

                var waitResult = await WaitForDiStateAsync(
                    cylinder.RetractFeedbackBit.Value,
                    true,
                    GetTimeout(cylinder.RetractTimeoutMs),
                    "气缸缩回到位",
                    "气缸缩回等待到位超时: " + cylinder.Name,
                    cancellationToken);

                if (!waitResult.Success && waitResult.Code == (int)MotionErrorCode.HomeTimeout)
                {
                    RaiseTimeoutAlarm(cylinder, false);
                }

                return waitResult;
            }
            catch (OperationCanceledException)
            {
                return Fail((int)MotionErrorCode.Unknown, "气缸缩回操作已取消");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)MotionErrorCode.Unknown, "气缸缩回执行失败");
            }
        }

        public Result<bool> IsExtended(string name)
        {
            CylinderConfig cylinder;
            var resolveResult = ResolveCylinder(name, out cylinder);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!cylinder.ExtendFeedbackBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "气缸未配置伸出反馈位");
            }

            bool value;
            var readResult = TryReadCachedDI(cylinder.ExtendFeedbackBit.Value, out value);
            if (!readResult.Success)
            {
                return ForwardSilentFailure<bool>(readResult);
            }

            return OkSilent(value, "气缸伸出状态读取成功");
        }

        public Result<bool> IsRetracted(string name)
        {
            CylinderConfig cylinder;
            var resolveResult = ResolveCylinder(name, out cylinder);
            if (!resolveResult.Success)
            {
                return Fail<bool>(resolveResult.Code, resolveResult.Message);
            }

            if (!cylinder.RetractFeedbackBit.HasValue)
            {
                return Fail<bool>((int)MotionErrorCode.IoMapNotFound, "气缸未配置缩回反馈位");
            }

            bool value;
            var readResult = TryReadCachedDI(cylinder.RetractFeedbackBit.Value, out value);
            if (!readResult.Success)
            {
                return ForwardSilentFailure<bool>(readResult);
            }

            return OkSilent(value, "气缸缩回状态读取成功");
        }

        private Result ResolveCylinder(string name, out CylinderConfig cylinder)
        {
            cylinder = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "气缸名称不能为空");
            }

            var machine = MachineContext.Instance;
            if (machine.Cylinders == null || machine.Cylinders.Count == 0)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "当前未注册任何气缸对象");
            }

            if (!machine.Cylinders.TryGetValue(name.Trim(), out cylinder) || cylinder == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "未找到气缸对象: " + name);
            }

            if (!cylinder.IsEnabled)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "气缸对象未启用: " + name);
            }

            return OkSilent("气缸对象解析成功");
        }

        private IMotionCardService GetMotionHub()
        {
            return MachineContext.Instance.MotionHub;
        }

        private bool NeedWaitFeedback(CylinderConfig cylinder, bool waitFeedback)
        {
            return waitFeedback && cylinder != null && cylinder.UseFeedbackCheck;
        }

        private int GetTimeout(int timeoutMs)
        {
            return timeoutMs > 0 ? timeoutMs : DefaultActionTimeoutMs;
        }

        private Result TryReadCachedDI(short logicalBit, out bool value)
        {
            return TryReadMotionIoCachedDI(
                logicalBit,
                DefaultPollIntervalMs,
                DefaultCacheStaleToleranceMs,
                RuntimeCacheStaleLogThrottleIntervalMs,
                out value);
        }

        private Result SetOutputsForExtend(CylinderConfig cylinder)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            if (string.Equals(cylinder.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                if (cylinder.RetractOutputBit.HasValue && !cylinder.AllowBothOn)
                {
                    var offRetractResult = motionHub.SetDO(cylinder.RetractOutputBit.Value, false);
                    if (!offRetractResult.Success)
                    {
                        return Fail(offRetractResult.Code, "关闭气缸缩回输出失败: " + cylinder.Name);
                    }
                }

                var extendResult = motionHub.SetDO(cylinder.ExtendOutputBit, true);
                if (!extendResult.Success)
                {
                    return Fail(extendResult.Code, "气缸伸出输出失败: " + cylinder.Name);
                }

                return Ok("气缸伸出输出成功");
            }

            var singleExtendResult = motionHub.SetDO(cylinder.ExtendOutputBit, true);
            if (!singleExtendResult.Success)
            {
                return Fail(singleExtendResult.Code, "单线圈气缸伸出输出失败: " + cylinder.Name);
            }

            return Ok("气缸伸出输出成功");
        }

        private Result SetOutputsForRetract(CylinderConfig cylinder)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            if (string.Equals(cylinder.DriveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                if (!cylinder.AllowBothOn)
                {
                    var offExtendResult = motionHub.SetDO(cylinder.ExtendOutputBit, false);
                    if (!offExtendResult.Success)
                    {
                        return Fail(offExtendResult.Code, "关闭气缸伸出输出失败: " + cylinder.Name);
                    }
                }

                if (cylinder.RetractOutputBit.HasValue)
                {
                    var retractResult = motionHub.SetDO(cylinder.RetractOutputBit.Value, true);
                    if (!retractResult.Success)
                    {
                        return Fail(retractResult.Code, "气缸缩回输出失败: " + cylinder.Name);
                    }
                }
                else if (!cylinder.AllowBothOff)
                {
                    return Fail((int)MotionErrorCode.InvalidIoBit, "双线圈气缸未配置缩回输出位: " + cylinder.Name);
                }

                return Ok("气缸缩回输出成功");
            }

            var singleRetractResult = motionHub.SetDO(cylinder.ExtendOutputBit, false);
            if (!singleRetractResult.Success)
            {
                return Fail(singleRetractResult.Code, "单线圈气缸缩回输出失败: " + cylinder.Name);
            }

            return Ok("气缸缩回输出成功");
        }

        private void RaiseTimeoutAlarm(CylinderConfig cylinder, bool isExtend)
        {
            if (cylinder == null)
            {
                return;
            }

            var alarmCode = isExtend
                ? cylinder.AlarmCodeOnExtendTimeout
                : cylinder.AlarmCodeOnRetractTimeout;

            var actionText = isExtend ? "伸出" : "缩回";
            var message = "气缸" + actionText + "超时: " + cylinder.Name;

            if (alarmCode.HasValue)
            {
                RaiseAlarm((AlarmCode)alarmCode.Value, AlarmLevel.Warning, message);
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

        #endregion
    }
}