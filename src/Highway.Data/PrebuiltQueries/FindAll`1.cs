// <copyright file="FindAll`1.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

namespace Highway.Data
{
    /// <summary>
    ///     Finds all items of a certain type in the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindAll<T> : Query<T>
        where T : class
    {
        /// <summary>
        ///     Constructs a find all query for the specified type
        /// </summary>
        public FindAll()
        {
            ContextQuery = context => context.AsQueryable<T>();
        }
    }
}
