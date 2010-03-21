using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;

namespace Orts.Core.Messages
{
    public class ObjectCreated:BaseMessage,IObjectLifetimeNotification
    {
        public IGameObject GameObject { get; private set; }

        public ObjectCreated(IGameTime timeSent, IGameObject gameObject)
        :base(timeSent)
        {
            GameObject = gameObject;
        }
    }
}
