// <copyright file="ShortIdentityStrategy`1.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Highway.Data.Contexts
{
    /// <summary>
    ///     An implementation of <see cref="IdentityStrategy{TType,TIdentity}" /> for entities where the identity property has
    ///     type short.
    /// </summary>
    /// <typeparam name="T">The type of the entities that will have identity values assigned.</typeparam>
    public class ShortIdentityStrategy<T> : IdentityStrategy<T, short>
        where T : class
    {
        /// <summary>
        ///     Creates an instance of <see cref="IdentityStrategy{TType,TIdentity}" /> for entities where the identity property
        ///     has type short.  Uses the provided identity <paramref name="property" /> setter.
        /// </summary>
        /// <param name="property">The property setter used to set the identity value of an entity.</param>
        public ShortIdentityStrategy(Expression<Func<T, short>> property)
            : base(property)
        {
            Generator = GenerateShort;
        }

        /// <summary>
        ///     Returns a value indicating whether a given value equals the default, unset identity value.
        /// </summary>
        /// <param name="id">The identity value to examine.</param>
        /// <returns>A value indicating whether a given value equals the default, unset identity value.</returns>
        protected override bool IsDefaultUnsetValue(short id)
        {
            return id == 0;
        }

        private short GenerateShort()
        {
            SetLastValue(++LastValue);

            return LastValue;
        }
    }
}
