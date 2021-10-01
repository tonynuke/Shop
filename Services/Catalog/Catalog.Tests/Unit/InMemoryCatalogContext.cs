using System;
using System.Threading.Tasks;
using Catalog.Brands;
using Catalog.Items;
using Catalog.Persistence;
using DataAccess.Entities;
using Domain;
using MongoDB.Driver;
using Moq;

namespace Catalog.Tests.Unit
{
    public class InMemoryCatalogContext : CatalogContext
    {
        protected InMemoryCatalogContext(IMongoDatabase database)
            : base(database)
        {
        }

        public Mock<IMongoCollection<Brand>> BrandsMock { get; } = new();

        public Mock<IMongoCollection<CatalogItem>> ItemsMock { get; } = new();

        public Mock<IMongoCollection<DomainEventBase>> EventsMock { get; } = new();

        public override IMongoCollection<Brand> Brands => BrandsMock.Object;

        public override IMongoCollection<CatalogItem> Items => ItemsMock.Object;

        public override IMongoCollection<DomainEventBase> Events => EventsMock.Object;

        public override Task ExecuteInTransaction(Func<Task> func)
        {
            return func();
        }

        public static InMemoryCatalogContext Create()
        {
            var databaseStub = new Mock<IMongoDatabase>();
            databaseStub
                .SetupGet(client => client.Client)
                .Returns(Mock.Of<IMongoClient>());

            return new InMemoryCatalogContext(databaseStub.Object);
        }
    }
}