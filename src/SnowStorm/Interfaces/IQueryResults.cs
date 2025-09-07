using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.Interfaces
{
    public interface IQueryResultList<out T> where T : class, IDomainEntity
    {
        /// <summary>
        /// Returns a List of IDomainEntity objects from the database using the provided queryable provider.
        /// </summary>
        /// <param name="queryableProvider"></param>
        /// <returns></returns>
        IQueryable<T> Get(IQueryableProvider queryableProvider);
    }

    public interface IQueryResultSingle<out T> where T : class, IDomainEntity
    {
        /// <summary>
        /// Returns a single IDomainEntity from the database using the provided queryable provider.
        /// </summary>
        /// <param name="queryableProvider"></param>
        /// <returns></returns>
        IQueryable<T> Get(IQueryableProvider queryableProvider);

        /// <summary>
        /// Allows  for the use of ExecuteUpdateAsync to run a direct SQL update statement.  This can sometimes boost performance and is compatible with table triggers.
        /// </summary>
        /// <param name="queryableProvider"></param>
        /// <returns></returns>
        Task Update(IQueryableProvider queryableProvider);

        /// <summary>
        /// Allows  for the use of ExecuteDeleteAsync to run a direct SQL update statement.  This can sometimes boost performance and is compatible with table triggers.
        /// </summary>
        /// <param name="queryableProvider"></param>
        /// <returns></returns>
        Task Delete(IQueryableProvider queryableProvider);
    }
}
