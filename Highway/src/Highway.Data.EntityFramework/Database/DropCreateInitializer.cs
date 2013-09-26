using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Highway.Data
{
    /// <summary>
    /// ****    FOR DEVELOPMENT USAGE ONLY              ****
    /// ****    DO NOT PUT THIS IN PRODUCTION CODE      ****
    /// This class will clear the existing connections to the database and drop the database
    /// </summary>
    public class DropCreateInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        private readonly Action<T> _seedAction;
        private readonly Func<IEnumerable<string>> _storedProcs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seedAction">actions to execute</param>
        /// <param name="storedProcs">stored procedure strings</param>
        public DropCreateInitializer(Action<T> seedAction = null, Func<IEnumerable<string>> storedProcs = null)
        {
            _seedAction = seedAction;
            _storedProcs = storedProcs;
        }

        #region IDatabaseInitializer<T> Members

        public void InitializeDatabase(T context)
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(
                    string.Format("use master; \r\n alter database [{0}] set single_user with rollback immediate;",
                                  context.Database.Connection.Database));
                context.Database.ExecuteSqlCommand(string.Format("use master; \r\n Drop database [{0}];",
                                                                 context.Database.Connection.Database));
            }
            context.Database.CreateIfNotExists();
            if (_storedProcs != null)
            {
                foreach (var sp in _storedProcs())
                {
                    if(string.IsNullOrWhiteSpace(sp)) continue;
                    context.Database.ExecuteSqlCommand(sp);
                }
            }
            if (_seedAction != null)
            {
                _seedAction(context);
                context.SaveChanges();
            }
        }

        #endregion
    }
}