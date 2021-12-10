using Confluent.Kafka;
using Domain;
using System.Text.Json;

namespace Common.Outbox.Publisher.Confluent
{
    /// <summary>
    /// Confluent Kafka publisher.
    /// </summary>
    public class KafkaPublisher : IPublisher
    {
        private const string TopicName = "topicName";
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerConfig _producerConfig = new()
        {
            // TODO: move to the settings class
            BootstrapServers = "127.0.0.1:29092",
            ClientId = "ASP.NET backend",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaPublisher"/> class.
        /// </summary>
        public KafkaPublisher()
        {
            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            var messages = events.Select(@event =>
                new Message<Null, string>
                {
                    // specify object type to serialize child classes properties
                    Value = JsonSerializer.Serialize<object>(@event),
                });

            foreach (var message in messages)
            {
                await _producer.ProduceAsync(TopicName, message);
            }
        }
    }
}