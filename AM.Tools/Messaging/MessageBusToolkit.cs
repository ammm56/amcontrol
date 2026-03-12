using AM.Core.Messaging;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Tools.Messaging
{
    public class MessageBusToolkit : IMessageBus
    {
        public void Publish(SystemMessage message)
        {
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}
