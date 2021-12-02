using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Client.V1;
using Catalog.WebService;
using Common.Hosting.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Polly;
using TestUtils;
using TestUtils.Component;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Tests.Component
{
    [Collection(StandCollectionFixture.Name)]
    public class ItemsTests
    {
        private readonly Fixture _fixture = new();
        private readonly ICatalogClient _client;
        private readonly StandFixture<Startup> _stand;
        private readonly Guid _userId = Guid.NewGuid();

        public ItemsTests(StandFixture<Startup> stand, ITestOutputHelper testOutputHelper)
        {
            _stand = stand;

            var configuration = _stand.Host.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByClaims(_userId, new[]
            {
                new Claim(AuthorizationPolicies.Catalog, "all"),
            });

            var httpClient = stand.Host.CreateClient();
            var authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, token);
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            _client = new CatalogClient(httpClient);
        }

        [Fact]
        public async Task Can_create_an_item()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var brandId = await _client.CreateBrandAsync(createBrandModel);

            var createItemModel = new CreateItemDto
            {
                BrandId = brandId,
                Description = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>(),
            };
            var itemId = await _client.CreateItemAsync(createItemModel);
            var item = await _client.FindItemByIdAsync(itemId);

            item.Should().NotBeNull();
        }

        [Fact]
        public async Task Can_find_the_item_after_creation()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var brandId = await _client.CreateBrandAsync(createBrandModel);

            var createItemModel = new CreateItemDto
            {
                BrandId = brandId,
                Description = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>(),
            };
            var itemId = await _client.CreateItemAsync(createItemModel);
            var expectedItem = new ItemDto
            {
                Id = itemId,
                Name = createItemModel.Name,
                Price = createItemModel.Price
            };

            var model = new ItemsQueryDto
            {
                Page = new PageDto
                {
                    Skip = 0,
                    Limit = 10
                },
                Query = createItemModel.Name
            };

            // HACK: call manually, don't wait for the scheduler.
            await _client.ProcessEventsAsync();

            // Use retry due to indexing delays.
            var retryPolicy = Policy
                .Handle<TimeoutException>()
                .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(100));

            await retryPolicy.ExecuteAsync(async () =>
            {
                var items = await _client.SearchItemsAsync(model);
                if (!items.Any())
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    throw new TimeoutException();
                }

                items.Should().NotBeEmpty();
                var item = items.Single();
                item.Id.Should().Be(expectedItem.Id);
                item.Name.Should().Be(expectedItem.Name);
                item.Price.Should().Be(expectedItem.Price);
            });
        }

        [Fact]
        public async Task Can_delete_the_item()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var brandId = await _client.CreateBrandAsync(createBrandModel);

            var createItemModel = new CreateItemDto
            {
                BrandId = brandId,
                Description = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>(),
            };
            var itemId = await _client.CreateItemAsync(createItemModel);
            await _client.DeleteItemAsync(itemId);
            Func<Task> func = () => _client.FindItemByIdAsync(itemId);
            var exception = await func.Should().ThrowAsync<ApiException>();
            exception.Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Can_not_find_the_item_after_deletion()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var brandId = await _client.CreateBrandAsync(createBrandModel);

            var createItemModel = new CreateItemDto
            {
                BrandId = brandId,
                Description = _fixture.Create<string>(),
                Name = _fixture.Create<string>(),
                Price = _fixture.Create<decimal>(),
            };
            var itemId = await _client.CreateItemAsync(createItemModel);
            await _client.DeleteItemAsync(itemId);

            var model = new ItemsQueryDto
            {
                Page = new PageDto
                {
                    Skip = 0,
                    Limit = 10
                },
                Query = createItemModel.Name
            };

            // Use retry due to indexing delays.
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(100));

            await retryPolicy.ExecuteAsync(async () =>
            {
                var items = await _client.SearchItemsAsync(model);
                if (items.Any())
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    throw new Exception();
                }

                items.Should().BeEmpty();
            });
        }

        [Fact]
        public async Task Search_returns_nothing_when_no_items_matched_to_the_query()
        {
            var model = new ItemsQueryDto
            {
                Page = new PageDto
                {
                    Skip = 0,
                    Limit = 10
                },
                Query = _fixture.Create<string>()
            };

            var items = await _client.SearchItemsAsync(model);
            items.Should().BeEmpty();
        }
    }
}