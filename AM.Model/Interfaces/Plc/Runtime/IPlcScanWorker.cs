using AM.Model.Common;
using AM.Model.Interfaces.Runtime;

namespace AM.Model.Interfaces.Plc.Runtime
{
    /// <summary>
    /// PLC 扫描工作单元接口。
    /// 该接口继承统一后台工作单元接口 <see cref="IRuntimeWorker"/>，
    /// 用于承载 PLC 后台循环采样与运行时缓存写入能力。
    ///
    /// 设计约束：
    /// 1. PLC 扫描属于后台工作单元，因此统一纳入 <see cref="IRuntimeWorker"/> 生命周期管理；
    /// 2. 扫描实现应优先复用已初始化的客户端对象与配置缓存；
    /// 3. 扫描循环中避免重复查询、重复分组、重复创建不必要临时对象；
    /// 4. 上层页面不直接操作具体线程或任务，仅通过本接口控制扫描服务。
    /// </summary>
    public interface IPlcScanWorker : IRuntimeWorker
    {
        /// <summary>
        /// 是否正在运行。
        /// 用于表示后台扫描循环当前是否处于活动状态。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 启动扫描。
        /// 创建后台扫描循环，并按既定周期执行 PLC 点位采样。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功：表示扫描循环已启动；
        /// - 失败：表示当前状态不允许启动，或初始化扫描环境失败。
        /// </returns>
        Result Start();

        /// <summary>
        /// 同步停止扫描。
        /// 该方法主要用于运行时调试、人工停止与故障恢复场景。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功：表示扫描已停止，或当前本就未运行；
        /// - 失败：表示停止过程中发生异常。
        /// </returns>
        Result Stop();

        /// <summary>
        /// 手动执行单轮扫描。
        /// 不依赖常驻后台循环，可用于调试、测试与即时刷新场景。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功：表示单轮扫描已完成；
        /// - 失败：表示扫描过程中发生连接、读取或运行态写入异常。
        /// </returns>
        Result ScanOnce();
    }
}