// <copyright file="RemoveById`2.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
    public class RemoveById<TId, T> : Command
        where T : class, IIdentifiable<TId>
        where TId : struct, IEquatable<TId>
    {
        /// <summary>
        ///     This pre-built command removes an object from the persistence store by the id provided
        /// </summary>
        /// <param name="id">id of the object to remove</param>
        public RemoveById(TId id)
        {
            ContextQuery = context =>
            {
                var item = context.AsQueryable<T>().FirstOrDefault(x => x.Id.Equals(id));
                if (item == null)
                {
                    return;
                }

                context.Remove(item);
                context.Commit();
            };
        }
    }
}
