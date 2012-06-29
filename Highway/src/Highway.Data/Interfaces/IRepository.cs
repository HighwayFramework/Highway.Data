using System.Collections.Generic;

namespace Highway.Data.Interfaces
{
    /// <summary>
    /// The interface used to interact with the ORM Specific Implementations
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 
        /// </summary>
        IDataContext Context { get; }

        /// <summary>
        /// 
        /// </summary>
        IEventManager EventManager { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> Find<T>(IQuery<T> query) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(IScalarObject<T> query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        void Execute(ICommandObject command);
    }
}