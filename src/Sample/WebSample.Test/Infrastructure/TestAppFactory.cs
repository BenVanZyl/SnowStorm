using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using WebSample.SnowStorm;
using WebSample.Tests.Infrastructure;

namespace WebSample.Test.Infrastructure
{
    public class TestAppFactory : WebApplicationFactory<Program>
    {
        // You can override the ConfigureWebHost method to configure the test server
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            DbManager.ConfigureDatabase();

            // For example, you can use a different appsettings file for testing
            builder.ConfigureAppConfiguration((context, config) =>
            {
                //config.AddJsonFile("appsettings.Test.json");
            })
            .UseSetting("ConnectionStrings:AppData", DbManager.ConnectionString)
            .UseEnvironment("Development");

            base.ConfigureWebHost(builder);

        }   
    }
}

