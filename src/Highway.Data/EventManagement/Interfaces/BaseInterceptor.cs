// <copyright file="BaseInterceptor.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

using Highway.Data.Interceptors;

namespace Highway.Data.EventManagement.Interfaces
{
    /// <summary>
    ///     Base interface for dealing with event interception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventInterceptor<in T> : IInterceptor
    {
        bool AppliesTo(T eventArgs);

        /// <summary>
        ///     Executes the interceptors that were applied during the domain creation
        /// </summary>
        /// <param name="context">The DataContext that the current interceptor is using</param>
        /// <param name="eventArgs">the EventArgs for the current event being intercepted</param>
        /// <returns></returns>
        InterceptorResult Apply(IDataContext context, T eventArgs);
    }

    /// <summary>
    ///     Base interface for dealing with event interception
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        ///     Priority for the interceptor, execution is low to high
        /// </summary>
        int Priority { get; }
    }
}
