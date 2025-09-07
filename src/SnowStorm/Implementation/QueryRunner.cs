using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnowStorm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm
{
    public class QueryRunner(ILogger<QueryRunner> logger, DataContext dbContext, IQueryableProvider queryableProvider, IMapper mapper)
    {
        private readonly ILogger<QueryRunner> _logger = logger;
        //private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly DataContext _dbContext = dbContext;
        private readonly IQueryableProvider _queryableProvider = queryableProvider;
        private readonly IMapper _mapper = mapper;

        //private DataContext CreateDbContext()
        //{
        //    return _serviceProvider.GetRequiredService<DataContext>();
        //}

        public async Task<T> Get<T>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                var result = await query.Get(_queryableProvider).FirstOrDefaultAsync();
                if (!defaultIfMissing && result == null)
                    throw new NullReferenceException($"'{typeof(T).Name}': Status404 - NotFound");

                return result;
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Get<T>(IQueryResultSingle<T>...) with sorting : {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }

        public async Task<TDto> Get<T, TDto>(IQueryResultSingle<T> query, bool defaultIfMissing = true) where T : class, IDomainEntity
        {
            try
            {
                var result = await query.Get(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                if (!defaultIfMissing && result == null)
                    throw new NullReferenceException($"'{typeof(T).Name}': Status404 - NotFound");

                return result;
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Get<T, TDto>(IQueryResultSingle<T>...) : {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }

        public async Task<List<T>> Get<T>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            //using var dbContext = CreateDbContext();
            try
            {
                return await query.Get(_queryableProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: QueryRunner.Get<T>(IQueryResultList<T>...) : {ex.Message} ");
                throw;
            }
        }

        public async Task<List<TDto>> Get<T, TDto>(IQueryResultList<T> query) where T : class, IDomainEntity
        {
            try
            {
                var queryable = await query.Get(_queryableProvider).ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
                return queryable;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error: QueryRunner.Get<T,TDto>(IQueryResultList<T>...) : {ex.Message} ");
                throw;
            }
        }


        public async Task<T> GetById<T>(long id) where T : DomainEntityWithId
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            //using var _dbContext = CreateDbContext();
            try
            {
                stopwatch.Start();
                var result = await _dbContext.Set<T>().OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"QueryRunner.GetById<t>(long) query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
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

        public async Task<T> GetById<T>(string id) where T : DomainEntityWithIdString
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            //using var _dbContext = CreateDbContext();
            try
            {
                stopwatch.Start();
                var result = await _dbContext.Set<T>().OrderByDescending(o => o.Id).FirstOrDefaultAsync(w => w.Id == id);
                stopwatch.Stop();
                _logger?.LogDebug(message: $"QueryRunner.GetById<t>(string) query ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
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

        public async Task<T> Add<T>(T entity, bool autoSave = true) where T : class, IDomainEntity
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(entity);
                if (autoSave)
                    _dbContext.SaveChanges(); // synchronous save to avoid concurrency issues

                return entity;
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Add() : {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }

        public async Task Update<T>(IQueryResultSingle<T> query) where T : class, IDomainEntity
        {
            try
            {
                await query.Update(_queryableProvider);
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Update<T>(IQueryResult<T>...) [DbSet<t>().Where().ExecuteAsync()]: {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }

        public async Task Delete<T>(IQueryResultSingle<T> query) where T : class, IDomainEntity
        {
            try
            {
                await query.Delete(_queryableProvider);
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Update<T>(IQueryResult<T>...) [DbSet<t>().Where().ExecuteAsync()]: {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = $"Error: QueryRunner.Save() : {ex.Message} ";
                _logger?.LogError(ex, message);
                throw;
            }
        }
    }
}
