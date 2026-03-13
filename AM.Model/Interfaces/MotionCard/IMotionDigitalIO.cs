namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 数字IO操作
    /// </summary>
    public interface IMotionDigitalIO
    {
        short SetDO(short bit, bool status);
        bool GetDI(short bit);
        bool GetDO(short bit);
    }
}