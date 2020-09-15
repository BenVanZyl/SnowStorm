using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SnowStorm.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<T> Execute<T>(INonMappableQuery<T> query)
        {
            return Execute(() => query.Execute(_queryableProvider), _dbContext, query);
        }

        public Task<List<T>> Execute<T>(IMappableQuery<T> query) where T : class, IDomainEntity
        {
            return Execute(() => query.Execute(_queryableProvider).ToListAsync(), _dbContext, query);
        }

        public Task<T> Execute<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            return Execute(async () =>
            {
                var result = await query.Execute(_queryableProvider).FirstOrDefaultAsync();
                if (!defaultIfMissing && result == null)
                    throw new Exception($"Error executing projectable single item query over '{typeof(T).Name}' (with no default if missing): no results returned");

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

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        internal static async Task<T> Execute<T>(Func<Task<T>> getResult, DbContext dbContext, object query)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            //var stringBuilder = new StringBuilder();
            try
            {
                stopwatch.Start(); //TODO: log query time events...
                var result = await getResult();
                stopwatch.Stop();
                return result;
            }
            catch (Exception ex)
            {
                //TODO: log errors
                stopwatch.Stop();
                throw ex;
            }
        }

    }
}
