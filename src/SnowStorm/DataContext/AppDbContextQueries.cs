using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using SnowStorm.Exceptions;
using SnowStorm.QueryExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.DataContext
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
                        string message = $"Error: AppDbContext.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ";
                        _logger?.LogError(ex, message);
                        throw;
                    }

                }, this, query);
            }
            catch (Exception ex)
            {
                string message = $"Error: AppDbContext.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ";
                _logger?.LogError(ex, message);
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
                string message = $"Error: CastingBuilder.Get<T>(IQueryResultSingle<T>...) : {ex.Message} ";
                _logger?.LogError(ex, message);
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
                _logger?.LogError(ex, $"Error: Get<T,TDto>(IQueryResultList<T>...) : {ex.Message} ");
                throw;
            }
        }

        public async Task<T> GetById<T>(string id) where T : DomainEntityWithIdString
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var result = await this.Set<T>().OrderByDescending(o => o.Id).SingleOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
                //null checks to be handle by client
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, $"Error.  GetById({id}) : {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetById<T>(int id) where T : DomainEntityWithIdInt
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var result = await this.Set<T>().OrderByDescending(o => o.Id).SingleOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
                //null checks to be handle by client
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, $"Error.  GetById({id}) : {ex.Message}");
                throw;
            }
        }

        public async Task<T> GetById<T>(long id) where T : DomainEntityWithId
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var result = await this.Set<T>().OrderByDescending(o => o.Id).SingleOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
                //null checks to be handle by client
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, $"Error.  GetById({id}) : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Read-only list of the all the rows of the selected table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetAll<T>() where T : class, IDomainEntity
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var result = await this.Set<T>().ToListAsync(); //.OrderByDescending(o => o.Id).SingleOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"GetAll query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
                //null checks to be handle by client
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger?.LogError(ex, $"Error.  GetAll() : {ex.Message}");
                throw;
            } 
        }

        public async Task<T> Get<T>(Func<Task<T>> GetResult, DbContext dbContext, object query)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();

            if (dbContext == null)
                throw new NullReferenceException("No database connection found.");

            if (query == null)
                throw new NullReferenceException("No query object defined.");

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
