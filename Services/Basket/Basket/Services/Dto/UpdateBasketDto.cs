using System;
using System.Collections.Generic;
using Basket.Domain;

namespace Basket.Services.Dto
{
    /// <summary>
    /// Add or update basket item dto.
    /// </summary>
    public class UpdateBasketDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBasketDto"/> class.
        /// </summary>
        /// <param name="buyerId">Buyer id.</param>
        /// <param name="items">Items.</param>
        public UpdateBasketDto(
            Guid buyerId,
            IReadOnlyCollection<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        /// <summary>
        /// Gets buyer id.
        /// </summary>
        public Guid BuyerId { get; }

        public IReadOnlyCollection<BasketItem> Items { get; }
    }
}