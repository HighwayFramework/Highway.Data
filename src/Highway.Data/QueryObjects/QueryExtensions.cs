// <copyright file="QueryExtensions.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Highway.Data
{
    /// <summary>
    ///     A collection of extension methods for extending reusable IQuery objects
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        ///     Skip the number of items specified
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> Skip<T>(this IQuery<T> extend, int count)
        {
            var generics = new[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Skip", generics, parameters);

            return extend;
        }

        /// <summary>
        ///     Skip the number of items specified
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="TSelection">The type to query.</typeparam>
        /// <typeparam name="TProjection">The type to return.</typeparam>
        /// <returns></returns>
        public static IQuery<TSelection, TProjection> Skip<TSelection, TProjection>(this IQuery<TSelection, TProjection> extend, int count)
        {
            var generics = new[] { typeof(TSelection) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Skip", generics, parameters);

            return extend;
        }

        /// <summary>
        ///     Takes the specified number of records
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuery<T> Take<T>(this IQuery<T> extend, int count)
        {
            var generics = new[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Take", generics, parameters);

            return extend;
        }

        /// <summary>
        ///     Takes the specified number of records
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQuery<TSelection, TProjection> Take<TSelection, TProjection>(this IQuery<TSelection, TProjection> extend, int count)
        {
            var generics = new[] { typeof(TSelection) };
            var parameters = new Expression[] { Expression.Constant(count) };
            ((IExtendableQuery)extend).AddMethodExpression("Take", generics, parameters);

            return extend;
        }

        public static IQuery<T> Where<T>(this IQuery<T> extend, Expression<Func<T, bool>> predicate)
        {
            var generics = new[] { typeof(T) };
            var parameters = new Expression[] { predicate };
            ((IExtendableQuery)extend).AddMethodExpression("Where", generics, parameters);

            return extend;
        }
    }
}
