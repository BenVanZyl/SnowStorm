using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.DataContext
{
    public partial class AppDbContext
    {

        //private ILogger<AppDbContext> _logger = null;

        //public ILogger<AppDbContext> Logger
        //{
        //    get
        //    {
        //        if (_logger == null)
        //        {
        //            var serviceProvider = Container.Instance;
        //            _logger = serviceProvider.GetService<ILogger<AppDbContext>>();
        //        }
        //        return _logger;
        //    }
        //}

        public async Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            try
            {
                await this.Set<T>().AddAsync(domainEntity);
                if (saveChanges)
                    await Save();
                return domainEntity;
            }
            catch (Exception ex)
            {
                //Logger?.LogError(ex, "AppDbContext.Add() Failed: {0}", ex.Message);
                throw new DbUpdateException("Error adding data.", ex);
            }

        }

        public async Task<bool> Delete<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            try
            {
                this.Set<T>().Remove(domainEntity);
                if (saveChanges)
                    await Save();
                return true;
            }
            catch (Exception ex)
            {
                //Logger?.LogError(ex, "AppDbContext.Delete() Failed [{0}]", ex.Message);
                throw new DbUpdateException("Error deleting data.", ex);
            }
        }

        public async Task Save()
        {
            try
            {
                await this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Logger?.LogError(ex, "AppDbContext.Save() Failed [{0}]", ex.Message);
                throw new DbUpdateException("Error saving data.", ex);
            }
        }

        public override int SaveChanges()
        {
            int result = 0;
            var t = Task.Run(async () => result = await this.SaveChangesAsync());
            t.Wait();

            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuditInfo();
            ChangeTracker.DetectChanges();
            int result = await base.SaveChangesAsync();
            return result;
        }

        public virtual void AddAuditInfo()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries().Where(x => x.Entity is DomainEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    ExecuteMethod(entry.Entity, "SetCreatedOn");

                ExecuteMethod(entry.Entity, "SetModifiedOn");
            }
        }

        public virtual async Task<object> Run(string sql)
        {
            var results = await this.Database.ExecuteSqlRawAsync(sql);
            return results;
        }

        public virtual void ExecuteMethod(object objectToUse, string methodName)
        {
            try
            {
                var type = objectToUse.GetType();
                if (type == null)
                    return;

                MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                if (method == null)
                    return;

                method.Invoke(objectToUse, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}

