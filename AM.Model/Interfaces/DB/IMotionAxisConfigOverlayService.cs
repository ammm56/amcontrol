using AM.Model.Common;
using AM.Model.MotionCard;
using System.Collections.Generic;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 运动轴参数覆盖服务接口。
    /// </summary>
    public interface IMotionAxisConfigOverlayService
    {
        Result ApplyToMotionCards(IList<MotionCardConfig> motionCards);
        Result ApplyToAxisConfigs(IList<AxisConfig> axisConfigs);
    }
}