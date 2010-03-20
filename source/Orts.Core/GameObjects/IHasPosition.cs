using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Primitives;

namespace Orts.Core.GameObjects
{
    public interface IHasPosition
    {
        Vector2 Position { get; }
    }
}
