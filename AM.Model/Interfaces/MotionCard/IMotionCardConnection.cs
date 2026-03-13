namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 控制卡连接与生命周期
    /// </summary>
    public interface IMotionCardConnection
    {
        bool Initialize(string configPath);
        short Connect();
        short Disconnect();
    }
}