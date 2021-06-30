using System.ComponentModel.DataAnnotations;
using Catalog.WebService.Validation;

namespace Catalog.WebService.Dto.Brands
{
    /// <summary>
    /// Brand creation model.
    /// </summary>
    public record CreateBrandDto
    {
        /// <summary>
        /// Gets name.
        /// </summary>
        [Name]
        [Required]
        public string Name { get; init; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; init; }
    }
}