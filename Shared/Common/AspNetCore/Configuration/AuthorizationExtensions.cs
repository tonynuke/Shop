using Microsoft.Extensions.DependencyInjection;

namespace Common.AspNetCore.Configuration
{
    /// <summary>
    /// Authorization configuration extensions.
    /// </summary>
    public static class AuthorizationExtensions
    {
        public const string SystemPolicyName = "system";
        public const string SystemRole = "system";

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SystemPolicyName, builder => builder.RequireRole(SystemRole));
            });
            return services;
        }
    }
}