﻿using System;
using System.Threading.Tasks;
using Basket.Persistence;
using Basket.Services.Dto;
using CSharpFunctionalExtensions;
using DataAccess;
using MongoDB.Driver;

namespace Basket.Services
{
    /// <summary>
    /// Baskets service.
    /// </summary>
    public class BasketsService : IBasketsService
    {
        private readonly BasketContext _basketContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsService"/> class.
        /// </summary>
        /// <param name="basketContext">Basket context.</param>
        public BasketsService(BasketContext basketContext)
        {
            _basketContext = basketContext;
        }

        /// <inheritdoc/>
        public async Task<Domain.Basket> GetOrCreateBasket(Guid buyerId)
        {
            var basket = await _basketContext.Baskets
                .Find(b => b.Id == buyerId)
                .SingleOrDefaultAsync();
            if (basket != null)
            {
                return basket;
            }

            basket = new Domain.Basket(buyerId, DateTime.UtcNow);
            await _basketContext.Baskets.InsertOneAsync(basket);
            return basket;
        }

        /// <inheritdoc/>
        public Task<Result<Domain.Basket>> UpdateBasket(UpdateBasketDto dto)
        {
            return FindBasket(dto.BuyerId)
                .Tap(basket => basket.ReplaceBasketItems(dto.Items))
                .Tap(UpdateOne);
        }

        /// <inheritdoc/>
        public Task<Result<Domain.Basket>> ClearBasket(Guid buyerId)
        {
            return FindBasket(buyerId)
                .Ensure(basket => !basket.IsEmpty, "Basket is empty!")
                .Tap(basket => basket.Clear())
                .Tap(UpdateOne);
        }

        private async Task<Result<Domain.Basket>> FindBasket(Guid buyerId)
        {
            Maybe<Domain.Basket> basketOrNothing = await _basketContext.Baskets
                .Find(b => b.Id == buyerId)
                .SingleOrDefaultAsync();
            return basketOrNothing.ToResult($"User {buyerId} doesn't have a basket.");
        }

        private Task UpdateOne(Domain.Basket basket)
        {
            var update = Builders<Domain.Basket>.Update.Set(b => b.Items, basket.Items);
            return _basketContext.Baskets.UpdateOneOcc(basket, update);
        }
    }
}