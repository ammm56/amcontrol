using AM.Model.Common;
using AM.Model.MotionCard;
using System.Collections.Generic;

namespace AM.Model.Interfaces.DB
{
    /// <summary>
    /// 轴配置覆盖服务接口。
    /// 负责将数据库中的轴参数覆盖到配置文件中的 AxisConfig 集合。
    /// </summary>
    public interface IAxisConfigOverlayService
    {
        /// <summary>
        /// 将数据库轴参数覆盖到多张控制卡配置。
        /// </summary>
        Result ApplyToMotionCards(IList<MotionCardConfig> motionCards);

        /// <summary>
        /// 将数据库轴参数覆盖到单张控制卡的轴配置集合。
        /// </summary>
        Result ApplyToAxisConfigs(IList<AxisConfig> axisConfigs);
    }
}