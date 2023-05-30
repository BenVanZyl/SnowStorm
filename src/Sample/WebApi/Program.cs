
using SnowStorm;
using System.Runtime.CompilerServices;
using WebApi.Services.Infrastructure;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

      
            CreateHostBuilder(args).Build().Run();

            /// Startup.cs have been dropped from the web app templates, but the TestServer requires it to work properly.
            #region Original Code

            //var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.
            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //builder.Services.AddHttpContextAccessor(); //TODO:  move into SnowStorm

            //var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true;Connection Lifetime=1;Timeout=35;Pooling=true;Min Pool Size=1;Max Pool Size=32;";
            //builder.Services.AddSnowStorm("WebApi", connectionString);

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            //app.MapControllers();

            //app.Run();

            #endregion
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            ;
    }
}