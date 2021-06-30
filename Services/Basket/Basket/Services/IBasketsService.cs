using System;
using System.Threading.Tasks;
using Basket.Services.Dto;
using CSharpFunctionalExtensions;

namespace Basket.Services
{
    /// <summary>
    /// Baskets service interface.
    /// </summary>
    public interface IBasketsService
    {
        /// <summary>
        /// Adds or updates the basket item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        Task<Result<Domain.Basket>> AddOrUpdateBasketItem(AddOrUpdateBasketItemDto dto);

        /// <summary>
        /// Removes the item from the basket.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        Task<Result<Domain.Basket>> RemoveItemFromBasket(RemoveItemFromBasketDto dto);

        /// <summary>
        /// Gets or creates a basket.
        /// </summary>
        /// <param name="buyerId">Buyer identifier.</param>
        /// <returns>Basket id.</returns>
        /// <remarks>If user basket is not found, then creates new one.</remarks>
        Task<Domain.Basket> GetOrCreateBasket(Guid buyerId);

        /// <summary>
        /// Clears the basket.
        /// </summary>
        /// <param name="buyerId">Buyer identifier.</param>
        /// <returns>Asynchronous operation.</returns>
        Task<Result> ClearBasket(Guid buyerId);
    }
}