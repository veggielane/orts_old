using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core;
using Orts.Core.Messages;
using Orts.Core.Primitives;

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
                var item = new TestTank(this.Bus) { Destination = new Vector2(200, 30) };

                this.GameObjects.Add(item);

                Bus.Add(new ObjectCreated(item));
            }
            base.CreateGameObject(request);
        }

    }
}
