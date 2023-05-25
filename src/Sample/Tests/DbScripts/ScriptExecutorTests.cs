using DbScripts;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DbScripts
{
    public class ScriptExecutorTests
    {
        private const string AssemblyNameToTest = "DbScripts";

        /// <summary>
        /// Error in this test means there is a script that has not been marked as embedded resource.  Checck the error message and scripts.
        /// </summary>
        [Fact]
        public void VerifyAllScriptsEmbedded()
        {
            string scriptPath = $@"{AssemblyNameToTest}\Scripts";  //TODO: Changes to Theory?
            //string scriptPath = @"C:\Dev\GitHub\BvZ\SnowStorm\src\Sample\DbScripts\Scripts\";

            var scriptsOnDisk = Directory.GetFiles(PathToScripts(scriptPath), "*.sql", SearchOption.AllDirectories).Select(Path.GetFileName);
            Console.WriteLine($" -- scriptsOnDisk Count = '{scriptsOnDisk.Count()}'");

            //var scriptsEmbedded = Assembly.GetAssembly(typeof(ScriptExecutor)).GetManifestResourceNames();
            var scriptsEmbedded = Assembly.Load(AssemblyNameToTest).GetManifestResourceNames();
            Console.WriteLine($" -- scriptsEmbedded Count = '{scriptsEmbedded.Count()}'");

            foreach (var f in scriptsOnDisk)
            {
                scriptsEmbedded.ShouldContain(e => e.EndsWith(f),  "Script file: " + f);
            }
        }

        /// <summary>
        /// Verify that none of the scripts is using the 'USE SomeDbName' statement
        /// </summary>
        [Fact]
        public void VerifyUseDbStatement()
        {

            //var scriptsEmbedded = Assembly.GetAssembly(typeof(ScriptExecutor)).GetManifestResourceNames().Where(w => w.EndsWith(".sql"));
            var scriptsEmbedded = Assembly.Load(AssemblyNameToTest).GetManifestResourceNames().Where(w => w.EndsWith(".sql"));
            Console.WriteLine($" -- scriptsEmbedded Count = '{scriptsEmbedded.Count()}'");

            foreach (var script in scriptsEmbedded)
            {
                //script.ToUpper().Contains("USE").ShouldBeFalse();
                var content = GetFromResources(script);
                content.ToUpper().Contains("USE ").ShouldBeFalse($"Script file contains USE statement: {script}");
            }
        }

        private string PathToScripts(string scriptPath)
        {

            Console.WriteLine(" ## Getting assemblyLocation ...");
            var baseDirectory = AppContext.BaseDirectory;
            Console.WriteLine($" ##   assemblyLocation = '{baseDirectory}' ");

            Console.WriteLine(" ## Getting srcPosition ...");
            var srcPosition = baseDirectory.IndexOf(@"src") + 4; // location = start position of search string + 4 position onward to include slash sign
            Console.WriteLine($" ##   srcPosition = '{srcPosition}' ");

            Console.WriteLine(" ## Getting rootPath ...");
            var rootPath = baseDirectory.Remove(srcPosition, baseDirectory.Count() - srcPosition);
            Console.WriteLine($" ##   rootPath = '{rootPath}' ");
            Directory.Exists(rootPath).ShouldBeTrue();

            Console.WriteLine(" ## Compiling Full Path ...");
            var path = $"{rootPath}Sample\\{scriptPath}";
            Console.WriteLine($" ##   Full Path = '{path}' ");
            if (!Directory.Exists(path))
            {
                Console.WriteLine($" ##   ** Full Path NOT FOUND! Switching context ... ");
                scriptPath = scriptPath.Replace(@"\", @"/");
                path = $"{rootPath}{scriptPath}";
                Console.WriteLine($" ##   ** Full Path = '{path}' ");
            }

            //final confirmation
            Directory.Exists(path).ShouldBeTrue();

            return path;
        }

        private string GetFromResources(string resourceName)
        {
            Assembly assem = Assembly.GetAssembly(typeof(ScriptExecutor));

            using Stream stream = assem.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        } 
    }
}
