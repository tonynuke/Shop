extern alias GrpcService;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Grpc.Net.Client;
using TestUtils.Component;
using Xunit;
using Basket.GrpcClient;
using FluentAssertions;
using Grpc.Core;
using GrpcService::Basket.GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Common.Hosting.Configuration;

namespace Basket.Tests.Component.Grpc
{
    extern alias GrpcService;

    [Collection(GrpcStandCollectionFixture.Name)]
    public class GrpcBasketTests
    {
        private readonly GrpcChannel _grpcChannel;
        private readonly TestContext<Startup> _testContext;
        private readonly GrpcClient.Basket.BasketClient _client;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Metadata _headers;

        public GrpcBasketTests(TestContext<Startup> stand)
        {
            _testContext = stand;

            var configuration = _testContext.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByClaims(_userId);

            var httpClient = stand.Factory.CreateDefaultClient();
            var authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, token);
            httpClient.DefaultRequestHeaders.Authorization = authorization;

            //var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            //{
            //    metadata.Add("Authorization", $"Bearer {token}");
            //    return Task.CompletedTask;
            //});
            var grpcChannelOptions = new GrpcChannelOptions
            {
                HttpClient = httpClient,
                //Credentials = ChannelCredentials.Create(ChannelCredentials.Insecure, credentials)
            };
            _grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress, grpcChannelOptions);
            _client = new GrpcClient.Basket.BasketClient(_grpcChannel);
        }

        [Fact]
        public async Task Authorized_user_has_exactly_one_basket()
        {
            var request = new GetOrCreateBasketRequest();

            //_headers = new Metadata { { "Authorization", $"Bearer {token}" } };
            var basket1 = await _client.GetOrCreateBasketAsync(request);//, _headers);
            var basket2 = await _client.GetOrCreateBasketAsync(request);//, _headers);

            basket1.Id.Should().Be(basket2.Id).Should().NotBeNull();
        }
    }
}