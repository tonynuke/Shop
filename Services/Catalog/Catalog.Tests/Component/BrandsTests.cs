using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Client.V1;
using Catalog.WebService;
using Common.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Catalog.Tests.Component
{
    [Collection(StandCollectionFixture.Name)]
    public class BrandsTests
    {
        private readonly Fixture _fixture = new ();
        private readonly ICatalogClient _client;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly StandFixture<Startup> _stand;

        public BrandsTests(StandFixture<Startup> stand)
        {
            _stand = stand;

            var configuration = _stand.Host.Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var tokenGenerator = new AccessTokenGenerator(configuration);
            var token = tokenGenerator.GetJwtTokenByScopes(_userId);

            var httpClient = stand.Host.CreateClient();
            var authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, token);
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            _client = new CatalogClient(httpClient);
        }

        [Fact]
        public async Task Can_create_a_brand()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var id = await _client.CreateBrandAsync(createBrandModel);

            var brand = await _client.FindBrandByIdAsync(id);
            brand.Should().NotBeNull();

            brand.Id.Should().Be(id);
            brand.Name.Should().Be(createBrandModel.Name);
        }

        [Fact]
        public async Task Can_update_the_brand()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var id = await _client.CreateBrandAsync(createBrandModel);

            var updateBrandModel = _fixture.Create<UpdateBrandDto>();
            await _client.UpdateBrandAsync(id, updateBrandModel);

            var brand = await _client.FindBrandByIdAsync(id);
            brand.Should().NotBeNull();

            brand.Id.Should().Be(id);
            brand.Name.Should().Be(updateBrandModel.Name);
        }

        [Fact]
        public async Task Can_delete_the_brand()
        {
            var createBrandModel = _fixture.Create<CreateBrandDto>();
            var id = await _client.CreateBrandAsync(createBrandModel);
            await _client.DeleteBrandAsync(id);

            Func<Task> func = () => _client.FindBrandByIdAsync(id);
            var exception = await func.Should().ThrowAsync<ApiException<ProblemDetails>>();
        }

        [Theory]
        [InlineData("")]
        public void Can_not_create_a_brand_when_the_name_is_invalid(string name)
        {
            var createBrandModel = new CreateBrandDto
            {
                Name = name
            };
            Func<Task> action = async () => await _client.CreateBrandAsync(createBrandModel);
            var exception = action.Should().ThrowAsync<ApiException>();
            exception.Should().NotBeNull();
        }
    }
}
