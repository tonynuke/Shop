using Basket.WebService;
using TestUtils.Component;
using Xunit;

namespace Basket.Tests.Component.Web
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<TestContext<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
