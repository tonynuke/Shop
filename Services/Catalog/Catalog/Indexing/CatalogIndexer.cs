using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Items;
using Nest;

namespace Catalog.Indexing
{
    /// <summary>
    /// Indexing service.
    /// </summary>
    public class CatalogIndexer
    {
        private readonly IElasticClient _elasticClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogIndexer"/> class.
        /// </summary>
        /// <param name="elasticClient">Elastic client.</param>
        public CatalogIndexer(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// Indexes the item.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Asynchronous operation.</returns>
        public async Task Index(CatalogItem item)
        {
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