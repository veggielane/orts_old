using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Brain;

namespace Orts.Core.GameObjects
{
    public interface IHasBrain
    {
        IBrain Brain { get; }

        void InitialiseBrain(IBrain brain);
    }
}
