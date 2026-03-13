using AM.Model.MotionCard;
using AM.Model.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 控制卡聚合接口（组合各职责子接口）
    /// </summary>
    public interface IMotionCardService :
        IMotionCardDiagnostics,
        IMotionCardConfiguration,
        IMotionCardConnection,
        IMotionAxisMaintenance,
        IMotionAxisControl,
        IMotionAxisMotion,
        IMotionDigitalIO,
        IMotionAxisParameter,
        IMotionAxisState
    {
    }
}
