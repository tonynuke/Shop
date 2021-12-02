using System;
using System.Threading.Tasks;
using Catalog.Brands.Dto;
using Catalog.Persistence;
using Commom.ApiErrors.Errors;
using Common.MongoDb;
using Common.Utils.Pagination;
using CSharpFunctionalExtensions;
using MongoDB.Driver;

namespace Catalog.Brands
{
    /// <summary>
    /// Brands service.
    /// </summary>
    public class BrandsService
    {
        private readonly CatalogContext _catalogContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandsService"/> class.
        /// </summary>
        /// <param name="catalogContext">Context.</param>
        public BrandsService(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        /// <summary>
        /// Creates brand.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Created brand id.</returns>
        public Task<Result<Guid>> CreateBrand(CreateBrand dto)
        {
            var brand = new Brand(dto.Name, dto.Description);
            return Result.Try(() => _catalogContext.Brands.InsertOneAsync(brand), BrandErrorHandler)
                .Map(() => brand.Id);
        }

        /// <summary>
        /// Updates the brand.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task<Result> UpdateBrand(UpdateBrand dto)
        {
            return FindBrand(dto.BrandId)
                .ToResult(ErrorCodes.NotFound)
                .Tap(brand => brand.Name = dto.BrandName)
                .OnSuccessTry(
                    brand => _catalogContext.Brands.ReplaceOneOcc(brand),
                    BrandErrorHandler);
        }

        /// <summary>
        /// Deletes brand.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task DeleteBrand(Guid id)
        {
            return _catalogContext.Brands.DeleteOneAsync(brand => brand.Id == id);
        }

        /// <summary>
        /// Finds brand.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Brand.</returns>
        public async Task<Maybe<Brand>> FindBrand(Guid id)
        {
            var brandOrNothing = await _catalogContext.Brands
                .Find(brand => brand.Id == id)
                .SingleOrDefaultAsync();
            return Maybe<Brand>.From(brandOrNothing);
        }

        /// <summary>
        /// Gets brands page.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <returns>Brands page.</returns>
        public async Task<PageContent<Brand>> GetBrandsPage(Page page)
        {
            var count = await _catalogContext.Brands.EstimatedDocumentCountAsync();

            var brands = await _catalogContext.Brands
                .Find(FilterDefinition<Brand>.Empty)
                .Skip(page.Skip)
                .Limit(page.Limit)
                .ToListAsync();

            return new PageContent<Brand>(brands, count);
        }

        private static string BrandErrorHandler(Exception exception)
        {
            switch (exception)
            {
                case MongoWriteException e:
                    if (e.WriteError.Code == 11000 && e.Message.Contains("name_ix"))
                    {
                        return "Brand name duplication";
                    }

                    break;
            }

            throw exception;
        }
    }
}