using AM.Model.Common;
using AM.Model.Structs;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴状态读取
    /// </summary>
    public interface IMotionAxisState
    {
        Result<AxisStatus> GetAxisStatus(short logicalAxis);

        // 脉冲位置
        Result<double> GetCommandPosition(short logicalAxis);
        Result<double> GetEncoderPosition(short logicalAxis);

        // 毫米位置（与脉冲方法一一对应）
        Result<double> GetCommandPositionMm(short logicalAxis);
        Result<double> GetEncoderPositionMm(short logicalAxis);

        Result<bool> IsMoving(short logicalAxis);
    }
}