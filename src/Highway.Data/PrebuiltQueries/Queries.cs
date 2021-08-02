// <copyright file="Queries.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;

namespace Highway.Data
{
    /// <summary>
    ///     Base Library of queries already built
    /// </summary>
    public static class Queries
    {
        public static GetById<int, T> GetById<T>(int id)
            where T : class, IIdentifiable<int>
        {
            return new GetById<int, T>(id);
        }

        public static GetById<Guid, T> GetById<T>(Guid id)
            where T : class, IIdentifiable<Guid>
        {
            return new GetById<Guid, T>(id);
        }

        public static GetById<long, T> GetById<T>(long id)
            where T : class, IIdentifiable<long>
        {
            return new GetById<long, T>(id);
        }
    }
}
