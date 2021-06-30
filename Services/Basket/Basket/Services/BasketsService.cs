using System;
using System.Threading.Tasks;
using Basket.Domain;
using Basket.Persistence;
using Basket.Services.Dto;
using Catalog.Client.V1;
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
        private readonly ICatalogClient _catalogClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsService"/> class.
        /// </summary>
        /// <param name="basketContext">Basket context.</param>
        /// <param name="catalogClient">Catalog client.</param>
        public BasketsService(BasketContext basketContext, ICatalogClient catalogClient)
        {
            _basketContext = basketContext;
            _catalogClient = catalogClient;
        }

        /// <inheritdoc/>
        public Task<Result<Domain.Basket>> AddOrUpdateBasketItem(AddOrUpdateBasketItemDto dto)
        {
            return Result.Try(() => GetOrCreateBasket(dto.BuyerId))
                .Bind(basket => basket.FindItem(dto.CatalogItemId)
                    .ToResult($"Item {dto.CatalogItemId} not found in basket {basket.Id}")
                    .OnFailureCompensate(() => CreateBasketItem(dto.CatalogItemId, dto.Quantity))
                    .Tap(basket.AddOrUpdateItem)
                    .Tap(() => UpdateOne(basket))
            .Finally(result => result.IsSuccess
                ? basket
                : Result.Failure<Domain.Basket>(result.Error)));

            Task<Result<BasketItem>> CreateBasketItem(Guid catalogItemId, int quantity)
            {
                return Result.Try(() => _catalogClient.FindItemByIdAsync(catalogItemId), ErrorHandler)
                    .Bind(item => BasketItem.Create(item.Id, quantity));
            }

            static string ErrorHandler(Exception exception)
            {
                return exception is ApiException { StatusCode: 404 }
                    ? "Item is not presented in catalog."
                    : exception.Message;
            }
        }

        /// <inheritdoc/>
        public Task<Result<Domain.Basket>> RemoveItemFromBasket(RemoveItemFromBasketDto dto)
        {
            return FindBasket(dto.BuyerId).Tap(basket => basket.RemoveItem(dto.CatalogItemId));
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

            basket = new Domain.Basket(buyerId);
            await _basketContext.Baskets.InsertOneAsync(basket);
            return basket;
        }

        /// <inheritdoc/>
        public Task<Result> ClearBasket(Guid buyerId)
        {
            return FindBasket(buyerId)
                .Tap(basket => basket.Clear())
                .Tap(UpdateOne)
                .Finally(result => result.IsSuccess
                    ? Result.Success()
                    : Result.Failure(result.Error));
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