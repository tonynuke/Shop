using System;
using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Messages.Events
{
    [BsonDiscriminator("itemChanged")]
    public class ItemChanged : DomainEventBase
    {
        public ItemChanged(Guid itemId)
        {
            ItemId = itemId;
        }

        [BsonElement("itemId")]
        public Guid ItemId { get; private set; }
    }
}