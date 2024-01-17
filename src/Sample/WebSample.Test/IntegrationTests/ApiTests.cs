using Bunit;
using Shouldly;
using System.Net.Http.Json;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;
using WebSample.Test.Infrastructure;
using static System.Net.WebRequestMethods;

namespace WebSample.Test.IntegrationTests
{
    public class ApiTests : BaseIntegrationTests
    {

        [Fact]
        public async Task GetForecastAsync_ReturnsWeatherForecasts_Based_On_Mvc()
        {
            // Arrange

            // Act
            var result = await Http.GetAsync("WeatherForecast");

            // Assert
            result.ShouldNotBeNull();

        }

        [Fact]
        public async Task GetForecastAsync_ReturnsWeatherForecasts_Based_On_Rest()
        {
            // Arrange

            // Act
            var result = await Http.GetAsync("/api/weather-forecasts/generate");

            // Assert
            result.ShouldNotBeNull();
        }

        // enabling this causes a conflict with the ui test for saving when all run together.
        [Fact(Skip = "This is for reference purposes.")] 
        public async Task SaveWeatherReport_ReturnsTrue()
        {
            // Arrange
            var data = new WeatherReportDto()
            {
                ReportName = Guid.NewGuid().ToString(),
                WeatherData = GetForecastForTesting()
            };

            // Act
            var result = await Http.PostAsJsonAsync("api/weather-forecasts/reports", data);

            // Assert
            if (result == null)
            {
                Console.WriteLine("error");
            }
            result.ShouldNotBeNull();
            result.IsSuccessStatusCode.ShouldBeTrue();  

        }

        #region Helpers.

        private static WeatherForecast[] GetForecastForTesting()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        #endregion
    }
}
