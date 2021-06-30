using System;
using Catalog.Domain;

namespace Catalog.Services.Brands.Dto
{
    /// <summary>
    /// Update bramd dto.
    /// </summary>
    public class UpdateBrand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBrand"/> class.
        /// </summary>
        /// <param name="brandId">Id.</param>
        /// <param name="brandName">Name.</param>
        public UpdateBrand(Guid brandId, Name brandName)
        {
            BrandId = brandId;
            BrandName = brandName;
        }

        /// <summary>
        /// Gets id.
        /// </summary>
        public Guid BrandId { get; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public Name BrandName { get; }
    }
}