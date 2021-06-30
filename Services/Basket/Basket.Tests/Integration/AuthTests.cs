using System;
using System.Threading.Tasks;
using Basket.Client.V1;
using Basket.WebService;
using Catalog.Client.V1;
using Common.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;
using ApiException = Basket.Client.V1.ApiException;

namespace Basket.Tests.Integration
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly Guid _userId = Guid.NewGuid();
        private readonly IBasketClient _basketClient;

        public AuthTests(WebApplicationFactory<Startup> factory)
        {
            var httpClient = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(collection =>
                    {
                    });
                })
                .CreateClient();

            _basketClient = new BasketClient(httpClient);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsHelper.AppSettingsPath)
                .AddEnvironmentVariables()
                .Build();

            var identityConfiguration = configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();

            // TODO: настроить конфигурацию
            _accessTokenGenerator = new AccessTokenGenerator(identityConfiguration);
        }

        [Fact]
        public async Task Returns_401_when_token_is_not_specified()
        {
            Func<Task> func = () => _basketClient.GetOrCreateBasketAsync();
            var exception = await func.Should().ThrowAsync<ApiException>();
            exception.Which.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}