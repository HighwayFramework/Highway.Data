using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data
{
    // ERIC:  Derive from this?  Seems heavy-handed.
    public abstract class AdoCommandBase : AdvancedCommand
    {
        protected abstract IEnumerable<IDataParameter> Parameters { get; }

        protected AdoCommandBase()
        {
            ContextQuery = dbContext =>
            {
                var dbCommand = GetDbCommand(dbContext);
                dbCommand.Execute();
            };
        }

        public void Execute(IDataContext dataContext)
        {
            var entityDbContext = dataContext.GetEntityDbContext();
            ContextQuery(entityDbContext);
        }

        protected abstract IDbCommand GetDbCommand(DbContext dbContext);
    }
}