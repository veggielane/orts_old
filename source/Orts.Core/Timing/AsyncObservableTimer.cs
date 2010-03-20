using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Orts.Core.Timing
{
    public class AsyncObservableTimer:ObservableTimer
    {
        public Task RunningTask { get; private set; }

        public override void Start()
        {
            RunningTask = new Task(() => base.Start());

            RunningTask.Start();
        }

        public override void Stop()
        {
            base.Stop();

            if(RunningTask != null && RunningTask.IsCompleted)
                RunningTask.Wait();
        }
    }
}
