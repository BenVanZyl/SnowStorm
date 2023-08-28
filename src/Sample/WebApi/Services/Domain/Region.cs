using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnowStorm;
using SnowStorm.Domain;
using WebApi.Shared.Dto.Regions;

namespace WebApi.Services.Domain
{
    public class Region : DomainEntityWithIdInt
    {
        protected Region() { }

        //public int Id { get; private set; }

        public string RegionDescription { get; private set; }

        #region Methods

        internal static async Task<Region> Create(RegionDto data, bool autoSave = true)
        {
            if (data == null)
                throw new ArgumentNullException("Create Failed due to missing data!: Region");
            
            var dataContext = Container.GetAppDbContext();

            var v = new Region(data);
            await dataContext.Add<Region>(v);

            if (autoSave)
                await dataContext.Save();


            return v;
        }

        private Region(RegionDto data)
        {
            Save(data);
        }

        public void Save(RegionDto data)
        {
            SetRegionDescription(data.RegionDescription);

        }

        public void SetRegionDescription(string v)
        {
            if (RegionDescription != v)
                RegionDescription = v;
        }


        #endregion Methods


        #region Configuration
        internal class Mapping : IEntityTypeConfiguration<Region>
        {
            public void Configure(EntityTypeBuilder<Region> builder)
            {
                builder.ToTable("Region", "dbo");
                builder.HasKey(u => u.Id);  // PK.
                builder.Property(p => p.Id).HasColumnName("RegionID");

                builder.Property(p => p.RegionDescription).HasMaxLength(50).IsRequired();

            }
        }
        #endregion //config
    }


}