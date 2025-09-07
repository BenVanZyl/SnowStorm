using SnowStorm.Interfaces;
using System.Linq;

namespace SnowStorm
{
    public class QueryableProvider(DataContext dbContext) : IQueryableProvider
    {
        private readonly DataContext _dbContext = dbContext;

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
    }
}
