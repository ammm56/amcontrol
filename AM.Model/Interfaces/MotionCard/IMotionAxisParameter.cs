using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴参数设置
    /// </summary>
    public interface IMotionAxisParameter
    {
        Result SetVel(short logicalAxis, double vel);
        Result SetAcc(short logicalAxis, double acc);
        Result SetDec(short logicalAxis, double dec);
    }
}