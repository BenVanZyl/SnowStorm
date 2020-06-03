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
        Task<T> ExecuteAsync<T>(INonMappableQuery<T> query);
        Task<List<T>> ExecuteAsync<T>(IMappableQuery<T> query) where T : class, IDomainEntity;
        Task<T> ExecuteAsync<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;

        IMappedQueryExecutor<TDto> WithMapping<TDto>();

        Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity;
        Task Save();
    }

    public interface INonMappableQuery<T>
    {
        Task<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface IMappableQuery<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface IMappableSingleItemQuery<out T> where T : class, IDomainEntity
    {
        IQueryable<T> Execute(IQueryableProvider queryableProvider);
    }

    public interface IMappedQueryExecutor<TDto>
    {
        Task<List<TDto>> Execute<T, TKeyBy>(IMappableQuery<T> query, Expression<Func<TDto, TKeyBy>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where T : class, IDomainEntity;

        Task<TDto> Execute<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity;

        Task<TDto> GetById<T>(long id) where T : class, IDomainEntityWithId;
    }


}
