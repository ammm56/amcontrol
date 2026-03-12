using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Alarm
{
    public enum AlarmCode
    {
        None = 0,

        PLCDisconnect = 1001,

        AxisServoAlarm = 2001,

        AxisFollowingError = 2002,

        CameraTimeout = 3001,

        CameraGrabFailed = 3002,

        DatabaseError = 4001
    }
}
