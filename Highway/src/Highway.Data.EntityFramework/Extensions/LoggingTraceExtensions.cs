using System;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data;

namespace Highway.Data.EntityFramework
{
    /// <summary>
    /// Extension for context to handle output of large numbers of queries
    /// </summary>
    public static class LoggingTraceExtensions
    {
        private static readonly ILog defaultLogger = new ConsoleOutLogger("SqlTrace", LogLevel.All, true, false, true,
                                                                          string.Empty);

        /// <summary>
        /// Compiles the Context to execution of a query and tracks the time spent
        /// </summary>
        /// <param name="context">the context to run the test against</param>
        /// <param name="log">The log to output the information to</param>
        /// <param name="queries">the list of queries to be output</param>
        /// <exception cref="InvalidOperationException">If the compilation does not meet the expected time, it will throw this error</exception>
        public static void OutputSQL(this IDataContext context, ILog log, params IQueryBase[] queries)
        {
            log.TraceFormat("Beginning Sql Output");
            foreach (IQueryBase query in queries)
            {
                log.Trace("************************************************");
                log.TraceFormat("SQL Statement for {0}", query.GetType().Name);
                log.Trace("------------------------------------------------");
                query.OutputQuery(context);
                log.Trace("************************************************");
            }
            log.Trace("SQL Output Completed");
        }

        /// <summary>
        /// Compiles the Context to execution of a query and tracks the time spent with a default console logger
        /// </summary>
        /// <param name="context">the context to run the test against</param>
        /// <param name="queries">the list of queries to be output</param>
        /// <exception cref="InvalidOperationException">If the compilation does not meet the expected time, it will throw this error</exception>
        public static void OutputSQL(this IDataContext context, params IQueryBase[] queries)
        {
            context.OutputSQL(defaultLogger, queries);
        }
    }
}