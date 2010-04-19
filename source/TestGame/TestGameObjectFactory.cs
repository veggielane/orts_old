using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core;
using Orts.Core.Messages;
using Orts.Core.Primitives;
using Orts.Core.Brain;
using Orts.Core.Collections;
using Orts.Core.GameObjects.Components;
using Orts.Core.Brain.Module;

namespace TestGame
{
    public class TestGameObjectFactory : GameObjectFactory
    {

        public TestGameObjectFactory(MessageBus bus)
            : base(bus)
        {
        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {

            if (request.ObjectType == typeof(TestTank))
            {
                var unit = new TestTank(new SimpleMovementController());

                var brain = new SimpleBrain(Bus, unit, new WaypointModule(unit));
                unit.InitialiseBrain(brain);
                
                this.GameObjects.Add(unit);

                Bus.Add(new ObjectCreated(unit));
            }
            base.CreateGameObject(request);
        }

    }
}
