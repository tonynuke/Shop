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
        /// Gets or creates a basket.
        /// </summary>
        /// <param name="buyerId">Buyer identifier.</param>
        /// <returns>Basket id.</returns>
        /// <remarks>If user basket is not found, then creates new one.</remarks>
        Task<Domain.Basket> GetOrCreateBasket(Guid buyerId);

        /// <summary>
        /// Updates the basket.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        Task<Result<Domain.Basket>> UpdateBasket(UpdateBasketDto dto);

        /// <summary>
        /// Clears the basket.
        /// </summary>
        /// <param name="buyerId">Buyer identifier.</param>
        /// <returns>Basket.</returns>
        Task<Result<Domain.Basket>> ClearBasket(Guid buyerId);
    }
}