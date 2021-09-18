using System;

namespace Basket.WebService.Dto
{
    /// <summary>
    /// Dto to add or update basket item.
    /// </summary>
    public record UpdateBasketDto
    {
        /// <summary>
        /// Gets items.
        /// </summary>
        public BasketItemDto[] Items { get; init; } = Array.Empty<BasketItemDto>();
    }
}