using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Brain.Module;
using Orts.Core.Timing;
using Orts.Core.Messages;
using Orts.Core.GameObjects;

namespace Orts.Core.Brain
{
    public interface IBrain : IHasMessageBus
    {
        List<IMobileUnit> VisibleUnits { get; }
        void Think(TickTime tickTime);
    }
}
