using System.Linq;
using Highway.Data;
using System;

namespace Highway.Data.PrebuiltQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class GetById<TId,T> : Scalar<T> where T : class, IIdentifiable<TId> where TId : struct, IEquatable<TId>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public GetById(TId id)
        {
            ContextQuery = context => context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
       T Id { get; set; }
    }
}