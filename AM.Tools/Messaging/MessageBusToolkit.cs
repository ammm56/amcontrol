using AM.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Tools.Messaging
{
    //public class MessageBusToolkit : IMessageBus
    //{
    //    public void Publish(SystemMessage message)
    //    {
    //        WeakReferenceMessenger.Default.Send(message);
    //    }

    //    public void Subscribe(object recipient, Action<SystemMessage> handler)
    //    {
    //        WeakReferenceMessenger.Default.Register<SystemMessage>(recipient, (r, m) =>
    //        {
    //            handler(m);
    //        });
    //    }

    //    public void Unsubscribe(object recipient)
    //    {
    //        WeakReferenceMessenger.Default.UnregisterAll(recipient);
    //    }

    //}
}
