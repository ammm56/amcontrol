using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴维护接口（清状态/清零/硬件配置）
    /// </summary>
    public interface IMotionAxisMaintenance
    {
        MotionResult ClearStatus(short logicalAxis);
        MotionResult ClearAllAxisStatus();
        MotionResult SetZeroPos(short logicalAxis);
        MotionResult SetAllZeroPos();
        MotionResult ConfigAxisHardware(AxisConfig cfg);
    }
}