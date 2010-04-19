using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;

namespace Orts.Core.Brain.Module
{
    public interface IBrainModule : IHasBrain
    {
        void Think(TickTime tickTime);
    }
}
