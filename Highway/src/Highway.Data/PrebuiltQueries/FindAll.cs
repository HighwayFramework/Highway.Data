using System;
using Highway.Data;

namespace Highway.Data.PrebuiltQueries
{
    /// <summary>
    /// Finds all items of a certain type in the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindAll<T> : Query<T> where T : class
    {
        /// <summary>
        /// Constructs a findall
        /// </summary>
        public FindAll()
        {
            ContextQuery = context => context.AsQueryable<T>();
        }
    }
}