using TestUtils.Component;
using Xunit;

namespace Basket.Tests.Component.Grpc
{
    extern alias GrpcService;

    [CollectionDefinition(Name)]
    public class GrpcStandCollectionFixture : ICollectionFixture<TestContext<GrpcService::Basket.GrpcService.Startup>>
    {
        public const string Name = nameof(GrpcStandCollectionFixture);
    }
}
