using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Timing
{
    public interface IGameTime
    {
        TimeSpan GameTimeElapsed { get; }
        TimeSpan GameTimeDelta { get; }
        long TickCount { get; }
    }
}
