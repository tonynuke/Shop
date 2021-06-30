using System.Threading.Tasks;
using DataAccess.Migrations;
using MongoDB.Driver;

namespace Basket.Persistence.Migrations
{
    public class Init : Migration<BasketContext>
    {
        public Init(BasketContext context)
            : base(context)
        {
        }

        public override string Id => "Init";

        public override Task Up()
        {
            var index = new CreateIndexModel<Domain.Basket>(
                Builders<Domain.Basket>.IndexKeys
                    .Ascending("items._id"),
                new CreateIndexOptions
                {
                    Name = "IX_ItemId"
                });

            return Context.Baskets.Indexes.CreateOneAsync(index);
        }

        public override Task Down()
        {
            return Context.Baskets.Indexes.DropOneAsync("IX_ItemId");
        }
    }
}