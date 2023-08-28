using SnowStorm.QueryExecutors;
using WebApi.Services.Domain;

namespace WebApi.Services.Queries.Locations
{
    public class GetRegionQuery : IQueryResultSingle<Region>
    {
        private readonly int _id;

        public GetRegionQuery(int id)
        {
            _id = id;
        }

        public IQueryable<Region> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<Region>()
               .Where(w => w.Id == _id);
        }
    }
}