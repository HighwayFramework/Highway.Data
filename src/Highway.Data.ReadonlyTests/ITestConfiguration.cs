using Microsoft.Extensions.Logging;

namespace Highway.Data.ReadonlyTests
{
    public interface ITestConfiguration
    {
        /// <summary>
        ///     Gets the configured <see cref="ILogger" />.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        ///     Gets a connection string for the master database.
        /// </summary>
        string MasterConnectionString { get; }

        /// <summary>
        ///     Gets a connection string for the test database.
        /// </summary>
        string TestDatabaseConnectionString { get; }

        /// <summary>
        ///     Gets the name of the test database.
        /// </summary>
        string TestDatabaseName { get; }
    }
}
