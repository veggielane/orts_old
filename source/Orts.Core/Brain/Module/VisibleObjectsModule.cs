using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;

namespace Orts.Core.Brain.Module
{
    public class VisibleObjectsModule : IBrainModule
    {
        public IBrain Brain {get; private set;}

        public List<IMobileUnit> VisibleUnits { get; private set; }

        public void Initialise(IBrain<T> brain, T unit)
        {
            Brain = brain;
            Unit = unit;

            Brain.Bus.Filters.ObjectPositions.Where(p => !p.Unit.Equals(Unit)).Where(p => p.Position.Subtract(Unit.Position).Length < 30).Subscribe(p => VisibleUnits.Add(p.Unit));
        }
    }
}
