using Identity.WebService;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Identity.Tests.Component
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<TestContext<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
