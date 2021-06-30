using System;
using System.Threading.Tasks;
using Basket.Services;
using Basket.Services.Dto;
using Basket.WebService.Dto;
using Common.Api;
using Common.AspNetCore.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AddOrUpdateBasketItemDto = Basket.Services.Dto.AddOrUpdateBasketItemDto;

namespace Basket.WebService.Controllers.V1
{
    /// <summary>
    /// Baskets controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketsService _basketsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class.
        /// </summary>
        /// <param name="basketsService">Baskets service.</param>
        public BasketsController(IBasketsService basketsService)
        {
            _basketsService = basketsService;
        }

        private Guid UserId => User.GetUserId();

        /// <summary>
        /// Gets or creates basket.
        /// </summary>
        /// <returns>Basket.</returns>
        [HttpPost("get")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrCreateBasket()
        {
            var basket = await _basketsService.GetOrCreateBasket(UserId);
            return Ok(basket.Adapt<BasketDto>());
        }

        /// <summary>
        /// Updates basket item.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Basket.</returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddOrUpdateBasketItem(Dto.AddOrUpdateBasketItemDto model)
        {
            var dto = new AddOrUpdateBasketItemDto(
                UserId, model.CatalogItemId, model.Quantity);
            var basket = await _basketsService.AddOrUpdateBasketItem(dto);
            return this.FromResult(basket);
        }

        /// <summary>
        /// Removes item from basket.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Basket.</returns>
        [HttpPost("remove/{itemId}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItemFromBasket(Guid itemId)
        {
            var dto = new RemoveItemFromBasketDto(UserId, itemId);
            var basket = await _basketsService.RemoveItemFromBasket(dto);
            return this.FromResult(basket);
        }

        /// <summary>
        /// Clears basket.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearBasket()
        {
            await _basketsService.ClearBasket(UserId);
            return NoContent();
        }
    }
}
