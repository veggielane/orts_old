using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Reactive
{
    public class AnonymousDisposable : IDisposable
    {
        Action dispose;
        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose();
        }
    }
}
