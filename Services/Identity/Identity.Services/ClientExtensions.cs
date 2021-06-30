using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;

namespace Identity.Services
{
    public static class ClientExtensions
    {
        public static void AddGrantType(this Client client, string grantType)
        {
            var clientGrantType = new ClientGrantType
            {
                GrantType = grantType,
            };
            client.AllowedGrantTypes.Add(clientGrantType);
        }

        public static void AddRedirectUris(this Client client, IEnumerable<string> redirectUris)
        {
            var clientRedirectUris = redirectUris.Select(
                redirectUri => new ClientRedirectUri
                {
                    RedirectUri = redirectUri,
                });
            client.RedirectUris.AddRange(clientRedirectUris);
        }

        public static void AddAllowedScopes(this Client client, IEnumerable<string> scopes)
        {
            var clientScopes = scopes.Select(
                scope => new ClientScope
                {
                    Scope = scope,
                });

            client.AllowedScopes.AddRange(clientScopes);
        }
    }
}