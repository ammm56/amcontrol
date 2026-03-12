using AM.Core.Messaging;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace AMControlWPF.MessageBus
{
    internal class MessageBusWPF : IMessageBus
    {
        public void Publish(SystemMessage message)
        {
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}
