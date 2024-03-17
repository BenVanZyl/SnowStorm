using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnowStorm;
using SnowStorm.Domain;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Domain
{
    public class WeatherReport : DomainEntityWithIdWithAudit
    {
        protected WeatherReport() { }

        public string ReportName { get; private set; }


        public List<WeatherData> WeatherData { get; set; }

        #region Methods

        internal static async Task<WeatherReport> Create(WeatherReportDto data, bool autoSave = true)
        {
            if (data == null)
                throw new NullReferenceException("Create Failed due to missing data!: WeatherReport");

            var dataContext = Container.GetAppDbContext();

            var v = new WeatherReport(data);
            await dataContext.Add<WeatherReport>(v, autoSave);
          
            return v;
        }

        internal static async Task<WeatherReport> Create(string reportName, bool autoSave = true)
        {
            var dataContext = Container.GetAppDbContext();

            var v = new WeatherReport();
            v.SetReportName(reportName);
            await dataContext.Add<WeatherReport>(v, autoSave);

            return v;
        }
        
        private WeatherReport(WeatherReportDto data)
        {
            Save(data);
        }

        public void Save(WeatherReportDto data)
        {
            SetReportName(data.ReportName);
        }

        public void SetReportName(string v)
        {
            if (ReportName != v)
                ReportName = v;
        }

        #endregion Methods


        #region Configuration
        internal class Mapping : IEntityTypeConfiguration<WeatherReport>
        {
            public void Configure(EntityTypeBuilder<WeatherReport> builder)
            {
                builder.ToTable("WeatherReport", "dbo");
                builder.HasKey(u => u.Id);  // PK.
                builder.Property(p => p.Id).HasColumnName("Id");

                builder.Property(p => p.ReportName).HasMaxLength(50).IsRequired();
                builder.Property(p => p.CreatedOn).IsRequired();
                builder.Property(p => p.ModifiedOn).IsRequired();

            }
        }
        #endregion //config
    }


}
