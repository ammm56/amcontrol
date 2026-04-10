using AM.Model.Common;
using AM.Model.Interfaces.Runtime;

namespace AM.Model.Interfaces.Plc.Runtime
{
    /// <summary>
    /// PLC 扫描工作单元接口。
    /// 该接口在统一后台工作单元接口 <see cref="IRuntimeWorker"/> 的基础上，
    /// 补充 PLC 场景专有的“立即执行一次扫描”与“同步停止”语义。
    ///
    /// 设计说明：
    /// 1. PLC 扫描服务本质上属于后台运行工作单元，因此直接继承 <see cref="IRuntimeWorker"/>；
    /// 2. 统一复用后台任务宿主 <c>RuntimeTaskManager</c> 的注册、启动、停止管理能力；
    /// 3. 额外保留 <see cref="Stop"/> 与 <see cref="ScanOnce"/>，
    ///    便于上层在调试、诊断、手动控制场景中直接调用；
    /// 4. 具体实现应优先复用初始化阶段已准备好的 PLC 站点、点位与客户端对象，
    ///    不应在高速扫描循环中重复做不必要的配置查询、复杂映射与临时对象构建。
    /// </summary>
    public interface IPlcScanWorker : IRuntimeWorker
    {
        /// <summary>
        /// 是否正在运行。
        /// 该属性用于表达扫描循环当前是否处于活动状态。
        /// 一般情况下：
        /// - 已启动且循环未停止时返回 <c>true</c>；
        /// - 未启动、已停止或因异常退出后返回 <c>false</c>。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 启动 PLC 扫描。
        /// 调用后应创建或恢复后台扫描循环，并开始按既定周期执行 PLC 采样。
        /// 若当前已经处于运行状态，实现可返回告警结果而不是重复启动。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功表示扫描循环已成功启动；
        /// - 失败表示启动前校验、配置状态或后台任务创建出现异常。
        /// </returns>
        Result Start();

        /// <summary>
        /// 同步停止 PLC 扫描。
        /// 该方法主要服务于上层手动停止、调试与诊断场景。
        /// 对于统一后台任务管理器，仍通过 <see cref="IRuntimeWorker.StopAsync"/> 进行异步停止。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功表示扫描已停止或当前本就未运行；
        /// - 失败表示停止过程中发生异常。
        /// </returns>
        Result Stop();

        /// <summary>
        /// 执行单轮 PLC 扫描。
        /// 该方法不依赖常驻后台循环，可用于：
        /// - 调试阶段手动触发一次扫描；
        /// - 启动前验证 PLC 配置与客户端状态；
        /// - 单元测试或集成测试中的显式扫描调用。
        /// </summary>
        /// <returns>
        /// 返回统一结果对象：
        /// - 成功表示本轮扫描已完成；
        /// - 失败表示本轮扫描过程中存在连接异常、点位读取异常或运行态写入异常。
        /// </returns>
        Result ScanOnce();
    }
}