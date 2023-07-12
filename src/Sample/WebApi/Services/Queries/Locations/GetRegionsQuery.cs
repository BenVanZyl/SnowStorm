using SnowStorm.QueryExecutors;
using WebApi.Services.Domain;

namespace WebApi.Services.Queries.Locations
{
    public class GetRegionsQuery : IQueryResultList<Region>
    {

        public IQueryable<Region> Get(IQueryableProvider queryableProvider)
        {
            return queryableProvider.Query<Region>()
               .OrderBy(o => o.RegionDescription)
               .AsQueryable();
        }
    }
}
