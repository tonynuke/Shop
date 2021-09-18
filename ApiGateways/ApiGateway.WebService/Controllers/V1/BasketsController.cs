using System.Linq;
using System.Threading.Tasks;
using ApiGateway.WebService.Dto;
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
        [ProducesResponseType(typeof(UserBasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrCreateBasket()
        {
            var basket = await _basketClient.GetOrCreateBasketAsync();
            var result = await MapToUserBasket(basket);

            return Ok(result);
        }

        /// <summary>
        /// Updates the basket.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Basket.</returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(UserBasketDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasket(UpdateBasketDto dto)
        {
            var basket = await _basketClient.UpdateBasketAsync(dto);
            var result = await MapToUserBasket(basket);

            return Ok(result);
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

        private async Task<UserBasketDto> MapToUserBasket(BasketDto basket)
        {
            var basketItemsDictionary = basket.Items.ToDictionary(item => item.Id);
            var catalogItems = await _catalogClient.FindItemsByIdsAsync(basketItemsDictionary.Keys);
            var items = catalogItems
                .Select(item => new UserBasketItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = basketItemsDictionary[item.Id].Quantity,
                    Price = item.Price,
                })
                .ToList();

            return new UserBasketDto
            {
                Id = basket.Id,
                Items = items,
                Price = items.Sum(item => item.Price)
            };
        }
    }
}
