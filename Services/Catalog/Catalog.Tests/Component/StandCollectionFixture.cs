using Catalog.WebService;
using TestUtils.Component;
using Xunit;

namespace Catalog.Tests.Component
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<TestContext<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
