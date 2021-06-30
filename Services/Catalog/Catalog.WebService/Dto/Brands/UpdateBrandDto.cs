using System.ComponentModel.DataAnnotations;
using Catalog.WebService.Validation;

namespace Catalog.WebService.Dto.Brands
{
    /// <summary>
    /// Brand update model.
    /// </summary>
    public record UpdateBrandDto
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required]
        [Name]
        public string Name { get; init; }
    }
}