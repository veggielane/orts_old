using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.MessageTypes
{
    public class ObjectCreationRequest : BaseMessage
    {

        public Type ObjectType { get; private set; }

        public ObjectCreationRequest(IGameTime timeSent, Type objectType)
            : base(timeSent)
        {
            ObjectType = objectType;
        }
    }
}
