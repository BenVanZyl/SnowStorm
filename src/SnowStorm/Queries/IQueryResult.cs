using SnowStorm.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.QueryExecutors
{
    public interface IQueryResult<T>
    {
        Task<T> Get(IQueryableProvider queryableProvider);
    }

    public interface IQueryResultList<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Get(IQueryableProvider queryableProvider);
    }

    public interface IQueryResultSingle<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Get(IQueryableProvider queryableProvider);
    }
}