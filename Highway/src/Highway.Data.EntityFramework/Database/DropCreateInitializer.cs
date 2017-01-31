
using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace Highway.Data
{
    /// <summary>
    ///     ****    FOR DEVELOPMENT USAGE ONLY              ****
    ///     ****    DO NOT PUT THIS IN PRODUCTION CODE      ****
    ///     This class will clear the existing connections to the database and drop the database
    /// </summary>
    public class DropCreateInitializer<T> : IDatabaseInitializer<T>
        where T : DbContext
    {
        private readonly Action<T> _seedAction;

        private readonly Func<IEnumerable<string>> _adoScripts;

        /// <summary>
        /// </summary>
        /// <param name="seedAction">actions to execute</param>
        /// <param name="adoScripts">stored procedure strings</param>
        public DropCreateInitializer(Action<T> seedAction = null, Func<IEnumerable<string>> adoScripts = null)
        {
            _seedAction = seedAction;
            _adoScripts = adoScripts;
        }


        public void InitializeDatabase(T context)
        {
            context.Database.CreateIfNotExists();
            if (_adoScripts != null)
            {
                foreach (var script in _adoScripts())
                {
                    if (string.IsNullOrWhiteSpace(script))
                    {
                        continue;
                    }

                    context.Database.ExecuteSqlCommand(script);
                }
            }

            if (_seedAction != null)
            {
                _seedAction(context);
                context.SaveChanges();
            }
        }

    }
}