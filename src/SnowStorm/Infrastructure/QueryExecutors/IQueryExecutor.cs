using SnowStorm.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.QueryExecutors
{
    public interface IQueryExecutor
    {
        Task<T> Execute<T>(IQueryResult<T> query);
        Task<List<T>> Execute<T>(IQueryResultList<T> query) where T : class, IDomainEntity;
        Task<T> Execute<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;

        //Task<T> GetForId<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId;

        ICastingQueryExecutor<TDto> CastTo<TDto>();

        Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task Save();
    }

    public interface IQueryResult<T>
    {
        Task<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface IQueryResultList<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface IQueryResultSingle<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface ICastingQueryExecutor<TDto>
    {
        Task<List<TDto>> Execute<T>(IQueryResultList<T> query) where T : class, IDomainEntity;
        Task<TDto> Execute<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;

        //Task<TDto> GetForId<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId;
    }

}
