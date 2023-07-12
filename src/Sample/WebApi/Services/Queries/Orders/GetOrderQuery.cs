using Microsoft.EntityFrameworkCore;
using SnowStorm.QueryExecutors;
using WebApi.Services.Domain;

namespace WebApi.Services.Queries.Orders
{
    public class GetOrderQuery : IQueryResultSingle<Order>
    {
        public GetOrderQuery(int id)
        {
            _id = id;
        }

        public GetOrderQuery WithIncludes()
        {
            HasIncludes = true;
            return this;
        }

        public bool HasIncludes = false;
        private int _id;

        public IQueryable<Order> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<Order>();

            //if (HasIncludes)
            //    query = query.Include(i => i.Customer);

               query = query
                    .Where(w => w.Id == _id)
                    .OrderBy(o => o.OrderDate)
                    .AsQueryable();

            return query;
        }
    }
}
