using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSample.Tests.Infrastructure
{
    internal class DbCleanup
    {
        private readonly string _dbName;
        private readonly string _dbNameRoot;
        private readonly string _dbNameToday;
        private readonly string _connectionString;
        public DbCleanup(string dbName, string dbNameRoot, string connectionString)
        {
            _dbName = dbName;
            _dbNameRoot = dbNameRoot;
            _dbNameToday = $"{_dbNameRoot}-{DateTime.Today:yyyyMMdd}";
            _connectionString = connectionString;
        }

        public void Execute()
        {
            //define command object
            SqlCommand? cmd = null;

            try
            {
                // get script content
                var script = GetScriptContent();
                script = script.Replace("$(DatabaseName)", _dbName);  //assign the database name to be removed
                script = script.Replace("$(DatabaseNameRoot)", _dbNameRoot);  //assign the database name to be removed
                script = script.Replace("$(DatabaseNameToday)", _dbNameToday);  //assign the database name to be removed


                // setup command object
                cmd = new SqlCommand()
                {
                    Connection = new SqlConnection(_connectionString.Replace(_dbName, "master")),
                    CommandType = CommandType.Text,
                    CommandText = script
                };

                //open connection
                cmd.Connection.Open();

                // execute script
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error DbCleanup - {ex.Message}");
                //Log.Logger.Error($"Error DbCleanup - {ex.Message}", ex);
                //throw ex;
            }
            finally
            {
                //close command and connection
                if (cmd != null && cmd.Connection != null && cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        private static string GetScriptContent()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First(w => !string.IsNullOrEmpty(w.FullName) && w.FullName.Contains("Tests"));
            string scriptName = assembly.GetManifestResourceNames().First(w => w.EndsWith("DbCleanupScript.sql"));

            using Stream? stream = assembly.GetManifestResourceStream(scriptName);
            if (stream == null)
                return "";

            using var reader = new StreamReader(stream);
            if (reader == null)
                return "";

            string content = reader.ReadToEnd();

            return content;
        }

    }
}