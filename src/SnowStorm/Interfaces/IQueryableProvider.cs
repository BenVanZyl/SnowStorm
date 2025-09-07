using System.Linq;

namespace SnowStorm.Interfaces
{
    public interface IQueryableProvider
    {
        IQueryable<T> Query<T>() where T : class;
    }
}