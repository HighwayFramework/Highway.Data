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
            var representation = repo.Data<T>().Where(x => x.Entity == item).ToList();

            foreach (var typeObjectRepresentation in representation)
            {
                var success = repo.Remove(typeObjectRepresentation);
                if (!success) throw new InvalidDataException("Object was not removed");
                if (typeObjectRepresentation.EntityRemove != null)
                {
                    typeObjectRepresentation.EntityRemove();
                }
                foreach (var objectRepresentation in typeObjectRepresentation.AllRelated())
                {
                    var objRep = repo.Data.Where(x => x.Id == objectRepresentation.Id);

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