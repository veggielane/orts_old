using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Primitives;
using Orts.Core.GameObjects.Components;
using Orts.Core.Timing;
using Orts.Core.Brain;

namespace Orts.Core.GameObjects
{
    public abstract class BaseMobileUnit : IMobileUnit, IHasBrain
    {
        public Vector2 Position
        {
            get
            {
                return State.Position;
            }
            private set
            {
                State.Position = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return State.Velocity;
            }
            private set
            {
                State.Velocity = value;
            }
        }

        public IBrain Brain { get; private set; }
        public IMovementController MovementController { get; private set; }

        protected KinematicState State { get; set; }

        public BaseMobileUnit(IMovementController movementController)
        {
            MovementController = movementController;
            State = KinematicState.Zero;
        }

        public void InitialiseBrain(IBrain brain)
        {
            Brain = brain;
        }

        public void Update(TickTime tickTime)
        {
            State = MovementController.Update(State, tickTime);
        }
    }
}
