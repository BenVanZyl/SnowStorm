using SnowStorm.QueryExecutors;
using System.Security.Cryptography;
using WebSample.SnowStorm.Server.Services.Domain;

namespace WebSample.SnowStorm.Server.Services.Queries
{
    public class GetWeatherForReportQuery : IQueryResultList<WeatherData>
    {
        private readonly long _reportId;

        public GetWeatherForReportQuery(long reportId)
        {
            _reportId = reportId;
        }
        public IQueryable<WeatherData> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<WeatherData>()
                .Where(w => w.ReportId == _reportId)
               .OrderBy(o => o.ForecastDate)
               .AsQueryable();
        }
    }
}