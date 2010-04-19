using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;
using Orts.Core.Messages;

namespace Orts.Core.Brain.Module
{
    public class WaypointModule :IBrainModule
    {
        private static Random rand = new Random();

        public IBrain Brain { get; private set; }
        public BaseMobileUnit Unit { get; private set; }
        public List<IMobileUnit> VisibleUnits { get; private set; }
        public List<UnitMoveRequest> MoveRequests { get; private set; }

        public WaypointModule(BaseMobileUnit unit)
        {
            Unit = unit;
            VisibleUnits = new List<IMobileUnit>();
            MoveRequests = new List<UnitMoveRequest>();
        }

        public void InitialiseBrain(IBrain brain)
        {
            Brain = brain;

            Brain.Bus.Filters.ObjectPositions.Where(p => !p.Unit.Equals(Unit)).Where(p => p.Position.Subtract(Unit.Position).Length < 30).Subscribe(p => VisibleUnits.Add(p.Unit));
            Brain.Bus.Filters.UserCommands.OfType<UnitMoveRequest>().Where(c => c.Unit == Unit).Subscribe(c => MoveRequests.Add(c));
        }


        public void Think(TickTime tickTime)
        {
            if (MoveRequests.Count > 0)
            {
                Unit.MovementController.AddWaypoints(MoveRequests.Select(r => r.Waypoint).ToList());
                MoveRequests.Clear();
            }
            else
            {
            }

            VisibleUnits.Clear();
        }
    }
}
