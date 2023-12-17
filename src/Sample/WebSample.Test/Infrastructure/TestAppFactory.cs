using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Xml.Linq;
using WebSample.DbScripts;
using WebSample.SnowStorm;
using WebSample.Tests.Infrastructure;

namespace WebSample.Test.Infrastructure
{
    public class TestAppFactory : WebApplicationFactory<Program>
    {
        // You can override the ConfigureWebHost method to configure the test server
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ConfigureDatabase();

            // For example, you can use a different appsettings file for testing
            builder.ConfigureAppConfiguration((context, config) =>
            {
                //config.AddJsonFile("appsettings.Test.json");
            })
            .UseSetting("ConnectionStrings:AppData", ConnectionString)
            .UseEnvironment("Development");

            base.ConfigureWebHost(builder);
        
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                // Cleanup of test databases.
                var cleanUp = new DbCleanup(DbName, CoreDbName, ConnectionString);
                cleanUp.Execute();
            }

            base.Dispose(disposing);
        }


        #region Using DbUp to spin up database and run scripts.

        private static readonly object _lock = new object();
        public static bool UpgradePerformed { get; set; } = false;
        public static string ConnectionString { get; set; } = "";

        public static void ConfigureDatabase()
        {
            if (UpgradePerformed)
                return;

            lock (_lock)
            {
                if (!string.IsNullOrEmpty(ConnectionString))
                    return; //has a connection string

                //create cnn string
                ConnectionString = $"Server=(localdb)\\mssqllocaldb;Database={DbName};Trusted_Connection=True;MultipleActiveResultSets=true";

                try
                {
                    var dbScripts = new ScriptExecutor(new string[1] { ConnectionString });
                    dbScripts.PerformUpgrade();
                    UpgradePerformed = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ConfigureDatabase() FAILED: {ex.Message}");
                    throw;
                }
            }
        }

        private static string DbName
        {
            get
            {
                if (string.IsNullOrEmpty(_dbName))
                {
                    _dbName = $"{CoreDbName}-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid()}";
                }

                return _dbName;
            }
        }
        private static string _dbName = "";
        private const string CoreDbName = "TstSnowStormSample";

        #endregion
    }
}

