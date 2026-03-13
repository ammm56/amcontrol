using AM.Model.Common;
using AM.Model.MotionCard;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 控制卡连接与生命周期
    /// </summary>
    public interface IMotionCardConnection
    {
        Result Initialize(string configPath);
        Result Connect();
        Result Disconnect();
    }
}