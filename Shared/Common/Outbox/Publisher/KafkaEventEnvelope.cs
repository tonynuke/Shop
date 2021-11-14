using Domain;
using System.Text.Json;

namespace Common.Outbox.Publisher
{
    public class KafkaEventEnvelope
    {
        public string Type { get; }

        public string Payload { get; }

        public KafkaEventEnvelope(DomainEventBase @event)
        {
            Type = @event.GetType().FullName;
            Payload = JsonSerializer.Serialize(@event);
        }
    }
}
