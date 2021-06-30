namespace Common.Configuration
{
    /// <summary>
    /// ElasticSearch configuration.
    /// </summary>
    public class ElasticSearchConfiguration
    {
        /// <summary>
        /// Key.
        /// </summary>
        public const string Key = "ElasticSearch";

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets index name.
        /// </summary>
        public string IndexName { get; set; }
    }
}