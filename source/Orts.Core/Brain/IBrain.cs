using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using Orts.Core.GameObjects;
using Orts.Core.Messages;
using Orts.Core.Primitives;

namespace Orts.Core.Brain
{
    public interface IBrain :IHasMessageBus
    {
        IMapGO GameObject { get; }
        Vector2 Waypoint { get; }

        void Initialise(MessageBus bus, IMapGO gameObject);
        void Think(TickTime tickTime);
    }
}
