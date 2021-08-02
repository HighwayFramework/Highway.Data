// <copyright file="IAuditableEntity.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
    ///     The interface to implement when using with the default Auditable Interceptor to specify an Auditable Entity
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        ///     Who created this entity
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        ///     The date this entity was created
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Who last modified this entity
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        ///     The date this entity was last modified
        /// </summary>
        DateTime ModifiedDate { get; set; }
    }
}
