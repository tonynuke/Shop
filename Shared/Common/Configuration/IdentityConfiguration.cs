namespace Common.Configuration
{
    /// <summary>
    /// Identity configuration.
    /// </summary>
    public class IdentityConfiguration
    {
        /// <summary>
        /// Key.
        /// </summary>
        public const string Key = "Identity";

        /// <summary>
        /// Gets or sets certificate.
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// Gets or sets privateKey.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets audience.
        /// </summary>
        public string Audience { get; set; }
    }
}