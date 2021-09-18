using System;
using System.Threading.Tasks;
using AutoFixture;
using Basket.Persistence;
using FluentAssertions;
using MongoDB.Driver;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Basket.Tests.Integration
{
    public class BasketContextTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new();
        private readonly BasketContext _basketContext;

        public BasketContextTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _basketContext = new BasketContext(Database);
        }

        [Fact]
        public async Task Inserts_new_basket()
        {
            var basket = _fixture.Create<Domain.Basket>();
            await _basketContext.Baskets.InsertOneAsync(basket);

            var dbBasket = await _basketContext.Baskets.Find(b => basket.Id == b.Id).SingleOrDefaultAsync();

            basket.Should().BeEquivalentTo(dbBasket);
        }
    }
}