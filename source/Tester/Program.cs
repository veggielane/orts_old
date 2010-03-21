using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Orts.Core.GameObjects;
using Orts.Core.Primitives;
using Ninject;
using Ninject.Modules;
using System.Diagnostics;
using Orts.Core.Messages;

namespace Orts.Core
{
    class Program
    {
        static void Main(string[] args)
        {

            var kernal = new StandardKernel(new TestModule());

            var messages = new StringWriter();


            using (var engine = kernal.Get<GameEngine>())
            {
                engine.Timer.TimerMessages.Subscribe(m =>
                    {
                        Console.WriteLine("{0} - {1}".fmt(m.CurrentTickTime.GameTimeElapsed.ToString(), m.Message));
                        messages.WriteLine("{0} - {1}".fmt(m.CurrentTickTime.GameTimeElapsed.ToString(), m.Message));
                    });


                engine.Timer.Subscribe(t =>
                    {
                        Console.WriteLine("Doing stuff.");
                        messages.WriteLine("Doing stuff.");
                    });


                engine.Bus.OfType<SystemMessage>().Subscribe(m => Debug.WriteLine("{0} SYSTEM - {1}", m.TimeSent.ToString(),m.Message));

                Setup(engine);

                bool finish = false;
                while (!finish)
                {
                    engine.Start();

                    Console.ReadKey(true);

                    engine.Stop();

                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        finish = true;
                }

                Console.WriteLine("Timer should have stopped.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

            }
        }

        private static void Setup(GameEngine engine)
        {

            engine.Timer.Subscribe(t =>
                {
                    foreach (var item in engine.MapItems().OfType<TempItem>())
                    {
                        Console.WriteLine(item.ToString());
                    }
                });

            engine.ObjectFactory.GameObjects.Add(new TempItem(engine.Bus) { Velocity = new Vector2(0, 0) });
            engine.ObjectFactory.GameObjects.Add(new TempItem(engine.Bus) { Velocity = new Vector2(1, 1) });

        }
    }

    public class TestModule : NinjectModule
    {
        public override void  Load()
        {
            Kernel.Bind<GameEngine>().ToSelf();
            Kernel.Bind<ObservableTimer>().To<AsyncObservableTimer>();
            Kernel.Bind<MessageBus>().ToSelf().InSingletonScope();
            Kernel.Bind<BusFilters>().ToSelf();
        }
    }


    class TempItem : IMapGO
    {
        public MessageBus Bus { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public TempItem(MessageBus bus)
        {
            Bus = bus;
            Position = new Vector2();
            Velocity = new Vector2();
        }


        public void Update(TickTime tickTime)
        {
            Position = Position.Add(Velocity.Multiply(tickTime.GameTimeDelta.TotalSeconds));
        }



        public override string ToString()
        {
            return "TempItem - {{Pos:{0}}}".fmt(Position);
        }
    }
}
