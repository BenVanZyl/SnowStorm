using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSample.Tests.Infrastructure
{
    public class AllTestsCompleteFixture : IDisposable
    {
        public void Dispose()
        {
            // Your cleanup code goes here
            DbManager.Cleanup();
        }
    }

    [CollectionDefinition("All Tests Complete")]
    public class AllTestsCompleteCollection : ICollectionFixture<AllTestsCompleteFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
