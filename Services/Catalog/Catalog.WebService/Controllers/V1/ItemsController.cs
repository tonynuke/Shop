using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Indexing;
using Catalog.Items;
using Catalog.WebService.Dto.Items;
using Common.ApiErrors;
using Common.Utils.Pagination;
using CSharpFunctionalExtensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CreateItem = Catalog.Items.Dto.CreateItemDto;
using ItemsQueryDto = Catalog.WebService.Dto.Items.ItemsQueryDto;
using Name = Catalog.Items.Name;

namespace Catalog.WebService.Controllers.V1
{
    /// <summary>
    /// Catalog items controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly CatalogItemsService _itemsService;
        private readonly CatalogIndexer _indexingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="itemsService">Items service.</param>
        /// <param name="indexingService">Indexing service.</param>
        public ItemsController(CatalogItemsService itemsService, CatalogIndexer indexingService)
        {
            _itemsService = itemsService;
            _indexingService = indexingService;
        }

        /// <summary>
        /// Creates new item.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Item id.</returns>
        [Authorize(AuthorizationPolicies.Catalog)]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateItem(CreateItemDto model)
        {
            return await Name.Create(model.Name)
                .Map(name => new CreateItem(
                    model.BrandId,
                    name,
                    model.Description,
                    model.Price))
                .Bind(_itemsService.Create)
                .Finally(this.FromResult);
        }

        /// <summary>
        /// Finds item by id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindItemById(Guid id)
        {
            var item = await _itemsService.FindOne(id);
            return item != null
                ? Ok(item.Adapt<ItemDto>())
                : NotFound();
        }

        /// <summary>
        /// Finds items by ids.
        /// </summary>
        /// <param name="ids">Items ids.</param>
        /// <returns>Item.</returns>
        [HttpPost("get-by-ids")]
        [ProducesResponseType(typeof(ItemDto[]), StatusCodes.Status200OK)]
        public async Task<IReadOnlyCollection<ItemDto>> FindItemsByIds([FromBody] IReadOnlyCollection<Guid> ids)
        {
            var items = await _itemsService.FindMany(ids);
            return items.Adapt<ItemDto[]>();
        }

        /// <summary>
        /// Deletes item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            await _itemsService.DeleteOne(id);
            return NoContent();
        }

        /// <summary>
        /// Searches items.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Items.</returns>
        [AllowAnonymous]
        [HttpPost("search")]
        [ProducesResponseType(typeof(IReadOnlyCollection<ItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchItems(ItemsQueryDto model)
        {
            var page = Page.Create(model.Page.Skip, model.Page.Limit).Value;
            var searchModel = new Indexing.ItemsQueryDto(model.Query, page);
            var items = await _indexingService.SearchItems(searchModel);

            return Ok(items);
        }
    }
}
