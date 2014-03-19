#region

using System;
using System.Linq;

#endregion

namespace Highway.Data
{
    /// <summary>
    ///     This pre-built query get a specific type by the Id provided
    /// </summary>
    /// <typeparam name="TId">The type of the Id.</typeparam>
    /// <typeparam name="T">The type to get.</typeparam>
    public class GetById<TId, T> : Scalar<T> where T : class, IIdentifiable<TId> where TId : struct, IEquatable<TId>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetById{TId,T}"/> class.
        /// </summary>
        /// <param name="id">The Id of the <see cref="T"/> to return.</param>
        public GetById(TId id)
        {
            ContextQuery = context => context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }

    /// <summary>
    ///     Defines a generalized Id property for identifying an individual entity.
    /// </summary>
    /// <typeparam name="T">The type of the Id.</typeparam>
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        /// <summary>
        ///     Gets or sets a value identifying the individual entity.
        /// </summary>
        T Id { get; set; }
    }
}