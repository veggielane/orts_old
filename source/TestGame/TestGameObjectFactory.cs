using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core;
using Orts.Core.Messages;
using Orts.Core.Primitives;
using Orts.Core.Brain;
using Orts.Core.Collections;

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
                var item = new TestTank(this.Bus, new SimpleBrain()) { Position = new Vector2(300, 300) };

                this.GameObjects.Add(item);

                Bus.Add(new ObjectCreated(item));
            }
            base.CreateGameObject(request);
        }

    }
}
