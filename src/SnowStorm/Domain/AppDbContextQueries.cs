using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnowStorm.Exceptions;
using SnowStorm.QueryExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Domain
{
    public partial class AppDbContext
    {

        public Task<T> Get<T>(IQueryResult<T> query) where T : class, IDomainEntity
        {
            return Get(async () => await query.Get(QueryableProvider), this, query);
        }

        public Task<T> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return Get(async () =>
                {
                    try
                    {
                        var result = await query.Get(QueryableProvider).FirstOrDefaultAsync();
                        if (!defaultIfMissing && result == null)
                            throw new ArgumentNullException($"'{typeof(T).Name}': Status404 - NotFound");

                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"Error: AppDbContext.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ");
                        throw;
                    }

                }, this, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: AppDbContext.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ");
                throw;
            }
        }


        public Task<TDto> Get<T, TDto>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return Get(async () =>
                {
                    var result = await query.Get(QueryableProvider).ProjectTo<TDto>(Mapper.ConfigurationProvider).FirstOrDefaultAsync();

                    if (!defaultIfMissing && result == null)
                        throw new GenericException($"'{typeof(T).Name}': Status404 - NotFound");

                    return result;

                }, this, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: CastingBuilder.Get<T>(IQueryResultSingle<T>...) : {ex.Message} ");
                throw;
            }
        }

        public Task<List<T>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            return Get(async () => await query.Get(QueryableProvider).ToListAsync(), this, query);
        }

        public Task<List<TDto>> Get<T, TDto>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            try
            {
                return Get(async () =>
                {
                    var queryable = query.Get(QueryableProvider).ProjectTo<TDto>(Mapper.ConfigurationProvider);

                    return await queryable.ToListAsync();

                }, this, query);
            }
            catch (Exception ex)
            {
                //_logger?.LogError(ex, $"Error: CastingBuilder.Get<T>(IQueryResultList<T>...) : {ex.Message} ");
                throw;
            }
        }

        public async Task<T> Get<T>(Func<Task<T>> GetResult, DbContext dbContext, object query)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();

            if (dbContext == null)
                throw new GenericException("No database connection found.");

            if (query == null)
                throw new GenericException("No query object defined.");

            try
            {
                stopwatch.Start();
                Logger?.LogDebug(message: $"AppDbContext.Get() => {query}");
                var result = await GetResult();
                return result;
            }
            catch (Exception ex)
            {
                string message = $"AppDbContext.Get() failed. [{ex.Message}]";
                Logger?.LogError(exception: ex, message: message);
                throw new GenericException("Error getting data.", ex);
            }
            finally
            {
                if (stopwatch.IsRunning)
                    stopwatch.Stop();
                string message = $"AppDbContext.Get() => {stopwatch.Elapsed.TotalSeconds}";
                Logger?.LogDebug(message: message);

            }
        }

    }
}
