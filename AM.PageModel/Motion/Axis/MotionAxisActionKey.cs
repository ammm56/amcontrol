using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.PageModel.Motion.Axis
{
    public enum MotionAxisActionKey
    {
        Enable = 10,
        Disable = 20,
        Home = 30,
        ClearStatus = 40,
        Stop = 50,
        EmergencyStop = 60,
        JogNegative = 70,
        JogStop = 80,
        JogPositive = 90,
        ApplyVelocity = 100,
        MoveAbsolute = 110,
        MoveRelative = 120
    }
}
