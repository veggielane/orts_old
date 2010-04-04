using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Orts.Core;
using Orts.Core.Timing;
using Orts.Core.Messages;

namespace TestGame
{
    public class TestGameModule:NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<GameEngine>().ToSelf();
            Kernel.Bind<ObservableTimer>().To<AsyncObservableTimer>();
            Kernel.Bind<MessageBus>().ToSelf().InSingletonScope();
            Kernel.Bind<BusFilters>().ToSelf();
        }
    }
}
