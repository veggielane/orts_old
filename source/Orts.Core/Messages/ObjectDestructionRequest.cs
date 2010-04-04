using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using Orts.Core.GameObjects;

namespace Orts.Core.Messages
{

    public class ObjectDestructionRequest : BaseMessage, IObjectLifetimeRequest
    {
        public IGameObject GameObject { get; private set; }

        public ObjectDestructionRequest(IGameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
