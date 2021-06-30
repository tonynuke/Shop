using System;
using System.Threading.Tasks;
using Catalog.Domain;
using Catalog.Services.Brands;
using Catalog.Services.Brands.Dto;
using Catalog.WebService.Dto.Brands;
using Common.Api;
using Common.Pagination;
using CSharpFunctionalExtensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PageDto = Common.Pagination.Dto.PageDto;

namespace Catalog.WebService.Controllers.V1
{
    /// <summary>
    /// Brands controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandsService _brandsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandsController"/> class.
        /// </summary>
        /// <param name="brandsService">Brands service.</param>
        public BrandsController(IBrandsService brandsService)
        {
            _brandsService = brandsService;
        }

        /// <summary>
        /// Gets brands page.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <returns>Brands.</returns>
        [AllowAnonymous]
        [HttpPost("get")]
        [ProducesResponseType(typeof(PageContent<Brand>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBrandsPage(PageDto page)
        {
            var pageModel = Page.Create(page.Skip, page.Limit).Value;
            var pageContent = await _brandsService.GetBrandsPage(pageModel);
            return Ok(pageContent);
        }

        /// <summary>
        /// Searches brand by id.
        /// </summary>
        /// <param name="id">Brand id.</param>
        /// <returns>Brand.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindBrandById(Guid id)
        {
            var brandOrNothing = await _brandsService.FindBrand(id);
            return brandOrNothing.HasValue
                ? Ok(brandOrNothing.Value.Adapt<BrandDto>())
                : NotFound();
        }

        /// <summary>
        /// Creates brand.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Brand id.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBrand(CreateBrandDto model)
        {
            return await Name.Create(model.Name)
                .Map(name => new CreateBrand(name, model.Description))
                .Bind(_brandsService.CreateBrand)
                .Finally(this.FromResult);
        }

        /// <summary>
        /// Updates brand.
        /// </summary>
        /// <param name="id">Brand id.</param>
        /// <param name="model">Model.</param>
        /// <returns>No content.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateBrand(Guid id, UpdateBrandDto model)
        {
            return await Name.Create(model.Name)
                .Map(name => new UpdateBrand(id, name))
                .Bind(_brandsService.UpdateBrand)
                .Finally(this.FromResult);
        }

        /// <summary>
        /// Deletes brand.
        /// </summary>
        /// <param name="id">Brand id.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            await _brandsService.DeleteBrand(id);
            return NoContent();
        }
    }
}
