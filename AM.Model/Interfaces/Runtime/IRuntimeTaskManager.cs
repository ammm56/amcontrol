using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// 统一后台任务宿主管理接口。
    /// </summary>
    public interface IRuntimeTaskManager
    {
        /// <summary>
        /// 注册后台工作单元。
        /// </summary>
        Result Register(IRuntimeWorker worker);

        /// <summary>
        /// 注册后台工作单元，并根据 autoStart 决定是否立即启动。
        /// 注册失败返回 Fail；启动失败仅发出 Warn，注册结果仍为成功。
        /// </summary>
        Result Register(IRuntimeWorker worker, bool autoStart);

        Result<IRuntimeWorker> QueryAll();

        Result StartAll();

        Task<Result> StopAllAsync();

        Result Start(string name);

        Task<Result> StopAsync(string name);
    }
}