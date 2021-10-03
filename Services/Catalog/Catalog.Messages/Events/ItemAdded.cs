using System;
using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Messages.Events
{
    [BsonDiscriminator("itemAdded")]
    public class ItemAdded : DomainEventBase
    {
        public ItemAdded(Guid itemId)
        {
            ItemId = itemId;
        }

        [BsonElement("itemId")]
        public Guid ItemId { get; private set; }
    }
}