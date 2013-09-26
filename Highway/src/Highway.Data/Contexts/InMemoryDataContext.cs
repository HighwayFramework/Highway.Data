using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Highway.Data.Contexts.TypeUtilities;

namespace Highway.Data.Contexts
{
    public class InMemoryDataContext : IDataContext
    {
        internal List<ObjectRepresentation> Data = new List<ObjectRepresentation>();

        public void Dispose() { }

        public IEventManager EventManager { get; set; }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return GetCollection<T>();
        }

        private IQueryable<T> GetCollection<T>() where T : class
        {
            return Data.Where(x => x.IsType<T>()).Select(x => x.Entity).Cast<T>().AsQueryable();
        }

        public T Add<T>(T item) where T : class
        {
            var typeObjectRepresentation = ObjectRepresentation.Create(item);
            Data.Add(typeObjectRepresentation);
            Data.AddRange(typeObjectRepresentation.AllRelated());
            return item;
        }
    
        public T Remove<T>(T item) where T : class
        {
            var representation = Data.Where(x => x.IsType<T>()).Where(x => x.Entity == item).ToList();

            foreach (var typeObjectRepresentation in representation)
            {
                var success = Data.Remove(typeObjectRepresentation);
                if (!success) throw new InvalidDataException("Object was not removed");
                if (typeObjectRepresentation.EntityRemove != null)
                {
                    typeObjectRepresentation.EntityRemove();
                }
                foreach (var objectRepresentation in typeObjectRepresentation.AllRelated())
                {
                    var objRep = Data.Where(x => x.Id == objectRepresentation.Id);

                    success = Data.Remove(objectRepresentation);
                    if(!success) throw new InvalidDataException("Object was not removed");
                }
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
            return 0;
        }
    }
}