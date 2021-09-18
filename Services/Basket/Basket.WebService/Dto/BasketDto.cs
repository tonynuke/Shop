using System;

namespace Basket.WebService.Dto
{
    /// <summary>
    /// Basket.
    /// </summary>
    public record BasketDto
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets items.
        /// </summary>
        public BasketItemDto[] Items { get; init; } = Array.Empty<BasketItemDto>();
    }
}