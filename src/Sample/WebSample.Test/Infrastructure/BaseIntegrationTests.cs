using Bunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSample.Test.Infrastructure
{
    public class BaseIntegrationTests : TestContext
    {
        public TestAppFactory Factory { get; set; }
        public HttpClient Client { get; set; }

        public BaseIntegrationTests()
        {
            Factory = new TestAppFactory();
            Client = Factory.CreateClient();
        }
    }
}
