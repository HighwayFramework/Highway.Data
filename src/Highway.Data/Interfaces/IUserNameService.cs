// <copyright file="IUserNameService.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>

namespace Highway.Data
{
    /// <summary>
    ///     This gives a standard interface for the user name service,
    ///     Which is leveraged by the BeforeSave interceptors to supply user names for audit tagging on entities.
    /// </summary>
    public interface IUserNameService
    {
        /// <summary>
        ///     Basic Method for returning the current user name
        /// </summary>
        /// <returns>The current user's name or login</returns>
        string GetCurrentUserName();
    }
}
