// <copyright file="GetById`2.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;
using System.Linq;

namespace Highway.Data
{
    /// <summary>
    ///     This pre-built query get a specific type by the Id provided.
    /// </summary>
    /// <typeparam name="TId">The type of the Id</typeparam>
    /// <typeparam name="T">The type to get</typeparam>
    public class GetById<TId, T> : Scalar<T>
        where T : class, IIdentifiable<TId>
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GetById{TId,T}" /> class.
        /// </summary>
        /// <param name="id">The Id of the <see cref="T" /> to return</param>
        public GetById(TId id)
        {
            ContextQuery = context => context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
