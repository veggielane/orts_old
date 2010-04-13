using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;
using Orts.Core.Messages;
using Orts.Core.Collections;
using Orts.Core.Primitives;

namespace Orts.Core.Brain
{
    public class SimpleBrain :IBrain
    {
        private static Random rand = new Random();
        public MessageBus Bus { get; private set; }
        public IMapGO GameObject { get; private set; }

        public ICollection<IMapGO> VisibleObjects { get; private set; }
        public ICollection<UnitMoveRequest> MoveRequests { get; private set; }

        public Vector2 Waypoint { get; private set; }

        private bool holdWaypoint = false;

        public void Initialise(MessageBus bus, IMapGO gameObject)
        {
            Bus = bus;
            GameObject = gameObject;
            VisibleObjects = new List<IMapGO>();
            MoveRequests = new List<UnitMoveRequest>();

            Bus.Filters.ObjectPositions.Where(p => p.Object != GameObject).Where(p => p.Position.Subtract(GameObject.Position).Length < 30).Subscribe(p => VisibleObjects.Add(p.Object));
            Bus.Filters.UserCommands.OfType<UnitMoveRequest>().Where(c => c.Unit == GameObject).Subscribe(c => MoveRequests.Add(c));
        }


        public void Think(TickTime tickTime)
        {
            if (!holdWaypoint && VisibleObjects.Count > 0)
            {
                var min = VisibleObjects.Min(a => (a.Position.Subtract(GameObject.Position)).Length);

                var closestObjects = (from o in VisibleObjects
                                     where o.Position.Subtract(GameObject.Position).Length == min
                                     select o).ToList();

                var diff = closestObjects[0].Position.Subtract(GameObject.Position);

                if (diff.Length == 0)
                {
                    diff = new Vector2(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5);
                }

                Waypoint = GameObject.Position.Add(diff.Normal.Multiply(-10));
            }

            VisibleObjects.Clear();

            if (GameObject.Position == Waypoint)
            {
                Waypoint = null;
                holdWaypoint = false;
            }

            if (MoveRequests.Count > 0)
            {
                Waypoint = MoveRequests.Last().Waypoint;
                holdWaypoint = true;

                MoveRequests.Clear();
            }

            Bus.Add(new ObjectPosition(GameObject, GameObject.Position));
        }
    }
}
