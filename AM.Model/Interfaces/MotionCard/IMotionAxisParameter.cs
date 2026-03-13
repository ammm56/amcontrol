namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴参数设置
    /// </summary>
    public interface IMotionAxisParameter
    {
        short SetVel(short logicalAxis, double vel);
        short SetAcc(short logicalAxis, double acc);
        short SetDec(short logicalAxis, double dec);
    }
}