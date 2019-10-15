using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SnowStorm.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.QueryExecutors
{
    internal class MappingBuilder<TDto> : IMappedQueryExecutor<TDto>
    {
        private readonly DbContext _dbContext;
        private readonly IQueryableProvider _queryableProvider;
        private readonly IMapper _mapper;


        public MappingBuilder(DbContext dbContext, IQueryableProvider queryableProvider, IMapper mapper) 
        {
            _dbContext = dbContext;
            _queryableProvider = queryableProvider;
            _mapper = mapper;
        }

        public Task<List<TDto>> ExecuteAsync<T, TKeyBy>(IMappableQuery<T> query, Expression<Func<TDto, TKeyBy>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where T : class, IDomainEntity
        {
            return QueryExecutor.ExecuteAsync(() =>
            {
                var queryable = query.Execute(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider);
                var orderedQueryable = sortOrder == SortOrder.Descending ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
                return orderedQueryable.ToListAsync();

            }, _dbContext, query);
        }

        public Task<TDto> ExecuteAsync<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return QueryExecutor.ExecuteAsync(async () =>
                {
                    var result = await query.Execute(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                    if (!defaultIfMissing && result == null)
                        throw new DomainException($"Error executing projectable single item query over '{typeof(T).Name}' (with no default if missing): no results returned");

                    return result;

                }, _dbContext, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // TODO: Implement GetByIdAsync
        public async Task<TDto> GetByIdAsync<T>(int id) where T : class, IDomainEntityWithId
        {
            throw new NotImplementedException();
        }
    }
}
