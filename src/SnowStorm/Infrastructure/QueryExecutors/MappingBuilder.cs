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

        public Task<List<TDto>> Execute<T, TKeyBy>(IMappableQuery<T> query, Expression<Func<TDto, TKeyBy>> orderBy, SortOrder sortOrder = SortOrder.Ascending) where T : class, IDomainEntity
        {
            return QueryExecutor.Execute(() =>
            {
                var queryable = query.Execute(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider);
                var orderedQueryable = sortOrder == SortOrder.Descending ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
                return orderedQueryable.ToListAsync();

            }, _dbContext, query);
        }

        public Task<TDto> Execute<T>(IMappableSingleItemQuery<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return QueryExecutor.Execute(async () =>
                {
                    var result = await query.Execute(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                    if (!defaultIfMissing && result == null)
                        throw new Exception($"Error executing projectable single item query over '{typeof(T).Name}' (with no default if missing): no results returned");

                    return result;

                }, _dbContext, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<T> GetById<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId
        //{
        //    var stopwatch = new System.Diagnostics.Stopwatch();
        //    try
        //    {
        //        stopwatch.Start();
        //        var result = await includes(_dbContext.Set<T>()).SingleOrDefaultAsync(w => w.Id == id);
        //        stopwatch.Stop(); //TODO: Log query time
        //        //null checks to be handle by client
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {   //TODO: Log error occurred
        //        stopwatch.Stop();
        //        throw ex;
        //    }
        //}

        //Task<TDto> IMappedQueryExecutor<TDto>.GetById<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
