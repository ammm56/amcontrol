using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// 统一后台任务宿主管理接口。
    /// </summary>
    public interface IRuntimeTaskManager
    {
        Result Register(IRuntimeWorker worker);

        Result<IRuntimeWorker> QueryAll();

        Result StartAll();

        Task<Result> StopAllAsync();

        Result Start(string name);

        Task<Result> StopAsync(string name);
    }
}