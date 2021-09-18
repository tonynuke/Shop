using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain;
using Catalog.Services.Items.Dto;
using CSharpFunctionalExtensions;

namespace Catalog.Services.Items
{
    /// <summary>
    /// Items service interface.
    /// </summary>
    public interface ICatalogItemsService
    {
        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Item id.</returns>
        Task<Result<Guid>> Create(CreateItemDto dto);

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Result.</returns>
        Task<Result> UpdateOne(UpdateItemDto dto);

        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item.</returns>
        Task<CatalogItem> FindOne(Guid id);

        /// <summary>
        /// Finds items.
        /// </summary>
        /// <param name="ids">Items ids.</param>
        /// <returns>Items.</returns>
        Task<IReadOnlyCollection<CatalogItem>> FindMany(IEnumerable<Guid> ids);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Asynchronous operation.</returns>
        Task DeleteOne(Guid id);
    }
}