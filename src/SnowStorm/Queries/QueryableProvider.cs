using SnowStorm.DataContext;
using System.Linq;

namespace SnowStorm.Queries
{
    public class QueryableProvider(AppDbContext dbContext) : IQueryableProvider
    {
        private readonly AppDbContext _dbContext = dbContext;

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
    }
}