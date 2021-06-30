using System;

namespace Catalog.WebService.Dto.Brands
{
    /// <summary>
    /// Brand model.
    /// </summary>
    public record BrandDto
    {
        /// <summary>
        /// Gets id.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets image url.
        /// </summary>
        public string ImageUrl { get; init; }
    }
}