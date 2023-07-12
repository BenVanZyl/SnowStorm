using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnowStorm.Domain;
using System;

namespace SnowStorm.Users
{
    public class AspNetUser : IDomainEntity
    {
        protected AspNetUser() { }

        public string Id { get; private set; } //GUID
        public string UserName { get; private set; }
        public string NormalizedUserName { get; private set; }
        public string Email { get; private set; }
        public string NormalizedEmail { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }
        public string ConcurrencyStamp { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool PhoneNumberConfirmed { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        public DateTimeOffset? LockoutEnd { get; private set; }
        public bool LockoutEnabled { get; private set; }
        public int AccessFailedCount { get; private set; }



        #region Methods

        #endregion Methods

        #region Configuration
        internal class Mapping : IEntityTypeConfiguration<AspNetUser>
        {
            public void Configure(EntityTypeBuilder<AspNetUser> builder)
            {
                builder.ToTable("AspNetUsers", "dbo");
                builder.HasKey(u => u.Id);  // PK.
                builder.Property(p => p.Id).HasColumnName("Id");

                builder.Property(p => p.Id).HasMaxLength(450).IsRequired();
                builder.Property(p => p.UserName).HasMaxLength(256).IsRequired();
                builder.Property(p => p.NormalizedUserName).HasMaxLength(256).IsRequired();
                builder.Property(p => p.Email).HasMaxLength(256).IsRequired();
                builder.Property(p => p.NormalizedEmail).HasMaxLength(256).IsRequired();
                builder.Property(p => p.PasswordHash).IsRequired();
                builder.Property(p => p.SecurityStamp).IsRequired();
                builder.Property(p => p.ConcurrencyStamp).IsRequired();
                builder.Property(p => p.PhoneNumber).IsRequired();

            }
        }
        #endregion //config
    }


}
