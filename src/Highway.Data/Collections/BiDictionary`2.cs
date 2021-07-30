using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Highway.Data.Collections
{
    [Serializable]
    public class BiDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>, IDictionary
    {
        private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();

        [NonSerialized]
        private readonly ReverseDictionary _reverseDictionary;

        [NonSerialized]
        private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();

        public BiDictionary()
        {
            _reverseDictionary = new ReverseDictionary(this);
        }

        public int Count => _firstToSecond.Count;

        public bool IsReadOnly => _firstToSecond.IsReadOnly || _secondToFirst.IsReadOnly;

        public ICollection<TFirst> Keys => _firstToSecond.Keys;

        public IDictionary<TSecond, TFirst> Reverse => _reverseDictionary;

        public ICollection<TSecond> Values => _firstToSecond.Values;

        bool IDictionary.IsFixedSize => ((IDictionary)_firstToSecond).IsFixedSize;

        bool ICollection.IsSynchronized => ((ICollection)_firstToSecond).IsSynchronized;

        ICollection IDictionary.Keys => ((IDictionary)_firstToSecond).Keys;

        object ICollection.SyncRoot => ((ICollection)_firstToSecond).SyncRoot;

        ICollection IDictionary.Values => ((IDictionary)_firstToSecond).Values;

        public TSecond this[TFirst key]
        {
            get => _firstToSecond[key];
            set
            {
                _firstToSecond[key] = value;
                _secondToFirst[value] = key;
            }
        }

        object IDictionary.this[object key]
        {
            get => ((IDictionary)_firstToSecond)[key];
            set
            {
                ((IDictionary)_firstToSecond)[key] = value;
                ((IDictionary)_secondToFirst)[value] = key;
            }
        }

        public void Add(TFirst key, TSecond value)
        {
            _firstToSecond.Add(key, value);
            _secondToFirst.Add(value, key);
        }

        public void Clear()
        {
            _firstToSecond.Clear();
            _secondToFirst.Clear();
        }

        public bool ContainsKey(TFirst key)
        {
            return _firstToSecond.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            return _firstToSecond.GetEnumerator();
        }

        public bool Remove(TFirst key)
        {
            if (_firstToSecond.TryGetValue(key, out var value))
            {
                _firstToSecond.Remove(key);
                _secondToFirst.Remove(value);

                return true;
            }

            return false;
        }

        public bool TryGetValue(TFirst key, out TSecond value)
        {
            return _firstToSecond.TryGetValue(key, out value);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            _secondToFirst.Clear();
            foreach (var item in _firstToSecond)
            {
                _secondToFirst.Add(item.Value, item.Key);
            }
        }

        void IDictionary.Add(object key, object value)
        {
            ((IDictionary)_firstToSecond).Add(key, value);
            ((IDictionary)_secondToFirst).Add(value, key);
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.Add(KeyValuePair<TFirst, TSecond> item)
        {
            _firstToSecond.Add(item);
            _secondToFirst.Add(item.Reverse());
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Contains(KeyValuePair<TFirst, TSecond> item)
        {
            return _firstToSecond.Contains(item);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)_firstToSecond).Contains(key);
        }

        void ICollection<KeyValuePair<TFirst, TSecond>>.CopyTo(KeyValuePair<TFirst, TSecond>[] array, int arrayIndex)
        {
            _firstToSecond.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary)_firstToSecond).CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_firstToSecond).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            var firstToSecond = (IDictionary)_firstToSecond;
            if (!firstToSecond.Contains(key))
            {
                return;
            }

            var value = firstToSecond[key];
            firstToSecond.Remove(key);
            ((IDictionary)_secondToFirst).Remove(value);
        }

        bool ICollection<KeyValuePair<TFirst, TSecond>>.Remove(KeyValuePair<TFirst, TSecond> item)
        {
            return _firstToSecond.Remove(item);
        }

        private class ReverseDictionary : IDictionary<TSecond, TFirst>, IDictionary
        {
            private readonly BiDictionary<TFirst, TSecond> _owner;

            public ReverseDictionary(BiDictionary<TFirst, TSecond> owner)
            {
                _owner = owner;
            }

            public int Count => _owner._secondToFirst.Count;

            public bool IsReadOnly => _owner._secondToFirst.IsReadOnly || _owner._firstToSecond.IsReadOnly;

            public ICollection<TSecond> Keys => _owner._secondToFirst.Keys;

            public ICollection<TFirst> Values => _owner._secondToFirst.Values;

            bool IDictionary.IsFixedSize => ((IDictionary)_owner._secondToFirst).IsFixedSize;

            bool ICollection.IsSynchronized => ((ICollection)_owner._secondToFirst).IsSynchronized;

            ICollection IDictionary.Keys => ((IDictionary)_owner._secondToFirst).Keys;

            object ICollection.SyncRoot => ((ICollection)_owner._secondToFirst).SyncRoot;

            ICollection IDictionary.Values => ((IDictionary)_owner._secondToFirst).Values;

            public TFirst this[TSecond key]
            {
                get => _owner._secondToFirst[key];
                set
                {
                    _owner._secondToFirst[key] = value;
                    _owner._firstToSecond[value] = key;
                }
            }

            object IDictionary.this[object key]
            {
                get => ((IDictionary)_owner._secondToFirst)[key];
                set
                {
                    ((IDictionary)_owner._secondToFirst)[key] = value;
                    ((IDictionary)_owner._firstToSecond)[value] = key;
                }
            }

            public void Add(TSecond key, TFirst value)
            {
                _owner._secondToFirst.Add(key, value);
                _owner._firstToSecond.Add(value, key);
            }

            public void Clear()
            {
                _owner._secondToFirst.Clear();
                _owner._firstToSecond.Clear();
            }

            public bool ContainsKey(TSecond key)
            {
                return _owner._secondToFirst.ContainsKey(key);
            }

            public IEnumerator<KeyValuePair<TSecond, TFirst>> GetEnumerator()
            {
                return _owner._secondToFirst.GetEnumerator();
            }

            public bool Remove(TSecond key)
            {
                if (_owner._secondToFirst.TryGetValue(key, out var value))
                {
                    _owner._secondToFirst.Remove(key);
                    _owner._firstToSecond.Remove(value);

                    return true;
                }

                return false;
            }

            public bool TryGetValue(TSecond key, out TFirst value)
            {
                return _owner._secondToFirst.TryGetValue(key, out value);
            }

            void IDictionary.Add(object key, object value)
            {
                ((IDictionary)_owner._secondToFirst).Add(key, value);
                ((IDictionary)_owner._firstToSecond).Add(value, key);
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.Add(KeyValuePair<TSecond, TFirst> item)
            {
                _owner._secondToFirst.Add(item);
                _owner._firstToSecond.Add(item.Reverse());
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Contains(KeyValuePair<TSecond, TFirst> item)
            {
                return _owner._secondToFirst.Contains(item);
            }

            bool IDictionary.Contains(object key)
            {
                return ((IDictionary)_owner._secondToFirst).Contains(key);
            }

            void ICollection<KeyValuePair<TSecond, TFirst>>.CopyTo(KeyValuePair<TSecond, TFirst>[] array, int arrayIndex)
            {
                _owner._secondToFirst.CopyTo(array, arrayIndex);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                ((IDictionary)_owner._secondToFirst).CopyTo(array, index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                return ((IDictionary)_owner._secondToFirst).GetEnumerator();
            }

            void IDictionary.Remove(object key)
            {
                var firstToSecond = (IDictionary)_owner._secondToFirst;
                if (!firstToSecond.Contains(key))
                {
                    return;
                }

                var value = firstToSecond[key];
                firstToSecond.Remove(key);
                ((IDictionary)_owner._firstToSecond).Remove(value);
            }

            bool ICollection<KeyValuePair<TSecond, TFirst>>.Remove(KeyValuePair<TSecond, TFirst> item)
            {
                return _owner._secondToFirst.Remove(item);
            }
        }
    }
}
