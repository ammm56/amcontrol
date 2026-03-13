using AM.Model.Common;
using System.Threading.Tasks;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴基础控制
    /// </summary>
    public interface IMotionAxisControl
    {
        Result Enable(short logicalAxis, bool onOff);
        Result Stop(short logicalAxis, bool isEmergency = false);
        Result StopAll(bool isEmergency = false);
        Task<Result> HomeAsync(short logicalAxis);
        Result Home(short logicalAxis);
    }
}