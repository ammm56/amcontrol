using AM.Model.Structs;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴状态读取
    /// </summary>
    public interface IMotionAxisState
    {
        AxisStatus GetAxisStatus(short logicalAxis);

        // 脉冲位置
        double GetCommandPosition(short logicalAxis);
        double GetEncoderPosition(short logicalAxis);

        // 毫米位置（与脉冲方法一一对应）
        double GetCommandPositionMm(short logicalAxis);
        double GetEncoderPositionMm(short logicalAxis);

        bool IsMoving(short logicalAxis);
    }
}