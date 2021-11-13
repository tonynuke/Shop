using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Domain;

namespace Common.Outbox.Publisher
{
    /// <summary>
    /// Confluent Kafka publisher.
    /// </summary>
    public class ConfluentKafkaPublisher : IPublisher
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
        /// Initializes a new instance of the <see cref="ConfluentKafkaPublisher"/> class.
        /// </summary>
        public ConfluentKafkaPublisher()
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