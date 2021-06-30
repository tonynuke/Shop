using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Stores;

namespace Identity.Services
{
    public class ClientsService
    {
        private readonly ConfigurationDbContext _context;
        private readonly IClientStore _clientStore;
        private readonly IPersistedGrantStore _grantStore;

        public ClientsService(
            ConfigurationDbContext context,
            IClientStore clientStore,
            IPersistedGrantStore grantStore)
        {
            _context = context;
            _clientStore = clientStore;
            _grantStore = grantStore;
        }

        public async ValueTask CreateClient(
            string clientName,
            IEnumerable<string> scopes,
            IEnumerable<string> redirectUris)
        {
            var client = new Client
            {
                ClientId = clientName,
                ClientName = clientName,
                RequireClientSecret = true,
                RequirePkce = false,
                AllowedGrantTypes = new List<ClientGrantType>(),
                RedirectUris = new List<ClientRedirectUri>(),
                AllowedScopes = new List<ClientScope>(),
                AlwaysSendClientClaims = false,
                ClientSecrets = new List<ClientSecret>(),
            };

            client.AddGrantType(OidcConstants.GrantTypes.ClientCredentials);
            //client.AddRedirectUris(redirectUris);
            client.AddAllowedScopes(scopes);
            client.ClientSecrets.Add(new ClientSecret
            {
                Value = "secret".ToSha256()
            });

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            return await _clientStore.FindClientByIdAsync(clientId);
        }
    }
}