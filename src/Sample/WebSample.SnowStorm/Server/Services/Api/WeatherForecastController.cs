using Azure;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.DataContext;
using WebSample.SnowStorm.Server.Services.Commands;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Server.Services.Queries;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Api
{
    [ApiController]
    public class WeatherForecastController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _db;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator, AppDbContext db)
        {
            _logger = logger;
            _mediator = mediator;
            _db = db;
        }

        [HttpGet]
        [Route("/api/weather-forecasts/generate")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToDateTime(new TimeOnly(0,0)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }



        [HttpGet]
        [Route("/api/weather-forecasts/reports")]
        public async Task<IEnumerable<WeatherReportDto>> GetReports()
        {
            try
            {
                var results = await _db.Get<WeatherReport, WeatherReportDto>(new GetWeatherReportsQuery());
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetReports ERROR");
                throw new RequestFailedException(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/weather-forecasts/{id:long}")]
        [Route("/api/weather-forecasts/reports/{id:long}/data")]
        public async Task<WeatherReportDto> GetWeatherReport(long id)
        {
            try
            {
                var results = await _db.Get<WeatherReport, WeatherReportDto>(new GetWeatherReportQuery(id));
                if (results == null)
                    throw new Exception("Report not found");

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWeatherReport ERROR");
                throw new RequestFailedException(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/weather-forecasts/{id:long}/data")]
        [Route("/api/weather-forecasts/reports/{id:long}/data")]
        public async Task<IEnumerable<WeatherDataDto>> GetWeatherDataForReport(long id)
        {
            try
            {
                var results = await _db.Get<WeatherData, WeatherDataDto>(new GetWeatherForReportQuery(id));
                if (results == null)
                    throw new Exception("Report not found");

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWeatherDataForReport ERROR");
                throw new RequestFailedException(ex.Message);
            }
        }



        [HttpPost]
        [Route("/api/weather-forecasts/reports")]
        public async Task<long> PostReport([FromBody] WeatherReportDto data)
        {
            try
            {
                _logger.LogDebug($"{data.ReportName}; {data.WeatherData?.Length}; {data.WeatherData};  ");
                var result = await _mediator.Send(new AddWeatherReportCommand(data));
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "PostReport ERROR");
                throw new RequestFailedException(ex.Message);
            }
        }
    }
}
