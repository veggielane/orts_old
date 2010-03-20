using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.MessageTypes
{
    public class SystemMessage : BaseMessage
    {
        public string Message { get; private set; }

        public SystemMessage(IGameTime timeSent, string message)
            :base(timeSent)
        {
            Message = message;
        }
    }
}
