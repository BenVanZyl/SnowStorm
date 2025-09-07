using SnowStorm.Interfaces;
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

        public Task Delete(IQueryableProvider queryableProvider)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WeatherData> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<WeatherData>()
               .Where(w => w.Id == _id);
        }

        public Task Update(IQueryableProvider queryableProvider)
        {
            throw new NotImplementedException();
        }
    }
}
