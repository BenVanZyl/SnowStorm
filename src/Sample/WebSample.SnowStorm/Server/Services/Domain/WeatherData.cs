using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SnowStorm.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using WebSample.SnowStorm.Shared.Dtos;
using SnowStorm;

namespace WebSample.SnowStorm.Server.Services.Domain
{
    public class WeatherData : DomainEntityWithIdWithAudit
    {
        protected WeatherData() { }

        public long ReportId { get; private set; }
        public DateTime ForecastDate { get; private set; }
        public int TemperatureC { get; private set; }
        public string? Summary { get; private set; }

        [ForeignKey("ReportId")]
        public WeatherReport WeatherReport { get; private set; }


        #region Methods

        internal static async Task<WeatherData> Create(WeatherDataDto data, bool autoSave = true)
        {
            if (data == null)
                throw new NullReferenceException("Create Failed due to missing data!: WeatherData");

            var dataContext = Container.GetAppDbContext();

            var v = new WeatherData(data);
            await dataContext.Add<WeatherData>(v, autoSave);

            return v;
        }

        private WeatherData(WeatherDataDto data)
        {
            Save(data);
        }

        public void Save(WeatherDataDto data)
        {
            if (data == null)
                throw new NullReferenceException("Missing data: WeatherData");

            if (!data.ReportId.HasValue)
                throw new NullReferenceException("Missing data: WeatherData");

            SetReportId(data.ReportId.Value);
            SetForecastDate(data.ForecastDate.ToDateTime(new TimeOnly(0, 0)));
            SetTemperatureC(data.TemperatureC);
            SetSummary(data.Summary);

        }

        public void SetReportId(long v)
        {
            if (ReportId != v)
                ReportId = v;
        }

        public void SetForecastDate(DateTime v)
        {
            if (ForecastDate != v)
                ForecastDate = v;
        }

        public void SetTemperatureC(int v)
        {
            if (TemperatureC != v)
                TemperatureC = v;
        }

        public void SetSummary(string? v)
        {
            if (Summary != v)
                Summary = v;
        }

        #endregion Methods

        #region Configuration
        internal class Mapping : IEntityTypeConfiguration<WeatherData>
        {
            public void Configure(EntityTypeBuilder<WeatherData> builder)
            {
                builder.ToTable("WeatherData", "dbo");
                builder.HasKey(u => u.Id);  // PK.
                builder.Property(p => p.Id).HasColumnName("Id");

                builder.Property(p => p.ForecastDate).IsRequired();
                builder.Property(p => p.Summary).HasMaxLength(255).IsRequired();
                builder.Property(p => p.CreatedOn).IsRequired();
                builder.Property(p => p.ModifiedOn).IsRequired();

            }
        }
        #endregion //config
    }


}
