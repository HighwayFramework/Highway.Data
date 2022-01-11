using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Highway.Data.ReadonlyTests
{
    internal class TestConfiguration : ITestConfiguration
    {
        private static readonly Lazy<TestConfiguration> LazyInstance = new(Instantiate);

        public static TestConfiguration Instance = LazyInstance.Value;

        private readonly IConfigurationRoot _configurationRoot;

        private readonly Assembly _testAssembly;

        private readonly string _testDatabaseGuid;

        private TestConfiguration()
        {
            _testAssembly = GetType().Assembly;
            _testDatabaseGuid = Guid.NewGuid().ToString().Replace("-", string.Empty);
            _configurationRoot = new ConfigurationBuilder()
                                 .AddEnvironmentVariables()
                                 .AddUserSecrets(_testAssembly)
                                 .Build();

            ILoggerFactory loggerFactory = new LoggerFactory(new List<ILoggerProvider>(), new LoggerFilterOptions());

            Logger = loggerFactory.CreateLogger("integration-tests");
            MasterConnectionString = GetConnectionStringForDatabase("master");
            TestDatabaseConnectionString = GetConnectionStringForDatabase(TestDatabaseName);
        }

        public ILogger Logger { get; }

        public string MasterConnectionString { get; }

        public string TestDatabaseConnectionString { get; }

        public string TestDatabaseName { get; }

        private static TestConfiguration Instantiate()
        {
            return new TestConfiguration();
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
