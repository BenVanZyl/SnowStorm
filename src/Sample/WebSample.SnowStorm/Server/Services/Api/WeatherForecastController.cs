using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebSample.SnowStorm.Server.Services.Commands;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Api
{
    [ApiController]
    public class WeatherForecastController 
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("/api/weather-forecasts/generate")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("/api/weather-forecasts/{id:long}")]
        [Route("/api/weather-forecasts/reports/{id:long}")]
        public async Task<IEnumerable<WeatherForecast>> GetFromId(long id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/api/weather-forecasts/reports")]
        public Task<IEnumerable<WeatherForecast>> GetReports()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("/api/weather-forecasts/reports")]
        public async Task<bool> PostReport([FromBody] WeatherReportDto data)
        {
            try
            {
                var result = await _mediator.Send(new AddWeatherReportCommand(data));
                return result;
            }
            catch (System.Exception ex)
            {
                //Log.Error(ex, "PostReport ERROR");
                throw new RequestFailedException(ex.Message);
            }
        }
    }
}
