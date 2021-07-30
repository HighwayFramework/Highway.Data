using System;
using System.Linq;

namespace Highway.Data
{
    public class RemoveById<TId, T> : Command where T : class, IIdentifiable<TId> where TId : struct, IEquatable<TId>
    {
        /// <summary>
        ///     This pre-built command removes an object from the persistence store by the id provided
        /// </summary>
        /// <param name="id">id of the object to remove</param>
        public RemoveById(TId id)
        {
            ContextQuery = context =>
            {
                var item = context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
                if (item == null)
                {
                    return;
                }

                context.Remove(item);
                context.Commit();
            };
        }
    }
}