using System;

namespace Basket.WebService.Dto
{
    /// <summary>
    /// Dto to add or update basket item.
    /// </summary>
    public record AddOrUpdateBasketItemDto
    {
        /// <summary>
        /// Gets catalog item id.
        /// </summary>
        public Guid CatalogItemId { get; init; }

        /// <summary>
        /// Gets quantity.
        /// </summary>
        public int Quantity { get; init; }
    }
}