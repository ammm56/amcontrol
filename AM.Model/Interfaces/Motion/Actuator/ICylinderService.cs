using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using System.Threading;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Motion.Actuator
{
    /// <summary>
    /// 气缸对象运行时服务接口。
    /// 基于第三层对象配置执行伸出、缩回、反馈等待等动作。
    /// </summary>
    public interface ICylinderService
    {
        /// <summary>
        /// 查询全部已注册气缸。
        /// </summary>
        Result<CylinderConfig> QueryAll();

        /// <summary>
        /// 按名称查询气缸对象。
        /// </summary>
        Result<CylinderConfig> QueryByName(string name);

        /// <summary>
        /// 伸出气缸。
        /// </summary>
        Result Extend(string name, bool waitFeedback = true);

        /// <summary>
        /// 缩回气缸。
        /// </summary>
        Result Retract(string name, bool waitFeedback = true);

        /// <summary>
        /// 异步伸出气缸。
        /// </summary>
        Task<Result> ExtendAsync(string name, bool waitFeedback = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 异步缩回气缸。
        /// </summary>
        Task<Result> RetractAsync(string name, bool waitFeedback = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 判断是否伸出到位。
        /// </summary>
        Result<bool> IsExtended(string name);

        /// <summary>
        /// 判断是否缩回到位。
        /// </summary>
        Result<bool> IsRetracted(string name);
    }
}