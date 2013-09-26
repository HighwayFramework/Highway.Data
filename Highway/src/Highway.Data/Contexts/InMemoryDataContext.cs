using System.Collections.Generic;
using System.Linq;

namespace Highway.Data.Contexts
{
    public class InMemoryDataContext : IDataContext
    {
        internal List<ObjectRepresentationBase> Data = new List<ObjectRepresentationBase>();

        public void Dispose() { }

        public IEventManager EventManager { get; set; }

        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return GetCollection<T>();
        }

        private IQueryable<T> GetCollection<T>() where T : class
        {
            return Data.Where(x => x.IsType<T>()).Cast<TypeObjectRepresentation<T>>().Select(x => x.Entity).AsQueryable();
        }

        public T Add<T>(T item) where T : class
        {
            TypeObjectRepresentation<T> typeObjectRepresentation = ObjectRepresentationBase.Create(item);
            Data.Add(typeObjectRepresentation);
            Data.AddRange(typeObjectRepresentation.AllRelated());
            return item;
        }
    
        public T Remove<T>(T item) where T : class
        {
            var representation = Data.Where(x => x.IsType<T>()).Cast<TypeObjectRepresentation<T>>().Where(x => x.Entity == item).ToList();
            foreach (var typeObjectRepresentation in representation)
            {
                Data.Remove(typeObjectRepresentation);
                if (typeObjectRepresentation.EntityRemove != null)
                {
                    typeObjectRepresentation.EntityRemove();
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