using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Persistence;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nest;

namespace Catalog.Indexing
{
    /// <summary>
    /// Indexing service.
    /// </summary>
    public class IndexingService
    {
        private readonly CatalogContext _context;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<IndexingService> _logger;

        public IndexingService(
            CatalogContext context,
            IElasticClient elasticClient,
            ILogger<IndexingService> logger)
        {
            _context = context;
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// Indexes the item.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Asynchronous operation.</returns>
        public async Task Index(Guid itemId)
        {
            var item = await _context.Items
                .Find(x => x.Id == itemId)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                _logger.LogWarning("Item with id {Id} not found.", itemId);
                return;
            }

            var indexItem = new Item
            {
                Id = item.Id,
                Name = item.Name.Value,
                Description = item.Description,
                Brand = item.Brand.Name.Value,
                Price = item.Price
            };

            await _elasticClient.IndexDocumentAsync(indexItem);
        }

        /// <summary>
        /// Removes item by it's id.
        /// </summary>
        /// <param name="itemId">Id.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task RemoveFromIndex(Guid itemId)
        {
            return _elasticClient.DeleteAsync<Item>(itemId);
        }

        /// <summary>
        /// Searches items.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Items.</returns>
        public async Task<IReadOnlyCollection<Item>> SearchItems(ItemsQueryDto dto)
        {
            var response = await _elasticClient.SearchAsync<Item>(
                selector => selector
                    .From(dto.Page.Skip)
                    .Size(dto.Page.Limit)
                    .Query(q => q.MultiMatch(
                        c => c.Query(dto.Query)
                            .Fields(descriptor => descriptor.Fields(
                                item => item.Name,
                                item => item.Description,
                                item => item.Brand)))));

            return response.Documents;
        }
    }
}