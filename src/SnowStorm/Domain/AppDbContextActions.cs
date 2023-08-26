using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnowStorm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Domain
{
    public partial class AppDbContext
    {

        public async Task<T> Add<T>(T domainEntity, bool saveChanges = true) where T : class, IDomainEntity
        {
            try
            {
                this.Set<T>().Add(domainEntity);
                if (saveChanges)
                    await Save();
                return domainEntity;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"QueryExecutor.Add() Failed [{ex.Message}]");
                throw new GenericException("Error adding data.", ex);
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
                Logger?.LogError(ex, $"QueryExecutor.Delete() Failed [{ex.Message}]");
                throw new GenericException("Error deleting data.", ex);
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
                Logger?.LogError(ex, $"QueryExecutor.Save() Failed [{ex.Message}]");
                throw new GenericException("Error saving data.", ex);
            }
        }
    }
}
