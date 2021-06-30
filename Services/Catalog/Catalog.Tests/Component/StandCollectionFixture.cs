using Catalog.WebService;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Catalog.Tests.Component
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<StandFixture<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
