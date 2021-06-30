using System;

namespace Basket.Services.Dto
{
    /// <summary>
    /// Add or update basket item dto.
    /// </summary>
    public class AddOrUpdateBasketItemDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddOrUpdateBasketItemDto"/> class.
        /// </summary>
        /// <param name="buyerId">Buyer id.</param>
        /// <param name="catalogItemId">Catalog item id.</param>
        /// <param name="quantity">Quantity.</param>
        public AddOrUpdateBasketItemDto(
            Guid buyerId,
            Guid catalogItemId,
            int quantity)
        {
            BuyerId = buyerId;
            CatalogItemId = catalogItemId;
            Quantity = quantity;
        }

        /// <summary>
        /// Gets buyer id.
        /// </summary>
        public Guid BuyerId { get; }

        /// <summary>
        /// Gets catalog item id.
        /// </summary>
        public Guid CatalogItemId { get; }

        /// <summary>
        /// Gets quantity.
        /// </summary>
        public int Quantity { get; }
    }
}