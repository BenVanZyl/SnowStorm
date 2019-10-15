using System.Linq;

namespace SnowStorm.Infrastructure.QueryExecutors
{
    public interface IQueryableProvider
    {
        IQueryable<T> Query<T>() where T : class;
    }
}
