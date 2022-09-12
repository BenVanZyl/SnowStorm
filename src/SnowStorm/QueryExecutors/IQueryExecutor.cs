using Microsoft.Data.SqlClient;
using SnowStorm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SnowStorm.QueryExecutors
{
    public interface IQueryExecutor : IAsyncDisposable
    {
        Task<T> Get<T>(IQueryResult<T> query);
        Task<List<T>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity;
        Task<T> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;

        //Task<T> GetById<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId;
        Task<T> GetById<T>(long id) where T : class, IDomainEntityWithId;

        //Task<List<T>> GetAll<T>(Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId;
        //Task<List<T>> GetAll<T>() where T : class, IDomainEntityWithId;

        ICastingQueryExecutor<TDto> CastTo<TDto>();

        Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task Save();
    }

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

    public interface ICastingQueryExecutor<TDto>
    {
        Task<List<TDto>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity;
        Task<TDto> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;
        Task<List<TDto>> Get<T, TKeyBy>(IQueryResultList<T> query, Expression<Func<TDto, TKeyBy>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where T : class, IDomainEntity;

        Task<TDto> GetById<T>(long id) where T : class, IDomainEntityWithId;
        //Task<TDto> GetById<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId;
        //Task<List<TDto>> GetAll<T>(Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntity;
    }

}
