using System;

namespace Common.Configuration
{
    /// <summary>
    /// Service to service authorization config.
    /// </summary>
    public class CrossServiceAuthConfiguration
    {
        /// <summary>
        /// Key.
        /// </summary>
        public const string Key = "CrossServiceAuth";

        /// <summary>
        /// Gets or sets client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets auth service address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets required scopes.
        /// </summary>
        public string[] Scopes { get; set; } = Array.Empty<string>();
    }
}