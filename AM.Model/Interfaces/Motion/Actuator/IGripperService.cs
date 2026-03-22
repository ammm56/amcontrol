using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using System.Threading;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Motion.Actuator
{
    /// <summary>
    /// 夹爪对象运行时服务接口。
    /// 基于第三层对象配置执行夹紧、打开、反馈等待与工件检测等动作。
    /// </summary>
    public interface IGripperService
    {
        /// <summary>
        /// 查询全部已注册夹爪对象。
        /// </summary>
        Result<GripperConfig> QueryAll();

        /// <summary>
        /// 按名称查询夹爪对象。
        /// </summary>
        Result<GripperConfig> QueryByName(string name);

        /// <summary>
        /// 夹紧夹爪。
        /// </summary>
        Result Close(string name, bool waitFeedback = true, bool waitWorkpiece = false);

        /// <summary>
        /// 打开夹爪。
        /// </summary>
        Result Open(string name, bool waitFeedback = true);

        /// <summary>
        /// 异步夹紧夹爪。
        /// </summary>
        Task<Result> CloseAsync(
            string name,
            bool waitFeedback = true,
            bool waitWorkpiece = false,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 异步打开夹爪。
        /// </summary>
        Task<Result> OpenAsync(
            string name,
            bool waitFeedback = true,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 判断是否夹紧到位。
        /// </summary>
        Result<bool> IsClosed(string name);

        /// <summary>
        /// 判断是否打开到位。
        /// </summary>
        Result<bool> IsOpened(string name);

        /// <summary>
        /// 判断是否检测到工件。
        /// </summary>
        Result<bool> HasWorkpiece(string name);
    }
}