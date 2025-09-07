using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Interfaces;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        /// <summary>
        /// The name of the assembly where the domain classes for your application is located.
        /// </summary>
        public static string DataAssemblyName { get; set; } = string.Empty;

        /// <summary>
        /// Adding and configuring the EF DataContext, QueryRunner, QueryProvider and Automapper components.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="dataAssemblyName">GetCallingAssembly will be used if this is empty.</param>
        public static void AddSnowStorm(this IServiceCollection services, string connectionString, string dataAssemblyName = "")
        {
             DataAssemblyName = dataAssemblyName;

            DataContext.AppAssembly = string.IsNullOrEmpty(dataAssemblyName) ? Assembly.GetCallingAssembly() : Assembly.Load(dataAssemblyName);

            services.AddAutoMapper(DataContext.AppAssembly);

            //services.AddAutoMapper(Assembly.GetCallingAssembly());
            //services.AddAutoMapper(Assembly.GetEntryAssembly());            
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IQueryableProvider, QueryableProvider>();
            services.AddScoped<QueryRunner>();

            services.AddDbContextFactory<DataContext>(o =>
            {
                o.UseSqlServer(connectionString, options => options.EnableRetryOnFailure(3));
            }, 
            ServiceLifetime.Scoped);
        }
    }
}