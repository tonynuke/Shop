using System;

namespace Catalog.Items.Dto
{
    /// <summary>
    /// Create catalog item dto.
    /// </summary>
    public class CreateItemDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateItemDto"/> class.
        /// </summary>
        /// <param name="brandId">Brand id.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="price">Price.</param>
        public CreateItemDto(
            Guid brandId, Name name, string description, decimal price)
        {
            BrandId = brandId;
            Name = name;
            Description = description;
            Price = price;
        }

        /// <summary>
        /// Gets brandId.
        /// </summary>
        public Guid BrandId { get; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public Name Name { get; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; }
    }
}