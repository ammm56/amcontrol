using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// IO 后台扫描服务接口。
    /// 负责周期性扫描 Motion DI/DO，并写入 RuntimeContext。
    /// </summary>
    public interface IIoScanService
    {
        /// <summary>
        /// 是否正在运行。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 启动后台扫描。
        /// </summary>
        Result Start(int scanIntervalMs = 50);

        /// <summary>
        /// 停止后台扫描。
        /// </summary>
        Task<Result> StopAsync();

        /// <summary>
        /// 立即执行一次扫描。
        /// </summary>
        Result ScanOnce();
    }
}