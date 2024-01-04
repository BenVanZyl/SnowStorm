using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace WebSample.DbScripts
{
    public class ScriptExecutor
    {
        private readonly string _connectionString;

        public ScriptExecutor(string[] args)  //todo:... change to command line options.
        {
            if (args.Count() == 1)
                _connectionString = args[0];
            else if (Configuration != null && Configuration.GetConnectionString("Data") != null)
                _connectionString = Configuration.GetConnectionString("Data");

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                string msg = "ERROR! => missing connection string.";
                Write(msg);
                throw new MissingMemberException(msg);
            }

            Write($"_connectionString = '{_connectionString[..31]}';");
        }

        public bool PerformUpgrade()
        {
            Write("EnsureDatabase exists");
            EnsureDatabase.For.SqlDatabase(_connectionString);

            // scripts that reset already run scripts in order to run updates on the speciifed object (table, proc, view, function, etc)
            DeployScripts(ScriptTokens.PreDeployment);

            // actual deployment scripts
            DeployScripts(ScriptTokens.DeployDbObjects);

            // All Data scripts
            DeployScripts(ScriptTokens.DeployDataAllEnvironments);

            //// only run these scripts when actually doing a deployment but not when doing testing.
            //if (!RunningAs.UnitTest)
            //    DeployScripts(ScriptTokens.DeployOnly)

            //// only run these when testing
            //if (RunningAs.UnitTest)
            //    DeployScripts(ScriptTokens.TestOnly)

            // setup and update db users
            //SetupUsers.ConfigureDatabase(_connectionString, Configuration);

            Write("Success!", ConsoleColor.Green);
            return true;
        }

        private void DeployScripts(string stageToken)
        {
            Write($"Stage Started: '{stageToken}'", ConsoleColor.DarkCyan);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithTransactionPerScript()
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(stageToken, StringComparison.OrdinalIgnoreCase))
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Write($"Stage ERROR: '{stageToken}'", ConsoleColor.Red);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                //return -1;
                throw new Exception($"Errors Occurred performing database update!/n{result.Error}");
            }

            Write($"Stage completed: '{stageToken}'", ConsoleColor.DarkCyan);
        }


        #region configuration

        private IConfigurationRoot? _configuration = null;
        private IConfigurationRoot Configuration 
        {
            get
            {
                if (_configuration == null)
                {
                    var fileName = "appsettings.json";
//#if DEBUG
//                    Console.WriteLine("Debug version");
//                    fileName = "appsettings.Development.json";
//#endif

                    var builder = new ConfigurationBuilder()
                       .SetBasePath(AppContext.BaseDirectory)
                       .AddJsonFile(fileName, optional: true, reloadOnChange: true);

                    Write($"config setup: {fileName}", ConsoleColor.Blue);

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        #endregion

        #region console 

        private void Write(string msg) => Console.WriteLine($"Performing updgrade: {msg}");

        private void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Write(msg);
            Console.ResetColor();
        }

        #endregion
    }
}
