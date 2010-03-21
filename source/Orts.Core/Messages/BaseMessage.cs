using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.Messages
{
    public abstract class BaseMessage:IMessage
    {
        public IGameTime TimeSent { get; private set; }

        public BaseMessage(IGameTime timeSent)
        {
            TimeSent = timeSent;
        }
    }
}
