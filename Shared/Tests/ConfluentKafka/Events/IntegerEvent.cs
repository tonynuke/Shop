using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Tests.ConfluentKafka.Events
{
    [BsonDiscriminator("integer")]
    public record IntegerEvent : DomainEventBase
    {
        [BsonElement("int")]
        public int Int { get; set; }
    }
}