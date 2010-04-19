﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Brain.Module;
using Orts.Core.Timing;
using Orts.Core.Messages;

namespace Orts.Core.Brain
{
    public interface IBrain : IHasMessageBus
    {
        void Think(TickTime tickTime);
    }
}
