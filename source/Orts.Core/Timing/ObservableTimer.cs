using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Reactive;
using System.Threading;

namespace Orts.Core.Timing
{
    public class ObservableTimer:Observable<DateTime>
    {
        public Observable<TimerMessage> TimerMessages { get; private set; }
        public int TickMilliseconds { get; private set; }
        public TimerState State { get; private set; }
        public DateTime? LastTick { get; private set; }

        public ObservableTimer(int tickMilliseconds)
        {
            TickMilliseconds = tickMilliseconds;
            TimerMessages = new Observable<TimerMessage>();
            State = TimerState.Stopped;
        }

        public void Start()
        {
            if (State == TimerState.Running)
            {
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot start, Timer already running.", CurrentTime = LastTick });
                return;
            }

            if (State == TimerState.Stopping)
            {
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot start, Timer stopping.", CurrentTime = LastTick });
                return;
            }

            State = TimerState.Running;
            TimerMessages.OnNext(new TimerMessage() { Message = "Timer started.", CurrentTime = LastTick });

            while (State == TimerState.Running)
            {
                LastTick = OrtsGlobals.Now();

                this.OnNext(LastTick.Value);

                var currentTime = OrtsGlobals.Now();

                var tickTime = currentTime - LastTick.Value;

                if (tickTime.Milliseconds < TickMilliseconds)
                {
                    Thread.Sleep(TickMilliseconds - tickTime.Milliseconds);
                }
                else
                    TimerMessages.OnNext(new TimerMessage() { Message = "Tick over ran by {0}ms.".fmt(tickTime.Milliseconds - TickMilliseconds), CurrentTime = LastTick });

            }

            State = TimerState.Stopped;
            TimerMessages.OnNext(new TimerMessage() { Message = "Timer stopped.", CurrentTime = LastTick });
            
        }

        public void Stop()
        {
            if (State == TimerState.Running)
            {
                State = TimerState.Stopping;
                TimerMessages.OnNext(new TimerMessage() { Message = "Timer stopping.", CurrentTime = LastTick });
            }
            else
                TimerMessages.OnNext(new TimerMessage() { Message = "Cannot stop, Timer already stopped.", CurrentTime = LastTick });

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
