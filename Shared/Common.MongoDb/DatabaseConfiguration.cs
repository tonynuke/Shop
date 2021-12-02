namespace Common.MongoDb
{
    /// <summary>
    /// Database configuration.
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Key.
        /// </summary>
        public const string Key = nameof(DatabaseConfiguration);

        /// <summary>
        /// Gets or sets database name.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether query log enabled.
        /// </summary>
        public bool EnableQueryLog { get; set; }
    }
}