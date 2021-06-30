using System;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Common.AspNetCore.Auth
{
    /// <summary>
    /// <see cref="ClaimsPrincipal"/> extensions.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get user id.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Id.</returns>
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            return Guid.Parse(id);
        }
    }
}