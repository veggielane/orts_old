using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Messages;

namespace Orts.Core
{
    public class GameObjectFactory : IHasMessageBus
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
            Bus.Filters.ObjectLifeTimeRequests.OfType<ObjectCreationRequest>().Subscribe(r => CreateGameObject(r));
            Bus.Filters.ObjectLifeTimeRequests.OfType<ObjectDestructionRequest>().Subscribe(r => DestroyGameObject(r));
        }

        public virtual void CreateGameObject(ObjectCreationRequest request)
        {
        }

        public void DestroyGameObject(ObjectDestructionRequest request)
        {
            if (request.GameObject != null)
            {
                GameObjects.Remove(request.GameObject);
                Bus.Add(new ObjectDestroyed(request.TimeSent, request.GameObject));
            }
        }
    }
}
