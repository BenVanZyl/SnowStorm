using WebSample.DbScripts;

namespace WebSample.Tests.Infrastructure
{
    /// <summary>
    /// Using DbUp to spin up database and run scripts.
    /// Cleanup Db by running a DbCleanup script that drops the used test db
    /// </summary>
    internal static class DbManager
    {
        #region Using DbUp to spin up database and run scripts.

        private static readonly object _lock = new();
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

        #region Cleanup

        public static void Cleanup()
        {
            var cleanUp = new DbCleanup(DbName, CoreDbName, ConnectionString);
            cleanUp.Execute();
        }

        #endregion
    }
}
