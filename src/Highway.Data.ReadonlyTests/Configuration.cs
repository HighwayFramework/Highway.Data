using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Highway.Data.ReadonlyTests
{
    internal class Configuration
    {
        private static readonly Lazy<Configuration> LazyInstance = new(Instantiate);

        public static Configuration Instance = LazyInstance.Value;

        private readonly IConfigurationRoot _configurationRoot;

        private Configuration()
        {
            var testDatabaseGuid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            TestDatabaseName = $"secured-query-engine-{testDatabaseGuid}";

            _configurationRoot = new ConfigurationBuilder()
                                 .AddEnvironmentVariables()
                                 .AddUserSecrets(GetType().Assembly)
                                 .Build();

            var loggerFactory = new LoggerFactory(new List<ILoggerProvider>(), new LoggerFilterOptions());
            Logger = loggerFactory.CreateLogger("integration-tests");
            MasterConnectionString = GetConnectionStringForDatabase("master");
            TestDatabaseConnectionString = GetConnectionStringForDatabase(TestDatabaseName);
        }

        public ILogger Logger { get; }

        public string MasterConnectionString { get; }

        public string TestDatabaseConnectionString { get; }

        public string TestDatabaseName { get; }

        private static Configuration Instantiate()
        {
            return new Configuration();
        }

        private string GetConnectionStringForDatabase(string databaseName)
        {
            var dbConnection = _configurationRoot.GetValue<string>("SqlConnectionString");

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(dbConnection)
            {
                InitialCatalog = databaseName
            };

            return sqlConnectionStringBuilder.ConnectionString;
        }
    }
}
