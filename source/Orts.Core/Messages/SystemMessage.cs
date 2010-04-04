using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.Messages
{
    public class SystemMessage : BaseMessage
    {
        public string Message { get; private set; }

        public SystemMessage(string message)
        {
            Message = message;
        }
    }
}
