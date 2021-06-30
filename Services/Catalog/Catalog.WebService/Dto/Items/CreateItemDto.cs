using System;

namespace Catalog.WebService.Dto.Items
{
    /// <summary>
    /// Create item dto.
    /// </summary>
    public record CreateItemDto
    {
        /// <summary>
        /// Gets brand id.
        /// </summary>
        public Guid BrandId { get; init; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; init; }
    }
}