using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture;
using Basket.Client.V1;
using Basket.WebService;
using Common.Hosting.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Basket.Tests.Component.Web
{
    [Collection(StandCollectionFixture.Name)]
    public class BasketsTests
    {
        private readonly Fixture _fixture = new();
        private readonly IBasketClient _client;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly StandFixture<Startup> _stand;

        public BasketsTests(StandFixture<Startup> stand)
        {
            _stand = stand;

            var configuration = _stand.Host.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByClaims(_userId);

            var httpClient = stand.Host.CreateClient();
            var authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, token);
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            _client = new BasketClient(httpClient);
        }

        [Fact]
        public async Task Authorized_user_has_exactly_one_basket()
        {
            var basket1 = await _client.GetOrCreateBasketAsync();
            var basket2 = await _client.GetOrCreateBasketAsync();

            basket1.Id.Should().Be(basket2.Id).Should().NotBeNull();
        }

        [Fact]
        public async Task Add_an_item_to_the_basket()
        {
            await _client.GetOrCreateBasketAsync();

            var updateBasketDto = _fixture.Create<UpdateBasketDto>();
            await _client.UpdateBasketAsync(updateBasketDto);

            var basket = await _client.GetOrCreateBasketAsync();
            basket.Items.Should().BeEquivalentTo(updateBasketDto.Items);
        }

        [Fact]
        public async Task Remove_an_item_from_the_basket()
        {
            await _client.GetOrCreateBasketAsync();

            var updateBasketDto = _fixture.Create<UpdateBasketDto>();
            await _client.UpdateBasketAsync(updateBasketDto);

            var updateBasketDtoV2 = new UpdateBasketDto()
            {
                Items = updateBasketDto.Items.Take(2).ToList()
            };
            await _client.UpdateBasketAsync(updateBasketDtoV2);

            var basket = await _client.GetOrCreateBasketAsync();
            basket.Items.Should().BeEquivalentTo(updateBasketDtoV2.Items);
        }

        [Fact]
        public async Task Clear_the_basket()
        {
            var updateBasketDto = _fixture.Create<UpdateBasketDto>();

            await _client.GetOrCreateBasketAsync();
            await _client.UpdateBasketAsync(updateBasketDto);
            var basketBefore = await _client.GetOrCreateBasketAsync();
            basketBefore.Items.Should().NotBeEmpty();

            await _client.ClearBasketAsync();
            var basketAfter = await _client.GetOrCreateBasketAsync();
            basketAfter.Items.Should().BeEmpty();
        }
    }
}