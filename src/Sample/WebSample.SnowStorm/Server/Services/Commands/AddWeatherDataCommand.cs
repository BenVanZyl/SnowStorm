using MediatR;
using SnowStorm.DataContext;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Server.Services.Queries;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Commands
{
    public class AddWeatherDataCommand : IRequest<bool>
    {
        public List<WeatherDataDto> Data { get; set; }

        public AddWeatherDataCommand(WeatherDataDto data) => Data = new() { data };

        public AddWeatherDataCommand(List<WeatherDataDto> data) => Data = data;

        public AddWeatherDataCommand(WeatherForecast[] data) => 
            Data = data
                .Select(s => new WeatherDataDto() { ForecastDate = s.Date, TemperatureC = s.TemperatureC, Summary = s.Summary })
                .OrderBy(o => o.ForecastDate)
                .ToList();
    }

    public class AddWeatherDataCommandHandler : IRequestHandler<AddWeatherDataCommand, bool>
    {
        private readonly AppDbContext _dataContext;

        public AddWeatherDataCommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Handle(AddWeatherDataCommand request, CancellationToken cancellationToken)
        {
            if (request.Data == null || request.Data.Count == 0)
                throw new NullReferenceException("Missing weather data.");

            try
            {
                foreach (var data in request.Data)
                {
                    _ = await WeatherData.Create(data, false);
                }

                await _dataContext.Save();

            }
            catch (Exception)
            {
                _dataContext.ChangeTracker.Clear();
                throw new IOException("Cannot save weather data. Transaction cancelled.");
            }
            
            return true;

        }
    }
}
