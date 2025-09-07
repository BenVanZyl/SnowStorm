using Bunit;
using Shouldly;
using System.Net.Http.Json;
using WebSample.SnowStorm.Shared;
using WebSample.SnowStorm.Shared.Dtos;
using WebSample.Test.Infrastructure;

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
            if (result == null || !result.IsSuccessStatusCode)
            {
                Console.WriteLine("error");
            }
            result.ShouldNotBeNull();
            result.IsSuccessStatusCode.ShouldBeTrue();

            var id = await result.Content.ReadFromJsonAsync<long>();

            //get the record from the back end and verify that the data is there.
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

        [Fact]
        public async Task PerformanceTestingSync()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i < 1000; i++)
            {
                await SaveWeatherReport_ReturnsSuccess();
            }
            stopwatch.Stop();
            Console.WriteLine($"PerformanceTestingSync() ran for {stopwatch.Elapsed.TotalSeconds} seconds.");
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 30)]
        [InlineData(1000, 60, 4)]
        [InlineData(10000, 300, 4)]
        public async Task LoadTestingAsync(int iterations, double maxSeconds, int maxParallelism = 2)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ParallelOptions options = new() { MaxDegreeOfParallelism = maxParallelism };
            await Parallel.ForAsync(1, iterations, options, async (i, ct) =>
            {
                await SaveWeatherReport_ReturnsSuccess();
            });
            stopwatch.Stop();
            double totalSeconds = stopwatch.Elapsed.TotalSeconds;
            totalSeconds.ShouldBeLessThan(maxSeconds);
            
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
