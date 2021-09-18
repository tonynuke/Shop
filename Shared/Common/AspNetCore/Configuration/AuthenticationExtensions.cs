using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Common.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Common.AspNetCore.Configuration
{
    /// <summary>
    /// Authentication configuration extensions.
    /// </summary>
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            // https://stackoverflow.com/questions/62475109/asp-net-core-jwt-authentication-changes-claims-sub
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var authConfiguration =
                        configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
                    var bytes = Encoding.ASCII.GetBytes(authConfiguration.Certificate);
                    var certificate = new X509Certificate2(bytes);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new X509SecurityKey(certificate),

                        // enable validation
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false
                    };
                });
            return services;
        }
    }
}