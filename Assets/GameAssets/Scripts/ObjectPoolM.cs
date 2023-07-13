using System;
using System.Collections.Generic;

namespace Hypercasual
{
    public class ObjectPoolM<T> where T : class
    {
        private List<T> _objects;
        private Func<T> _create;
        private Action<T> _onRelease;
        private Action<T> _onGet;

        public ObjectPoolM(Func<T> create, Action<T> onRelease, Action<T> onGet)
        {
            _onGet = onGet;
            _onRelease = onRelease;
            _create = create;
            _objects = new List<T>();
        }

        public T Get(Func<T, bool> predicate)
        {
            T obj;
            if (_objects.Count == 0)
            {
                obj = _create.Invoke();
            }
            else
            {
                T searchFor = SearchFor(predicate);
                if (searchFor != null)
                {
                    obj = searchFor;
                    _objects.Remove(searchFor);
                }
                else
                {
                    obj = _create.Invoke();
                }
            }

            _onGet?.Invoke(obj);
            return obj;
        }

        private T SearchFor(Func<T, bool> predicate)
        {
            foreach (T obj in _objects)
            {
                if (predicate.Invoke(obj))
                    return obj;
            }

            return null;
        }

        public void Release(T item)
        {
            if (!_objects.Contains(item))
                _objects.Add(item);
            _onRelease?.Invoke(item);
        }
    }
}