using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using Orts.Core.Brain;

namespace Orts.Core.GameObjects
{
    public interface IGameObject :IHasMessageBus
    {
        void Update(TickTime tickTime);
    }
}
