using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴参数设置
    /// </summary>
    public interface IMotionAxisParameter
    {
        MotionResult SetVel(short logicalAxis, double vel);
        MotionResult SetAcc(short logicalAxis, double acc);
        MotionResult SetDec(short logicalAxis, double dec);
    }
}