using Catalog.Messages.Events;
using DataAccess.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Domain
{
    /// <summary>
    /// Catalog item.
    /// </summary>
    public class CatalogItem : EntityBase
    {
        private decimal _price;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItem"/> class.
        /// </summary>
        /// <param name="brand">Brand.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="price">Price.</param>
        public CatalogItem(Brand brand, Name name, string description, decimal price)
        {
            Brand = brand;
            Name = name;
            Description = description;
            Price = price;
        }

        /// <summary>
        /// Gets brand.
        /// </summary>
        [BsonElement("brand")]
        public Brand Brand { get; private set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [BsonElement("name")]
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [BsonElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        [BsonElement("price")]
        public decimal Price
        {
            get => _price;

            set
            {
                _price = value;
                var @event = new ItemChanged(Id);
                AddEvent(@event);
            }
        }
    }
}