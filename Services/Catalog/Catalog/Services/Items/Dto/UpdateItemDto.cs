using System;
using Catalog.Domain;

namespace Catalog.Services.Items.Dto
{
    /// <summary>
    /// Update item dto.
    /// </summary>
    public class UpdateItemDto
    {
        /// <summary>
        /// Gets item identifier.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// Gets name.
        /// </summary>
        public Name Name { get; }

        /// <summary>
        /// Gets description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; }
    }
}