using SnowStorm.QueryExecutors;
using WebSample.SnowStorm.Server.Services.Domain;

namespace WebSample.SnowStorm.Server.Services.Queries
{
    public class GetWeatherReportsQuery : IQueryResultList<WeatherReport>
    {

        public IQueryable<WeatherReport> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<WeatherReport>()
               .OrderBy(o => o.ReportName)
               .AsQueryable();
        }
    }
}
