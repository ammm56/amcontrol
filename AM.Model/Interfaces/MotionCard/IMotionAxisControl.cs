using AM.Model.MotionCard;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴基础控制
    /// </summary>
    public interface IMotionAxisControl
    {
        MotionResult Enable(short logicalAxis, bool onOff);
        MotionResult Stop(short logicalAxis, bool isEmergency = false);
        MotionResult StopAll(bool isEmergency = false);
        Task<MotionResult> HomeAsync(short logicalAxis);
        MotionResult Home(short logicalAxis);
    }
}