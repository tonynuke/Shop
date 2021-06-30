using Basket.WebService;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Basket.Tests.Component
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<StandFixture<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
