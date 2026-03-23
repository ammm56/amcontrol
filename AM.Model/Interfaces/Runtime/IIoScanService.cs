using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.Runtime
{
    /// <summary>
    /// IO 后台扫描工作单元接口。
    /// 负责周期性扫描 Motion DI/DO，并写入 RuntimeContext。
    /// 工业设备要求：扫描出错后立即停止并触发报警，需手动重启。
    /// </summary>
    public interface IIoScanService : IRuntimeWorker
    {
        /// <summary>
        /// 当前扫描周期，单位 ms。
        /// </summary>
        int ScanIntervalMs { get; }

        /// <summary>
        /// 当前扫描状态。
        /// Idle：未运行；Running：扫描中；Error：因错误自动停止，需手动重启。
        /// </summary>
        IoScanState ScanState { get; }

        /// <summary>
        /// 立即执行一次扫描。
        /// 扫描失败时返回 Fail 结果，由调用方决定后续处理。
        /// </summary>
        Result ScanOnce();
    }
}