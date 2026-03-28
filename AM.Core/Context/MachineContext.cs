using AM.Model.Interfaces.MotionCard;
using AM.Model.Interfaces.Plc;
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
        /// </summary>
        public IDictionary<short, IMotionCardService> MotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑轴到所属控制卡服务的映射。
        /// </summary>
        public IDictionary<short, IMotionCardService> AxisMotionCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DI 位到所属控制卡服务的映射。
        /// </summary>
        public IDictionary<short, IMotionCardService> DICards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 逻辑 DO 位到所属控制卡服务的映射。
        /// </summary>
        public IDictionary<short, IMotionCardService> DOCards { get; } = new Dictionary<short, IMotionCardService>();

        /// <summary>
        /// 全局统一运动控制入口。
        /// </summary>
        public IMotionCardService MotionHub { get; set; }

        /// <summary>
        /// 气缸对象运行时配置索引。
        /// Key: Name
        /// </summary>
        public IDictionary<string, CylinderConfig> Cylinders { get; } = new Dictionary<string, CylinderConfig>();

        /// <summary>
        /// 真空对象运行时配置索引。
        /// Key: Name
        /// </summary>
        public IDictionary<string, VacuumConfig> Vacuums { get; } = new Dictionary<string, VacuumConfig>();

        /// <summary>
        /// 灯塔/声光报警对象运行时配置索引。
        /// Key: Name
        /// </summary>
        public IDictionary<string, StackLightConfig> StackLights { get; } = new Dictionary<string, StackLightConfig>();

        /// <summary>
        /// 夹爪对象运行时配置索引。
        /// Key: Name
        /// </summary>
        public IDictionary<string, GripperConfig> Grippers { get; } = new Dictionary<string, GripperConfig>();

        /// <summary>
        /// PLC 客户端运行时入口。
        /// Key: PlcStationConfig.Name
        /// </summary>
        public IDictionary<string, IPlcClient> Plcs { get; } = new Dictionary<string, IPlcClient>();


        /// <summary>
        /// 预留：相机服务对象。
        /// </summary>
        // public ICameraService Camera { get; set; }
    }
}