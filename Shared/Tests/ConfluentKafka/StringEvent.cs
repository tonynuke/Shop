using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Tests.ConfluentKafka
{
    [BsonDiscriminator("string")]
    public class StringEvent : DomainEventBase
    {
        [BsonElement("string")]
        public string String { get; set; }
    }
}