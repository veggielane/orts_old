using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.MessageTypes;

namespace Orts.Core
{
    public class GameObjectFactory :IHasMessageBus
    {
        public List<IGameObject> GameObjects { get; private set; }
        public MessageBus Bus { get; private set; }

        public GameObjectFactory(MessageBus bus)
        {
            GameObjects = new List<IGameObject>();
            Bus = bus;

            Initialise();
        }

        private void Initialise()
        {
            Bus.OfType<ObjectCreationRequest>().Subscribe(r => CreateGameObject(r));
        }

        public virtual void CreateGameObject(ObjectCreationRequest request)
        {
        }
    }
}
