using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SnowStorm.Domain;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.DataContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Static helper property to assist with Unit and Integration Testing were the Domain classes is in a different assembly...ll
        /// </summary>
        public static Assembly AppAssembly { get; set; }

        /// <summary>
        /// Get the underlaying connection string for this DB Context
        /// </summary>
        public string ConnectionString => this.Database.GetConnectionString();

        /// <summary>
        /// Get the underlaying connection for this DB Context
        /// </summary>
        public DbConnection Connection => this.Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (AppAssembly == null)
                throw new InvalidOperationException($"SnowStorm.Domain.AppDbContext(...) : Missing appAssembly.");

            modelBuilder.ApplyConfigurationsFromAssembly(AppAssembly);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
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

