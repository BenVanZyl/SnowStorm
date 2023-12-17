using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using WebSample.SnowStorm.Client.Pages;
using WebSample.Test.Infrastructure;

namespace WebSample.Test.IntegrationTests
{
    public class UiWithMockTests :BaseIntegrationTests
    {
        public const string appJson = "application/json";
        public const string baseAdr = "https://localhost/";

        public UiWithMockTests()
        {     
            InitApi();
        }

        [Fact]
        public void CounterShouldIncrementWhenClicked()
        {
            // Arrange: render the Counter.razor component
            //InitApi(1);

            var cut = RenderComponent<Counter>();
            // Wait for the component to finish rendering
            cut.WaitForState(() => cut.FindAll("p").Count > 0);

            // Act: find and click the <button> element to increment
            // the counter in the <p> element
            cut.Find("button").Click();

            // Assert: first find the <p> element, then verify its content
            cut.Find("p").MarkupMatches("<p role=\"status\">Current count: 1</p>");

        }

        [Fact]
        public void ShowWeatherForecastOnPageLoad()
        {
            // Arrange: render the Counter.razor component
            //InitApi(2);

            // Act: page load occurs automatically
            var cut = RenderComponent<FetchData>();
            cut.WaitForState(() => cut.FindAll("table").Count > 0);

            // Assert: first find the <p> element, then verify its content
            var result = cut.Nodes.FirstOrDefault(w => w.NodeName.Contains("table", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(result);
            var tbl = result as AngleSharp.Html.Dom.IHtmlTableElement;
            Assert.NotNull(tbl);
            Assert.Matches("6", tbl.Rows.Count().ToString());

        }

        [Fact]
        public void ShowWeatherForecastFromRestApiOnPageLoad()
        {
            // Arrange: render the Counter.razor component

            // Act: page load occurs automatically
            var cut = RenderComponent<Weather>();
            cut.WaitForState(() => cut.FindAll("table").Count > 0);

            // Assert: first find the <p> element, then verify its content
            var result = cut.Nodes.FirstOrDefault(w => w.NodeName.Contains("table", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(result);
            var tbl = result as AngleSharp.Html.Dom.IHtmlTableElement;
            Assert.NotNull(tbl);

            Assert.Matches("11", tbl.Rows.Count().ToString());

        }

        private void InitApi()
        {
            var mockHttpClient = GetMockClient();
            Services.AddSingleton(mockHttpClient);
        }
        
        private HttpClient GetMockClient()
        {
            var mockHttp = new MockHttpMessageHandler(BackendDefinitionBehavior.Always);

            mockHttp
                .When("/WeatherForecast")
                .Respond(appJson, GetWeatherForecastMvcBased());

            mockHttp
                .When("/api/weather-forecasts/generate")
                .Respond(appJson, GetWeatherForecastRestBased());

            var mockHttpClient = mockHttp.ToHttpClient();
            mockHttpClient.BaseAddress = new Uri(baseAdr);

            return mockHttpClient;
        }

        public string GetWeatherForecastMvcBased()
        {
            string results = string.Empty;
            var t = Task.Run(async () => { results = await Client.GetStringAsync("WeatherForecast"); });
            t.Wait();
            return results;
        }
        public string GetWeatherForecastRestBased()
        {
            string results = string.Empty;
            var t = Task.Run(async () => { results = await Client.GetStringAsync("api/weather-forecasts/generate"); });
            t.Wait();
            return results;
        }

    }
}
