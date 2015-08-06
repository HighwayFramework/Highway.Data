
using System;
using System.Linq;


namespace Highway.Data
{
    /// <summary>
    ///     This pre-built query get a specific type by the Id provided.
    /// </summary>
    /// <typeparam name="TId">The type of the Id</typeparam>
    /// <typeparam name="T">The type to get</typeparam>
    public class GetById<TId, T> : Scalar<T> where T : class, IIdentifiable<TId> where TId : struct, IEquatable<TId>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetById{TId,T}"/> class.
        /// </summary>
        /// <param name="id">The Id of the <see cref="T"/> to return</param>
        public GetById(TId id)
        {
            ContextQuery = context => context.AsQueryable<T>().SingleOrDefault(x => x.Id.Equals(id));
        }
    }
}