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
        public MessageBus Bus { get; private set; }
        public IMapGO GameObject { get; private set; }

        public ICollection<IMapGO> VisibleObjects { get; private set; }

        public Vector2 Waypoint { get; private set; }

        public void Initialise(MessageBus bus, IMapGO gameObject)
        {
            Bus = bus;
            GameObject = gameObject;
            VisibleObjects = new List<IMapGO>();

            Bus.Filters.ObjectPositions.Where(p => p.Object != GameObject).Where(p => p.Position.Subtract(GameObject.Position).Length < 100).Subscribe(p => VisibleObjects.Add(p.Object));
        }

        public void Think(TickTime tickTime)
        {
            if (VisibleObjects.Count > 0)
            {
                var min = VisibleObjects.Min(a => (a.Position.Subtract(GameObject.Position)).Length);

                var closestObject = (from o in VisibleObjects
                                     where o.Position.Subtract(GameObject.Position).Length == min
                                     select o).First();

                var diff = closestObject.Position.Subtract(GameObject.Position);

                if (diff.Length == 0)
                {
                    var rand = new Random();
                    diff = new Vector2(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5);
                }

                Waypoint = GameObject.Position.Add(diff.Normal.Multiply(-10));
            }

            VisibleObjects.Clear();

            if (GameObject.Position == Waypoint)
            {
                Waypoint = null;
            }

            Bus.Add(new ObjectPosition(GameObject, GameObject.Position));
        }
    }
}
