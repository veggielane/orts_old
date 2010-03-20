using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Timing
{
    public class TickTime:IGameTime
    {
        private DateTime RealTime { get; set; }
        public TimeSpan RealTimeElapsed { get; private set; }
        public TimeSpan RealTimeDelta { get; private set; }
        public TimeSpan GameTimeElapsed { get; private set; }
        public TimeSpan GameTimeDelta { get; private set; }
        public long TickCount { get; private set; }

        public void Update(TimeSpan timeDelta)
        {
            GameTimeDelta = timeDelta;
            GameTimeElapsed += GameTimeDelta;

            var now = OrtsGlobals.Now();

            if (TickCount == 0)
                RealTime = now;

            RealTimeDelta = now - RealTime;
            RealTimeElapsed += RealTimeDelta;
            RealTime = now;

            TickCount++;
        }

        public TimeSpan CurrentElapsed()
        {
            if (TickCount == 0)
                return TimeSpan.Zero;
            else
                return OrtsGlobals.Now() - RealTime;
        }
    }
}
