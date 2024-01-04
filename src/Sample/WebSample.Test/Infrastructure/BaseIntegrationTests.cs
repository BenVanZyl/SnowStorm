using Bunit;

namespace WebSample.Test.Infrastructure
{
    public class BaseIntegrationTests : TestContext
    {
        public TestAppFactory Factory { get; set; }
        public HttpClient Http { get; set; }

        public BaseIntegrationTests()
        {
            Factory = new TestAppFactory();
            Http = Factory.CreateClient();
        }

    }

}
