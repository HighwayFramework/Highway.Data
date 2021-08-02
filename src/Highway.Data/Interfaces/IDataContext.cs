// <copyright file="IDataContext.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;
using System.Linq;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    /// <summary>
    ///     The standard interface used to interact with an ORM specific implementation
    /// </summary>
    public interface IDataContext : IUnitOfWork, IDisposable
    {
        /// <summary>
        ///     The event fired just before the commit of the persistence
        /// </summary>
        event EventHandler<BeforeSave> BeforeSave;

        /// <summary>
        ///     The event fired just after the commit of the persistence
        /// </summary>
        event EventHandler<AfterSave> AfterSave;

        /// <summary>
        ///     This gives a mock-able wrapper around normal Set method that allows for testability
        /// </summary>
        /// <typeparam name="T">The Entity being queried</typeparam>
        /// <returns>
        ///     <see cref="IQueryable{T}" />
        /// </returns>
        IQueryable<T> AsQueryable<T>()
            where T : class;
    }
}
