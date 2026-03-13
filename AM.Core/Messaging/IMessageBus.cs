using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Core.Messaging
{
    public interface IMessageBus
    {
        void Publish(SystemMessage message);

        void Subscribe(object recipient, Action<SystemMessage> handler);
        void Unsubscribe(object recipient);
    }
}
