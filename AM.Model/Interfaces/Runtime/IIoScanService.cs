using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// IO 后台扫描工作单元接口。
    /// 负责周期性扫描 Motion DI/DO，并写入 RuntimeContext。
    /// </summary>
    public interface IIoScanService : IRuntimeWorker
    {
        /// <summary>
        /// 当前扫描周期，单位 ms。
        /// </summary>
        int ScanIntervalMs { get; }

        /// <summary>
        /// 立即执行一次扫描。
        /// </summary>
        Result ScanOnce();
    }
}