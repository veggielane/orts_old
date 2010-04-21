using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Timing;
using Orts.Core.Messages;
using Orts.Core.Collections;
using Orts.Core.Primitives;
using Orts.Core.Brain.Module;

namespace Orts.Core.Brain
{
    public class SimpleBrain :IBrain
    {
        public MessageBus Bus { get; private set; }
        public IMobileUnit Unit { get; private set; }
        public List<IMobileUnit> VisibleUnits { get; private set; }

        public List<IBrainModule> Modules { get; private set; }

        public SimpleBrain(MessageBus bus, IMobileUnit unit, params IBrainModule[] modules)
        {
            Bus = bus;
            Unit = unit;
            Modules = modules.ToList();

            Bus.Filters.ObjectPositions.Where(p => !p.Unit.Equals(Unit)).Where(p => p.Position.Subtract(Unit.Position).Length < 30).Subscribe(p => VisibleUnits.Add(p.Unit));

            VisibleUnits = new List<IMobileUnit>();

            foreach (var module in Modules)
            {
                module.InitialiseBrain(this);
            }
        }

        public void Think(TickTime tickTime)
        {
            foreach (var module in Modules)
            {
                module.Think(tickTime);
            }

            VisibleUnits.Clear();
            Bus.Add(new ObjectPosition(Unit, Unit.Position));
        }
    }
}
