using AM.Model.Entity.Motion;
using System.Collections.Generic;

namespace AM.Model.MotionCard
{
    /// <summary>
    /// 数据库层设备配置聚合数据。
    /// 仅用于仓储层与应用服务层之间传递，不参与运行时控制。
    /// </summary>
    public class MachineConfigData
    {
        public List<MotionCardEntity> MotionCards { get; set; } = new List<MotionCardEntity>();

        public List<MotionAxisEntity> MotionAxes { get; set; } = new List<MotionAxisEntity>();

        public List<MotionIoMapEntity> MotionIoMaps { get; set; } = new List<MotionIoMapEntity>();
    }
}
