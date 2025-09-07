
using SnowStorm.Extensions;
using SnowStorm.Interfaces;
using WebSample.SnowStorm.Server.Services.Domain;

namespace WebSample.SnowStorm.Server.Services.Queries
{
    public class GetWeatherReportQuery : IQueryResultSingle<WeatherReport>
    {
        private readonly long? _id;
        private readonly string? _descriptionContains;

        public GetWeatherReportQuery(long id)
        {
            _id = id;
        }

        public GetWeatherReportQuery(string descriptionContains)
        {
            _descriptionContains = descriptionContains;
        }

        public Task Delete(IQueryableProvider queryableProvider)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WeatherReport> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<WeatherReport>();

            if (_id.HasValue)
                query = query.Where(w => w.Id == _id.Value);

            if (_descriptionContains.HasValue())
                query = query.Where(w => w.ReportName == _descriptionContains);

            //query = query.Include(i => i.wea)

            return query;
        }

        public Task Update(IQueryableProvider queryableProvider)
        {
            throw new NotImplementedException();
        }
    }
}
