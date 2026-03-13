using AM.Model.MotionCard;
using AM.Model.Structs;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴维护接口（清状态/清零/硬件配置）
    /// </summary>
    public interface IMotionAxisMaintenance
    {
        short ClearStatus(short logicalAxis);
        short ClearAllAxisStatus();
        short SetZeroPos(short logicalAxis);
        short SetAllZeroPos();
        short ConfigAxisHardware(AxisConfig cfg);
    }
}