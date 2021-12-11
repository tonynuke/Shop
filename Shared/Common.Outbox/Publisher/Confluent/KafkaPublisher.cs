using Confluent.Kafka;
using Domain;
using System.Text;
using System.Text.Json;

namespace Common.Outbox.Publisher.Confluent
{
    /// <summary>
    /// Confluent Kafka publisher.
    /// </summary>
    public class KafkaPublisher : IPublisher
    {
        private readonly string _topic;
        private readonly IProducer<string, string> _producer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaPublisher"/> class.
        /// </summary>
        public KafkaPublisher(ProducerConfig config, string topic)
        {
            _topic = topic;
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            var messages = events.Select(@event =>
                {
                    var type = @event.GetType().FullName;
                    var typeAsBytes = Encoding.UTF8.GetBytes(type!);
                    var header = new Header("Type", typeAsBytes);
                    return new Message<string, string>
                    {
                        // Specify object type to serialize child classes properties.
                        Value = JsonSerializer.Serialize<object>(@event),
                        Key = @event.ParentEntityId,
                        Headers = new Headers() { header },
                    };
                });

            foreach (var message in messages)
            {
                await _producer.ProduceAsync(_topic, message);
            }
        }
    }
}