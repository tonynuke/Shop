using System;
using System.Threading.Tasks;
using Catalog.Domain;
using Catalog.Services.Brands.Dto;
using Common.Pagination;
using CSharpFunctionalExtensions;

namespace Catalog.Services.Brands
{
    /// <summary>
    /// Brands service interface.
    /// </summary>
    public interface IBrandsService
    {
        /// <summary>
        /// Creates brand.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Created brand id.</returns>
        Task<Result<Guid>> CreateBrand(CreateBrand dto);

        /// <summary>
        /// Updates the brand.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Asynchronous operation.</returns>
        Task<Result> UpdateBrand(UpdateBrand dto);

        /// <summary>
        /// Deletes brand.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Asynchronous operation.</returns>
        Task DeleteBrand(Guid id);

        /// <summary>
        /// Finds brand.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Brand.</returns>
        Task<Maybe<Brand>> FindBrand(Guid id);

        /// <summary>
        /// Gets brands page.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <returns>Brands page.</returns>
        Task<PageContent<Brand>> GetBrandsPage(Page page);
    }
}