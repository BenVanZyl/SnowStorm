using Bunit;
using Shouldly;
using WebSample.Test.Infrastructure;

namespace WebSample.Test.IntegrationTests
{
    public class ApiTests : BaseIntegrationTests
    {
        
        [Fact]
        public async Task GetForecastAsync_ReturnsWeatherForecasts_Based_On_Mvc()
        {
            // Arrange

            // Act
            var result = await Client.GetAsync("WeatherForecast");

            // Assert
            result.ShouldNotBeNull();

        }

        [Fact]
        public async Task GetForecastAsync_ReturnsWeatherForecasts_Based_On_Rest()
        {
            // Arrange

            // Act
            var result = await Client.GetAsync("/api/weather-forecasts/generate");

            // Assert
            result.ShouldNotBeNull();

        }
    }
}
