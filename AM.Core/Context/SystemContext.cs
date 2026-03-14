using AM.Core.Alarm;
using AM.Core.Logging;
using AM.Core.Messaging;
using AM.Core.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Context
{
    public sealed class SystemContext
    {
        public static SystemContext Instance { get; } = new SystemContext();

        private SystemContext() { }

        public IAMLogger Logger { get; private set; }

        public IMessageBus MessageBus { get; private set; }

        public AlarmManager AlarmManager { get; private set; }

        public IErrorCatalog ErrorCatalog { get; private set; }

        public IAppReporter Reporter { get; private set; }

        public void Initialize(IAMLogger logger,IMessageBus bus,IErrorCatalog errorCatalog,IAppReporter reporter)
        {
            Logger = logger;
            MessageBus = bus;
            ErrorCatalog = errorCatalog;
            AlarmManager = new AlarmManager(bus, logger);
            Reporter = reporter;
        }
    }
}
