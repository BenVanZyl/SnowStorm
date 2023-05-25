using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Infrastructure
{
    public class BaseForIntegrationTests : IDisposable, IClassFixture<TestServerFixture>
    {
        public BaseForIntegrationTests(TestServerFixture fixture) => Fixture = fixture;

        public TestServerFixture Fixture { get; }
        public TestServer TestServer => Fixture.TestServer;
        public HttpClient Client => Fixture.Client;

        public void Dispose()
        {
            //Client.Dispose();
            //TestServer.Dispose();
        }
    }
}
