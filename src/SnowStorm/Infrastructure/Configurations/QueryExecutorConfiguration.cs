using Microsoft.Extensions.DependencyInjection;
using SnowStorm.Infrastructure.QueryExecutors;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class QueryExecutorConfiguration
    {
        public static void Configure(ref IServiceCollection services)
        {
            services.AddScoped<IQueryableProvider, QueryableProvider>();

            services.AddScoped<IQueryExecutor, QueryExecutor>();

            //TODO: inject user info data object here.

        }
    }
}
