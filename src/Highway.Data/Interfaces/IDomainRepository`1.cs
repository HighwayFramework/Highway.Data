// <copyright file="IDomainRepository`1.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;

using Highway.Data.Interceptors.Events;

namespace Highway.Data
{
    public interface IDomainRepository<in T> : IRepository
        where T : class
    {
        event EventHandler<BeforeQuery> BeforeQuery;

        event EventHandler<BeforeScalar> BeforeScalar;

        event EventHandler<BeforeCommand> BeforeCommand;

        event EventHandler<AfterQuery> AfterQuery;

        event EventHandler<AfterScalar> AfterScalar;

        event EventHandler<AfterCommand> AfterCommand;

        /// <summary>
        ///     Gets the contract for a Domain Context
        /// </summary>
        IDomainContext<T> DomainContext { get; }
    }
}
