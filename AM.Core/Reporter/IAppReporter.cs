using AM.Model.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Reporter
{
    public interface IAppReporter
    {
        void Info(string source, string message, string code = null, short? cardId = null);
        void Warn(string source, string message, string code = null, short? cardId = null);
        void Error(string source, string message, string code = null, short? cardId = null);
        void Error(string source, System.Exception ex, string message, string code = null, short? cardId = null);
        void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null);
    }
}
