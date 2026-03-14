using AM.Core.Messaging;
using AM.Model.Alarm;
using AM.Model.Common;
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
        void Info(string source, string message, int code, short? cardId = null);

        void Warn(string source, string message, string code = null, short? cardId = null);
        void Warn(string source, string message, int code, short? cardId = null);

        void Error(string source, string message, string code = null, short? cardId = null);
        void Error(string source, string message, int code, short? cardId = null);

        void Error(string source, Exception ex, string message, string code = null, short? cardId = null);
        void Error(string source, Exception ex, string message, int code, short? cardId = null);

        void Alarm(string source, AlarmCode code, AlarmLevel level, string message, short? cardId = null);

        void Report(string source, ErrorDescriptor error, SystemMessageType type, Exception ex = null, short? cardId = null);
    }
}
