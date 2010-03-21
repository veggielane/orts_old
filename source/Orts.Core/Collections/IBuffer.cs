using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Collections
{
    public interface IBuffer<T> :IEnumerable<T>
    {
        void Add(T item);
        int Count();
        void Clear();
    }
}
