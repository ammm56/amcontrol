using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴运动控制
    /// </summary>
    public interface IMotionAxisMotion
    {
        Result MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec);
        Result MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        Result JogMove(short logicalAxis, int direction, double velocity);

        Result MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        Result MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        Result JogMoveMm(short logicalAxis, bool direction, double velMm);
        Result JogStop(short logicalAxis);
    }
}