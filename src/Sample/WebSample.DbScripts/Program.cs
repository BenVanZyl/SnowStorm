
namespace WebSample.DbScripts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            try
            {
                Console.WriteLine("--------------------------------");

                ScriptExecutor scriptExecutor = new(args);
                scriptExecutor.PerformUpgrade();

                Console.WriteLine("DONE.  DB upgrade completed.");
                Console.WriteLine("--------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DONE.  DB upgrade FAILED!");
                Console.WriteLine("--------------------------------");
                Console.WriteLine(ex.Message);
                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                    Console.WriteLine(ex.StackTrace);
                Console.WriteLine("--------------------------------");
            }

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}