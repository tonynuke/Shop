using System;

namespace Catalog.WebService.Dto.Items
{
    /// <summary>
    /// Item model.
    /// </summary>
    public record ItemDto
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; init; }
    }
}