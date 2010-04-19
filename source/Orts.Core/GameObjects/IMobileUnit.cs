using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects.Components;
using Orts.Core.Primitives;

namespace Orts.Core.GameObjects
{
    public interface IMobileUnit : IUnit
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; }
    }
}
