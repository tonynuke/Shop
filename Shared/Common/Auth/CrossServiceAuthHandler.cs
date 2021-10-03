using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Common.Auth
{
    /// <summary>
    /// Service to service auth handler.
    /// </summary>
    public class CrossServiceAuthHandler : DelegatingHandler
    {
        /// <summary>
        /// Key.
        /// </summary>
        public const string Key = "crossServiceAuth";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CrossServiceAuthConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossServiceAuthHandler"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory.</param>
        /// <param name="options">Configuration.</param>
        public CrossServiceAuthHandler(IHttpClientFactory httpClientFactory, IOptions<CrossServiceAuthConfiguration> options)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = options.Value;
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(HeaderNames.Authorization))
            {
                var client = _httpClientFactory.CreateClient(Key);

                // TODO: is it ok? How does token endpoint can be changed?
                // var discoveryDocument = await client.GetDiscoveryDocumentAsync(_configuration.Address, cancellationToken);
                var tokenRequest = new ClientCredentialsTokenRequest
                {
                    Address = _configuration.Address,
                    ClientId = _configuration.ClientId,
                    ClientSecret = _configuration.ClientSecret,
                };
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

                var token = $"{JwtBearerDefaults.AuthenticationScheme} {tokenResponse.AccessToken}";
                request.Headers.TryAddWithoutValidation(HeaderNames.Authorization, token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}