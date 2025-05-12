using System.Linq;

namespace SnowStorm.Queries
{
    public interface IQueryableProvider
    {
        IQueryable<T> Query<T>() where T : class;
    }
}