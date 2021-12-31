using Confluent.Kafka;
using Domain;
using System.Text;
using System.Text.Json;

namespace Common.Outbox.Publisher.Confluent
{
    public class DomainEventSerializer : ISerializer<DomainEventBase>
    {
        public byte[] Serialize(DomainEventBase data, SerializationContext context)
        {
            var value = JsonSerializer.Serialize<object>(data);
            return Encoding.UTF8.GetBytes(value!);
        }
    }
}
