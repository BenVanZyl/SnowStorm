using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.Domain
{
    public class AppDbContext : DbContext
    {

        public AuditUserInfo AuditUser { get; set; } = new AuditUserInfo(); //TODO: Get audit user info automaticly

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

        public virtual void AddAuitInfo()
        {
            //TODO: Get User information to log audit info correctly.  Might need to do this from app...

            var entries = ChangeTracker.Entries().Where(x => x.Entity is DomainEntityWithIdWithAudit && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((DomainEntityWithIdWithAudit)entry.Entity).CreateDateTime = DateTime.UtcNow;
                }
                ((DomainEntityWithIdWithAudit)entry.Entity).ModifyDateTime = DateTime.UtcNow;
            }
        }
    }
}
