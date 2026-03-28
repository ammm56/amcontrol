using AM.Model.Common;

namespace AM.Model.Interfaces.Plc.Runtime
{
    /// <summary>
    /// PLC 扫描工作单元接口。
    /// 负责后台循环采样与运行时缓存写入。
    /// </summary>
    public interface IPlcScanWorker
    {
        /// <summary>
        /// 是否正在运行。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 启动扫描。
        /// </summary>
        Result Start();

        /// <summary>
        /// 停止扫描。
        /// </summary>
        Result Stop();

        /// <summary>
        /// 执行单轮扫描。
        /// </summary>
        Result ScanOnce();
    }
}
