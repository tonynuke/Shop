extern alias GrpcService;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Grpc.Net.Client;
using TestUtils.Component;
using Xunit;
using Basket.GrpcClient;
using Common.Configuration;
using FluentAssertions;
using Grpc.Core;
using GrpcService::Basket.GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using TestUtils;

namespace Basket.Tests.Component.Grpc
{
    extern alias GrpcService;

    [Collection(GrpcStandCollectionFixture.Name)]
    public class GrpcBasketTests
    {
        private readonly GrpcChannel _grpcChannel;
        private readonly StandFixture<Startup> _stand;
        private readonly GrpcClient.Basket.BasketClient _client;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Metadata _headers;

        public GrpcBasketTests(StandFixture<Startup> stand)
        {
            _stand = stand;

            var configuration = _stand.Host.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByClaims(_userId);

            var httpClient = stand.Host.CreateClient();
            //var authorization = new AuthenticationHeaderValue(
            //    JwtBearerDefaults.AuthenticationScheme, token);
            //httpClient.DefaultRequestHeaders.Authorization = authorization;
            var grpcChannelOptions = new GrpcChannelOptions
            {
                HttpClient = httpClient,
            };
            _grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress, grpcChannelOptions);
            _client = new GrpcClient.Basket.BasketClient(_grpcChannel);

            _headers = new Metadata { { "Authorization", $"Bearer {token}" } };
        }

        [Fact]
        public async Task Authorized_user_has_exactly_one_basket()
        {
            var basket1 = await _client.GetOrCreateBasketAsync(new GetOrCreateBasketRequest(), _headers);
            var basket2 = await _client.GetOrCreateBasketAsync(new GetOrCreateBasketRequest(), _headers);

            basket1.Id.Should().Be(basket2.Id).Should().NotBeNull();
        }
    }
}