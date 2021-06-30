using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Services.Items;
using Catalog.WebService.Dto.Items;
using Common.Api;
using Common.Pagination;
using CSharpFunctionalExtensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CreateItem = Catalog.Services.Items.Dto.CreateItemDto;
using Name = Catalog.Domain.Name;

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
        private readonly ICatalogItemsService _itemsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="itemsService">Items service.</param>
        public ItemsController(ICatalogItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        /// <summary>
        /// Creates new item.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Item id.</returns>
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
            var searchModel = new Services.Items.Dto.ItemsQueryDto(model.Query, page);
            var items = await _itemsService.SearchItems(searchModel);

            return Ok(items);
        }
    }
}
