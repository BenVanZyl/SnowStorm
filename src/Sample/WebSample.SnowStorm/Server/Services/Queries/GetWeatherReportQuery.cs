using Microsoft.EntityFrameworkCore;
using SnowStorm.Extensions;
using SnowStorm.QueryExecutors;
using System.Security.Cryptography;
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
    }
}
