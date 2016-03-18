using Highway.Data;
using System.Collections.Generic;
using System.Data.Common;

namespace Highway.Data.EntityFrameworkCore.MicrosoftSqlServer
{
    /// <summary>
    /// </summary>
    public interface IEntityDataContext : IDataContext
    {
        /// <summary>
        ///     Attaches the provided instance of <typeparamref name="T" /> to the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being attached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to attach</param>
        /// <returns>The <typeparamref name="T" /> you attached</returns>
        T Attach<T>(T item) where T : class;

        /// <summary>
        ///     Detaches the provided instance of <typeparamref name="T" /> from the data context
        /// </summary>
        /// <typeparam name="T">The Entity Type being detached</typeparam>
        /// <param name="item">The <typeparamref name="T" /> you want to detach</param>
        /// <returns>The <typeparamref name="T" /> you detached</returns>
        T Detach<T>(T item) where T : class;
    }
}
