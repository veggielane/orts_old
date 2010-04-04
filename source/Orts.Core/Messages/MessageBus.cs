using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Reactive;
using Orts.Core.Collections;

namespace Orts.Core.Messages
{
    public class MessageBus : Observable<IMessage>
    {
        public GameEngine Engine { get; private set; }
        public IBuffer<IMessage> PendingMessages { get; private set; }
        public BusFilters Filters { get; private set; }

        public MessageBus(BusFilters busFilter)
        {
            PendingMessages = new Buffer<IMessage>();
            Filters = busFilter;
            Filters.Initialise(this);
        }

        public void Initialise(GameEngine engine)
        {
            Engine = engine;
        }

        public void Add(IMessage message)
        {
            message.TimeSent = Engine.CurrentTickTime;
            PendingMessages.Add(message);
        }

        public void SendAll()
        {
            foreach (var message in PendingMessages)
            {
                OnNext(message);
            }
        }

        public override void Dispose()
        {
            PendingMessages = null;
            base.Dispose();
        }
    }
}
