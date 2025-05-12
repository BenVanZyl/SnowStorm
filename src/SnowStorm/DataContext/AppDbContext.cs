using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnowStorm.Domain;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SnowStorm.DataContext
{
    public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

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

    }
}

