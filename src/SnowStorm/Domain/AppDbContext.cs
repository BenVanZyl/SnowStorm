using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        /// <summary>
        /// Static helper property to assist with Unit and Integration Testingor were the Domain classes is in a different assembly...ll
        /// </summary>
        public static string ExternalAssemblyName { get; set; } = "";

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

            // Here UseConfiguration is any IEntityTypeConfiguration

            //snowstorm (this package)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //ef core
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());

            // (web)app or test host.  Note that test host will not have domain classes, must be supplied using DaminAssemblyName property.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetEntryAssembly());       //

            if (!string.IsNullOrWhiteSpace(ExternalAssemblyName))
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(ExternalAssemblyName));
        }


        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        public virtual void AddAuitInfo()
        {
            //TODO: Get User information to log audit info correctly.  Might need to do this from app...

            var entries = ChangeTracker.Entries().Where(x => x.Entity is DomainEntityWithIdWithAudit && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((DomainEntityWithIdWithAudit)entry.Entity).CreatedOn = DateTime.Now;
                }
                ((DomainEntityWithIdWithAudit)entry.Entity).ModifiedOn = DateTime.Now;
            }
        }

        public async Task<object> Run(string sql)
        {
            var results = await this.Database.ExecuteSqlRawAsync(sql);
            return results;
        }
    }
}
