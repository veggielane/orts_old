using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Messages
{
    public class BusFilters
    {
        private MessageBus Bus { get; set; }

        public BusFilters()
        {
        }

        public void Initialise(MessageBus bus)
        {
            Bus = bus;
            SystemMessages = Bus.OfType<SystemMessage>().AsObservable();
            ObjectLifeTimeRequests = Bus.OfType<IObjectLifetimeRequest>().AsObservable();
            ObjectLifeTimeNotifications = Bus.OfType<IObjectLifetimeNotification>().AsObservable();
        }

        public IObservable<SystemMessage> SystemMessages { get; private set; }
        public IObservable<IObjectLifetimeRequest> ObjectLifeTimeRequests { get; private set; }
        public IObservable<IObjectLifetimeNotification> ObjectLifeTimeNotifications { get; private set; }
    }
}
