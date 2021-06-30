using System;

namespace Basket.Services.Dto
{
    /// <summary>
    /// Remove basket item dto.
    /// </summary>
    public class RemoveItemFromBasketDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveItemFromBasketDto"/> class.
        /// </summary>
        /// <param name="buyerId">Buyer id.</param>
        /// <param name="catalogItemId">Catalog item id.</param>
        public RemoveItemFromBasketDto(
            Guid buyerId,
            Guid catalogItemId)
        {
            BuyerId = buyerId;
            CatalogItemId = catalogItemId;
        }

        /// <summary>
        /// Gets buyer id.
        /// </summary>
        public Guid BuyerId { get; }

        /// <summary>
        /// Gets catalog item id.
        /// </summary>
        public Guid CatalogItemId { get; }
    }
}