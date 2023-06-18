using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.Domain
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Static helper property to assist with Unit and Integration Testing were the Domain classes is in a different assembly...ll
        /// </summary>
        public static Assembly? AppAssembly { get; set; } 

        /// <summary>
        /// Get the underlaying connectionstring for this DB Context
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
            //TODO: Get User information to log audit info correctly.  Might need to do this from app...
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries().Where(x => x.Entity is DomainEntityWithIdWithAudit && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((DomainEntityWithIdWithAudit)entry.Entity).SetCreatedOn();
                }
                ((DomainEntityWithIdWithAudit)entry.Entity).SetModifiedOn();
            }
        }

        public async Task<object> Run(string sql)
        {
            var results = await this.Database.ExecuteSqlRawAsync(sql);
            return results;
        }
    }
}
