﻿// -----------------------------------------------------------------------
// <copyright file="StoredProcedureScalar.cs" company="Enterprise Products Partners L.P. (Enterprise)">
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
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class StoredProcedureScalar<T> : AdoBase, IScalar<T>
    {
        public abstract string StoredProcedureName { get; }

        public T Execute(IDataContext context)
        {
            var efContext = GetTypedContext(context);
            Func<DbContext, T> contextQuery = c =>
            {
                var cmd = c.CreateStoredProcedureCommand(StoredProcedureName, Parameters?.ToArray());
                return cmd.ExecuteCommandWithResult(MapReaderResults);
            };
            return contextQuery(efContext);
        }

        protected abstract T MapReaderResults(IDataReader reader);
    }
}
