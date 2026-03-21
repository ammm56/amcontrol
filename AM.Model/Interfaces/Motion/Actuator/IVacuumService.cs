using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using System.Threading;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Motion.Actuator
{
    /// <summary>
    /// 真空对象运行时服务接口。
    /// 基于第三层对象配置执行吸真空、释放真空、反馈等待与工件检测等动作。
    /// </summary>
    public interface IVacuumService
    {
        /// <summary>
        /// 查询全部已注册真空对象。
        /// </summary>
        Result<VacuumConfig> QueryAll();

        /// <summary>
        /// 按名称查询真空对象。
        /// </summary>
        Result<VacuumConfig> QueryByName(string name);

        /// <summary>
        /// 打开真空。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="waitFeedback">是否等待真空建立反馈。</param>
        /// <param name="waitWorkpiece">是否等待工件存在检测。</param>
        Result VacuumOn(string name, bool waitFeedback = true, bool waitWorkpiece = false);

        /// <summary>
        /// 关闭真空。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="waitFeedback">是否等待释放反馈。</param>
        Result VacuumOff(string name, bool waitFeedback = true);

        /// <summary>
        /// 异步打开真空。
        /// </summary>
        Task<Result> VacuumOnAsync(
            string name,
            bool waitFeedback = true,
            bool waitWorkpiece = false,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 异步关闭真空。
        /// </summary>
        Task<Result> VacuumOffAsync(
            string name,
            bool waitFeedback = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 判断真空是否建立。
        /// </summary>
        Result<bool> IsVacuumBuilt(string name);

        /// <summary>
        /// 判断真空是否已释放。
        /// </summary>
        Result<bool> IsReleased(string name);

        /// <summary>
        /// 判断是否检测到工件。
        /// </summary>
        Result<bool> HasWorkpiece(string name);
    }
}
