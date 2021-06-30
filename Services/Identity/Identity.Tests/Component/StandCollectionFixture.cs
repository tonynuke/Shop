using Identity.WebService;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Identity.Tests.Component
{
    [CollectionDefinition(Name)]
    public class StandCollectionFixture : ICollectionFixture<StandFixture<Startup>>
    {
        public const string Name = nameof(StandCollectionFixture);
    }
}
