﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Collections
{
    public interface IBag<T> :IEnumerable<T>
    {
        void Add(T item);
        int Count();
        void Clear();
        IEnumerable<T> GetEnumerableWithRemove();
    }
}
