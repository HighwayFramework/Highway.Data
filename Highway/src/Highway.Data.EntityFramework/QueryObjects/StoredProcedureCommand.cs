// -----------------------------------------------------------------------
// <copyright file="StoredProcedureCommand.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2019, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureCommand : AdvancedCommand
    {
        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string StoredProcedureName { get; }

        protected DataContext TypedContext { get; set; }

        public override void Execute(IDataContext context)
        {
            TypedContext = (DataContext)context;
            Parameters = GetParameters();
            ContextQuery = c =>
            {
                var cmd = c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
                cmd.ExecuteCommand();
            };
            ContextQuery(TypedContext);
        }

        protected abstract IEnumerable<IDataParameter> GetParameters();
    }

    public abstract class StoredProcedureCommand<T> : QueryBase, IScalar<T>
    {
        protected Func<DataContext, T> ContextQuery { get; set; }

        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string StoredProcedureName { get; }

        protected DataContext TypedContext { get; set; }

        public T Execute(IDataContext context)
        {
            TypedContext = (DataContext)context;
            Parameters = GetParameters();
            ContextQuery = c =>
            {
                var cmd = c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
                return cmd.ExecuteCommandWithResult(GetResultingValue);
            };

            return ContextQuery(TypedContext);
        }

        protected abstract IEnumerable<IDataParameter> GetParameters();

        protected abstract T GetResultingValue(IDbCommand cmd);
    }
}
