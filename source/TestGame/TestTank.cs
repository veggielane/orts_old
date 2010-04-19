using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core;
using Orts.Core.GameObjects;
using Orts.Core.Messages;
using Orts.Core.Primitives;
using Orts.Core.Timing;
using Orts.Core.Brain;
using Orts.Core.GameObjects.Components;

namespace TestGame
{
    public class TestTank : BaseMobileUnit
    {
        public bool Visible { get; private set; }

        public TestTank(IMovementController movementController)
            :base(movementController)
        {
            Visible = true;
        }

        public override string ToString()
        {
            return "TestTank - {{Pos:{0}}}".fmt(Position);
        }
    }
}
