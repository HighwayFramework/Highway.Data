// <copyright file="IExtendableQuery.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
    ///     This interface is used to allow for extension of prebuilt queries
    /// </summary>
    public interface IExtendableQuery
    {
        /// <summary>
        ///     Adds a method to the expression in the query object
        /// </summary>
        /// <param name="methodName">The name of the method to be added i.e. GroupBy</param>
        /// <param name="generics">Any type parameters needed by the method to be added</param>
        /// <param name="parameters">Any object parameters needed by the method to be added</param>
        void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters);
    }
}
