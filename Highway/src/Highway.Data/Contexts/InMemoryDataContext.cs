using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Highway.Data.Contexts.TypeRepresentations;
using Highway.Data.Contexts.TypeUtilities;

namespace Highway.Data.Contexts
{
    public class InMemoryDataContext : IDataContext
    {
        internal ObjectRepresentationRepository repo = new ObjectRepresentationRepository();

        public void Dispose() { }

        public IEventManager EventManager { get; set; }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return repo.Data<T>();
        }
        
        public T Add<T>(T item) where T : class
        {
            repo.Add(item);
            return item;
        }

        public T Remove<T>(T item) where T : class
        {
            repo.Remove(item);
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
            repo.CleanGraph();
            repo.FindChanges();
            return 0;
        }
    }
}