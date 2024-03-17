using Bunit;
using Shouldly;
using System.Net.Http.Json;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;
using WebSample.Test.Infrastructure;
using WebSample.Tests.Infrastructure;
using static System.Net.WebRequestMethods;

namespace WebSample.Test.IntegrationTests
{
    public class ApiTests : BaseIntegrationTests
    {

        public ApiTests()
        {
        }


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
        [Fact] 
        public async Task SaveWeatherReport_ReturnsSuccess()
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

            var id = await result.Content.ReadFromJsonAsync<long>();

            //get the record from the backend and verify that the data is there.
            var report = await Http.GetFromJsonAsync<WeatherReportDto>($"api/weather-forecasts/{id}");
            report.ShouldNotBeNull();
            report.Id.ShouldBe(id);
            report.ReportName.ShouldBe(data.ReportName);

            //get weather data for this report
            var weatherData = await Http.GetFromJsonAsync<List<WeatherDataDto>>($"api/weather-forecasts/{id}/data");
            weatherData.ShouldNotBeNull();
            weatherData.Count.ShouldBe(data.WeatherData.Count());
            foreach (var item in data.WeatherData)
            {
                weatherData.Exists(w => w.ReportId == id && w.ForecastDate == item.Date && w.TemperatureC == item.TemperatureC).ShouldBeTrue();
            }
        }

        #region Helpers.

        private static WeatherForecast[] GetForecastForTesting()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToDateTime(new TimeOnly(0,0)),
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
