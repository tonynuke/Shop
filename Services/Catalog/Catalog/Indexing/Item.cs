using System;

namespace Catalog.Indexing
{
    /// <summary>
    /// Catalog item for indexing.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public decimal Price { get; set; }
    }
}