using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Domain;
using SnowStorm.QueryExecutors;
using System;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        public static void AddSnowStorm(this IServiceCollection services, string assemblyName, string connectionString, bool includeAuditUserInfo = true, int poolSize = 128)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new InvalidOperationException($"SnowStorm.Setup.AddSnowStorm(...) : Missing assemblyName");

            Assembly appAssembly = Assembly.Load(assemblyName);

            //setup DbContext
            AddAppDbContext(ref services, ref appAssembly, connectionString, poolSize: poolSize);

            //setup query executors
            AddQueryExecutor(ref services);

            //Setup MediatR
            AddMediator(ref services, appAssembly);

            //setup automapper
            AddAutoMapper(ref services, ref appAssembly);

            //audit user info
            if (includeAuditUserInfo)
                AddUserInfo(ref services);

            //setup IOC container provider
            Container.SetInstance(services.BuildServiceProvider());
        }

        /// <summary>
        /// builder.Services.AddHttpContextAccessor();  -- Required First
        /// </summary>
        /// <param name="services"></param>
        private static void AddUserInfo(ref IServiceCollection services)
        {
            //services.AddHttpContextAccessor()
            //services.AddScoped<ICurrentUserInfo, CurrentUserInfo>();
        }

        public static void AddQueryExecutor(ref IServiceCollection services)
        {
            services.AddScoped<IQueryableProvider, QueryableProvider>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
        }

        public static void AddMediator(ref IServiceCollection services, Assembly appAssembly)
        {
            if (appAssembly == null)
                throw new InvalidOperationException($"SnowStorm.Setup.AddMediator(...) : Missing appAssembly.");

            //services.AddMediatR(appAssembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
        }

        public static void AddAutoMapper(ref IServiceCollection services, ref Assembly appAssembly)
        {
            if (appAssembly == null)
                throw new InvalidOperationException($"SnowStorm.Setup.AddMediator(...) : Missing appAssembly.");

            services.AddAutoMapper(appAssembly);
        }

        public static void AddAppDbContext(ref IServiceCollection services, ref Assembly appAssembly, string connectionString, int poolSize = 32)
        {
            AppDbContext.AppAssembly = appAssembly;

            //Auto apply azure managed identity connection to sql server if needed
            services.AddDbContextPool<AppDbContext>
            (o =>
                {
                    if (connectionString.Contains(".database.windows.net", System.StringComparison.OrdinalIgnoreCase) && !connectionString.Contains("password", System.StringComparison.OrdinalIgnoreCase))
                    {   // SQL Server Db in Azure, using Azure AD integrated auth
                        var credential = new Azure.Identity.DefaultAzureCredential();
                        var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                        SqlConnection connection = new()
                        {
                            ConnectionString = connectionString,
                            AccessToken = token.Token
                        };
                        o.UseSqlServer(connection, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        });
                    }
                    else
                        o.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure();
                        }); //identity provided in string, straight sql connection
                },
                poolSize: poolSize
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

