using Confluent.Kafka;
using Domain;
using System.Text;
using System.Text.Json;

namespace Common.Outbox.Publisher.Confluent
{
    /// <summary>
    /// Confluent Kafka publisher.
    /// </summary>
    public class KafkaPublisher : IPublisher, IDisposable
    {
        public const string EventTypeHeader = "Type";

        private readonly string _topic;
        private readonly IProducer<string, string> _producer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaPublisher"/> class.
        /// </summary>
        public KafkaPublisher(IProducer<string, string> producer, string topic)
        {
            _topic = topic;
            _producer = producer;
        }

        /// <inheritdoc/>
        public async Task Publish(IEnumerable<DomainEventBase> events)
        {
            var messages = events.Select(@event =>
                {
                    var header = GetTypeHeader(@event);
                    // Specify object type to serialize child classes properties.
                    var value = JsonSerializer.Serialize<object>(@event);
                    return new Message<string, string>
                    {
                        Value = value,
                        Key = @event.ParentEntityId,
                        Headers = new Headers() { header },
                    };
                });

            foreach (var message in messages)
            {
                await _producer.ProduceAsync(_topic, message);
            }
        }

        public static Header GetTypeHeader(DomainEventBase @event)
        {
            var type = @event.GetType().FullName;
            var typeAsBytes = Encoding.UTF8.GetBytes(type!);
            return new Header(EventTypeHeader, typeAsBytes);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}