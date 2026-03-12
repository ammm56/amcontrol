using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    public class SystemMessage
    {
        public SystemMessageType Type { get; }

        public string Message { get; }

        public DateTime Time { get; }

        public SystemMessage(string msg, SystemMessageType type)
        {
            Message = msg;
            Type = type;
            Time = DateTime.Now;
        }
    }
}
