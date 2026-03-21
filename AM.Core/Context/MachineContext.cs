using AM.Model.Interfaces.MotionCard;
using AM.Model.MotionCard.Actuator;
using System.Collections.Generic;

namespace AM.Core.Context
{
    /// <summary>
    /// 设备上下文。
    /// 负责保存系统运行期间的全局设备对象引用。
    /// 该上下文中的对象均为运行时实例，是上层业务访问运动、IO 与第三层对象的统一入口。
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
        /// 按控制卡 CardId 获取具体控制卡服务实例。
        /// Key: CardId
        /// 用于初始化、连接、断开和卡级批量操作。
        /// </summary>
        public IDictionary<short, IMotionCardService> MotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑轴到所属控制卡服务的映射。
        /// Key: LogicalAxis
        /// 用于 MotionHub 按逻辑轴进行统一路由。
        /// 上层只关心逻辑轴，不需要关心轴位于哪张控制卡。
        /// </summary>
        public IDictionary<short, IMotionCardService> AxisMotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DI 位到所属控制卡服务的映射。
        /// Key: LogicalBit
        /// 用于 GetDI(logicalBit) 按逻辑输入点路由到目标控制卡。
        /// </summary>
        public IDictionary<short, IMotionCardService> DICards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DO 位到所属控制卡服务的映射。
        /// Key: LogicalBit
        /// 用于 SetDO(logicalBit, status) 按逻辑输出点路由到目标控制卡。
        /// </summary>
        public IDictionary<short, IMotionCardService> DOCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 全局统一运动控制入口。
        /// 上层应优先通过该入口访问运动控制，而不是直接持有某张控制卡实例。
        /// 该入口负责按轴、按 IO 进行统一路由。
        /// </summary>
        public IMotionCardService MotionHub { get; set; }

        /// <summary>
        /// 气缸对象运行时配置索引。
        /// Key: Name
        /// 第三层对象按名称索引，便于业务层直接按对象名调用。
        /// </summary>
        public IDictionary<string, CylinderConfig> Cylinders { get; } = new Dictionary<string, CylinderConfig>();

        /// <summary>
        /// 真空对象运行时配置索引。
        /// Key: Name
        /// 第三层对象按名称索引，便于业务层直接按对象名调用。
        /// </summary>
        public IDictionary<string, VacuumConfig> Vacuums { get; } = new Dictionary<string, VacuumConfig>();

        /// <summary>
        /// 预留：PLC 服务对象。
        /// </summary>
        // public IPLCService Plc { get; set; }

        /// <summary>
        /// 预留：相机服务对象。
        /// </summary>
        // public ICameraService Camera { get; set; }
    }
}
