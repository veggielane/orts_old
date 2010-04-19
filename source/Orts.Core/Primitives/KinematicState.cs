using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Primitives
{
    public class KinematicState
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public static KinematicState Zero
        {
            get
            {
                var state = new KinematicState();
                state.Position = Vector2.Zero;
                state.Velocity = Vector2.Zero;
                state.Acceleration = Vector2.Zero;

                return state;
            }
        }
    }
}
