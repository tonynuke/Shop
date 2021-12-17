using Domain;
using System.Text.Json;

namespace Common.Outbox.Publisher
{
    public record EventEnvelope
    {
        public EventEnvelope()
        {
        }

        public string Type { get; set; }

        public string Payload { get; set; }

        public EventEnvelope(DomainEventBase @event)
        {
            Type = @event.GetType().FullName;
            Payload = JsonSerializer.Serialize(@event);
        }

        public EventEnvelope(DomainEventBase @event, string type)
        {
            Type = type;
            Payload = JsonSerializer.Serialize(@event);
        }
    }
}
