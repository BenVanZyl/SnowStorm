using AutoMapper;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Configurations;
using SnowStorm.Domain;
using System;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        
        public static void All(ref IServiceCollection services, Assembly startup, Profile mappingProfile, string connectionString, string externalAssemblyName = "")
        {
            if (startup is null)
                throw new ArgumentNullException(nameof(startup));

            if (mappingProfile == null)
                throw new ArgumentNullException(nameof(mappingProfile));

            //setup DbContext
            DbContext(ref services, connectionString, externalAssemblyName);

            //setup query executors
            QueryExecutor(ref services);

            //Setup MediatR
            Mediator(ref services, startup, externalAssemblyName);

            //setup automapper
            AutoMapper(ref services, mappingProfile);
        }

        public static void QueryExecutor(ref IServiceCollection services)
        {
            QueryExecutorConfiguration.Configure(ref services);
        }

        public static void Mediator(ref IServiceCollection services, Assembly startup, string externalAssemblyName = "")
        {
            MediatorConfiguration.Configure(services, startup, externalAssemblyName);
        }

        public static void AutoMapper(ref IServiceCollection services, Profile mappingProfile)
        {
            AutoMapperConfiguration.Configure(ref services, mappingProfile);
        }

        public static void DbContext(ref IServiceCollection services, string connectionString, string externalAssemblyName = "")
        {
            AppDbContext.ExternalAssemblyName = externalAssemblyName;
            //services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString)) // straight sql connection

            //Auto apply azure managed identity connection to sql server if needed
            services.AddDbContext<AppDbContext>
            (o =>
            {
                if (connectionString.Contains(".database.windows.net", System.StringComparison.OrdinalIgnoreCase) && !connectionString.Contains("password", System.StringComparison.OrdinalIgnoreCase))
                {   // SQL Server Db in Azure, using Azure AD integrated auth
                    SqlConnection connection = new()
                    {
                        ConnectionString = connectionString,
                        AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result
                    };
                    o.UseSqlServer(connection);
                }
                else
                    o.UseSqlServer(connectionString); //identity provided in string, straight sql connection
            });
        }
    }
}
