using Microsoft.EntityFrameworkCore;
using SnowStorm.Domain;
using System.Linq;

namespace SnowStorm.QueryExecutors
{
    public class QueryableProvider : IQueryableProvider
    {
        private readonly AppDbContext _dbContext;

        public QueryableProvider(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
    }
}
