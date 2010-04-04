using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;

namespace Orts.Core.Messages
{
    public interface IMessage
    {
        IGameTime TimeSent { get; set; }
    }
}
