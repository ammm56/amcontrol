using AM.Model.MotionCard;
using System.Collections.Generic;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 运行前配置加载
    /// </summary>
    public interface IMotionCardConfiguration
    {
        void LoadAxisConfig(List<AxisConfig> configs);
    }
}