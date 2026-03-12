using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Alarm
{
    public class AlarmInfo
    {
        public AlarmCode Code { get; }

        public AlarmLevel Level { get; }

        public string Message { get; }

        public DateTime Time { get; }

        public bool IsCleared { get; set; }

        public AlarmInfo(AlarmCode code,AlarmLevel level,string message)
        {
            Code = code;
            Level = level;
            Message = message;
            Time = DateTime.Now;
            IsCleared = false;
        }
    }
}
