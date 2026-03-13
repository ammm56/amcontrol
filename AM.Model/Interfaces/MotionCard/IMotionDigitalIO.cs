using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 数字IO操作
    /// </summary>
    public interface IMotionDigitalIO
    {
        Result SetDO(short bit, bool status);
        Result<bool> GetDI(short bit);
        Result<bool> GetDO(short bit);
    }
}