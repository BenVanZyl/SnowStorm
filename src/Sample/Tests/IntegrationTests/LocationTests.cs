using Newtonsoft.Json;
using Shouldly;
using Tests.Infrastructure;
using WebApi.Services.Domain;
using WebApi.Shared;
using WebApi.Shared.Dto;

namespace Tests.IntegrationTests
{
    public class LocationTests : BaseForIntegrationTests
    {
        public LocationTests(TestServerFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ValidateGetRegionsTest()
        {
            //Arrange
            //Nothing to do.  Data is already there.

            //Act
            var response = await Client.GetAsync(Routes.LocationsRegions);

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var regions = JsonConvert.DeserializeObject<List<RegionDto>>(apiResponse);

            regions.Any(w => w.RegionDescription.Trim() == "Eastern").ShouldBeTrue();
            regions.Any(w => w.RegionDescription.Trim() == "Western").ShouldBeTrue();
            regions.Any(w => w.RegionDescription.Trim() == "Northern").ShouldBeTrue();
            regions.Any(w => w.RegionDescription.Trim() == "Southern").ShouldBeTrue();
        }

        [Theory]
        [InlineData(1, "Eastern")]
        [InlineData(2, "Western")]
        [InlineData(3, "Northern")]
        [InlineData(4, "Southern")]
        public async Task ValidateGetRegionTest(int request, string result)
        {
            //Arrange
            //Nothing to do.  Data is already there.

            //Act
            var response = await Client.GetAsync($"{Routes.LocationsRegions}/{request}");

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var region = JsonConvert.DeserializeObject<RegionDto>(apiResponse);

            region.Id.ShouldBe(request);
            region.RegionDescription.Trim().ShouldBe(result);

        }
    }
}
