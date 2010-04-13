using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.GameObjects;
using Orts.Core.Messages;
using Orts.Core.Collections;

namespace Orts.Core
{
    public class GameObjectFactory : IHasMessageBus
    {
        public List<IGameObject> GameObjects { get; private set; }
        public MessageBus Bus { get; private set; }
        public ICollection<ObjectCreationRequest> CreationRequests { get; private set; }
        public ICollection<ObjectDestructionRequest> DestroyRequests { get; private set; }

        public GameObjectFactory(MessageBus bus)
        {
            GameObjects = new List<IGameObject>();
            Bus = bus;
            CreationRequests = new List<ObjectCreationRequest>();
            DestroyRequests = new List<ObjectDestructionRequest>();

            Initialise();
        }

        private void Initialise()
        {
            Bus.Filters.ObjectLifeTimeRequests.OfType<ObjectCreationRequest>().Subscribe(r => CreationRequests.Add(r));
            Bus.Filters.ObjectLifeTimeRequests.OfType<ObjectDestructionRequest>().Subscribe(r => DestroyRequests.Add(r));
        }

        public void ProcessRequests()
        {
            foreach (var request in CreationRequests)
            {
                CreateGameObject(request);
            }

            CreationRequests.Clear();

            foreach (var request in DestroyRequests)
            {
                DestroyGameObject(request);
            }

            DestroyRequests.Clear();
        }

        public virtual void CreateGameObject(ObjectCreationRequest request)
        {
        }

        public void DestroyGameObject(ObjectDestructionRequest request)
        {
            if (request.GameObject != null)
            {
                GameObjects.Remove(request.GameObject);
                Bus.Add(new ObjectDestroyed(request.GameObject));
            }
        }
    }
}
