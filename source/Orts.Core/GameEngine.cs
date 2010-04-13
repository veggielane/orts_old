using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using Orts.Core.GameObjects;
using System.Threading.Tasks;
using Orts.Core.Messages;
using Orts.Core.Players;

namespace Orts.Core
{
    public class GameEngine:IHasMessageBus, IDisposable
    {
        public ObservableTimer Timer { get; private set; }
        public MessageBus Bus { get; private set; }
        public GameObjectFactory ObjectFactory { get; private set; }
        public bool IsRunning { get; private set; }
        public TickTime CurrentTickTime { get; private set; }
        public List<PlayerController> Players { get; set; }

        public IEnumerable<IMapGO> MapItems()
        {
            return ObjectFactory.GameObjects.OfType<IMapGO>();
        }

        public GameEngine(ObservableTimer timer, MessageBus bus, GameObjectFactory objectFactory)
        {
            Timer = timer;
            Bus = bus;
            ObjectFactory = objectFactory;
            IsRunning = false;
            Players = new List<PlayerController>();
            Initialise();
        }

        protected virtual void Initialise()
        {
            Bus.Initialise(this);

            Timer.Subscribe(t => CurrentTickTime = t);
            Timer.Subscribe(t => this.Update(t));
            Timer.SubSample(5).Subscribe(t =>
                {
                    Bus.SendAll();
                    foreach (var item in ObjectFactory.GameObjects.OfType<IMapGO>())
                    {
                        item.Brain.Think(t);
                    }

                    ObjectFactory.ProcessRequests();
                });
        }

        public void Update(TickTime tickTime)
        {
            foreach (var item in ObjectFactory.GameObjects)
            {
                item.Update(tickTime);
            }
        }

        public void Start()
        {
            if (!IsRunning)
            {
                Bus.Add(new SystemMessage("Engine starting."));
                Timer.Start();
                IsRunning = true;
                Bus.Add(new SystemMessage("Engine started."));
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                Bus.Add(new SystemMessage("Engine stopping."));
                IsRunning = false;
                Timer.Stop();
                Bus.Add(new SystemMessage("Engine stopped."));
            }
        }

        public virtual void Dispose()
        {
            Stop();
        }
    }
}
