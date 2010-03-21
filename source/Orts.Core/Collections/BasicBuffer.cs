using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Collections
{
    public class Buffer<T> : IBuffer<T>
    {
        private List<T> Items { get; set; }

        public Buffer()
        {
            Items = new List<T>();
        }

        #region BasIBuffericBuffer<T> Members

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
        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            var tempItems = Items.ToList();
            Items.Clear();
            return tempItems.GetEnumerator();
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
