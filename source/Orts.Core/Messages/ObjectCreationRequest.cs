using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.Messages
{
    public class ObjectCreationRequest : BaseMessage, IObjectLifetimeRequest
    {

        public Type ObjectType { get; private set; }

        public ObjectCreationRequest(IGameTime timeSent, Type objectType)
            : base(timeSent)
        {
            ObjectType = objectType;
        }
    }
}
