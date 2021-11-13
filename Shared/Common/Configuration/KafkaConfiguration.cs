namespace Common.Configuration
{
    /// <summary>
    /// Kafka configuration.
    /// </summary>
    public class KafkaConfiguration
    {
        public const string Key = "Kafka";

        public string ClientId { get; set; }

        public string Host { get; set; }
    }
}