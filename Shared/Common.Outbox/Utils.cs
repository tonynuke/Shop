using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Common.Outbox
{
    public static class Utils
    {
        public static Task CreateTopic(string bootstrapServers, string topicName)
        {
            var config = new AdminClientConfig { BootstrapServers = bootstrapServers };
            using var adminClient = new AdminClientBuilder(config).Build();
            var topics = new TopicSpecification[] {
                    new TopicSpecification {
                        Name = topicName, ReplicationFactor = 1, NumPartitions = 1
                    }
            };

            return adminClient.CreateTopicsAsync(topics);
        }
    }
}
