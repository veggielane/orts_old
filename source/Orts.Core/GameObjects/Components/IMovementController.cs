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
        void SetWaypoints(List<Vector2> waypoints);
        void AddWaypoints(List<Vector2> waypoints);
        KinematicState Update(KinematicState state, TickTime tickTime);
    }
}
