using System;
using Highway.Data;

namespace Highway.Data
{
    /// <summary>
    /// Finds all items of a certain type in the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindAll<T> : Query<T> where T : class
    {
        /// <summary>
        /// Constructs a find all query for the specified type
        /// </summary>
        public FindAll()
        {
            ContextQuery = context => context.AsQueryable<T>();
        }
    }
}