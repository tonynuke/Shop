using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Tests.ConfluentKafka
{
    [BsonDiscriminator("string")]
    public record StringEvent : DomainEventBase
    {
        [BsonElement("string")]
        public string String { get; set; }
    }
}