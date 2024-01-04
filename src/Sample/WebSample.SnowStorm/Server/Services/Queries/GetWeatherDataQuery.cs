using SnowStorm.QueryExecutors;
using WebSample.SnowStorm.Server.Services.Domain;

namespace WebSample.SnowStorm.Server.Services.Queries
{
    public class GetWeatherDataQuery : IQueryResultSingle<WeatherData>
    {
        private readonly long _id;

        public GetWeatherDataQuery(long id)
        {
            _id = id;
        }

        public IQueryable<WeatherData> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<WeatherData>()
               .Where(w => w.Id == _id);
        }
    }
}
