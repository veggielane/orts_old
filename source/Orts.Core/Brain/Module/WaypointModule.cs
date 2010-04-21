using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;
using Orts.Core.Messages;
using Orts.Core.Primitives;

namespace Orts.Core.Brain.Module
{
    public class WaypointModule :IBrainModule
    {
        private static Random rand = new Random();

        public IBrain Brain { get; private set; }
        public BaseMobileUnit Unit { get; private set; }
        public List<UnitMoveRequest> MoveRequests { get; private set; }

        public WaypointModule(BaseMobileUnit unit)
        {
            Unit = unit;
            MoveRequests = new List<UnitMoveRequest>();
        }

        public void InitialiseBrain(IBrain brain)
        {
            Brain = brain;

            Brain.Bus.Filters.UserCommands.OfType<UnitMoveRequest>().Where(c => c.Unit == Unit).Subscribe(c => MoveRequests.Add(c));
        }


        public void Think(TickTime tickTime)
        {
            if (MoveRequests.Count > 0)
            {
                Unit.MovementController.AddWaypoints(MoveRequests.Select(r => r.Waypoint).ToList());
                MoveRequests.Clear();
            }
            else if (Unit.MovementController.Waypoints.Count == 0 && Brain.VisibleUnits.Count > 0)
            {

                var closestUnitDistance = Brain.VisibleUnits.Min(u => (u.Position - Unit.Position).Length);
                
                if (closestUnitDistance == 0)
                {
                    Unit.MovementController.SetWaypoint(Unit.Position + (new Vector2(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5).Normal * 10));
                }
                else if(closestUnitDistance <= 100)
                {
                    var closestUnit = Brain.VisibleUnits.Where(u => (u.Position - Unit.Position).Length == closestUnitDistance).First();

                    if (closestUnit != null)
                    {
                        Unit.MovementController.SetWaypoint(Unit.Position + ((closestUnit.Position - Unit.Position).Normal * -10));
                    }
                }
            }

        }
    }
}
