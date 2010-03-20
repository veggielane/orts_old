using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Reactive;
using System.Threading;

namespace Orts.Core.Timing
{
    public class ObservableTimer : Observable<TickTime>
    {
        public Observable<TimerMessage> TimerMessages { get; private set; }
        public TimeSpan TickDelta { get; set; }
        public TimerState State { get; private set; }
        public TickTime LastTickTime { get; private set; }

        public ObservableTimer()
        {
            TickDelta = TimeSpan.FromMilliseconds(25);
            TimerMessages = new Observable<TimerMessage>();
            State = TimerState.Stopped;
            LastTickTime = new TickTime();
        }

        public virtual void Start()
        {
            if (State == TimerState.Running)
            {
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot start, Timer already running.", CurrentTickTime = LastTickTime });
                return;
            }

            if (State == TimerState.Stopping)
            {
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot start, Timer stopping.", CurrentTickTime = LastTickTime });
                return;
            }

            State = TimerState.Running;
            TimerMessages.OnNext(new TimerMessage() { Message = "Timer started.", CurrentTickTime = LastTickTime });

            while (State == TimerState.Running)
            {
                this.OnNext(LastTickTime);

                var currentElapsed = LastTickTime.CurrentElapsed();

                if (currentElapsed < TickDelta)
                {
                    Thread.Sleep(TickDelta - currentElapsed);
                }
                else
                    TimerMessages.OnNext(new TimerMessage() { Message = "Tick over ran by {0:0.000}ms.".fmt((currentElapsed - TickDelta).TotalMilliseconds), CurrentTickTime = LastTickTime });


                TimerMessages.OnNext(new TimerMessage() { Message = "Total tick time {0:0.000}ms.".fmt(LastTickTime.CurrentElapsed().TotalMilliseconds), CurrentTickTime = LastTickTime });

                LastTickTime.Update(TickDelta);
            }

            State = TimerState.Stopped;
            TimerMessages.OnNext(new TimerMessage() { Message = "Timer stopped.", CurrentTickTime = LastTickTime });
            
        }

        public virtual void Stop()
        {
            if (State == TimerState.Running)
            {
                State = TimerState.Stopping;
                TimerMessages.OnNext(new TimerMessage() { Message = "Timer stopping.", CurrentTickTime = LastTickTime });
            }
            else
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot stop, Timer already stopped.", CurrentTickTime = LastTickTime });

        }

        public override void Dispose()
        {
            TimerMessages.OnCompleted();
            
            base.Dispose();
        }

        public enum TimerState
        {
            Stopped = 0,
            Running,
            Stopping
        }
    }
}
