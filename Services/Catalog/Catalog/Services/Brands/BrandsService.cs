using System;
using System.Threading.Tasks;
using Catalog.Domain;
using Catalog.Persistence;
using Catalog.Services.Brands.Dto;
using Common.Api.Errors;
using Common.Pagination;
using CSharpFunctionalExtensions;
using DataAccess;
using MongoDB.Driver;

namespace Catalog.Services.Brands
{
    /// <summary>
    /// Brands service.
    /// </summary>
    public class BrandsService : IBrandsService
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

        /// <inheritdoc/>
        public Task<Result<Guid>> CreateBrand(CreateBrand dto)
        {
            var brand = new Brand(dto.Name, dto.Description);
            return Result.Try(() => _catalogContext.Brands.InsertOneAsync(brand), BrandErrorHandler)
                .Map(() => brand.Id);
        }

        /// <inheritdoc/>
        public Task<Result> UpdateBrand(UpdateBrand dto)
        {
            return FindBrand(dto.BrandId)
                .ToResult(ErrorCodes.NotFound)
                .Tap(brand => brand.Name = dto.BrandName)
                .OnSuccessTry(
                    brand => _catalogContext.Brands.ReplaceOneOcc(brand),
                    BrandErrorHandler);
        }

        /// <inheritdoc/>
        public Task DeleteBrand(Guid id)
        {
            return _catalogContext.Brands.DeleteOneAsync(brand => brand.Id == id);
        }

        /// <inheritdoc/>
        public async Task<Maybe<Brand>> FindBrand(Guid id)
        {
            var brandOrNothing = await _catalogContext.Brands
                .Find(brand => brand.Id == id)
                .SingleOrDefaultAsync();
            return Maybe<Brand>.From(brandOrNothing);
        }

        /// <inheritdoc/>
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