using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain;
using Catalog.Persistence;
using Catalog.Services.Brands;
using Catalog.Services.Items.Dto;
using Common.Api.Errors;
using CSharpFunctionalExtensions;
using DataAccess;
using MongoDB.Driver;
using Nest;
using Result = CSharpFunctionalExtensions.Result;

namespace Catalog.Services.Items
{
    /// <summary>
    /// Items service.
    /// </summary>
    public class CatalogItemsService : ICatalogItemsService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IBrandsService _brandsService;
        private readonly CatalogContext _catalogContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemsService"/> class.
        /// </summary>
        /// <param name="elasticClient"></param>
        /// <param name="brandsService"></param>
        /// <param name="catalogContext"></param>
        public CatalogItemsService(
            IElasticClient elasticClient,
            IBrandsService brandsService,
            CatalogContext catalogContext)
        {
            _elasticClient = elasticClient;
            _brandsService = brandsService;
            _catalogContext = catalogContext;
        }

        /// <inheritdoc/>
        public Task<Result<Guid>> Create(CreateItemDto dto)
        {
            return _brandsService.FindBrand(dto.BrandId)
                .ToResult(ErrorCodes.NotFound)
                .Map(brand => brand.CreateItem(dto.Name, dto.Description, dto.Price))
                .Tap(InsertOne)
                .Map(item => new Indexing.Item
                {
                    Id = item.Id,
                    Name = item.Name.Value,
                    Description = item.Description,
                    Brand = item.Brand.Name.Value,
                    Price = item.Price
                })
                .Tap(item => _elasticClient.IndexDocumentAsync(item))
                .Map(item => item.Id);

            Task InsertOne(CatalogItem item)
            {
                return _catalogContext.ExecuteInTransaction(async () =>
                {
                    await _catalogContext.Items.InsertOneAsync(item);
                    await _catalogContext.Events.InsertManyAsync(item.DomainEvents);
                });
            }
        }

        /// <inheritdoc/>
        public async Task<Result> UpdateOne(UpdateItemDto dto)
        {
            Maybe<CatalogItem> itemOtNothing = await _catalogContext.Items
                .Find(item => item.Id == dto.ItemId)
                .SingleOrDefaultAsync();
            return await itemOtNothing
                .ToResult(ErrorCodes.NotFound)
                .Tap(item => item.Name = dto.Name)
                .Tap(item => item.Description = dto.Description)
                .Tap(item => item.Price = dto.Price)
                .Tap(UpdateItem);

            Task UpdateItem(CatalogItem item)
            {
                return _catalogContext.ExecuteInTransaction(async () =>
                {
                    await _catalogContext.Items.ReplaceOneOcc(item);
                    await _catalogContext.Events.InsertManyAsync(item.DomainEvents);
                });
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Indexing.Item>> SearchItems(ItemsQueryDto dto)
        {
            var response = await _elasticClient.SearchAsync<Indexing.Item>(
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

        /// <inheritdoc/>
        public Task<CatalogItem> FindOne(Guid id)
        {
            return _catalogContext.Items.Find(item => item.Id == id).SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public Task DeleteOne(Guid id)
        {
            return Task.WhenAll(
                _catalogContext.Items.DeleteOneAsync(item => item.Id == id),
                _elasticClient.DeleteAsync<Indexing.Item>(id));
        }
    }
}