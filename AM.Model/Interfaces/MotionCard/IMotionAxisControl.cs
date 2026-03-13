using System.Threading.Tasks;

namespace AM.Model.Interfaces.MotionCard
{
    /// <summary>
    /// 轴基础控制
    /// </summary>
    public interface IMotionAxisControl
    {
        short Enable(short logicalAxis, bool onORoff);
        short Stop(short logicalAxis, bool isEmergency = false);
        short StopAll(bool isEmergency = false);
        Task<short> HomeAsync(short logicalAxis);
        short Home(short logicalAxis);
    }
}