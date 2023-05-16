using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using SnowStorm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.QueryExecutors
{
    [Obsolete]
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
            return Get(async () => await query.Get(QueryableProvider), DbContext, query);
        }

        public Task<List<T>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            return Get(async () => await query.Get(QueryableProvider).ToListAsync(), DbContext, query);
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
                        _logger?.LogError(ex, $"Error: QueryExecutor.Get<T>(IQueryResultList<T>...) with sorting : {ex.Message} ");
                        throw;
                    }
                    
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

        public async Task<T> GetById<T>(long id) where T : class, IDomainEntityWithId
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            try
            {
                stopwatch.Start();
                var result = await DbContext.Set<T>().OrderByDescending(o => o.Id).SingleOrDefaultAsync(w => w.Id == id);
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
            return await DbContext.Add<T>(domainEntity, saveChanges);
        }

        public async Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            return await DbContext.Delete<T>(domainEntity, saveChanges);
        }

        public async Task Save()
        {
            await DbContext.Save();
        }

        public static async Task<T> Get<T>(Func<Task<T>> GetResult, DbContext dbContext, object query, ILogger<AppDbContext> _logger = null)
        {
            return await ((AppDbContext)dbContext).Get(GetResult, dbContext, query, _logger);

            //var stopwatch = new System.Diagnostics.Stopwatch();

            //if (dbContext == null)
            //    throw new GenericException("No database connection found.");

            //if (query == null)
            //    throw new GenericException("No query object defined.");

            //try
            //{
            //    stopwatch.Start();
            //    _logger?.LogDebug(message: $"QueryExecutor.Get() => {query}");
            //    var result = await GetResult();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"QueryExecutor.Get() failed. [{ex.Message}]";
            //    _logger?.LogError(exception: ex, message: message);
            //    throw new GenericException("Error getting data.", ex);
            //}
            //finally
            //{
            //    if (stopwatch.IsRunning)
            //        stopwatch.Stop();
            //    string message = $"QueryExecutor.Get() => {stopwatch.Elapsed.TotalSeconds}";
            //    _logger?.LogDebug(message: message);

            //}
        }

        public async Task DisposeAsync(bool disposing)
        {
            if (!disposing)
                return;

            if (DbContext != null && DbContext.Connection != null && DbContext.Connection.State != System.Data.ConnectionState.Closed)
                await DbContext.Connection.CloseAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
        }
    }
}
