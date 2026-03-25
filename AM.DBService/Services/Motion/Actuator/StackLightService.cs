using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
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
    /// 灯塔/声光报警对象运行时服务。
    /// 通过 MachineContext 中已注册的灯塔对象配置与 MotionHub 执行 DO 控制。
    /// 首版采用固定业务语义映射：
    /// Idle/Running -> 绿灯；
    /// Warning -> 黄灯；
    /// Alarm -> 红灯；
    /// 可按参数决定是否带蜂鸣器。
    /// </summary>
    public class StackLightService : ServiceBase, IStackLightService
    {
        protected override string MessageSourceName
        {
            get { return "StackLightService"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Motion; }
        }

        public StackLightService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public StackLightService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 查询全部已注册灯塔对象。
        /// </summary>
        public Result<StackLightConfig> QueryAll()
        {
            var machine = MachineContext.Instance;

            if (machine.StackLights == null || machine.StackLights.Count == 0)
            {
                return Warn<StackLightConfig>((int)MotionErrorCode.IoMapNotFound, "当前未注册任何灯塔对象");
            }

            var list = machine.StackLights.Values
                .Where(p => p != null && p.IsEnabled)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();

            return OkListLogOnly(list, "灯塔对象查询成功");
        }

        /// <summary>
        /// 按名称查询灯塔对象。
        /// </summary>
        public Result<StackLightConfig> QueryByName(string name)
        {
            StackLightConfig stackLight;
            var resolveResult = ResolveStackLight(name, out stackLight);
            if (!resolveResult.Success)
            {
                return Fail<StackLightConfig>(resolveResult.Code, resolveResult.Message);
            }

            return OkLogOnly(stackLight, "灯塔对象查询成功");
        }

        /// <summary>
        /// 设置指定状态。
        /// </summary>
        public Result SetState(string name, StackLightState state, bool withBuzzer = false)
        {
            return SetStateAsync(name, state, withBuzzer, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// 异步设置指定状态。
        /// </summary>
        public Task<Result> SetStateAsync(
            string name,
            StackLightState state,
            bool withBuzzer = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            StackLightConfig stackLight;
            var resolveResult = ResolveStackLight(name, out stackLight);
            if (!resolveResult.Success)
            {
                return Task.FromResult(resolveResult);
            }

            var applyResult = ApplyState(stackLight, state, withBuzzer);
            return Task.FromResult(applyResult);
        }

        /// <summary>
        /// 熄灭灯塔。
        /// </summary>
        public Result TurnOff(string name)
        {
            return SetState(name, StackLightState.Off, false);
        }

        /// <summary>
        /// 设置为空闲状态。
        /// </summary>
        public Result SetIdle(string name)
        {
            return SetState(name, StackLightState.Idle, false);
        }

        /// <summary>
        /// 设置为运行状态。
        /// </summary>
        public Result SetRunning(string name)
        {
            return SetState(name, StackLightState.Running, false);
        }

        /// <summary>
        /// 设置为警告状态。
        /// </summary>
        public Result SetWarning(string name, bool withBuzzer = false)
        {
            return SetState(name, StackLightState.Warning, withBuzzer);
        }

        /// <summary>
        /// 设置为报警状态。
        /// </summary>
        public Result SetAlarm(string name, bool withBuzzer = true)
        {
            return SetState(name, StackLightState.Alarm, withBuzzer);
        }

        private Result ResolveStackLight(string name, out StackLightConfig stackLight)
        {
            stackLight = null;

            if (string.IsNullOrWhiteSpace(name))
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "灯塔对象名称不能为空");
            }

            var machine = MachineContext.Instance;
            if (machine.StackLights == null || machine.StackLights.Count == 0)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "当前未注册任何灯塔对象");
            }

            if (!machine.StackLights.TryGetValue(name.Trim(), out stackLight) || stackLight == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "未找到灯塔对象: " + name);
            }

            if (!stackLight.IsEnabled)
            {
                return Fail((int)MotionErrorCode.InvalidIoBit, "灯塔对象未启用: " + name);
            }

            return OkSilent("灯塔对象解析成功");
        }

        private IMotionCardService GetMotionHub()
        {
            return MachineContext.Instance.MotionHub;
        }

        private Result ApplyState(StackLightConfig stackLight, StackLightState state, bool withBuzzer)
        {
            var motionHub = GetMotionHub();
            if (motionHub == null)
            {
                return Fail((int)MotionErrorCode.IoMapNotFound, "MotionHub 未初始化");
            }

            bool red = false;
            bool yellow = false;
            bool green = false;
            bool blue = false;
            bool buzzer = false;

            switch (state)
            {
                case StackLightState.Off:
                    break;

                case StackLightState.Idle:
                case StackLightState.Running:
                    green = true;
                    break;

                case StackLightState.Warning:
                    yellow = true;
                    buzzer = withBuzzer || stackLight.EnableBuzzerOnWarning;
                    break;

                case StackLightState.Alarm:
                    red = true;
                    buzzer = withBuzzer || stackLight.EnableBuzzerOnAlarm;
                    break;

                case StackLightState.Red:
                    red = true;
                    break;

                case StackLightState.Yellow:
                    yellow = true;
                    break;

                case StackLightState.Green:
                    green = true;
                    break;

                case StackLightState.Blue:
                    blue = true;
                    break;

                default:
                    return Fail((int)MotionErrorCode.InvalidIoBit, "不支持的灯塔状态: " + state);
            }

            if (!stackLight.AllowMultiSegmentOn)
            {
                var onCount = (red ? 1 : 0) + (yellow ? 1 : 0) + (green ? 1 : 0) + (blue ? 1 : 0);
                if (onCount > 1)
                {
                    return Fail((int)MotionErrorCode.InvalidIoBit, "当前灯塔配置不允许多颜色同时点亮");
                }
            }

            Result result;

            result = SetOptionalOutput(motionHub, stackLight.RedOutputBit, red, "红灯", stackLight.Name);
            if (!result.Success) return result;

            result = SetOptionalOutput(motionHub, stackLight.YellowOutputBit, yellow, "黄灯", stackLight.Name);
            if (!result.Success) return result;

            result = SetOptionalOutput(motionHub, stackLight.GreenOutputBit, green, "绿灯", stackLight.Name);
            if (!result.Success) return result;

            result = SetOptionalOutput(motionHub, stackLight.BlueOutputBit, blue, "蓝灯", stackLight.Name);
            if (!result.Success) return result;

            result = SetOptionalOutput(motionHub, stackLight.BuzzerOutputBit, buzzer, "蜂鸣器", stackLight.Name);
            if (!result.Success) return result;

            return Ok("灯塔状态切换成功");
        }

        private Result SetOptionalOutput(
            IMotionCardService motionHub,
            short? logicalBit,
            bool targetState,
            string partName,
            string stackLightName)
        {
            if (!logicalBit.HasValue)
            {
                if (targetState)
                {
                    return Fail((int)MotionErrorCode.IoMapNotFound, stackLightName + " 未配置" + partName + "输出位");
                }

                return OkSilent("未配置输出位且目标状态为关闭");
            }

            var setResult = motionHub.SetDO(logicalBit.Value, targetState);
            if (!setResult.Success)
            {
                return Fail(setResult.Code, stackLightName + " 设置" + partName + "失败");
            }

            return OkSilent(partName + "输出设置成功");
        }
    }
}