using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Interfaces;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        /// <summary>
        /// Adding and configuring the EF DataContext, QueryRunner, QueryProvider and AutoMapper components.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="dataAssemblyName">GetCallingAssembly will be used if this is empty.</param>
        public static void AddSnowStorm(this IServiceCollection services, string connectionString, string dataAssemblyName = "")
        {
            DataContext.AppAssembly = string.IsNullOrEmpty(dataAssemblyName) ? Assembly.GetCallingAssembly() : Assembly.Load(dataAssemblyName);

            services.AddAutoMapper(cfg => { }, DataContext.AppAssembly);

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