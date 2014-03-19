#region

using System;
using System.Linq;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     This pre-built query get a specific type by the Id provided
    /// </summary>
    /// <typeparam name="TId">The type of ID.</typeparam>
    /// <typeparam name="T">The type to get.</typeparam>
    public class GetById<TId, T> : Scalar<T> where T : class, IIdentifiable<TId> where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        public GetById(TId id)
        {
            ContextQuery = context => context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        T Id { get; set; }
    }
}