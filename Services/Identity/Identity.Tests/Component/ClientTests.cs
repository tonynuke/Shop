using System.Threading.Tasks;
using FluentAssertions;
using Identity.Client.V1;
using Identity.WebService;
using IdentityModel.Client;
using TestUtils.Component;
using Xunit;

namespace Identity.Tests.Component
{
    [Collection(StandCollectionFixture.Name)]
    public class ClientTests
    {
        private readonly TestContext<Startup> _testContext;

        public string Host => _testContext.Factory.Server.BaseAddress.AbsoluteUri;

        public ClientTests(TestContext<Startup> stand)
        {
            _testContext = stand;
        }

        [Fact]
        public async Task Can_authorize_service_to_service_call()
        {
            var httpClient = _testContext.Factory.CreateClient();

            var identityClient = new IdentityClient(httpClient);
            //var createClientModel = new CreateClientModel
            //{
            //    Name = "develop",
            //    Scopes = new[]
            //    {
            //        "system"
            //    }
            //};
            //await identityClient.CreateClientAsync(createClientModel);
            //await identityClient.CreateApiResourceAsync();
            var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync();
            var request = new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "develop",
                ClientSecret = "secret",
                Scope = "system",
            };
            var credentials = await httpClient.RequestClientCredentialsTokenAsync(request);
            credentials.AccessToken.Should().NotBeEmpty();
        }
    }
}