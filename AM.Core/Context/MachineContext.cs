using AM.Model.Interfaces.MotionCard;
using System.Collections.Generic;

namespace AM.Core.Context
{
    /// <summary>
    /// 设备上下文。
    /// 负责保存系统运行期间的全局设备对象引用，是对象实例。
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
        /// 按卡号拿到具体控制卡服务实例
        /// CardId -> IMotionCardService
        /// 初始化、连接、断开、批量操作时使用
        /// Key: CardId
        /// </summary>
        public IDictionary<short, IMotionCardService> MotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑轴到所属控制卡服务的映射。
        /// LogicalAxis -> IMotionCardService
        /// 给 MotionHub 做逻辑轴路由，上层只知道逻辑轴，不需要知道轴在哪张卡上
        /// MotionHub 先查 AxisMotionCards，再转发到目标卡服务。
        /// Key: LogicalAxis
        /// </summary>
        public IDictionary<short, IMotionCardService> AxisMotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DI 位到所属控制卡服务的映射。
        /// LogicalBit -> IMotionCardService
        /// 给 GetDI(logicalBit) 做路由
        /// 上层不需要关心这个 DI 在哪张卡上
        /// Key: LogicalBit
        /// </summary>
        public IDictionary<short, IMotionCardService> DICards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DO 位到所属控制卡服务的映射。
        /// LogicalBit -> IMotionCardService
        /// 给 SetDO(logicalBit, status) 做路由
        /// 上层只用逻辑位，不需要知道卡号/核号/硬件位
        /// Key: LogicalBit
        /// </summary>
        public IDictionary<short, IMotionCardService> DOCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 全局统一运动控制入口。
        /// 上层应优先通过该入口访问运动控制，而不是直接持有某张卡实例。
        /// 不直接依赖某张卡实例，保证多卡情况下调用方式一致
        /// </summary>
        public IMotionCardService MotionHub { get; set; }

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
