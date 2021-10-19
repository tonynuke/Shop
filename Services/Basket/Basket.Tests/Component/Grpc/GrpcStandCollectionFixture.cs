using TestUtils.Component;
using Xunit;

namespace Basket.Tests.Component.Grpc
{
    extern alias GrpcService;

    [CollectionDefinition(Name)]
    public class GrpcStandCollectionFixture : ICollectionFixture<StandFixture<GrpcService::Basket.GrpcService.Startup>>
    {
        public const string Name = nameof(GrpcStandCollectionFixture);
    }
}
