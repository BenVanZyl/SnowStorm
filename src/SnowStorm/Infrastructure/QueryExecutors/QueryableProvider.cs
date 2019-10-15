using SnowStorm.Infrastructure.Domain;
using System.Linq;

namespace SnowStorm.Infrastructure.QueryExecutors
{
    public class QueryableProvider : IQueryableProvider
    {
        private readonly AppDbContext _context;

        public QueryableProvider(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}
