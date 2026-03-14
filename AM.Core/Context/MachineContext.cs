using AM.Model.Interfaces.MotionCard;

namespace AM.Core.Context
{
    /// <summary>
    /// 设备上下文。
    /// 负责保存系统运行期间的全局设备对象引用。
    /// </summary>
    public sealed class MachineContext
    {
        /// <summary>
        /// 全局唯一实例。
        /// </summary>
        public static MachineContext Instance { get; } = new MachineContext();

        /// <summary>
        /// 私有构造，禁止外部实例化。
        /// </summary>
        private MachineContext()
        {
        }

        /// <summary>
        /// 全局唯一运动控制卡服务实例。
        /// </summary>
        public IMotionCardService MotionCard { get; set; }

        /// <summary>
        /// 预留 PLC 服务对象。
        /// </summary>
        // public IPLCService Plc { get; set; }

        /// <summary>
        /// 预留相机服务对象。
        /// </summary>
        // public ICameraService Camera { get; set; }
    }
}
