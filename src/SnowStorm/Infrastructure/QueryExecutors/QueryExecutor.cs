using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnowStorm.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.QueryExecutors
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly AppDbContext _dbContext;
        private readonly IQueryableProvider _queryableProvider;
        private readonly IMapper _mapper;

        public QueryExecutor(AppDbContext dbContext, IQueryableProvider queryableProvider, IMapper mapper) 
        {
            _dbContext = dbContext;
            _queryableProvider = queryableProvider;
            _mapper = mapper;
        }       

        public Task<T> ExecuteAsync<T>(INonMappableQuery<T> query)
        {
            return ExecuteAsync(() => query.Execute(_queryableProvider), _dbContext, query);
        }

        public Task<List<T>> ExecuteAsync<T>(IMappableQuery<T> query) where T : class, IDomainEntity
        {
            return ExecuteAsync(() => query.Execute(_queryableProvider).ToListAsync(), _dbContext, query);
        }

        public Task<T> ExecuteAsync<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            return ExecuteAsync(async () =>
            {
                var result = await query.Execute(_queryableProvider).FirstOrDefaultAsync();
                if (!defaultIfMissing && result == null)
                    throw new DomainException($"Error executing projectable single item query over '{typeof(T).Name}' (with no default if missing): no results returned");

                return result;
            }, _dbContext, query);
        }

        public IMappedQueryExecutor<TDto> WithMapping<TDto>()
        {
            return new MappingBuilder<TDto>(_dbContext, _queryableProvider, _mapper);
        }
        
        public async Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            _dbContext.Set<T>().Add(domainEntity);
            if (saveChanges)
                await Save();
            return domainEntity;
        }

        public async Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            _dbContext.Set<T>().Remove(domainEntity);
            if (saveChanges)
                await Save();
            return true;
        }

        public Task Save()
        {
            return _dbContext.SaveChangesAsync();
        }                

        internal static async Task<T> ExecuteAsync<T>(Func<Task<T>> getResult, DbContext dbContext, object query)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            var stringBuilder = new StringBuilder();
            try
            {
                stopwatch.Start();
                var result = await getResult();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
