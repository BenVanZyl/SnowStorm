using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.Domain
{
    //public interface IEntityTypeConfiguration<TEntityType> where TEntityType : class
    //{
    //    void Map(EntityTypeBuilder<TEntityType> builder);
    //}

    public class AppDbContext : DbContext
    {
        //public AppCoreDbContext(string connectionstring)
        //{
        //    AppCoreDbContext(o => o.UseSqlServer(connectionString));
        //}

        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Here UseConfiguration is any IEntityTypeConfiguration
            builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly()); // Here UseConfiguration is any IEntityTypeConfiguration
            builder.ApplyConfigurationsFromAssembly(Assembly.GetEntryAssembly()); // Here UseConfiguration is any IEntityTypeConfiguration

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

        private void AddAuitInfo()
        {
            //TODO: implement variation of this sample

            var entries = ChangeTracker.Entries().Where(x => x.Entity is DomainEntityWithIdAudit && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((DomainEntityWithIdAudit)entry.Entity).CreateDateTime = DateTime.UtcNow;
                }
                ((DomainEntityWithIdAudit)entry.Entity).ModifyDateTime = DateTime.UtcNow;
            }
        }

    }
}
