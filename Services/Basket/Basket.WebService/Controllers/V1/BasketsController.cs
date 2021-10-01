using System;
using System.Threading.Tasks;
using Basket.Domain;
using Basket.Services;
using Basket.WebService.Dto;
using Common.Api;
using Common.AspNetCore.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UpdateBasketDto = Basket.Services.Dto.UpdateBasketDto;

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
        private readonly BasketsService _basketsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class.
        /// </summary>
        /// <param name="basketsService">Baskets service.</param>
        public BasketsController(BasketsService basketsService)
        {
            _basketsService = basketsService;
        }

        private Guid UserId => User.GetUserId();

        /// <summary>
        /// Gets or creates a basket.
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
        /// Updates the basket.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasket(Dto.UpdateBasketDto dto)
        {
            var basketItems = dto.Items.Adapt<BasketItem[]>();
            var updateBasketDto = new UpdateBasketDto(UserId, basketItems);
            var basket = await _basketsService.UpdateBasket(updateBasketDto);
            return this.FromResult(basket);
        }

        /// <summary>
        /// Clears the basket.
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
