using System;
using Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Messages.Events
{
    [BsonDiscriminator("itemDeleted")]
    public class ItemDeleted : DomainEventBase
    {
        public ItemDeleted(Guid itemId)
        {
            ItemId = itemId;
        }

        [BsonElement("itemId")]
        public Guid ItemId { get; private set; }
    }
}