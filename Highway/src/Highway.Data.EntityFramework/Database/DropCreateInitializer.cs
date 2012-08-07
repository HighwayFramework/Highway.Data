using System;
using System.Data.Entity;
using System.Data.SqlClient;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seedAction"></param>
        public DropCreateInitializer(Action<T> seedAction = null)
        {
            _seedAction = seedAction;
        }

        public void InitializeDatabase(T context)
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(string.Format("use master; \r\n alter database {0} set single_user with rollback immediate;", context.Database.Connection.Database));
                context.Database.ExecuteSqlCommand(string.Format("use master; \r\n Drop database {0};", context.Database.Connection.Database));
            }
            context.Database.CreateIfNotExists();
            if (_seedAction != null)
            {
                _seedAction(context);
                context.SaveChanges();
            }
        }
    }
}