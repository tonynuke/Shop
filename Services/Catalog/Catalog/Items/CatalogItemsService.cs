using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Brands;
using Catalog.Items.Dto;
using Catalog.Messages.Events;
using Catalog.Persistence;
using Commom.ApiErrors.Errors;
using Common.MongoDb;
using CSharpFunctionalExtensions;
using MongoDB.Driver;
using Result = CSharpFunctionalExtensions.Result;

namespace Catalog.Items
{
    /// <summary>
    /// Items service.
    /// </summary>
    public class CatalogItemsService
    {
        private readonly BrandsService _brandsService;
        private readonly CatalogContext _catalogContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemsService"/> class.
        /// </summary>
        /// <param name="brandsService">Brands service.</param>
        /// <param name="catalogContext">Context.</param>
        public CatalogItemsService(
            BrandsService brandsService,
            CatalogContext catalogContext)
        {
            _brandsService = brandsService;
            _catalogContext = catalogContext;
        }

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Item id.</returns>
        public Task<Result<Guid>> Create(CreateItemDto dto)
        {
            return _brandsService.FindBrand(dto.BrandId)
                .ToResult(ErrorCodes.NotFound)
                .Map(brand => brand.CreateItem(dto.Name, dto.Description, dto.Price))
                .Tap(InsertOne)
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

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Result.</returns>
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

        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item.</returns>
        public Task<CatalogItem> FindOne(Guid id)
        {
            return _catalogContext.Items.Find(item => item.Id == id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Finds items.
        /// </summary>
        /// <param name="ids">Items ids.</param>
        /// <returns>Items.</returns>
        public async Task<IReadOnlyCollection<CatalogItem>> FindMany(IEnumerable<Guid> ids)
        {
            return await _catalogContext.Items.Find(item => ids.Contains(item.Id)).ToListAsync();
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task DeleteOne(Guid id)
        {
            return _catalogContext.ExecuteInTransaction(async () =>
                   {
                       await _catalogContext.Items.DeleteOneAsync(item => item.Id == id);
                       await _catalogContext.Events.InsertOneAsync(new ItemDeleted(id));
                   });
        }
    }
}