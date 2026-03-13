namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴运动控制
    /// </summary>
    public interface IMotionAxisMotion
    {
        // 脉冲级运动
        short MoveRelative(short logicalAxis, double distance, double velocity, double acc, double dec);
        short MoveAbsolute(short logicalAxis, double position, double velocity, double acc, double dec);
        short JogMove(short logicalAxis, int direction, double velocity);

        // 业务层工程单位（mm）
        short MoveRelativeMm(short logicalAxis, double distanceMm, double velMm);
        short MoveAbsoluteMm(short logicalAxis, double positionMm, double velMm);
        short JogMoveMm(short logicalAxis, bool direction, double velMm);
        short JogStop(short logicalAxis);
    }
}