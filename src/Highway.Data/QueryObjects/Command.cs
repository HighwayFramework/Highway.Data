// <copyright file="Command.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
    ///     An implementation that executes functions against the database independent of the data access layer
    /// </summary>
    public class Command : QueryBase, ICommand
    {
        /// <summary>
        ///     The Command that will be executed at some point in the future
        /// </summary>
        protected Action<IDataContext> ContextQuery { get; set; }

        /// <summary>
        ///     Executes the expression against the passed in context and ignores the returned value if any
        /// </summary>
        /// <param name="context">The data context that the command is executed against</param>
        public virtual void Execute(IDataContext context)
        {
            Context = context;
            CheckContextAndQuery(ContextQuery);
            ContextQuery(context);
        }
    }
}
