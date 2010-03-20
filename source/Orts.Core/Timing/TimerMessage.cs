using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Timing
{
    public class TimerMessage
    {
        public String Message { get; set; }
        public TickTime CurrentTickTime { get; set; }
    }
}
