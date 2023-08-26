using Newtonsoft.Json;
using Shouldly;
using System.Net.Http.Json;
using Tests.Infrastructure;
using WebApi.Shared;
using WebApi.Shared.Dto;
using WebApi.Shared.Dto.Locations;
using WebApi.Shared.Dto.Regions;

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
            regions.ShouldNotBeNull();
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

            region.ShouldNotBeNull();
            region.Id.ShouldBe(request);
            region.RegionDescription.Trim().ShouldBe(result);

        }

        [Fact]
        public async Task ValidateRegionCreateTest()
        {
            //Arrange
            var data = new RegionDto()
            {
                RegionDescription = "Mountains"
            };

            //Act
            var response = await Client.PostAsJsonAsync(Routes.LocationsRegions, data);

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommandResultDto>(apiResponse);
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe(Messages.SuccessRecordCreated);

            //Retrieve and confirm
            var validateData = await Client.GetFromJsonAsync<RegionDto>($"{Routes.LocationsRegions}/{result.Id}");
            validateData.ShouldNotBeNull();
            validateData.RegionDescription.ShouldBe(data.RegionDescription);
        }

        [Fact]
        public async Task ValidateRegionUpdateTest()
        {
            //Arrange
            var data = await Client.GetFromJsonAsync<RegionDto>($"{Routes.LocationsRegions}/1");
            data.ShouldNotBeNull();

            data.RegionDescription = $"{data.RegionDescription} abc";

            //Act
            var response = await Client.PostAsJsonAsync(Routes.LocationsRegions, data);

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommandResultDto>(apiResponse);
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe(Messages.SuccessRecordUpdated);
            result.Id.ShouldBe("1");

            //Retrieve and confirm
            var validateData = await Client.GetFromJsonAsync<RegionDto>($"{Routes.LocationsRegions}/{result.Id}");
            validateData.ShouldNotBeNull();
            validateData.RegionDescription.ShouldBe(data.RegionDescription);

        }

        [Fact]
        public async Task ValidateRegionPatchTest()
        {
            //Arrange
            var data = await Client.GetFromJsonAsync<RegionDto>($"{Routes.LocationsRegions}/1");
            data.ShouldNotBeNull();

            var patchData = new RegionPatchDto()
            {
                Id = data.Id,
                Name = "RegionDescription",
                Value = $"{data.RegionDescription} abc"
            };


            //Act
            var response = await Client.PatchAsJsonAsync(Routes.LocationsRegions, patchData);

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CommandResultDto>(apiResponse);
            result.ShouldNotBeNull();
            result.Success.ShouldBeTrue();
            result.Message.ShouldBe(Messages.SuccessRecordUpdated);
            result.Id.ShouldBe("1");

            //Retrieve and confirm
            var validateData = await Client.GetFromJsonAsync<RegionDto>($"{Routes.LocationsRegions}/{result.Id}");
            validateData.ShouldNotBeNull();
            validateData.RegionDescription.ShouldBe(patchData.Value);

        }
    }
}
