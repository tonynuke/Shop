using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture;
using Basket.Client.V1;
using Basket.WebService;
using Catalog.Client.V1;
using Common.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TestUtils;
using TestUtils.Component;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;
using AddOrUpdateBasketItem = Basket.Client.V1.AddOrUpdateBasketItemDto;

namespace Basket.Tests.Component
{
    [Collection(StandCollectionFixture.Name)]
    public class BasketsTests
    {
        private readonly Fixture _fixture = new();
        private readonly HttpClient _httpClient;
        private readonly IBasketClient _client;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly StandFixture<Startup> _stand;

        public BasketsTests(StandFixture<Startup> stand)
        {
            _stand = stand;
            _stand.SetupCrossAuth();

            var configuration = _stand.Host.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByScopes(_userId);

            _httpClient = stand.Host.CreateClient();
            var authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, token);
            _httpClient.DefaultRequestHeaders.Authorization = authorization;
            _client = new BasketClient(_httpClient);
        }

        [Fact]
        public async Task Can_make_service_to_service_call()
        {
            //TODO: not valid check. Create "pink pong" controller with other service dependency.
        }

        [Fact]
        public async Task Authorized_user_has_exactly_one_basket()
        {
            var basket1 = await _client.GetOrCreateBasketAsync();
            basket1.Should().NotBeNull();

            var basket2 = await _client.GetOrCreateBasketAsync();
            basket2.Should().NotBeNull();

            basket1.Id.Should().Be(basket2.Id);
        }

        [Fact]
        public async Task Can_add_an_item_to_the_basket()
        {
            var itemModel = _fixture.Create<AddOrUpdateBasketItem>();
            var expectedCatalogItem = new ItemDto
            {
                Id = itemModel.CatalogItemId,
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>()
            };

            _stand.WireMockServer
                .Given(Request.Create()
                    .WithPath($"/api/v1/items/{itemModel.CatalogItemId}")
                    .UsingMethod(HttpMethods.Get))
                .RespondWith(Response.Create()
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(expectedCatalogItem));

            await _client.GetOrCreateBasketAsync();
            await _client.AddOrUpdateBasketItemAsync(itemModel);

            var basket = await _client.GetOrCreateBasketAsync();
            var item = basket.Items.Single(model => model.Id == itemModel.CatalogItemId);
            item.Should().NotBeNull();
            item.Id.Should().Be(expectedCatalogItem.Id);
        }

        [Fact]
        public async Task Can_clear_the_basket()
        {
            var itemModel = _fixture.Create<AddOrUpdateBasketItem>();
            var expectedCatalogItem = new ItemDto
            {
                Id = itemModel.CatalogItemId,
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>()
            };

            _stand.WireMockServer
                .Given(Request.Create()
                    .WithPath($"/api/v1/items/{itemModel.CatalogItemId}")
                    .UsingMethod(HttpMethods.Get))
                .RespondWith(Response.Create()
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(expectedCatalogItem));

            await _client.GetOrCreateBasketAsync();
            await _client.AddOrUpdateBasketItemAsync(itemModel);
            var basketBefore = await _client.GetOrCreateBasketAsync();
            basketBefore.Items.Should().NotBeEmpty();

            await _client.ClearBasketAsync();
            var basketAfter = await _client.GetOrCreateBasketAsync();
            basketAfter.Items.Should().BeEmpty();
        }
    }
}