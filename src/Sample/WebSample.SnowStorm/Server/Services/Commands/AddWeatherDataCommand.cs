using MediatR;
using SnowStorm;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Commands
{
    public class AddWeatherDataCommand : IRequest<bool>
    {
        public List<WeatherDataDto> Data { get; set; }
        public long? ReportId { get; set; }

        public AddWeatherDataCommand(WeatherDataDto data) => Data = new() { data };

        public AddWeatherDataCommand(List<WeatherDataDto> data) => Data = data;

        public AddWeatherDataCommand(WeatherForecast[] data) => 
            Data = data
                .Select(s => new WeatherDataDto() { ForecastDate = s.Date, TemperatureC = s.TemperatureC, Summary = s.Summary })
                .OrderBy(o => o.ForecastDate)
                .ToList();

        public AddWeatherDataCommand  WithReportId(long id)
        {
            ReportId = id;
            return this;
        }

    }

    public class AddWeatherDataCommandHandler : IRequestHandler<AddWeatherDataCommand, bool>
    {
        private readonly QueryRunner _queryRunner;

        public AddWeatherDataCommandHandler(QueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        public async Task<bool> Handle(AddWeatherDataCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || request.Data.Count == 0)
                throw new NullReferenceException("Missing weather data.");

            try
            {
                foreach (var data in request.Data)
                {
                    if (request.ReportId.HasValue)
                        data.ReportId = request.ReportId.Value;

                    _ = await WeatherData.Create(_queryRunner, data); 
                }

            }
            catch (Exception)
            {
                //_queryRunner.ChangeTracker.Clear();
                throw new IOException("Cannot save weather data. Transaction cancelled.");
            }
            
            return true;

        }
    }
}
