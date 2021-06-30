using DataAccess.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Domain
{
    /// <summary>
    /// Brand.
    /// </summary>
    public class Brand : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brand"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="imageUrl">Image url.</param>
        public Brand(Name name, string imageUrl = "")
        {
            Name = name;
            ImageUrl = imageUrl;
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [BsonElement("name")]
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets image url.
        /// </summary>
        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Creates <see cref="CatalogItem"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="price">Price.</param>
        /// <returns>Catalog item.</returns>
        public CatalogItem CreateItem(Name name, string description, decimal price)
        {
            return new CatalogItem(this, name, description, price);
        }
    }
}