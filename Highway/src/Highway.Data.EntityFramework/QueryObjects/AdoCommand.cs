using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Highway.Data.EntityFramework.Extensions;
using Highway.Data.Extensions;

namespace Highway.Data
{
    public abstract class AdoCommand : AdvancedCommand
    {
        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string Query { get; }

        protected DataContext TypedContext { get; set; }

        public override void Execute(IDataContext context)
        {
            TypedContext = (DataContext)context;
            Parameters = GetParameters();
            ContextQuery = c =>
            {
                var cmd = c.CreateSqlCommand(Query, Parameters?.ToArray());
                cmd.ExecuteCommand();
            };
            ContextQuery(TypedContext);
        }

        public abstract IEnumerable<IDataParameter> GetParameters();
    }

    public abstract class AdoCommand<T> : QueryBase, IScalar<T>
    {
        protected Func<DbContext, T> ContextQuery { get; set; }

        protected IEnumerable<IDataParameter> Parameters { get; set; }

        protected abstract string Query { get; }

        protected DbContext TypedContext { get; set; }

        public T Execute(IDataContext context)
        {
            TypedContext = (DbContext)context;
            Parameters = GetParameters();
            ContextQuery = c =>
            {
                var cmd = c.CreateSqlCommand(Query, Parameters?.ToArray());
                return cmd.ExecuteCommandWithResult(GetResultingValue);
            };

            return ContextQuery(TypedContext);
        }

        protected abstract IEnumerable<IDataParameter> GetParameters();

        protected abstract T GetResultingValue(IDbCommand cmd);
    }
}
