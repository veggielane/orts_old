using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using Orts.Core.Primitives;

namespace Orts.Core.GameObjects.Components
{
    public interface IMovementController
    {
        List<Vector2> Waypoints { get; }
        void SetWaypoint(Vector2 waypoint);
        void SetWaypoints(List<Vector2> waypoints);
        void AddWaypoint(Vector2 waypoint);
        void AddWaypoints(List<Vector2> waypoints);
        KinematicState Update(KinematicState state, TickTime tickTime);
    }
}
