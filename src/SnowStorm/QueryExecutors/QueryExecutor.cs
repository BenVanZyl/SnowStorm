using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using SnowStorm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.QueryExecutors
{
    public class QueryExecutor : IQueryExecutor
    {
        public AppDbContext DbContext { get; set; }
        public IQueryableProvider QueryableProvider { get; set; }
        public IMapper Mapper { get; set; }

        private readonly ILogger<QueryExecutor> _logger;

        public QueryExecutor(AppDbContext dbContext, IQueryableProvider queryableProvider, IMapper mapper, ILogger<QueryExecutor> logger)
        {
            DbContext = dbContext;
            QueryableProvider = queryableProvider;
            Mapper = mapper;
            _logger = logger;
        }

        public Task<T> Get<T>(IQueryResult<T> query)
        {
            return Get(() => query.Get(QueryableProvider), DbContext, query);
        }

        public Task<List<T>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            return Get(() => query.Get(QueryableProvider).ToListAsync(), DbContext, query);
        }

        public Task<T> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                return Get(async () =>
                {
                    var result = await query.Get(QueryableProvider).FirstOrDefaultAsync();
                    if (!defaultIfMissing && result == null)
                        throw new ArgumentNullException($"'{typeof(T).Name}': Status404 - NotFound");

                    return result;
                }, DbContext, query);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: QueryExecutor.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ");
                throw;
            }
        }

        //public async Task<T> GetById<T>(long id, Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId
        //{
        //    throw new NotImplementedException();

        //    //var stopwatch = new System.Diagnostics.Stopwatch();
        //    //try
        //    //{
        //    //    stopwatch.Start();
        //    //    var result = await includes(DbContext.Set<T>()).SingleOrDefaultAsync(w => w.Id == id);
        //    //    stopwatch.Stop();
        //    //    _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
        //    //    //null checks to be handle by client
        //    //    return result;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    stopwatch.Stop();
        //    //    _logger?.LogError(ex, $"Error.  GetById({id}) : {ex.Message}");
        //    //    throw;
        //    //}
        //}

        //public async Task<T> GetById<T>(long id) where T : class, IDomainEntityWithId
        //{
        //    var stopwatch = new System.Diagnostics.Stopwatch();
        //    try
        //    {
        //        stopwatch.Start();
        //        var result = await DbContext.Set<T>().SingleOrDefaultAsync(w => w.Id == id);
        //        stopwatch.Stop();
        //        _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
        //        //null checks to be handle by client
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        stopwatch.Stop();
        //        _logger?.LogError(ex, $"Error.  GetById({id}) : {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<List<T>> GetAll<T>(Func<IQueryable<T>, IQueryable<T>> includes) where T : class, IDomainEntityWithId
        //{
        //    throw new NotImplementedException();

        //    //var stopwatch = new System.Diagnostics.Stopwatch();
        //    //try
        //    //{
        //    //    stopwatch.Start();
        //    //    var result = await includes(DbContext.Set<T>()).ToList();
        //    //    stopwatch.Stop();
        //    //    _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
        //    //    //null checks to be handle by client
        //    //    return result;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    stopwatch.Stop();
        //    //    _logger?.LogError(ex, $"Error.  GetAll() : {ex.Message}");
        //    //    throw;
        //    //}
        //}

        //public async Task<List<T>> GetAll<T>() where T : class, IDomainEntityWithId
        //{
        //    var stopwatch = new System.Diagnostics.Stopwatch();
        //    try
        //    {
        //        stopwatch.Start();
        //        var result = await DbContext.Set<T>().ToListAsync();
        //        stopwatch.Stop();
        //        _logger?.LogDebug(message: $"GetById query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
        //        //null checks to be handle by client
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        stopwatch.Stop();
        //        _logger?.LogError(ex, $"Error.  GetAll() : {ex.Message}");
        //        throw;
        //    }
        //}

        public ICastingQueryExecutor<TDto> CastTo<TDto>()
        {
            return new CastingBuilder<TDto>(DbContext, QueryableProvider, Mapper, _logger);
        }

        public async Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            try
            {
                DbContext.Set<T>().Add(domainEntity);
                if (saveChanges)
                    await Save();
                return domainEntity;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"QueryExecutor.Add() Failed [{ex.Message}]");
                throw new GenericException("Error adding data.");
            }
            
        }

        public async Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            try
            {
                DbContext.Set<T>().Remove(domainEntity);
                if (saveChanges)
                    await Save();
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"QueryExecutor.Delete() Failed [{ex.Message}]");
                throw new GenericException("Error deleting data.");
            }
        }

        public async Task Save()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"QueryExecutor.Save() Failed [{ex.Message}]");
                throw new GenericException("Error saving data.");
            }
        }

        internal static async Task<T> Get<T>(Func<Task<T>> getResult, DbContext dbContext, object query, ILogger<QueryExecutor> _logger = null)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();

            if (dbContext == null)
                throw new GenericException("No database connection found.");

            if (query == null)
                throw new GenericException("No query object defined.");

            try
            {
                stopwatch.Start(); 
                _logger?.LogDebug(message: $"QueryExecutor.Get() => {query}");
                var result = await getResult();
                return result;
            }
            catch (Exception ex)
            {
                string message = $"QueryExecutor.Get() failed. [{ex.Message}]";
                _logger?.LogError(exception: ex, message: message);
                throw;
            }
            finally
            {
                if (stopwatch.IsRunning)
                    stopwatch.Stop();
                string message = $"QueryExecutor.Get() => {stopwatch.Elapsed.TotalSeconds}";
                _logger?.LogDebug(message: message);
                
            }
        }

    }
}
