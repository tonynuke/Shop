using System;
using System.Threading.Tasks;
using Basket.Client.V1;
using Catalog.Client.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebService.Controllers.V1
{
    /// <summary>
    /// Baskets controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketClient _basketClient;
        private readonly ICatalogClient _catalogClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketsController"/> class.
        /// </summary>
        /// <param name="basketClient">Baskets client.</param>
        /// <param name="catalogClient">Catalog client.</param>
        public BasketsController(IBasketClient basketClient, ICatalogClient catalogClient)
        {
            _basketClient = basketClient;
            _catalogClient = catalogClient;
        }

        /// <summary>
        /// Gets or creates basket.
        /// </summary>
        /// <returns>Basket.</returns>
        [HttpPost("get")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrCreateBasket()
        {
            var basket = await _basketClient.GetOrCreateBasketAsync();
            return Ok(basket);
        }

        /// <summary>
        /// Updates the basket item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddOrUpdateBasketItem(AddOrUpdateBasketItemDto dto)
        {
            // TODO: use cache
            var basket = await _basketClient.AddOrUpdateBasketItemAsync(dto);
            var item = _catalogClient.FindItemByIdAsync(dto.CatalogItemId);
            return Ok(basket);
        }

        /// <summary>
        /// Removes the item from the basket.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Basket.</returns>
        [HttpPost("remove/{itemId}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveItemFromBasket(Guid itemId)
        {
            var basket = await _basketClient.RemoveItemFromBasketAsync(itemId);
            return Ok(basket);
        }

        /// <summary>
        /// Clears the basket.
        /// </summary>
        /// <returns>Asynchronous operation.</returns>
        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearBasket()
        {
            await _basketClient.ClearBasketAsync();
            return NoContent();
        }
    }
}
