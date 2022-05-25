
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using SnowStorm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SnowStorm.QueryExecutors
{
    internal class CastingBuilder<TDto> : ICastingQueryExecutor<TDto>
    {
        private readonly DbContext _dbContext;
        private readonly IQueryableProvider _queryableProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<QueryExecutor> _logger;

        public CastingBuilder(DbContext dbContext, IQueryableProvider queryableProvider, IMapper mapper, ILogger<QueryExecutor> logger)
        {
            _dbContext = dbContext;
            _queryableProvider = queryableProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<List<TDto>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            try
            {
                return QueryExecutor.Get(async () =>
                {
                    var queryable = query.Get(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider);

                    return await queryable.ToListAsync();

                }, _dbContext, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: CastingBuilder.Get<T>(IQueryResultList<T>...) : {ex.Message} ");
                throw;
            }
        }

        public Task<TDto> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return QueryExecutor.Get(async () =>
                {
                    var result = await query.Get(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                    if (!defaultIfMissing && result == null)
                        throw new GenericException($"'{typeof(T).Name}': Status404 - NotFound");

                    return result;

                }, _dbContext, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: CastingBuilder.Get<T>(IQueryResultSingle<T>...) : {ex.Message} ");
                throw;
            }
        }

        public Task<List<TDto>> Get<T, TKeyBy>(IQueryResultList<T> query, Expression<Func<TDto, TKeyBy>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where T : class, IDomainEntity
        {
            try
            {
                return QueryExecutor.Get(async () =>
                {
                    var queryable = query.Get(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider);
                    if (sortOrder != SortOrder.Unspecified)
                        queryable = sortOrder == SortOrder.Descending ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
                    return await queryable.ToListAsync();

                }, _dbContext, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: CastingBuilder.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ");
                throw;
            }
        }

        public async Task<TDto> GetById<T>(long id) where T : class, IDomainEntityWithId
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var resultDb = await _dbContext.Set<T>().SingleOrDefaultAsync(w => w.Id == id);
                var result = _mapper.Map<TDto>(resultDb); 
                stopwatch.Stop();
                _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
                //null checks to be handle by client
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                string msg = $"Error.  GetForId({id}) : {ex.Message}";
                _logger?.LogError(ex, msg);
                throw;
            }
        }

    }
}
