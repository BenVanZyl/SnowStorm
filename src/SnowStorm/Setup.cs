using AutoMapper;
using MediatR;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Domain;
using SnowStorm.QueryExecutors;
using System;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        public static void AddSnowStorm(this IServiceCollection services, string connectionString, string externalAssemblyName = "")
        {
            //setup DbContext
            AddAppDbContext(ref services, connectionString, externalAssemblyName);

            //setup query executors
            AddQueryExecutor(ref services);

            //Setup MediatR
            AddMediator(ref services, externalAssemblyName);

            //setup automapper
            AddAutoMapper(ref services);
        }

        public static void AddQueryExecutor(ref IServiceCollection services)
        {
            services.AddScoped<IQueryableProvider, QueryableProvider>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
        }

        public static void AddMediator(ref IServiceCollection services, string externalAssemblyName = "")
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            
            if (!string.IsNullOrWhiteSpace(externalAssemblyName))
                services.AddMediatR(Assembly.Load(externalAssemblyName));
        }
                
        public static void AddAutoMapper(ref IServiceCollection services)
        {
            // Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddAppDbContext(ref IServiceCollection services, string connectionString, string externalAssemblyName = "")
        {
            AppDbContext.ExternalAssemblyName = externalAssemblyName;
            //services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString)) // straight sql connection

            //Auto apply azure managed identity connection to sql server if needed
            services.AddDbContextPool<AppDbContext>
            (o =>
                {
                    if (connectionString.Contains(".database.windows.net", System.StringComparison.OrdinalIgnoreCase) && !connectionString.Contains("password", System.StringComparison.OrdinalIgnoreCase))
                    {   // SQL Server Db in Azure, using Azure AD integrated auth
                        SqlConnection connection = new()
                        {
                            ConnectionString = connectionString,
                            AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result
                        };
                        o.UseSqlServer(connection, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        });
                    }
                    else
                        o.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(5);
                        }); //identity provided in string, straight sql connection
                }
            );
        }
    }
}


/// Notes
/// https://ovaismehboob.com/2018/01/31/implementing-mediator-pattern-in-net-core-using-mediatr/
/// https://github.com/jbogard/MediatR/wiki
/// https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection
///
/// Install-Package MediatR
///

