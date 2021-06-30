using System;
using System.Threading.Tasks;
using Basket.Persistence;
using MongoDB.Driver;
using Moq;

namespace Basket.Tests.Unit
{
    public class InMemoryBasketContext : BasketContext
    {
        protected InMemoryBasketContext(IMongoDatabase database) 
            : base(database)
        {
        }

        public Mock<IMongoCollection<Domain.Basket>> BasketsMock { get; } = new();

        public override IMongoCollection<Domain.Basket> Baskets => BasketsMock.Object;

        public override Task ExecuteInTransaction(Func<Task> func)
        {
            return func();
        }

        public static InMemoryBasketContext Create()
        {
            var databaseStub = new Mock<IMongoDatabase>();
            databaseStub
                .SetupGet(client => client.Client)
                .Returns(Mock.Of<IMongoClient>());

            return new InMemoryBasketContext(databaseStub.Object);
        }
    }
}