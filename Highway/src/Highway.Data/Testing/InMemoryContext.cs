using System;
using System.Collections.Generic;
using System.Linq;
using Highway.Data.EventManagement;
using Highway.Data.Interceptors.Events;
using Highway.Data.Interfaces;

namespace Highway.Data.Testing
{
    public class InMemoryContext : IObservableDataContext
    {
        public InMemoryContext() : this(new EventManager())
        {
            
        }

        public InMemoryContext(IEventManager eventManager)
        {
            EventManager = eventManager;
        }

        private Dictionary<Type,IEnumerable<object>> _inMemoryStorage = new Dictionary<Type, IEnumerable<object>>();

        public void Dispose()
        {
            _inMemoryStorage = null;
        }

        public IEventManager EventManager { get; set; }

        public void Load<T>(IEnumerable<T> items) where T : class
        {
            var type = typeof(T);
            if (_inMemoryStorage.ContainsKey(type))
            {
                _inMemoryStorage[type] = items;
            }
            else
            {
                _inMemoryStorage.Add(type, items);
            }

        }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            IEnumerable<T> list = null;
            Type type = typeof (T);
            if (_inMemoryStorage.ContainsKey(type))
            {
                list = _inMemoryStorage[type].Cast<T>();
            }
            else
            {
                list = new List<T>();
                _inMemoryStorage.Add(type,list);
            }
            return list.AsQueryable();
        }

        public T Add<T>(T item) where T : class
        {
            IList<T> list = null;
            var type = typeof(T);
            if (_inMemoryStorage.ContainsKey(type))
            {
                list = _inMemoryStorage[type].Cast<T>().ToList();
                list.Add(item);
                _inMemoryStorage[type] = list;
            }
            else
            {
                list = new List<T>();
                list.Add(item);
                _inMemoryStorage.Add(type, list);
            }
            return item;
        }

        public T Remove<T>(T item) where T : class
        {
            IList<T> list = null;
            var type = typeof(T);
            if (_inMemoryStorage.ContainsKey(type))
            {
                list = _inMemoryStorage[type].Cast<T>().ToList();
                list.Remove(item);
                _inMemoryStorage[type] = list;
            }
            else
            {
                list = new List<T>();
                list.Remove(item);
                _inMemoryStorage.Add(type, list);
            }
            return item;
        }

        public T Update<T>(T item) where T : class
        {
            return item;
        }

        public T Reload<T>(T item) where T : class
        {
            return item;
        }

        public int Commit()
        {
            return 1;
        }

        public event EventHandler<PreSaveEventArgs> PreSave;
        public event EventHandler<PostSaveEventArgs> PostSave;
    }
}