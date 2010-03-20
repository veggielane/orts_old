using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.MessageTypes;
using Orts.Core.Reactive;

namespace Orts.Core
{
    public class MessageBus : Observable<IMessage>
    {

        public List<IMessage> PendingMessages { get; private set; }

        public MessageBus()
        {
            PendingMessages = new List<IMessage>();
        }

        public void Add(IMessage message)
        {
            PendingMessages.Add(message);
        }

        public void SendAll()
        {

            foreach (var message in PendingMessages)
            {
                OnNext(message);
            }

            PendingMessages.Clear();
        }

        public override void Dispose()
        {
            PendingMessages = null;
            base.Dispose();
        }
    }
}
