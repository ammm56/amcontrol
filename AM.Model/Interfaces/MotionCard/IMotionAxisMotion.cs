using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴运动控制
    /// </summary>
    public interface IMotionAxisMotion
    {
        MotionResult MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec);
        MotionResult MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        MotionResult JogMove(short logicalAxis, int direction, double velocity);

        MotionResult MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        MotionResult MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        MotionResult JogMoveMm(short logicalAxis, bool direction, double velMm);
        MotionResult JogStop(short logicalAxis);
    }
}