using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Primitives;
using Orts.Core.Timing;

namespace Orts.Core.GameObjects.Components
{
    public class SimpleMovementController :IMovementController
    {
        public List<Vector2> Waypoints { get; private set; }

        private double MaxVelocity { get; set; }

        public SimpleMovementController()
        {
            Waypoints = new List<Vector2>();
            MaxVelocity = 60;
        }

        public void SetWaypoints(List<Vector2> waypoints)
        {
            Waypoints = waypoints;
        }

        public void AddWaypoints(List<Vector2> waypoints)
        {
            Waypoints.AddRange(waypoints);
        }

        public KinematicState Update(KinematicState state, TickTime tickTime)
        {
            var nextWaypoint = Waypoints.FirstOrDefault();

            while (nextWaypoint != null && nextWaypoint == state.Position)
            {
                Waypoints.RemoveAt(0);

                nextWaypoint = Waypoints.FirstOrDefault();
            }

            if (nextWaypoint == null)
                return state;

            var newState = new KinematicState();

            var path = nextWaypoint - state.Position;

            if (path.Length <= MaxVelocity * tickTime.GameTimeDelta.TotalSeconds)
            {
                newState.Position = nextWaypoint;
                newState.Velocity = Vector2.Zero;
            }
            else
            {
                newState.Velocity = path.Normal * MaxVelocity;
                newState.Position = state.Position + newState.Velocity * tickTime.GameTimeDelta.TotalSeconds;
            }

            return newState;
        }

    }
}
