using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Catalog.Brands;
using Catalog.Items;
using Catalog.Persistence;
using Common.MongoDb;
using Common.MongoDb.Entities;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MongoDB.Bson.Serialization;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Tests.Integration
{
    public class DomainEventsTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new();

        public DomainEventsTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            try
            {
                MongoEntitiesMapsRegistrar.RegisterEntitiesMapsFromAssembly(typeof(EntityBaseMap).Assembly);
                BsonSerializer.RegisterSerializer(new NameSerializer());
            }
            catch
            {
                // dont throw exception
            }
        }

        [Fact]
        public async Task Concurrency()
        {
            var catalogContext = new CatalogContext(Database);
            var brandsService = new BrandsService(catalogContext);

            var name = Name.Create(_fixture.Create<string>()).Value;
            var brand = await brandsService.CreateBrand(new Brands.Dto.CreateBrand(name, _fixture.Create<string>()));

            var updates = Enumerable.Range(0, 5).Select(x =>
                brandsService.UpdateBrand(new Brands.Dto.UpdateBrand(brand.Value, Name.Create(_fixture.Create<string>()).Value))
            );

            Func<Task> action = () => Task.WhenAll(updates);
            await action.Should().ThrowAsync<MongoDbConcurrencyException>();
        }

        // TODO: add assertions
        [Fact]
        public async Task Save_same_events_only_once()
        {
            var catalogContext = new CatalogContext(Database);
            var brandsService = new BrandsService(catalogContext);
            var itemsService = new CatalogItemsService(brandsService, catalogContext);

            var name = Name.Create(_fixture.Create<string>()).Value;

            var result = await brandsService.CreateBrand(new Brands.Dto.CreateBrand(name, _fixture.Create<string>()))
                .Check(brandId => itemsService.Create(
                    new Items.Dto.CreateItemDto(
                        brandId, name, _fixture.Create<string>(), _fixture.Create<decimal>())));

            result.IsSuccess.Should().BeTrue();
            var item = result.Value;

            //var name = Name.Create(_fixture.Create<string>()).Value;
            //var item = new CatalogItem(new Brand(name), name, "", 100);
            //item.Price = 300;

            //item.DomainEvents.Should().HaveCount(1);
        }
    }
}