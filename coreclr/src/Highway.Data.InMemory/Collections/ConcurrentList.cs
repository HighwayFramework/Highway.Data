using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Highway.Data.InMemory.Collections
{
    public class ConcurrentList<T> : IList<T>
    {
        private readonly List<T> _underlyingList = new List<T>();
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<T> _underlyingQueue;
        private bool _requiresSync;
        private bool _isDirty;

        public ConcurrentList()
        {
            _underlyingQueue = new ConcurrentQueue<T>();
        }

        public ConcurrentList(IEnumerable<T> items)
        {
            _underlyingQueue = new ConcurrentQueue<T>(items);
        }

        private void UpdateLists()
        {
            if (!_isDirty)
                return;
            lock (_syncRoot)
            {
                _requiresSync = true;
                T temp;
                while (_underlyingQueue.TryDequeue(out temp))
                    _underlyingList.Add(temp);
                _requiresSync = false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (_requiresSync)
                lock (_syncRoot)
                    _underlyingQueue.Enqueue(item);
            else
                _underlyingQueue.Enqueue(item);
            _isDirty = true;
        }

        public int Add(object value)
        {
            if (_requiresSync)
                lock (_syncRoot)
                    _underlyingQueue.Enqueue((T)value);
            else
                _underlyingQueue.Enqueue((T)value);
            _isDirty = true;
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.IndexOf((T)value);
            }
        }

        public bool Contains(object value)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.Contains((T)value);
            }
        }

        public int IndexOf(object value)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.IndexOf((T)value);
            }
        }

        public void Insert(int index, object value)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.Insert(index, (T)value);
            }
        }

        public void Remove(object value)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.Remove((T)value);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_syncRoot)
                {
                    UpdateLists();
                    return _underlyingList[index];
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    UpdateLists();
                    _underlyingList[index] = value;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.Remove(item);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.CopyTo((T[])array, index);
            }
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    UpdateLists();
                    return _underlyingList.Count;
                }
            }
        }

        public object SyncRoot
        {
            get { return _syncRoot; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public int IndexOf(T item)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                return _underlyingList.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                UpdateLists();
                _underlyingList.Insert(index, item);
            }
        }
    }
}
