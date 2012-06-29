using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Data;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// The standard interface used to interact with an ORM specific implementation
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// This gives a mockable wrapper around the normal Set<typeparam name="T"></typeparam> method that allows for testablity
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>IQueryable<typeparam name="T"></typeparam></returns>
        IQueryable<T> AsQueryable<T>() where T : class;
        
        /// <summary>
        /// Adds the provided instance of <typeparam name="T"></typeparam> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being added</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to add</param>
        /// <returns>The <typeparam name="T"></typeparam> you added</returns>
        T Add<T>(T item) where T : class;

        /// <summary>
        /// Removes the provided instance of <typeparam name="T"></typeparam> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being removed</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to remove</param>
        /// <returns>The <typeparam name="T"></typeparam> you removed</returns>
        T Remove<T>(T item) where T : class;

        /// <summary>
        /// Updates the provided instance of <typeparam name="T"></typeparam> in the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being updated</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to update</param>
        /// <returns>The <typeparam name="T"></typeparam> you updated</returns>
        T Update<T>(T item) where T : class;

        /// <summary>
        /// Attaches the provided instance of <typeparam name="T"></typeparam> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to attach</param>
        /// <returns>The <typeparam name="T"></typeparam> you attached</returns>
        T Attach<T>(T item) where T : class;

        /// <summary>
        /// Detaches the provided instance of <typeparam name="T"></typeparam> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to detach</param>
        /// <returns>The <typeparam name="T"></typeparam> you detached</returns>
        T Detach<T>(T item) where T : class;

        /// <summary>
        /// Reloads the provided instance of <typeparam name="T"></typeparam> from the database
        /// </summary>
        /// <typeparam name="T">The Entity Type being reloaded</typeparam>
        /// <param name="item">The <typeparam name="T"></typeparam> you want to reload</param>
        /// <returns>The <typeparam name="T"></typeparam> you reloaded</returns>
        T Reload<T>(T item) where T : class;

        /// <summary>
        /// Reloads all tracked objects of the type <typeparam name="T"></typeparam>
        /// </summary>
        /// <typeparam name="T">The type of objects to reload</typeparam>
        void Reload<T>() where T : class;

        /// <summary>
        /// Commits all currently tracked entity changes
        /// </summary>
        /// <returns>the number of rows affected</returns>
        int Commit();

        /// <summary>
        /// Executes a SQL command and tries to map the returned datasets into an IEnumerable<typeparam name="T"></typeparam>
        /// The results should have the same column names as the Entity Type has properties
        /// </summary>
        /// <typeparam name="T">The Entity Type that the return should be mapped to</typeparam>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>An IEnumerable<typeparam name="T"></typeparam> from the query return</returns>
        IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams);

        /// <summary>
        /// Executes a SQL command and returns the standard int return from the query
        /// </summary>
        /// <param name="sql">The Sql Statement</param>
        /// <param name="dbParams">A List of Database Parameters for the Query</param>
        /// <returns>The rows affected</returns>
        int ExecuteSqlCommand(string sql, params DbParameter[] dbParams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dbParams"></param>
        /// <returns></returns>
        int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams);
        
        
        IEventManager EventManager { get; set; }
    }

}