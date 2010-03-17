using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Collections
{
    public class BasicBag<T> :IBag<T>
    {
        private List<T> Items { get; set; }

        public BasicBag()
        {
            Items = new List<T>();
        }

        #region IBag<T> Members

        public void Add(T item)
        {
            Items.Add(item);
        }
        
        public int Count()
        {
            return Items.Count;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public IEnumerable<T> GetEnumerableWithRemove()
        {
            foreach (var item in this)
            {
                yield return item;
            }

            Items.Clear();
        }
        #endregion
        
        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
