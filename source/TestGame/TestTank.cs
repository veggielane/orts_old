using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core;
using Orts.Core.GameObjects;
using Orts.Core.Messages;
using Orts.Core.Primitives;
using Orts.Core.Timing;

namespace TestGame
{
    public class TestTank : IMapGO
    {
        public MessageBus Bus { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public TestTank(MessageBus bus)
        {
            Bus = bus;
            Position = new Vector2();
            Velocity = new Vector2();
        }


        public void Update(TickTime tickTime)
        {
            Position = Position.Add(Velocity.Multiply(tickTime.GameTimeDelta.TotalSeconds));
        }

        public override string ToString()
        {
            return "TestTank - {{Pos:{0}}}".fmt(Position);
        }
    }
}
