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
        public Vector2 Destination { get; set; }
        public double MaxVelocity { get; set; }
        public bool Visible { get; private set; }

        public TestTank(MessageBus bus)
        {
            Bus = bus;
            Position = new Vector2();
            Velocity = new Vector2();
            MaxVelocity = 60;
            Visible = true;
        }


        public void Update(TickTime tickTime)
        {
            if (Destination == null)
            {
                Velocity = Vector2.Zero;
            }
            else
            {
                var path = Destination.Subtract(Position);

                if (path.Length >= MaxVelocity * tickTime.GameTimeDelta.TotalSeconds)
                {
                    Velocity = path.Normal.Multiply(MaxVelocity);
                }
                else
                {
                    Velocity = path.Normal.Multiply(path.Length / tickTime.GameTimeDelta.TotalSeconds);
                }
            }

            Position = Position.Add(Velocity.Multiply(tickTime.GameTimeDelta.TotalSeconds));

            if (Position == Destination)
            {
                Destination = null;
                //Visible = false;
                //Bus.Add(new ObjectDestructionRequest(this));
            }
        }

        public override string ToString()
        {
            return "TestTank - {{Pos:{0}}}".fmt(Position);
        }
    }
}
