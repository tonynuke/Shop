using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Basket.Persistence;
using DataAccess;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Specifications;
using TestUtils;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Basket.Tests.Integration
{
    public class RepositoryTests : MongoClientFixture
    {
        private readonly Fixture _fixture = new ();
        private readonly BasketContext _basketContext;

        public RepositoryTests(ITestOutputHelper testOutputHelper) 
            : base(testOutputHelper)
        {
            _basketContext = new BasketContext(Database);
        }

        [Fact]
        public async Task Inserts_new_basket()
        {
            var basket = _fixture.Create<Domain.Basket>();
            await _basketContext.Baskets.InsertOneAsync(basket);

            var dbBasket = await _basketContext.Baskets.Find(b => b.Id == basket.Id).SingleOrDefaultAsync();

            basket.Id.Should().Be(dbBasket.Id);
            basket.Items.Should().AllBeEquivalentTo(dbBasket.Items);
        }

        [Fact]
        public async Task Uses_index()
        {
            var basket = _fixture.Create<Domain.Basket>();
            await _basketContext.Baskets.InsertOneAsync(basket);

            //var options = new FindOptions
            //{
            //    Modifiers = new BsonDocument("$explain", true)
            //};

            //var specification = Specification<Domain.Basket>.Create(b => b.Id == basket.Id);
            //var dbBasket = await _basketContext.Baskets
            //    .Find(specification, options)
            //    .Project(new BsonDocument())
            //    .SingleOrDefaultAsync();

            var indexes = new List<BsonDocument>();
            var indexesCursor = await _basketContext.Baskets.Indexes.ListAsync();
            await indexesCursor.ForEachAsync(document => indexes.Add(document));
        }
    }
}