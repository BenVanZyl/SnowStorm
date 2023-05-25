using Microsoft.EntityFrameworkCore;
using SnowStorm.QueryExecutors;
using WebApi.Services.Domain;

namespace WebApi.Services.Queries.Orders
{
    public class GetOrdersQuery : IQueryResultList<Order>
    {
        public GetOrdersQuery WithIncludes() 
        {
            HasIncludes = true;
            return this;
        }

        public bool HasIncludes = false;

        public IQueryable<Order> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<Order>();

            //if (HasIncludes)
            //    query = query.Include(i => i.Customer);

            query = query.OrderBy(o => o.OrderDate).AsQueryable();

            return query;
        }
    }

}
