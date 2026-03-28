using AM.Model.Runtime;

namespace AM.Core.Context
{
    /// <summary>
    /// 全局运行时状态上下文。
    /// 用于保存 Motion / PLC / Vision / Actuator 等实时状态缓存。
    /// </summary>
    public sealed class RuntimeContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static RuntimeContext Instance { get; } = new RuntimeContext();

        /// <summary>
        /// 私有构造，禁止外部实例化。
        /// </summary>
        private RuntimeContext()
        {
        }

        /// <summary>
        /// Motion IO 运行时状态。
        /// </summary>
        public MotionIoRuntimeState MotionIo { get; } = new MotionIoRuntimeState();

        /// <summary>
        /// Motion 轴运行时状态。
        /// 仅供 UI / 监视 / 诊断使用，不参与控制安全逻辑。
        /// 内部字典维护了多轴
        /// </summary>
        public MotionAxisRuntimeState MotionAxis { get; } = new MotionAxisRuntimeState();

        /// <summary>
        /// PLC 运行时状态。
        /// </summary>
        public PlcRuntimeState Plc { get; } = new PlcRuntimeState();

        /// <summary>
        /// 预留：PLC 运行时状态。
        /// </summary>
        // public PlcRuntimeState Plc { get; } = new PlcRuntimeState();

        /// <summary>
        /// 预留：视觉运行时状态。
        /// </summary>
        // public VisionRuntimeState Vision { get; } = new VisionRuntimeState();
    }
}