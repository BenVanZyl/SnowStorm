using Microsoft.Extensions.DependencyInjection;
using SnowStorm.QueryExecutors;

namespace SnowStorm.Configurations
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
