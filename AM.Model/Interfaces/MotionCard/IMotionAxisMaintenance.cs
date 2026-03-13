using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴维护接口（清状态/清零/硬件配置）
    /// </summary>
    public interface IMotionAxisMaintenance
    {
        Result ClearStatus(short logicalAxis);
        Result ClearAllAxisStatus();
        Result SetZeroPos(short logicalAxis);
        Result SetAllZeroPos();
        Result ConfigAxisHardware(AxisConfig cfg);
    }
}