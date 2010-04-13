using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Primitives;

namespace Orts.Core.Messages
{
    public class ObjectPosition : BaseMessage
    {
        public IMapGO Object { get; private set; }
        public Vector2 Position { get; private set; }

        public ObjectPosition(IMapGO mapGO, Vector2 position)
        {
            Object = mapGO;
            Position = position;
        }
    }
}
