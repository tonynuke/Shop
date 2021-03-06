using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Common.Hosting.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TestUtils
{
    /// <summary>
    /// Генератор токенов.
    /// </summary>
    public class AccessTokenGenerator
    {
        private readonly IdentityConfiguration _identityConfiguration;
        private readonly SigningCredentials _signingCredentials;

        private static Claim SubjectClaim(string id) => new ("sub", id);

        public AccessTokenGenerator(IdentityConfiguration configuration)
        {
            _identityConfiguration = configuration;
            var certificate = X509Certificate2.CreateFromPem(
                configuration.Certificate, configuration.PrivateKey);
            _signingCredentials = new X509SigningCredentials(certificate);
        }

        public string GetJwtTokenByRole(Guid userId, string role)
        {
            string id = userId.ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role),
                SubjectClaim(id),
                new Claim(ClaimsIdentity.DefaultNameClaimType, id)
            };

            return GenerateJwtToken(claims);
        }

        public string GetJwtTokenByClaims(Guid userId, params Claim[] claims)
        {
            string id = userId.ToString();

            var resultClaims = claims
                .Append(SubjectClaim(id))
                .Append(new Claim(ClaimsIdentity.DefaultNameClaimType, id));

            return GenerateJwtToken(resultClaims);
        }

        private static DateTimeOffset TruncateMilliseconds(DateTimeOffset dateTime)
        {
            long ticks = dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond);
            return new DateTimeOffset(ticks, dateTime.Offset);
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            DateTimeOffset issuedAt = TruncateMilliseconds(DateTime.UtcNow);
            DateTimeOffset accessTokenExpiresAt = issuedAt.Add(TimeSpan.FromDays(365));

            var jwt = new JwtSecurityToken(
                _identityConfiguration.Issuer,
                _identityConfiguration.Audience,
                claims,
                issuedAt.UtcDateTime,
                accessTokenExpiresAt.UtcDateTime,
                _signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}