using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Infrastructure;
using WebApi.Shared.Dto;
using WebApi.Shared;
using Shouldly;

namespace Tests.IntegrationTests
{
    public class ValidateUiTests : BaseForIntegrationTests
    {
        public ValidateUiTests(TestServerFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task ValidateSwaggerTest()
        {
            //Arrange
            //Nothing to do.  Data is already there.

            //Act
            var response = await Client.GetAsync(Routes.Swagger);

            //Assert
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.ShouldNotBeNull();
            response.IsSuccessStatusCode.ShouldBeTrue();

            string body = await response.Content.ReadAsStringAsync();

            body.ShouldNotBeNull();
            body.ShouldContain("<body>");
            body.ShouldContain("<div id=\"swagger-ui\"></div>");


        }
    }
}
