using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using System.Threading;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Motion.Actuator
{
    /// <summary>
    /// 灯塔/声光报警对象运行时服务接口。
    /// 基于第三层对象配置执行灯塔状态切换与蜂鸣器控制。
    /// </summary>
    public interface IStackLightService
    {
        /// <summary>
        /// 查询全部已注册灯塔对象。
        /// </summary>
        Result<StackLightConfig> QueryAll();

        /// <summary>
        /// 按名称查询灯塔对象。
        /// </summary>
        Result<StackLightConfig> QueryByName(string name);

        /// <summary>
        /// 设置指定状态。
        /// </summary>
        Result SetState(string name, StackLightState state, bool withBuzzer = false);

        /// <summary>
        /// 异步设置指定状态。
        /// </summary>
        Task<Result> SetStateAsync(
            string name,
            StackLightState state,
            bool withBuzzer = false,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 熄灭灯塔。
        /// </summary>
        Result TurnOff(string name);

        /// <summary>
        /// 设置为空闲状态。
        /// </summary>
        Result SetIdle(string name);

        /// <summary>
        /// 设置为运行状态。
        /// </summary>
        Result SetRunning(string name);

        /// <summary>
        /// 设置为警告状态。
        /// </summary>
        Result SetWarning(string name, bool withBuzzer = false);

        /// <summary>
        /// 设置为报警状态。
        /// </summary>
        Result SetAlarm(string name, bool withBuzzer = true);
    }
}