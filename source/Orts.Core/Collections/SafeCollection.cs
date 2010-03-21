using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core.Collections
{
    public class SafeCollection<T> : ICollection<T>
    {
        public List<T> InnerList { get; private set; }
        private object _listLock = new object();

        public SafeCollection()
        {
            InnerList = new List<T>();
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            lock (_listLock)
            {
                InnerList.Add(item);
            }
        }

        public void Clear()
        {
            lock (_listLock)
            {
                InnerList.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_listLock)
            {
                return InnerList.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_listLock)
            {
                InnerList.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_listLock)
                {
                    return InnerList.Count();
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (_listLock)
                {
                    return false;
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_listLock)
            {
                return InnerList.Remove(item);
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            lock (_listLock)
            {
                return InnerList.GetEnumerator();
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (_listLock)
            {
                return InnerList.GetEnumerator();
            }
        }

        #endregion
    }
}
