using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SnowStorm
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public static Assembly AppAssembly { get; set; } = default!;

        /// <summary>
        /// Get the underlying connection string for this DB Context
        /// </summary>
        public string ConnectionString => Database != null ? Database.GetConnectionString() : "not provided!";

        /// <summary>
        /// Get the underlying connection for this DB Context
        /// </summary>
        public DbConnection Connection => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(AppAssembly);

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetEntryAssembly());
            
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            ChangeTracker.DetectChanges();
            int result = base.SaveChanges();
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
