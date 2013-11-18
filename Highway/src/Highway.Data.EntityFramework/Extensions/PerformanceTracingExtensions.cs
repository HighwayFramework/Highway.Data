#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;

#endregion

namespace Highway.Data.EntityFramework
{
    /// <summary>
    ///     Extensions for testing and tracing performance of queries and context compilation
    /// </summary>
    public static class PerformanceTracingExtensions
    {
        private static readonly ConsoleOutLogger defaultLogger = new ConsoleOutLogger("Performance", LogLevel.All, false,
            false, false, string.Empty);

        /// <summary>
        ///     Runs the given query against the context and tracks execution time with a default console out logger
        /// </summary>
        /// <param name="query">The query to be executed</param>
        /// <param name="context">the context to run the test against</param>
        /// <param name="firstTimeRun">
        ///     Boolean flag to determine if the context compilation time will be ran outside of the query
        ///     time for traing purpose
        /// </param>
        /// <param name="maxAllowableMilliseconds">the maximum number of milliseconds the execution should take</param>
        /// <typeparam name="T">The type being queried</typeparam>
        /// <returns>a tuple of boolean ( executed under allowed max ) and IEnumberable{T} for the results of the query</returns>
        /// <exception cref="InvalidOperationException">
        ///     If the query execution does not meet the expected time, it will throw this
        ///     error
        /// </exception>
        public static IEnumerable<T> RunPerformanceTest<T>(this IQuery<T> query, IDataContext context,
            bool firstTimeRun = false, int maxAllowableMilliseconds = 250)
            where T : class
        {
            return query.RunPerformanceTest(context, defaultLogger, firstTimeRun, maxAllowableMilliseconds);
        }

        /// <summary>
        ///     Runs the given query against the context and tracks execution time
        /// </summary>
        /// <param name="query">The query to be executed</param>
        /// <param name="context">the context to run the test against</param>
        /// <param name="log">The log to output the information to</param>
        /// <param name="firstTimeRun">
        ///     Boolean flag to determine if the context compilation time will be ran outside of the query
        ///     time for traing purpose
        /// </param>
        /// <param name="maxAllowableMilliseconds">the maximum number of milliseconds the execution should take</param>
        /// <typeparam name="T">The type being queried</typeparam>
        /// <returns>a tuple of boolean ( executed under allowed max ) and IEnumberable{T} for the results of the query</returns>
        /// <exception cref="InvalidOperationException">
        ///     If the query execution does not meet the expected time, it will throw this
        ///     error
        /// </exception>
        public static IEnumerable<T> RunPerformanceTest<T>(this IQuery<T> query, IDataContext context, ILog log,
            bool firstTimeRun = false, int maxAllowableMilliseconds = 250)
            where T : class
        {
            if (!firstTimeRun) context.RunStartUpPerformanceTest(query, log, 10000);
            log.TraceFormat("Beginning Query {0}", query.GetType().Name);
            Stopwatch sw = Stopwatch.StartNew();
            T[] results = query.Execute(context).ToArray();
            sw.Stop();
            log.TraceFormat("Completed Query {0} in {1} ms for {2} records", query.GetType().Name,
                sw.ElapsedMilliseconds, results.Count());
            if (sw.ElapsedMilliseconds > maxAllowableMilliseconds)
                throw new InvalidOperationException(
                    string.Format(
                        "Query {0} in {1} ms but expected to complete in under {2} ms",
                        query.GetType().Name, sw.ElapsedMilliseconds, maxAllowableMilliseconds));
            return results;
        }

        /// <summary>
        ///     Compiles the Context to execution of a query and tracks the time spent
        /// </summary>
        /// <param name="context">the context to run the test against</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="log">The log to output the information to</param>
        /// <param name="maxAllowableMilliseconds">the maximum number of milliseconds the execution should take</param>
        /// <typeparam name="T">The type being queried</typeparam>
        /// <exception cref="InvalidOperationException">
        ///     If the compilation does not meet the expected time, it will throw this
        ///     error
        /// </exception>
        public static void RunStartUpPerformanceTest<T>(this IDataContext context, IQuery<T> query,
            int maxAllowableMilliseconds = 2500)
        {
            context.RunStartUpPerformanceTest(query, defaultLogger, maxAllowableMilliseconds);
        }

        /// <summary>
        ///     Compiles the Context to execution of a query and tracks the time spent
        /// </summary>
        /// <param name="context">the context to run the test against</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="log">The log to output the information to</param>
        /// <param name="maxAllowableMilliseconds">the maximum number of milliseconds the execution should take</param>
        /// <typeparam name="T">The type being queried</typeparam>
        /// <exception cref="InvalidOperationException">
        ///     If the compilation does not meet the expected time, it will throw this
        ///     error
        /// </exception>
        public static void RunStartUpPerformanceTest<T>(this IDataContext context, IQuery<T> query, ILog log,
            int maxAllowableMilliseconds = 2500)
        {
            log.TraceFormat("Beginning Context Compilation");
            Stopwatch sw = Stopwatch.StartNew();
            T[] results = query.Execute(context).ToArray();
            sw.Stop();
            log.TraceFormat("Completed Context Compilation in {0} ms for {1} records",
                sw.ElapsedMilliseconds, results.Count());
            if (sw.ElapsedMilliseconds > maxAllowableMilliseconds)
                throw new InvalidOperationException(
                    string.Format(
                        "Context Compilation in {0} ms but expected to complete in under {1} ms",
                        sw.ElapsedMilliseconds, maxAllowableMilliseconds));
        }
    }
}