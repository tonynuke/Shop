using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Identity.Client.V1;
using Identity.WebService;
using IdentityModel.Client;
using TestUtils;
using TestUtils.Component;
using Xunit;

namespace Identity.Tests.Component
{
    [Collection(StandCollectionFixture.Name)]
    public class ClientTests
    {
        private readonly StandFixture<Startup> _stand;

        public string Host => _stand.Host.Server.BaseAddress.AbsoluteUri;

        public ClientTests(StandFixture<Startup> stand)
        {
            _stand = stand;
        }

        [Fact]
        public async Task Can_authorize_service_to_service_call()
        {
            var httpClient = _stand.Host.CreateClient();

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