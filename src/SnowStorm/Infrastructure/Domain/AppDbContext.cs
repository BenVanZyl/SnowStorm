using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SnowStorm.Infrastructure.Domain
{
    public class AppDbContext : DbContext
    {
        public AuditUserInfo AuditUser { get; set; } = new AuditUserInfo(); //TODO: Get audit user info automaticly

        private AzureServiceTokenProvider _azureServiceTokenProvider = null;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public AppDbContext(DbContextOptions<AppDbContext> options, AzureServiceTokenProvider azureServiceTokenProvider)
           : base(options)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_azureServiceTokenProvider == null)
                return; //nothing to do

            var ext = optionsBuilder.Options.GetExtension<Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal.SqlServerOptionsExtension>();
            if (ext == null)
                return; // not found, nothing to do;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ext.ConnectionString;
            connection.AccessToken = _azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/").Result;

            // update connection in IOC container to include AccessToken
            optionsBuilder.UseSqlServer(connection);
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
                    ((DomainEntityWithIdWithAudit)entry.Entity).CreatedOn = DateTime.UtcNow;
                }
                ((DomainEntityWithIdWithAudit)entry.Entity).ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}
