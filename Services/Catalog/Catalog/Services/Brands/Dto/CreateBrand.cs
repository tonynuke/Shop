using System;
using Catalog.Domain;

namespace Catalog.Services.Brands.Dto
{
    /// <summary>
    /// Create brand dto.
    /// </summary>
    public class CreateBrand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBrand"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        public CreateBrand(Name name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }

        /// <summary>
        /// Gets name.
        /// </summary>
        public Name Name { get; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; }
    }
}