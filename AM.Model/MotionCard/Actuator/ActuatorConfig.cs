using System.Collections.Generic;

namespace AM.Model.MotionCard.Actuator
{
    /// <summary>
    /// 执行器运行时配置聚合。
    /// 用于承载第三层对象配置的运行时数据。
    /// 当前已正式实现的类别：
    /// 1. 气缸
    /// 2. 真空
    /// 3. 灯塔/声光报警
    /// 4. 夹爪
    /// </summary>
    public class ActuatorConfig
    {
        /// <summary>
        /// 气缸对象集合。
        /// </summary>
        public List<CylinderConfig> Cylinders { get; set; } = new List<CylinderConfig>();

        /// <summary>
        /// 真空对象集合。
        /// </summary>
        public List<VacuumConfig> Vacuums { get; set; } = new List<VacuumConfig>();

        /// <summary>
        /// 灯塔/声光报警对象集合。
        /// </summary>
        public List<StackLightConfig> StackLights { get; set; } = new List<StackLightConfig>();

        /// <summary>
        /// 夹爪对象集合。
        /// </summary>
        public List<GripperConfig> Grippers { get; set; } = new List<GripperConfig>();
    }
}