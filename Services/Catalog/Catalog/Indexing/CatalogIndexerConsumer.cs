using System;
using System.Threading.Tasks;
using Catalog.Items;
using Catalog.Messages.Events;
using Catalog.Persistence;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Catalog.Indexing
{
    /// <summary>
    /// Indexing consumer.
    /// </summary>
    public class CatalogIndexerConsumer
        : IConsumer<ItemAdded>,
        IConsumer<ItemChanged>,
        IConsumer<ItemDeleted>
    {
        private readonly CatalogContext _context;
        private readonly CatalogIndexer _indexingService;
        private readonly ILogger<CatalogIndexerConsumer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogIndexerConsumer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="indexingService">Indexing service.</param>
        /// <param name="logger">Logger.</param>
        public CatalogIndexerConsumer(
            CatalogContext context,
            CatalogIndexer indexingService,
            ILogger<CatalogIndexerConsumer> logger)
        {
            _context = context;
            _indexingService = indexingService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task Consume(ConsumeContext<ItemAdded> context)
        {
            return FindAndIndexItem(context.Message.ItemId);
        }

        /// <inheritdoc/>
        public Task Consume(ConsumeContext<ItemChanged> context)
        {
            return FindAndIndexItem(context.Message.ItemId);
        }

        /// <inheritdoc/>
        public Task Consume(ConsumeContext<ItemDeleted> context)
        {
            return _indexingService.RemoveFromIndex(context.Message.ItemId);
        }

        private async Task FindAndIndexItem(Guid itemId)
        {
            Maybe<CatalogItem> item = await _context.Items
                .Find(x => x.Id == itemId)
                .SingleOrDefaultAsync();

            await item
                .ToResult($"Item with id {itemId} not found.")
                .Tap(i => _indexingService.Index(i))
                .OnFailure(error => _logger.LogWarning(error));
        }
    }
}