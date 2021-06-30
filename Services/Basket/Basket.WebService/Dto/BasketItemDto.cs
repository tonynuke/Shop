using System;

namespace Basket.WebService.Dto
{
    /// <summary>
    /// Basket item.
    /// </summary>
    public record BasketItemDto
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets quantity.
        /// </summary>
        public int Quantity { get; init; }
    }
}