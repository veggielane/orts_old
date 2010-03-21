using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Orts.Core.Collections;

namespace Orts.Core.Reactive
{

    public class Observable<T> : IObservable<T>, IDisposable
    {
        private ICollection<IObserver<T>> _subscribers = new List<IObserver<T>>();

        public void OnNext(T value)
        {
            foreach (var sub in _subscribers)
                sub.OnNext(value);
        }

        public void OnCompleted()
        {
            foreach (var sub in _subscribers)
                sub.OnCompleted();
        }

        public void OnError(Exception ex)
        {
            foreach (var sub in _subscribers)
                sub.OnError(ex);
        }

        #region IObservable<T> Members

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("BufferedObservable<T>");

            _subscribers.Add(observer);

            return new AnonymousDisposable(() =>
            {
                _subscribers.Remove(observer);
            });

        }

        #endregion


        #region IDisposable Members
        private bool _isDisposed = false;
        public virtual void Dispose()
        {
            _isDisposed = true;
            OnCompleted();
        }

        #endregion
    }
}
