using Bunit;
using WebSample.Tests.Infrastructure;

namespace WebSample.Test.Infrastructure
{
    [Collection("All Tests Complete")]
    public class BaseIntegrationTests : TestContext//, ICollectionFixture<AllTestsCompleteFixture>
    {        
        public BaseIntegrationTests()
        {
            Factory = new TestAppFactory();
            Http = Factory.CreateClient();
        }

        public TestAppFactory Factory { get; set; }
        public HttpClient Http { get; set; }

    }

}
