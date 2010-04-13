using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Primitives;
using Orts.Core.GameObjects;

namespace Orts.Core.Messages
{
    public class UnitMoveRequest : BaseMessage, IUserCommand
    {
        public IMapGO Unit { get; private set; }
        public Vector2 Waypoint { get; private set; }

        public UnitMoveRequest(IMapGO unit, Vector2 waypoint)
        {
            Unit = unit;
            Waypoint = waypoint;
        }
    }
}
