using AM.Model.Common;

namespace AM.Model.Interfaces.Motion.Assembly
{
    /// <summary>
    /// 装配接线工作台调试服务接口。
    /// 负责单点 IO 调试。
    /// </summary>
    public interface IAssemblyWiringDebugService
    {
        /// <summary>
        /// 读取单个 DI 当前状态。
        /// </summary>
        /// <param name="logicalBit">逻辑位号。</param>
        Result<bool> ReadDi(short logicalBit);

        /// <summary>
        /// 设置单个 DO 输出状态。
        /// </summary>
        /// <param name="logicalBit">逻辑位号。</param>
        /// <param name="value">目标输出状态。</param>
        Result SetDo(short logicalBit, bool value);

        /// <summary>
        /// 对单个 DO 执行脉冲输出。
        /// </summary>
        /// <param name="logicalBit">逻辑位号。</param>
        /// <param name="pulseMs">脉冲宽度，单位 ms。</param>
        Result PulseDo(short logicalBit, int pulseMs);

    }
}
