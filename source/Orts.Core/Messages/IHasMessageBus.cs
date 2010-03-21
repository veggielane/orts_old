using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Messages;

namespace Orts.Core
{
    public interface IHasMessageBus
    {
        MessageBus Bus { get; }
    }
}
