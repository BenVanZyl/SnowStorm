using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowStorm.DataContext;
using SnowStorm.QueryExecutors;
using SnowStorm.Users;
using System;
using System.Reflection;

namespace SnowStorm
{
    public static class Setup
    {
        public static void AddSnowStorm(this IServiceCollection services, string connectionString, bool includeAuditUserInfo = false, int poolSize = 32)
        {
            Assembly appAssembly = Assembly.GetCallingAssembly();

            AddSnowStorm(services, appAssembly, connectionString, includeAuditUserInfo, poolSize);
        }

        public static void AddSnowStorm(this IServiceCollection services, string assemblyName, string connectionString, bool includeAuditUserInfo = false, int poolSize = 32)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new InvalidOperationException($"SnowStorm.Setup.AddSnowStorm(...) : Missing assemblyName");

            Assembly appAssembly = Assembly.Load(assemblyName);

            AddSnowStorm(services, appAssembly, connectionString, includeAuditUserInfo, poolSize);
        }

        public static void AddSnowStorm(this IServiceCollection services, Assembly appAssembly, string connectionString, bool includeAuditUserInfo = false, int poolSize = 32)
        {
            if (appAssembly == null)
                throw new InvalidOperationException($"SnowStorm.Setup.AddSnowStorm(...) : Missing assembly");

            //setup DbContext
            AddAppDbContext(services, appAssembly, connectionString, poolSize: poolSize);

            //setup auto mapper
            AddAutoMapper(services, appAssembly);

            //audit user info
            if (includeAuditUserInfo)
                AddUserInfo(services);

            //setup IOC container provider
            Container.SetInstance(services.BuildServiceProvider());
        }

        //
        // builder.Services.AddHttpContextAccessor();  -- Required First
        //

        private static void AddUserInfo(IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
        }

        public static void AddAutoMapper(IServiceCollection services, Assembly appAssembly)
        {
            if (appAssembly == null)
                throw new InvalidOperationException($"SnowStorm.Setup.AddMediator(...) : Missing appAssembly.");

            services.AddAutoMapper(appAssembly);
        }

        public static void AddAppDbContext(IServiceCollection services, Assembly appAssembly, string connectionString, int poolSize = 32)
        {
            AppDbContext.AppAssembly = appAssembly;

            services.AddScoped<IQueryableProvider, QueryableProvider>();

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