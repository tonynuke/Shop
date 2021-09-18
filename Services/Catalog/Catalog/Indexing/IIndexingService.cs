using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Indexing
{
    /// <summary>
    /// Indexing service interface.
    /// </summary>
    public interface IIndexingService
    {
        /// <summary>
        /// Indexes the item.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Index(Guid itemId);

        /// <summary>
        /// Removes item by it's id.
        /// </summary>
        /// <param name="itemId">Id.</param>
        /// <returns>Asynchronous operation.</returns>
        Task RemoveFromIndex(Guid itemId);

        /// <summary>
        /// Searches items.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Items.</returns>
        Task<IReadOnlyCollection<Item>> SearchItems(ItemsQueryDto dto);
    }
}