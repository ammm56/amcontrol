using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Core.Logging;

namespace AM.Tools.Logging
{
    public static class LoggerFactory
    {
        public static IAMLogger GetLogger<T>()
        {
            return new NLogLogger(typeof(T).FullName);
        }
    }
}
