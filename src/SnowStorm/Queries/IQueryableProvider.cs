using System.Linq;

namespace SnowStorm.QueryExecutors
{
    public interface IQueryableProvider
    {
        IQueryable<T> Query<T>() where T : class;
    }
}